using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;

public abstract class AnimationTaskHideoutEnterBase : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public HideoutObject Hideout { get; set; }

		public InteractionLocators.Locator SelectedLocator { get; set; }

		public Parameters(AnimationTaskType _taskType)
			: base(_taskType)
		{
		}
	}

	public readonly AnimationTaskType sTaskType;

	public override AnimationTaskType TaskType
	{
		get
		{
			return sTaskType;
		}
	}

	public AnimationTaskHideoutEnterBase(Parameters _parameters, AnimationTaskType _taskType)
		: base(_parameters)
	{
		sTaskType = _taskType;
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
	}

	public override bool CanCancel()
	{
		return false;
	}
}
