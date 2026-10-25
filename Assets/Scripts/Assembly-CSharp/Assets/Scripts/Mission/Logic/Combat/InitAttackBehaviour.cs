using Assets.Scripts.Animation;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Combat
{
	public class InitAttackBehaviour : CombatBehaviour
	{
		public override ECombatBehaviourType BehaviourType
		{
			get
			{
				return ECombatBehaviourType.InitAttack;
			}
		}

		public InitAttackBehaviour(CombatParticipant _participant)
			: base(_participant)
		{
		}

		public override void Start(params object[] _params)
		{
			base.Start(_params);
			base.Participant.TryUnsheathMelee();
			base.Animator.SetCurrentGeneralState(GeneralState.Combat);
		}

		public override void Update()
		{
			if (base.Participant.NextTarget == null && !base.CombatController.FindClosestTarget(base.Participant))
			{
				base.Participant.SwitchBehaviour(ECombatBehaviourType.Idle);
				return;
			}
			Vector3 position = base.Participant.NextTarget.Position;
			base.Participant.FacePosition(position);
			base.Participant.LookAtPosition(base.Participant.NextTarget.PositionEyes);
			if (!base.Participant.InAttackDistanceMax())
			{
				base.Participant.MoveToPosition(base.Participant.DesiredPosition, true);
				Run();
				return;
			}
			CombatHelper.ResetKillTokenDecayClock(base.Participant.Character);
			if (base.Participant.IsWeaponUnsheathed())
			{
				if (base.CombatController.PlayerExitCombatMode != 0)
				{
					base.Participant.SwitchBehaviour(ECombatBehaviourType.Idle);
					return;
				}
				base.Participant.SwitchBehaviour(ECombatBehaviourType.Attack, false);
			}
			else if (base.Participant.TargetPathDistance > base.CombatController.EnemyBestAttackDistance)
			{
				base.Participant.MoveToPosition(base.Participant.DesiredPosition, true);
				Run();
			}
			else
			{
				base.Animator.SetCurrentMovementState(MovementState.Idle);
				base.Animator.SetCurrentReactionType(ReactionType.Invalid);
				base.Animator.SetCurrentAttackMeleeType(AttackMeleeType.Invalid);
				base.Animator.SetCurrentComboAttack(Flag.False);
				base.Agent.ResetPath();
			}
		}

		public override bool React(CombatParticipant _source, params object[] _params)
		{
			return true;
		}
	}
}
