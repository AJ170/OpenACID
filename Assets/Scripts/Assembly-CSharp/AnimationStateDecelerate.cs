public class AnimationStateDecelerate : AnimationStateBase
{
	public AnimationStateDecelerate(AnimationController _animationController)
		: base(AnimationStateType.Decelerate, _animationController)
	{
	}

	public override void OnUpdate()
	{
		base.IsFinished = AnimationController.Agent.velocity.magnitude <= 0f;
		if (!base.IsFinished)
		{
			AnimationController.Velocity = AnimationController.Agent.velocity;
		}
		AnimationController.Animator.SetCurrentSpeed(AnimationController.GetSpeed(AnimationController.Agent.velocity));
	}
}
