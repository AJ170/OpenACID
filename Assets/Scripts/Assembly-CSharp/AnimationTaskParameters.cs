public abstract class AnimationTaskParameters
{
	public AnimationTaskType AnimationTaskType { get; private set; }

	public AnimationController AnimationController { get; set; }

	public AnimationTaskManager.ELayer Layer
	{
		get
		{
			return AnimationTaskManager.GetLayer(AnimationTaskType);
		}
	}

	protected AnimationTaskParameters(AnimationTaskType _type)
	{
		AnimationTaskType = _type;
	}
}
