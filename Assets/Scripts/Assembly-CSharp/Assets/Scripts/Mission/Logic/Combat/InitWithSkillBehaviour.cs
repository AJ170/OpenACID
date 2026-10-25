using Assets.Scripts.Animation;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Combat
{
	public class InitWithSkillBehaviour : CombatBehaviour
	{
		private int m_startTurn;

		public Enum<SkillTypes> QueuedSkillType { get; set; }

		public override ECombatBehaviourType BehaviourType
		{
			get
			{
				return ECombatBehaviourType.InitWithSkill;
			}
		}

		public InitWithSkillBehaviour(CombatParticipant _participant)
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
			base.Participant.TryUnsheathMelee();
			base.Agent.ResetPath();
			QueuedSkillType = Enum<SkillTypes>.Invalid;
			m_startTurn = LogicMain.CurrentTurn;
		}

		public override void Update()
		{
			if (base.CombatController.PlayerExitCombatMode != 0)
			{
				base.Participant.SwitchBehaviour(ECombatBehaviourType.Idle);
			}
			else if (base.Participant.NextTarget == null || QueuedSkillType == Enum<SkillTypes>.Invalid)
			{
				base.Participant.SwitchBehaviour(ECombatBehaviourType.InitAttack);
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
			bool flag = base.Participant.IsWeaponUnsheathed();
			flag &= !CombatHelper.TargetNeedsWeaponForSkill(QueuedSkillType) || base.Participant.NextTarget.IsWeaponUnsheathed();
			if (LogicMain.CurrentTurn > m_startTurn && flag)
			{
				base.Participant.SwitchBehaviour(ECombatBehaviourType.Idle);
				AssassinCharacter assassinCharacter = base.Participant.Character.Cast<AssassinCharacter>();
				assassinCharacter.Skills.Trigger(QueuedSkillType, new EntityIdSkillPayload(base.Participant.NextTarget.Character.Id));
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
	}
}
