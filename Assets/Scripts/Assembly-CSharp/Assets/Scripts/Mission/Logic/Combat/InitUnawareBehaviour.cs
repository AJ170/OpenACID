using Assets.Scripts.Animation;

namespace Assets.Scripts.Mission.Logic.Combat
{
	public class InitUnawareBehaviour : CombatBehaviour
	{
		public override ECombatBehaviourType BehaviourType
		{
			get
			{
				return ECombatBehaviourType.InitUnaware;
			}
		}

		public InitUnawareBehaviour(CombatParticipant _participant)
			: base(_participant)
		{
		}

		public override void Start(params object[] _params)
		{
			base.Start(_params);
			base.Animator.SetCurrentGeneralState(GeneralState.Combat);
			base.Animator.SetCurrentMovementState(MovementState.Idle);
			base.Animator.SetCurrentReactionType(ReactionType.Invalid);
			base.Animator.SetCurrentAttackMeleeType(AttackMeleeType.Invalid);
			base.Animator.SetCurrentComboAttack(Flag.False);
			base.Agent.ResetPath();
		}

		public override void Update()
		{
			ExitBehaviorWhenNotBeingAttacked();
		}

		private void ExitBehaviorWhenNotBeingAttacked()
		{
			CombatParticipant currentTarget = base.Participant.CurrentTarget;
			if (currentTarget == null || currentTarget.IsDead || currentTarget.CurrentTarget != base.Participant || currentTarget.ActiveBehaviour.BehaviourType == ECombatBehaviourType.Idle || currentTarget.ActiveBehaviour.BehaviourType == ECombatBehaviourType.GetHit)
			{
				base.Participant.SwitchBehaviour(ECombatBehaviourType.Idle);
			}
		}
	}
}
