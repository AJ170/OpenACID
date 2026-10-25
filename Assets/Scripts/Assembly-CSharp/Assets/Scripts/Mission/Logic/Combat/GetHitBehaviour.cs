using Assets.Scripts.Animation;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Combat
{
	public class GetHitBehaviour : CombatBehaviour
	{
		private bool m_playedAnimation;

		private IReactAnimation m_reactAnimation;

		private Vector3 m_reactDirection;

		public override ECombatBehaviourType BehaviourType
		{
			get
			{
				return ECombatBehaviourType.GetHit;
			}
		}

		public GetHitBehaviour(CombatParticipant participant)
			: base(participant)
		{
		}

		public override void Start(params object[] _params)
		{
			base.Start(_params);
			base.Participant.KnowinglyInCombat = true;
			CombatParticipant combatParticipant = (CombatParticipant)_params[0];
			IReactAnimation reactAnimation = (IReactAnimation)_params[1];
			Character character = base.Participant.Character;
			CharacterCondition characterCondition = character.CharacterStateMachines.Cast<ICharacterConditionCarrier>().CharacterCondition;
			if (!characterCondition.IsAnimationAllowed(CharacterCondition.EAnimAllowed.GetHit))
			{
				base.Participant.SwitchBehaviour(ECombatBehaviourType.External);
				return;
			}
			if (character.GetHasZeroHealth())
			{
				base.Participant.SwitchBehaviour(ECombatBehaviourType.Die, combatParticipant, reactAnimation);
				return;
			}
			base.Agent.ResetPath();
			base.Agent.velocity = Vector3.zero;
			base.Participant.LookAtDisable();
			base.Participant.CancelAction();
			base.Participant.AttackWasKillMove = false;
			m_reactDirection = CombatHelper.ReactionDirectionAngle(combatParticipant.Character, base.Participant.Character);
			m_playedAnimation = false;
			if (characterCondition.CurrentStateType == EStateType.CharacterCondition_Knockdowned)
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
			}
			else
			{
				if (m_reactDirection.z < 0f)
				{
					base.Participant.FacePosition(base.Participant.Position + (base.Participant.Position - combatParticipant.Position), 0f);
				}
				m_reactAnimation = reactAnimation;
			}
			PlayReactAnimation();
		}

		public override void End()
		{
			base.End();
			base.Animator.speed = 1f;
			if (!m_playedAnimation)
			{
				base.Animator.SetCurrentReactionType(ReactionType.Invalid);
				base.Animator.SetCurrentAttackMeleeType(AttackMeleeType.Invalid);
			}
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
			return m_playedAnimation;
		}

		private void PlayReactAnimation()
		{
			base.Animator.SetCurrentGeneralState(GeneralState.Combat);
			base.Animator.SetCurrentReactionType(m_reactAnimation.Reaction);
			base.Animator.SetCurrentReactionDirectionX(m_reactDirection.x);
			base.Animator.SetCurrentReactionDirectionZ(m_reactDirection.z);
			base.Animator.SetCurrentMovementState(MovementState.Idle);
			base.Animator.SetCurrentAttackMeleeType(m_reactAnimation.AttackMelee);
			base.Animator.SetCurrentComboAttack(Flag.False);
			m_playedAnimation = false;
		}

		public override void OnAnimatorStateEntered(object _sender, AnimatorStateMachine.StateEventArgs _state)
		{
			for (AnimatorStateMachineStatic.StateMachine parentStateMachine = _state.State.ParentStateMachine; parentStateMachine != null; parentStateMachine = parentStateMachine.ParentStateMachine)
			{
				if (!m_playedAnimation)
				{
					if (parentStateMachine.CompareNameHash(m_reactAnimation.WaitForHash))
					{
						base.Animator.SetCurrentReactionType(ReactionType.Invalid);
						base.Animator.SetCurrentAttackMeleeType(AttackMeleeType.Invalid);
						m_playedAnimation = true;
						break;
					}
				}
				else
				{
					if (parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_CombatWait) || parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_HighLowProfileWait) || parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_CombatStrafe))
					{
						base.Participant.SwitchBehaviour(ECombatBehaviourType.Idle);
						break;
					}
					if (parentStateMachine.CompareNameHash(m_reactAnimation.WaitForHash))
					{
						break;
					}
					if (parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_Reaction))
					{
						base.Participant.SwitchBehaviour(ECombatBehaviourType.External);
						break;
					}
				}
			}
		}
	}
}
