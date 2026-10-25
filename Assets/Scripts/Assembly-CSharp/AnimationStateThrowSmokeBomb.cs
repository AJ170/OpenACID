using Assets.Scripts.Animation;
using UnityEngine;

public class AnimationStateThrowSmokeBomb : AnimationStateBase
{
	public AnimationStateThrowSmokeBomb(AnimationController _animationController)
		: base(AnimationStateType.ThrowSmokeBomb, _animationController)
	{
		AnimationController.StateMachine.StateEntered += OnStateEntered;
		AnimationController.StateMachine.StateExited += OnStateExited;
	}

	private void OnStateEntered(object sender, AnimatorStateMachine.StateEventArgs _event)
	{
		if (_event.State.CompareNameHash(AnimationStateHashes.State_ThrowSmokeBomb))
		{
			AnimationController.Animator.SetCurrentActionTypeUpperBody(ActionTypeUpperBody.Invalid);
		}
	}

	private void OnStateExited(object sender, AnimatorStateMachine.StateEventArgs _event)
	{
		if (_event.State.CompareNameHash(AnimationStateHashes.State_ThrowSmokeBomb))
		{
			base.IsFinished = true;
		}
	}

	public override void OnEnter()
	{
		base.OnEnter();
		AnimationController.Animator.SetLayerWeight(1, 0f);
		AnimationController.Animator.SetCurrentActionTypeUpperBody(ActionTypeUpperBody.ThrowSmokeBomb);
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		AnimatorStateInfo currentAnimatorStateInfo = AnimationController.Animator.GetCurrentAnimatorStateInfo(1);
		if (currentAnimatorStateInfo.fullPathHash == AnimationStateHashes.State_ThrowSmokeBomb.NameHash)
		{
			float num = 0.3f;
			float num2 = 0f;
			num2 = ((!(currentAnimatorStateInfo.normalizedTime < num)) ? (1f - (currentAnimatorStateInfo.normalizedTime - num) / (1f - num)) : (1f * (currentAnimatorStateInfo.normalizedTime / num)));
			AnimationController.Animator.SetLayerWeight(1, Mathf.Clamp01(num2));
		}
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.Animator.SetLayerWeight(1, 0f);
		AnimationController.StateMachine.StateEntered -= OnStateEntered;
		AnimationController.StateMachine.StateExited -= OnStateExited;
	}
}
