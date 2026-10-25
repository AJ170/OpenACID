using Assets.Scripts.Animation;

namespace Assets.Scripts.Mission.Logic.Combat
{
	internal class ParryBehaviour : CombatBehaviour
	{
		private bool m_playedAnimation;

		private IReactAnimation m_reactAnimation;

		public override ECombatBehaviourType BehaviourType
		{
			get
			{
				return ECombatBehaviourType.Parry;
			}
		}

		public ParryBehaviour(CombatParticipant _participant)
			: base(_participant)
		{
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

		public override void Start(params object[] _params)
		{
			base.Start(_params);
			CombatParticipant combatParticipant = (CombatParticipant)_params[0];
			m_reactAnimation = (IReactAnimation)_params[1];
			base.Agent.ResetPath();
			PlayReactAnimation();
			base.Participant.NextTarget = combatParticipant;
			base.Participant.CurrentTarget = combatParticipant;
			base.Participant.LookAtDisable();
			base.Participant.FacePosition(combatParticipant.Position, 0f);
		}

		public override void End()
		{
			base.End();
			base.Animator.SetCurrentComboAttack(Flag.False);
			base.Animator.SetCurrentReactionType(ReactionType.Invalid);
			base.Animator.SetCurrentAttackMeleeType(AttackMeleeType.Invalid);
			base.Animator.speed = 1f;
		}

		public void PlayReactAnimation()
		{
			base.Animator.SetCurrentReactionType(m_reactAnimation.Reaction);
			base.Animator.SetCurrentAttackMeleeType(m_reactAnimation.AttackMelee);
			base.Animator.SetCurrentMovementState(MovementState.Idle);
			base.Animator.SetCurrentComboAttack(Flag.False);
			m_playedAnimation = false;
		}

		public override void OnComboEvent(AnimatorEvent _event)
		{
			base.OnComboEvent(_event);
			if (base.Participant.ActionQueued && base.Participant.NextTarget != null && (base.CombatController.EnableComboChangeTarget || base.Participant.NextTarget == base.Participant.CurrentTarget))
			{
				base.Participant.ConsumeAction();
				base.Participant.SwitchBehaviour(ECombatBehaviourType.Attack, true);
			}
			else
			{
				base.Animator.SetCurrentComboAttack(Flag.False);
			}
		}

		public override void OnAnimatorStateEntered(object _sender, AnimatorStateMachine.StateEventArgs _state)
		{
			for (AnimatorStateMachineStatic.StateMachine parentStateMachine = _state.State.ParentStateMachine; parentStateMachine != null; parentStateMachine = parentStateMachine.ParentStateMachine)
			{
				if (parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_ParryDeflect))
				{
					base.Animator.SetCurrentReactionType(ReactionType.Invalid);
					base.Animator.SetCurrentAttackMeleeType(AttackMeleeType.Invalid);
					base.Animator.SetCurrentComboAttack(Flag.Unset);
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
	}
}
