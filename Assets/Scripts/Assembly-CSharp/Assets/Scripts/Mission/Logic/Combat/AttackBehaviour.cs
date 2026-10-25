using Assets.Scripts.Animation;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Combat
{
	public class AttackBehaviour : CombatBehaviour
	{
		private enum ETryAgain
		{
			Invalid = 0,
			ReactionStart = 1,
			Hit = 2
		}

		private float m_targetHitTime;

		private float m_targetHitDistance;

		private float m_baseHitTime;

		private Vector3 m_baseHitPosition = Vector3.zero;

		private ETryAgain m_tryAgain;

		private int m_tryAgainTimeout = -1;

		private bool m_playedAnimation;

		private bool m_didStartKillReaction;

		private bool m_allowReact;

		private bool m_tryUpgradeToParry;

		private static int LastComboKillIndex;

		public override ECombatBehaviourType BehaviourType
		{
			get
			{
				return ECombatBehaviourType.Attack;
			}
		}

		protected IActAnimation ActAnimation { get; set; }

		protected IReactAnimation ReactAnimation { get; set; }

		public AttackBehaviour(CombatParticipant _participant)
			: base(_participant)
		{
		}

		public override void Start(params object[] _params)
		{
			base.Start(_params);
			bool isCombo = (bool)_params[0];
			Attack(isCombo);
		}

		public override void End()
		{
			base.End();
			base.Animator.SetCurrentAttackMeleeType(AttackMeleeType.Invalid);
			base.Animator.SetCurrentComboAttack(Flag.False);
			base.Animator.speed = 1f;
			if (base.Participant.AttackWasKillMove && !base.Participant.AttackHasHit)
			{
				OnHitEvent(null);
			}
			base.Participant.AttackWasKillMove = false;
			base.Participant.AttackHasHit = false;
			base.Participant.AttackWasCombo = false;
			base.Participant.AttackWasParried = false;
			base.Participant.KillMoveWasParried = false;
			base.Participant.AttackWasDodged = false;
			if (base.Participant.CurrentTarget != null && base.Participant.CurrentTarget.LastAttacker == base.Participant)
			{
				base.Participant.CurrentTarget.LastAttacker = null;
			}
		}

		public override void Update()
		{
			if (!m_playedAnimation)
			{
				PlayAttackAnimation();
			}
			CombatParticipant currentTarget = base.Participant.CurrentTarget;
			if (currentTarget != null)
			{
				AdjustHitPosition();
			}
			if (!base.Participant.AttackHasHit && !base.Participant.AttackWasParried && !m_tryUpgradeToParry && currentTarget != null && currentTarget.IsPlayer && currentTarget.CanParry())
			{
				m_tryUpgradeToParry = true;
				base.Animator.SetCurrentAttackMeleeType(ActAnimation.AttackMelee + 1000);
			}
			if (m_tryAgainTimeout >= LogicMain.CurrentTurn)
			{
				if (m_tryAgain == ETryAgain.ReactionStart)
				{
					OnReactionStartEvent(null);
				}
				else if (m_tryAgain == ETryAgain.Hit)
				{
					OnHitEvent(null);
				}
			}
			base.Participant.AttackTime = LogicMain.CurrentTime;
		}

		public override bool Act()
		{
			return false;
		}

		public override bool React(CombatParticipant _source, params object[] _params)
		{
			if (!m_playedAnimation)
			{
				return false;
			}
			if (base.Participant.AttackWasKillMove && (!base.Participant.AttackHasHit || !m_allowReact))
			{
				return false;
			}
			return true;
		}

		public override void OnReactionStartEvent(AnimatorEvent _event)
		{
			if (m_tryAgain == ETryAgain.Invalid)
			{
				m_tryAgainTimeout = -1;
			}
			else
			{
				m_tryAgain = ETryAgain.Invalid;
			}
			if (base.Participant.AttackWasDodged)
			{
				return;
			}
			CombatParticipant currentTarget = base.Participant.CurrentTarget;
			if (currentTarget != null && base.Participant.AttackWasKillMove && !currentTarget.Character.GetIsKnockdowned())
			{
				if (currentTarget.Character.GetDeadOrAboutToDie())
				{
					m_didStartKillReaction = true;
				}
				else if (currentTarget.ActiveBehaviour.React(base.Participant, ReactAnimation))
				{
					m_didStartKillReaction = true;
					base.Participant.FacePosition(currentTarget.Position, 0f);
					currentTarget.Character.CharacterStats.Set_bool(StatTypes.EStatType.Character_Conflict_AboutToDie, true);
					currentTarget.FacePosition(base.Participant.Position, 0f);
					currentTarget.SwitchBehaviour(ECombatBehaviourType.GetHit, base.Participant, ReactAnimation);
				}
				else
				{
					m_tryAgain = ETryAgain.ReactionStart;
					m_tryAgainTimeout = int.MaxValue;
				}
			}
		}

		public override void OnDodgeEvent(AnimatorEvent _event)
		{
			CombatParticipant currentTarget = base.Participant.CurrentTarget;
			if (currentTarget != null && base.Participant.AttackWasDodged)
			{
				CombatHelper.ResetKillTokenDecayClock(base.Participant.Character);
				UpdateStandardToDodgedAttack();
				if (currentTarget.ActiveBehaviour.React(base.Participant, ReactAnimation))
				{
					bool missed = base.Participant.Character.Buffs.HasBuffs(EBuffType.Blinded);
					CombatHelper.ApplyDodge(base.Participant, currentTarget, missed);
					currentTarget.SwitchBehaviour(ECombatBehaviourType.GetHit, base.Participant, ReactAnimation);
				}
			}
		}

		public override void OnHitEvent(AnimatorEvent _event)
		{
			if (m_tryAgain != ETryAgain.Hit)
			{
				m_tryAgainTimeout = -1;
			}
			m_tryAgain = ETryAgain.Invalid;
			base.Participant.AttackHasHit = true;
			base.Animator.SetCurrentAttackMeleeType(AttackMeleeType.Invalid);
			if (base.Participant.AttackWasDodged)
			{
				return;
			}
			CombatParticipant currentTarget = base.Participant.CurrentTarget;
			if (currentTarget == null)
			{
				return;
			}
			if (base.Participant.AttackWasParried || base.Participant.KillMoveWasParried)
			{
				Parry(currentTarget, base.Participant.KillMoveWasParried);
			}
			else if (base.Participant.AttackWasKillMove || currentTarget.ActiveBehaviour.React(base.Participant, ReactAnimation))
			{
				if (base.Participant.AttackWasKillMove)
				{
					if (!currentTarget.Character.GetHasZeroHealth())
					{
						if (base.Participant.IsPlayer)
						{
							CombatHelper.ApplyKillTokenKill(base.Participant, currentTarget);
						}
						object[] obj = new object[2] { base.Participant, null };
						IReactAnimation reactAnimation2;
						if (m_didStartKillReaction)
						{
							IReactAnimation reactAnimation = null;
							reactAnimation2 = reactAnimation;
						}
						else
						{
							reactAnimation2 = ReactAnimation;
						}
						obj[1] = reactAnimation2;
						currentTarget.SwitchBehaviour(ECombatBehaviourType.Die, obj);
						if (!currentTarget.IsPlayer)
						{
							CombatController combatController = base.CombatController;
							float currentTime = LogicMain.CurrentTime;
							base.CombatController.GlobalAllyGotComboKilledTime = currentTime;
							combatController.GlobalAllyGotAttackedTime = currentTime;
						}
					}
					m_didStartKillReaction = true;
					return;
				}
				if (!currentTarget.Character.GetDeadOrAboutToDie())
				{
					CombatHelper.ApplyWeaponDamage(base.Participant, currentTarget, EApplyDamageFlags.StandardAttack);
					if (base.Participant.Character == LogicMain.PlayerEntity)
					{
						CombatHelper.IncreaseKillTokenCount(currentTarget.Character.GetHasZeroHealth());
					}
					else if (currentTarget.Character == LogicMain.PlayerEntity)
					{
						CombatHelper.DecreaseKillTokenCount();
					}
				}
				currentTarget.SwitchBehaviour(ECombatBehaviourType.GetHit, base.Participant, ReactAnimation);
				if (!currentTarget.IsPlayer)
				{
					base.CombatController.GlobalAllyGotAttackedTime = LogicMain.CurrentTime;
				}
			}
			else
			{
				m_tryAgain = ETryAgain.Hit;
				if (m_tryAgainTimeout < 0)
				{
					m_tryAgainTimeout = LogicMain.CurrentTurn + ((!base.Participant.IsPlayer) ? 1 : 5);
				}
				base.Participant.AttackHasHit = false;
			}
		}

		public override void OnParryEvent(AnimatorEvent _event)
		{
			CombatParticipant currentTarget = base.Participant.CurrentTarget;
			if (currentTarget != null)
			{
				if (m_tryUpgradeToParry)
				{
					UpdateStandardToParriedAttack();
					base.Participant.AttackWasParried = true;
				}
				if (currentTarget.ActiveBehaviour.React(base.Participant, ReactAnimation))
				{
					currentTarget.SwitchBehaviour(ECombatBehaviourType.Parry, base.Participant, ReactAnimation);
				}
			}
		}

		public override void OnComboEvent(AnimatorEvent _event)
		{
			base.OnComboEvent(_event);
			base.Participant.Behave();
			if (base.CombatController.PlayerExitCombatMode == ExitCombatMode.Invalid && base.Participant.ActionQueued && !base.Participant.SkillQueued && base.Participant.NextTarget != null && (base.CombatController.EnableComboChangeTarget || base.Participant.NextTarget == base.Participant.CurrentTarget) && base.Participant.InAttackDistanceMax())
			{
				base.Participant.ConsumeAction();
				Attack(true);
			}
			else
			{
				base.Animator.SetCurrentComboAttack(Flag.False);
			}
		}

		public override void OnAllowReactEvent(AnimatorEvent _event)
		{
			m_allowReact = true;
		}

		public override void OnAnimatorTransitionStarted(object _sender, AnimatorStateMachine.TransitionEventArgs _transition)
		{
			for (AnimatorStateMachineStatic.StateMachine parentStateMachine = _transition.Transition.TargetState.ParentStateMachine; parentStateMachine != null; parentStateMachine = parentStateMachine.ParentStateMachine)
			{
				if (!m_playedAnimation && (parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_CombatAbilities) || parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_AttackMelee)))
				{
					base.Animator.SetCurrentAttackMeleeType(AttackMeleeType.Invalid);
					base.Animator.SetCurrentComboAttack(Flag.Unset);
					m_targetHitTime = base.CombatController.GetTargetHitTime(this, ActAnimation.AttackMelee);
					m_playedAnimation = true;
					break;
				}
				if (m_playedAnimation && (parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_CombatWait) || parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_HighLowProfileWait) || parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_CombatStrafe)))
				{
					base.Participant.SwitchBehaviour(ECombatBehaviourType.Idle);
					break;
				}
			}
		}

		private void Parry(CombatParticipant _target, bool _killMoveWasParried)
		{
			EApplyDamageFlags flags = EApplyDamageFlags.StandardDeflect;
			if (_killMoveWasParried)
			{
				flags = EApplyDamageFlags.KillMoveDeflect;
				CombatHelper.SpendTokensForKillMove(base.Participant.Character);
			}
			CombatHelper.ApplyWeaponDamage(base.Participant, _target, flags);
			if (_target.Character.GetHasZeroHealth())
			{
				_target.SwitchBehaviour(ECombatBehaviourType.Die, base.Participant, ReactAnimation);
			}
		}

		private void Attack(bool _isCombo = false)
		{
			CombatParticipant nextTarget = base.Participant.NextTarget;
			if (nextTarget == null)
			{
				base.Participant.SwitchBehaviour(ECombatBehaviourType.Idle);
				return;
			}
			base.Participant.CurrentTarget = nextTarget;
			base.Participant.AttackWasKillMove = CombatHelper.CanKillTokenKill(base.Participant.Character);
			base.Participant.AttackHasHit = false;
			base.Participant.AttackWasDodged = false;
			base.Participant.AttackTime = LogicMain.CurrentTime;
			base.Participant.AttackWasCombo = _isCombo;
			base.Participant.AttackComboCount = (_isCombo ? (base.Participant.AttackComboCount + 1) : 0);
			nextTarget.EnableDodge = false;
			nextTarget.IsAttackedFlag = true;
			nextTarget.LastAttacker = base.Participant;
			nextTarget.Behave();
			if (!nextTarget.EnableDodge)
			{
				nextTarget.EnableDodge = CombatHelper.DecideToMiss(base.Participant, nextTarget);
			}
			m_allowReact = true;
			if (base.Participant.AttackWasKillMove && nextTarget.CanParryKillMove())
			{
				if (nextTarget.IsPlayer)
				{
					LogicMain.MissionResult.IncreaseSuccessfullyParried();
				}
				SetStandardAttackParried();
				base.Participant.AttackWasKillMove = false;
				base.Participant.KillMoveWasParried = true;
				base.Participant.AttackWasParried = false;
				base.Participant.AttackWasCombo = false;
			}
			else if (base.Participant.AttackWasKillMove)
			{
				SetStandardComboKill();
				base.Participant.AttackWasKillMove = true;
				base.Participant.KillMoveWasParried = false;
				base.Participant.AttackWasParried = false;
				base.Participant.AttackWasCombo = false;
				m_allowReact = false;
			}
			else if (nextTarget.CanParry())
			{
				if (nextTarget.IsPlayer)
				{
					LogicMain.MissionResult.IncreaseSuccessfullyParried();
				}
				SetStandardAttackParried();
				base.Participant.AttackWasKillMove = false;
				base.Participant.KillMoveWasParried = false;
				base.Participant.AttackWasParried = true;
				base.Participant.AttackWasCombo = false;
			}
			else
			{
				SetStandardAttack();
				base.Participant.AttackWasKillMove = false;
				base.Participant.KillMoveWasParried = false;
				base.Participant.AttackWasParried = false;
				base.Participant.AttackWasCombo = _isCombo;
				base.Participant.AttackWasDodged = nextTarget.CanDodge();
			}
			base.CombatController.UpdateFinalKillTarget(base.Participant);
			base.Agent.ResetPath();
			m_tryUpgradeToParry = false;
			m_tryAgain = ETryAgain.Invalid;
			m_tryAgainTimeout = -1;
			m_didStartKillReaction = false;
			m_baseHitTime = LogicMain.CurrentTime;
			m_baseHitPosition = base.Participant.Position;
			m_targetHitTime = m_baseHitTime + 2f;
			m_targetHitDistance = base.CombatController.HitDistance;
			if (base.CombatController.HitDistanceOverride.ContainsKey((int)ActAnimation.AttackMelee))
			{
				float num = base.CombatController.HitDistanceOverride[(int)ActAnimation.AttackMelee][1];
				if (num >= 0f)
				{
					m_targetHitDistance = num;
				}
			}
			base.Animator.speed = 1f;
			PlayAttackAnimation();
		}

		private void PlayAttackAnimation()
		{
			base.Animator.SetCurrentMovementState(MovementState.Idle);
			base.Animator.SetCurrentReactionType(ReactionType.Invalid);
			base.Animator.SetCurrentAttackMeleeType(ActAnimation.AttackMelee);
			base.Animator.SetCurrentComboAttack(base.Participant.AttackWasCombo ? Flag.True : Flag.False);
			m_playedAnimation = false;
		}

		private void AdjustHitPosition()
		{
			Vector3 position = base.Participant.Position;
			Vector3 position2 = base.Participant.CurrentTarget.Position;
			Vector3 vector = position2 - base.Participant.Position;
			vector.y = 0f;
			Vector3 normalized = vector.normalized;
			Vector3 b = position2 - normalized * m_targetHitDistance;
			if (LogicMain.CurrentTime <= m_targetHitTime + LogicMain.FrameTime)
			{
				float num = Mathf.Max(m_targetHitTime - LogicMain.CurrentTime, 0f);
				Vector3 b2 = Vector3.Lerp(m_baseHitPosition, b, 1f - num / (m_targetHitTime - m_baseHitTime));
				float num2 = num * base.CombatController.HitCorrectionDamping;
				base.Participant.Position = Vector3.Lerp(position, b2, LogicMain.FrameTime / (LogicMain.FrameTime + num2));
				base.Participant.FacePosition(base.Participant.CurrentTarget.Position, num2);
				base.Participant.LookAtDisable();
			}
		}

		private void SetStandardAttack()
		{
			int maxInclusive = 4;
			CharacterCondition characterCondition = base.Participant.CurrentTarget.Character.StateMachines.Cast<ICharacterConditionCarrier>().CharacterCondition;
			if (characterCondition.CurrentStateType == EStateType.CharacterCondition_Knockdowned)
			{
				maxInclusive = 2;
			}
			int lastValue = ((ActAnimation != null) ? ((int)ActAnimation.AttackMelee % 1000) : 0);
			lastValue = CombatHelper.RandomWithoutRepetition(1, maxInclusive, lastValue);
			if (base.CombatController.ForceAttackAnimationId)
			{
				lastValue = base.CombatController.AttackAnimationId;
			}
			ActAnimation = new ActAnimation
			{
				AttackMelee = (AttackMeleeType)lastValue,
				ActionUpperBody = ActionTypeUpperBody.Invalid,
				WaitForHash = AnimationStateMachineHashes.StateMachine_AttackMelee
			};
			ReactAnimation = new ReactAnimation
			{
				AttackMelee = AttackMeleeType.Invalid,
				AttackMeleeDie = (AttackMeleeType)lastValue,
				Reaction = ReactionType.GetHit,
				ActionUpperBody = ActionTypeUpperBody.Invalid,
				WaitForHash = AnimationStateMachineHashes.StateMachine_GetHit,
				ReactionEvent = AnimatorEventType.Hit,
				ReactionDie = ReactionType.Die
			};
		}

		private void UpdateStandardToDodgedAttack()
		{
			int attackMeleeDie = ((ActAnimation == null) ? 1 : ((int)ActAnimation.AttackMelee % 1000));
			ReactAnimation = new ReactAnimation
			{
				AttackMelee = AttackMeleeType.Invalid,
				AttackMeleeDie = (AttackMeleeType)attackMeleeDie,
				Reaction = ReactionType.DodgeJump,
				ActionUpperBody = ActionTypeUpperBody.Invalid,
				WaitForHash = AnimationStateMachineHashes.StateMachine_Dodge,
				ReactionEvent = AnimatorEventType.Dodge,
				ReactionDie = ReactionType.Die
			};
		}

		private void UpdateStandardToParriedAttack()
		{
			int num = ((ActAnimation == null) ? 1 : ((int)ActAnimation.AttackMelee % 1000));
			ActAnimation = new ActAnimation
			{
				AttackMelee = (AttackMeleeType)(1000 + num),
				ActionUpperBody = ActionTypeUpperBody.Invalid,
				WaitForHash = AnimationStateMachineHashes.StateMachine_AttackMelee
			};
			ReactAnimation = new ReactAnimation
			{
				AttackMelee = (AttackMeleeType)num,
				AttackMeleeDie = (AttackMeleeType)num,
				Reaction = ReactionType.ParryDeflect,
				ActionUpperBody = ActionTypeUpperBody.Invalid,
				WaitForHash = AnimationStateMachineHashes.StateMachine_ParryDeflect,
				ReactionEvent = AnimatorEventType.Parry,
				ReactionDie = ReactionType.Die
			};
		}

		private void SetStandardAttackParried()
		{
			int lastValue = ((ActAnimation != null) ? ((int)ActAnimation.AttackMelee % 1000) : 0);
			lastValue = CombatHelper.RandomWithoutRepetition(1, 4, lastValue);
			if (base.CombatController.ForceAttackAnimationId)
			{
				lastValue = base.CombatController.AttackAnimationId;
			}
			ActAnimation = new ActAnimation
			{
				AttackMelee = (AttackMeleeType)(1000 + lastValue),
				ActionUpperBody = ActionTypeUpperBody.Invalid,
				WaitForHash = AnimationStateMachineHashes.StateMachine_AttackMelee
			};
			ReactAnimation = new ReactAnimation
			{
				AttackMelee = (AttackMeleeType)lastValue,
				AttackMeleeDie = (AttackMeleeType)lastValue,
				Reaction = ReactionType.ParryDeflect,
				ActionUpperBody = ActionTypeUpperBody.Invalid,
				WaitForHash = AnimationStateMachineHashes.StateMachine_ParryDeflect,
				ReactionEvent = AnimatorEventType.Parry,
				ReactionDie = ReactionType.Die
			};
		}

		private void SetStandardComboKill()
		{
			CharacterCondition characterCondition = base.Participant.CurrentTarget.Character.StateMachines.Cast<ICharacterConditionCarrier>().CharacterCondition;
			int num = (LastComboKillIndex = ((characterCondition.CurrentStateType != EStateType.CharacterCondition_Knockdowned) ? CombatHelper.RandomWithoutRepetition(1, 5, LastComboKillIndex) : ((LastComboKillIndex == 3) ? 5 : ((LastComboKillIndex != 5) ? ((!(LogicMain.Random.RandF() < 0.5f)) ? 5 : 3) : 3))));
			if (num == 5)
			{
				num++;
			}
			if (base.CombatController.ForceComboAnimationId)
			{
				num = base.CombatController.ComboAnimationId;
			}
			ActAnimation = new ActAnimation
			{
				AttackMelee = (AttackMeleeType)(1100 + num),
				ActionUpperBody = ActionTypeUpperBody.Invalid,
				WaitForHash = AnimationStateMachineHashes.StateMachine_AttackMelee
			};
			ReactAnimation = new ReactAnimation
			{
				AttackMelee = AttackMeleeType.Invalid,
				AttackMeleeDie = AttackMeleeType.Invalid,
				Reaction = (ReactionType)(1100 + num),
				ActionUpperBody = ActionTypeUpperBody.Invalid,
				WaitForHash = AnimationStateMachineHashes.StateMachine_GetKilledCombo,
				ReactionEvent = AnimatorEventType.ReactionStart,
				ReactionDie = (ReactionType)(1100 + num)
			};
		}
	}
}
