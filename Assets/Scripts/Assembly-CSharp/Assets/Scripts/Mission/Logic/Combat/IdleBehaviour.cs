using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic.States;
using Assets.Scripts.Mission.UI.LogicInterface;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Combat
{
	public class IdleBehaviour : CombatBehaviour
	{
		public override ECombatBehaviourType BehaviourType
		{
			get
			{
				return ECombatBehaviourType.Idle;
			}
		}

		public IdleBehaviour(CombatParticipant participant)
			: base(participant)
		{
		}

		public override void Start(params object[] _params)
		{
			base.Start(_params);
			base.Participant.KnowinglyInCombat = true;
			base.Animator.SetCurrentGeneralState(GeneralState.Combat);
			base.Animator.SetCurrentMovementState(MovementState.Idle);
			base.Animator.SetCurrentReactionType(ReactionType.Invalid);
			base.Animator.SetCurrentAttackMeleeType(AttackMeleeType.Invalid);
			base.Animator.SetCurrentComboAttack(Flag.False);
			base.Participant.TryUnsheathMelee();
			if (base.Participant.IsPlayer)
			{
				base.Participant.Character.Buffs.RemoveBuffs(EBuffType.InitAnonymousBonus, 1);
			}
			base.Agent.ResetPath();
		}

		public override void Update()
		{
			if (!TryFleeFromCombat())
			{
				base.Participant.TryUnsheathMelee();
				FindNextTarget();
				ApproachNextTarget();
				Strafe();
				base.Participant.FollowAgent();
			}
		}

		private bool TryFleeFromCombat()
		{
			if (base.Participant.IsPlayer || !base.Participant.WantsToFlee || !base.CombatController.IsFleeingAllowed)
			{
				return false;
			}
			CombatHelper.EscapeKillTokenChance();
			CombatHelper.StartFleeing(base.Participant.Character, FleeingConditionState.Reason.Fleeing);
			base.Participant.ExitCombat();
			return true;
		}

		public override bool Act()
		{
			if (!base.Participant.IsWeaponUnsheathed())
			{
				return false;
			}
			CombatParticipant nextTarget = base.Participant.NextTarget;
			if (nextTarget == null)
			{
				return false;
			}
			if (base.CombatController.PlayerExitCombatMode != 0)
			{
				return false;
			}
			bool result = false;
			switch (base.Participant.ActionQueuedType)
			{
			case CombatParticipant.EActionQueuedType.Attack:
				result = HandleAttack();
				break;
			case CombatParticipant.EActionQueuedType.Taunt:
				result = HandleTaunt();
				break;
			}
			return result;
		}

		private bool HandleAttack()
		{
			if (!base.Participant.InAttackDistanceMax() || !base.Participant.InAttackDistanceMin())
			{
				base.Participant.MoveToPosition(base.Participant.DesiredPosition);
				return false;
			}
			if (base.Participant.WarningEndTime > LogicMain.CurrentTime)
			{
				return false;
			}
			if (!base.Participant.NextTarget.ActiveBehaviour.React(base.Participant))
			{
				return false;
			}
			base.Participant.SwitchBehaviour(ECombatBehaviourType.Attack, false);
			return true;
		}

		private bool HandleTaunt()
		{
			bool flag = base.Participant.CanTaunt();
			if (flag)
			{
				base.Participant.SwitchBehaviour(ECombatBehaviourType.Taunt);
			}
			return flag;
		}

		public override bool React(CombatParticipant _source, params object[] _params)
		{
			return true;
		}

		private void FindNextTarget()
		{
			if (base.Participant.NextTarget == null)
			{
				base.CombatController.FindClosestTarget(base.Participant);
			}
		}

		private void ApproachNextTarget()
		{
			if (base.Participant.NextTarget != null)
			{
				if (base.CombatController.EnableSpeedBoost)
				{
					float speedReduceFactor;
					if (base.Participant.IsPlayer)
					{
						speedReduceFactor = base.CombatController.SpeedReduceFactor;
						speedReduceFactor *= 0f - AcierInputHandler.Instance.CombatInterpreter.StrafingFactor;
					}
					else
					{
						speedReduceFactor = base.CombatController.SpeedBoostFactor;
						if (!base.Participant.ActionOrSkillQueued)
						{
							float value = Mathf.InverseLerp(base.CombatController.SpeedBoostThresholdMin, base.CombatController.SpeedBoostThresholdMax, base.Participant.TargetPathDistance);
							speedReduceFactor *= Mathf.Clamp01(value);
						}
					}
					base.Animator.speed = 1f + speedReduceFactor;
				}
				Vector3 position = base.Participant.NextTarget.Position;
				base.Participant.FacePosition(position);
				base.Participant.LookAtPosition(base.Participant.NextTarget.PositionEyes);
				if (!base.Participant.IsPlayer && !base.Participant.ActionQueued)
				{
					base.Participant.MoveToPosition(base.Participant.DesiredPosition);
				}
			}
			else
			{
				base.Participant.LookAtDisable();
			}
		}

		private bool IsAttackBehaviour(ECombatBehaviourType _type)
		{
			return _type == ECombatBehaviourType.Attack || _type == ECombatBehaviourType.SkillAct;
		}
	}
}
