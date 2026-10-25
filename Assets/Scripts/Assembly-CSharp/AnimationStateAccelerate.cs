using UbiCore.Utils.Extensions;

public class AnimationStateAccelerate : AnimationStateBase
{
	private float m_TimeoutSec = float.PositiveInfinity;

	private float m_StartTimeSec;

	public AnimationStateAccelerate(AnimationController _animationController, float _timeoutSec = float.PositiveInfinity)
		: base(AnimationStateType.Accelerate, _animationController)
	{
		m_TimeoutSec = _timeoutSec;
	}

	public override void OnEnter()
	{
		base.OnEnter();
		m_StartTimeSec = LogicMain.CurrentTime;
	}

	public override void OnDrawGizmos()
	{
	}

	public override void OnUpdate()
	{
		bool flag = AnimationController.Agent.remainingDistance <= AnimationController.StopAnimDistance;
		bool flag2 = LogicMain.CurrentTime - m_StartTimeSec >= m_TimeoutSec;
		bool flag3 = AnimationController.Agent.velocity.magnitude >= AnimationController.Agent.speed - AnimationController.Agent.speed / 100f * 5f;
		bool isOnOffMeshLink = AnimationController.Agent.isOnOffMeshLink;
		bool flag4 = AnimationController.Agent.DistanceToOffMeshLinkStart() <= 1f;
		base.IsFinished = flag4 || flag || flag2 || flag3 || isOnOffMeshLink;
		if (!base.IsFinished)
		{
			AnimationController.Animator.SetCurrentSpeed(AnimationController.GetSpeed(AnimationController.Agent.velocity));
			AnimationController.Animator.SetCurrentBankLeftRight(AnimationController.GetBankAmount(AnimationController.Agent));
			AnimationController.Velocity = AnimationController.Agent.velocity;
		}
	}
}
