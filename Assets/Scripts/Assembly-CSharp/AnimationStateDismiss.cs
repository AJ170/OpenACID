using Assets.Scripts.Animation;
using Assets.Scripts.Animation.States;
using UbiCore.Utils.Extensions;

internal class AnimationStateDismiss : AnimationStatePlaySimple
{
	private const float MaxDuration = 0.8f;

	private const string StateName = "Dismiss";

	public const int MinAnimatorNpcId = 1;

	public const int MaxAnimatorNpcId = 5;

	public AnimationStateDismiss(AnimationController _animationController, int[] _npcIds)
		: base(_animationController, new ParameterValues
		{
			ReactionType = ReactionType.AwarenessDismiss,
			AnimatorNpcId = ChooseNpcId(_npcIds)
		}, AnimationStateMachineHashes.StateMachine_AwarenessDismiss, "Dismiss")
	{
	}

	public override void OnEnter()
	{
		base.OnEnter();
		AnimationController.Animator.SetCurrentStopAction(false);
		AnimationController.ApplyRootMotion = true;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (AnimationController.StateMachine.GetCurrentStateMachineHash() == AnimationStateMachineHashes.StateMachine_AwarenessDismiss.NameHash && AnimationController.Animator.NormalizedTime() >= 0.8f)
		{
			base.IsFinished = true;
		}
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
		AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
		AnimationController.Animator.SetCurrentStopAction(true);
		AnimationController.ApplyRootMotion = false;
	}

	public override bool CanCancel()
	{
		return true;
	}

	private static int ChooseNpcId(int[] _npcIds)
	{
		if (_npcIds != null)
		{
			return _npcIds[LogicMain.Random.Rand((uint)(_npcIds.Length - 1))];
		}
		return LogicMain.Random.Rand(1, 5);
	}
}
