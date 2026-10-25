public abstract class AnimationPlotStateBase
{
	public AnimationPlotSequence Owner { get; set; }

	public bool Finished { get; protected set; }

	public AnimationPlotStateBase()
	{
	}

	public virtual void OnEntryFinished(AnimationPlotEntryBase _entry)
	{
	}

	public virtual void OnStart()
	{
	}

	public virtual void OnStop()
	{
	}
}
