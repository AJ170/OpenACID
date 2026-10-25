using Assets.Scripts.Animation;
using Assets.Scripts.Animation.States;
using Assets.Scripts.Mission.Logic.Entities;

internal class AnimationStateLureReaction : AnimationStatePlaySimple
{
	private const string StateName = "LureReaction";

	public AnimationStateLureReaction(AnimationController _animationController)
		: base(_animationController, new ParameterValues
		{
			ReactionType = ReactionType.GetAwarenessReaction
		}, AnimationStateMachineHashes.StateMachine_GetAwareness, "LureReaction")
	{
	}

	public override void OnEnter()
	{
		base.OnEnter();
		AnimationController.ApplyRootMotion = true;
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		if (AnimationController.EntityId.TryGetEntity<AssassinCharacter>() != null)
		{
			AnimationController.Animator.SetCurrentIdleType(IdleType.LowProfile);
		}
		else
		{
			AnimationController.Animator.SetCurrentIdleType(IdleType.Neutral);
		}
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
		AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
	}

	public override bool CanCancel()
	{
		return true;
	}
}
