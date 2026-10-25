using System;

public class AnimationPlotEntry<T> : AnimationPlotEntryBase where T : AnimationTaskBase, new()
{
	public AnimationPlotEntry(AnimationTaskParameters _parameters)
		: base(_parameters)
	{
	}

	public override void OnStart()
	{
		base.OnStart();
		base.Parameters.AnimationController.TaskManager.CancelCurrentTaskForced(base.Parameters.Layer);
		base.AnimationTask = (T)Activator.CreateInstance(typeof(T), base.Parameters);
		base.Parameters.AnimationController.TaskManager.AddAnimationTask(base.AnimationTask);
	}
}
