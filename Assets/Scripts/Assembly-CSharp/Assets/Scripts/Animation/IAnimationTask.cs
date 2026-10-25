namespace Assets.Scripts.Animation
{
	public interface IAnimationTask : IAnimation
	{
		AnimationTaskType TaskType { get; }
	}
}
