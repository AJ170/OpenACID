using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.StateMachines;

namespace Assets.Scripts.Mission.Logic.Combat
{
	public class ExternalBehaviour : CombatBehaviour
	{
		public override ECombatBehaviourType BehaviourType
		{
			get
			{
				return ECombatBehaviourType.External;
			}
		}

		public ExternalBehaviour(CombatParticipant participant)
			: base(participant)
		{
		}

		public override void Start(params object[] _params)
		{
			base.Start(_params);
			base.Participant.EnableParry = false;
			base.Participant.LookAtDisable();
		}

		public override bool Act()
		{
			return false;
		}

		public override bool React(CombatParticipant _source, params object[] _params)
		{
			AnimationTaskBase currentTask = base.AnimationController.TaskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base);
			if (currentTask == null || currentTask.TaskType == AnimationTaskType.Combat || currentTask.CanCancel())
			{
				return true;
			}
			if (base.Participant.Character.GetDeadOrAboutToDie())
			{
				return true;
			}
			CharacterCondition characterCondition = base.Participant.Character.StateMachines.Cast<ICharacterConditionCarrier>().CharacterCondition;
			return characterCondition.CurrentStateType != EStateType.CharacterCondition_Normal;
		}

		public override void Update()
		{
			base.Update();
			if (base.AnimationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) == AnimationTaskType.Combat)
			{
				base.Participant.SwitchBehaviour(ECombatBehaviourType.Idle);
			}
		}
	}
}
