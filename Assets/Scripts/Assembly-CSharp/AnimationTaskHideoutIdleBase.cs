using Assets.Scripts.Animation;

public abstract class AnimationTaskHideoutIdleBase : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
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

	public AnimationTaskHideoutIdleBase(Parameters _parameters, AnimationTaskType _taskType)
		: base(_parameters)
	{
		sTaskType = _taskType;
		AnimationController.Animator.SetCurrentGeneralState(GeneralState.Default);
		AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
		AnimationController.Animator.SetCurrentGeneralState(GeneralState.HayStack);
	}

	public override bool CanCancel()
	{
		return true;
	}
}
