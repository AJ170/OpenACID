using Assets.Scripts.Animation;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Combat
{
	public class DieBehaviour : CombatBehaviour
	{
		private bool m_playedAnimation;

		private IReactAnimation m_reactAnimation;

		private Vector3 m_reactDirection;

		public override ECombatBehaviourType BehaviourType
		{
			get
			{
				return ECombatBehaviourType.Die;
			}
		}

		public DieBehaviour(CombatParticipant participant)
			: base(participant)
		{
		}

		public override void Start(params object[] _params)
		{
			base.Start(_params);
			CombatParticipant combatParticipant = (CombatParticipant)_params[0];
			m_reactAnimation = (IReactAnimation)_params[1];
			base.Agent.enabled = false;
			base.Participant.IsDead = true;
			base.Participant.CancelAction();
			base.Participant.AttackWasKillMove = false;
			base.Participant.LookAtDisable();
			if (base.Participant.Character == LogicMain.PlayerEntity)
			{
				CombatHelper.ExitKillTokenMode();
			}
			base.Participant.EnableParry = false;
			if (m_reactAnimation == null)
			{
				base.Participant.ExitCombat();
				m_playedAnimation = true;
				return;
			}
			if (base.Participant.Character.GetIsKnockdowned())
			{
				m_reactAnimation = new ReactAnimation
				{
					AttackMelee = AttackMeleeType.Invalid,
					AttackMeleeDie = AttackMeleeType.Invalid,
					Reaction = ReactionType.GetHurtOnGround,
					ActionUpperBody = ActionTypeUpperBody.Invalid,
					WaitForHash = AnimationStateMachineHashes.StateMachine_PushBack,
					ReactionEvent = AnimatorEventType.Hit,
					ReactionDie = ReactionType.GetHurtOnGround
				};
				PlayReactAnimation();
				return;
			}
			m_reactDirection = CombatHelper.ReactionDirection(combatParticipant.Character, base.Participant.Character);
			if (m_reactDirection.z < 0f)
			{
				m_reactAnimation = new ReactAnimation
				{
					AttackMelee = AttackMeleeType.Invalid,
					AttackMeleeDie = AttackMeleeType.Invalid,
					Reaction = ReactionType.PushBack,
					ActionUpperBody = ActionTypeUpperBody.Invalid,
					WaitForHash = AnimationStateMachineHashes.StateMachine_PushBack,
					ReactionEvent = AnimatorEventType.Hit,
					ReactionDie = ReactionType.PushBack
				};
			}
			else
			{
				base.Participant.FacePosition(combatParticipant.Position, 0f);
			}
			PlayReactAnimation();
		}

		public override void Update()
		{
			if (!m_playedAnimation)
			{
				PlayReactAnimation();
			}
		}

		public override bool React(CombatParticipant _source, params object[] _params)
		{
			return false;
		}

		public void PlayReactAnimation()
		{
			base.Animator.SetCurrentMovementState(MovementState.Idle);
			base.Animator.SetCurrentReactionType(m_reactAnimation.ReactionDie);
			base.Animator.SetCurrentJumpDistanceZ(0.4f);
			base.Animator.SetCurrentReactionDirectionX(m_reactDirection.x);
			base.Animator.SetCurrentReactionDirectionZ(m_reactDirection.z);
			AttackMeleeType type = CombatHelper.DieAnimationDirection(m_reactAnimation.AttackMeleeDie, m_reactDirection);
			base.Animator.SetCurrentAttackMeleeType(type);
			base.Animator.SetCurrentComboAttack(Flag.False);
			base.Animator.SetCurrentStopAction(false);
			m_playedAnimation = false;
		}

		public override void OnAnimatorStateEntered(object _sender, AnimatorStateMachine.StateEventArgs _state)
		{
			if (m_playedAnimation || m_reactAnimation == null)
			{
				return;
			}
			bool flag = false;
			if (m_reactAnimation.WaitForHash.NameHash == AnimationStateMachineHashes.StateMachine_PushBack.NameHash)
			{
				flag = AnimationStateHashes.IsStateContainedInArray(_state.State, AnimationStateHashes.States_PushBackHurt);
			}
			else
			{
				for (AnimatorStateMachineStatic.StateMachine parentStateMachine = _state.State.ParentStateMachine; parentStateMachine != null; parentStateMachine = parentStateMachine.ParentStateMachine)
				{
					if (parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_Die) || parentStateMachine.CompareNameHashNoSanityCheck(AnimationStateMachineHashes.StateMachine_GetKilledCombo))
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				base.Animator.SetCurrentReactionType(ReactionType.Invalid);
				base.Animator.SetCurrentAttackMeleeType(AttackMeleeType.Invalid);
				if (base.Participant.IsInCombat)
				{
					base.Participant.ExitCombat();
				}
				m_playedAnimation = true;
			}
		}
	}
}
