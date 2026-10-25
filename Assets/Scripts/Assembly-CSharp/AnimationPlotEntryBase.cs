public abstract class AnimationPlotEntryBase
{
	public bool Finished { get; private set; }

	public AnimationPlotStateBase Owner { get; set; }

	public AnimationTaskParameters Parameters { get; private set; }

	public AnimationTaskBase AnimationTask { get; protected set; }

	public AnimationPlotEntryBase(AnimationTaskParameters _parameters)
	{
		Parameters = _parameters;
	}

	public virtual void OnStart()
	{
		Parameters.AnimationController.TaskManager.AnimationTaskExited += OnAnimationTaskExited;
	}

	protected void OnAnimationTaskExited(object _sender, AnimationTaskManager.AnimationTaskEventArgs _eventArgs)
	{
		if (!Finished && _eventArgs.AnimationTask == AnimationTask)
		{
			Parameters.AnimationController.TaskManager.AnimationTaskExited -= OnAnimationTaskExited;
			Finished = true;
			Owner.OnEntryFinished(this);
		}
	}
}
