using System;
using Assets.Scripts.Animation;
using UnityEngine;

public abstract class AnimationTaskSimpleUpperbodyBase : AnimationTaskBase
{
	[Flags]
	public enum EFade
	{
		In = 1,
		Out = 2,
		InOut = 3
	}

	protected ActionTypeUpperBody ActionType { get; private set; }

	protected AnimatorStateMachineStatic.StateHashValue AnimationState { get; private set; }

	protected float FadeInTimeFactor { get; private set; }

	protected float FadeOutTimeFactor { get; private set; }

	protected EFade Fade { get; private set; }

	protected AnimationTaskSimpleUpperbodyBase(AnimationTaskParameters _parameters, ActionTypeUpperBody _actionType, AnimatorStateMachineStatic.StateHashValue _animationState, float _fadeInTimeFactor, float _fadeOutTimeFactor, EFade _fade = EFade.InOut)
		: base(_parameters)
	{
		ActionType = _actionType;
		AnimationState = _animationState;
		float num = _fadeInTimeFactor + _fadeOutTimeFactor;
		if (num > 1f)
		{
			_fadeInTimeFactor /= num;
			_fadeOutTimeFactor /= num;
		}
		FadeInTimeFactor = _fadeInTimeFactor;
		FadeOutTimeFactor = _fadeOutTimeFactor;
		Fade = _fade;
	}

	public override bool CanCancel()
	{
		return false;
	}

	private void OnStateEntered(object _sender, AnimatorStateMachine.StateEventArgs _event)
	{
		if (_event.State.CompareNameHash(AnimationState))
		{
			AnimationController.Animator.SetCurrentActionTypeUpperBody(ActionTypeUpperBody.Invalid);
		}
	}

	private void OnStateExited(object _sender, AnimatorStateMachine.StateEventArgs _event)
	{
		if (_event.State.CompareNameHash(AnimationState))
		{
			base.IsFinished = true;
		}
	}

	public override void OnEnter()
	{
		base.OnEnter();
		AnimationController.StateMachine.StateEntered += OnStateEntered;
		AnimationController.StateMachine.StateExited += OnStateExited;
		if ((Fade & EFade.In) != 0)
		{
			AnimationController.Animator.SetLayerWeight(1, 0f);
		}
		AnimationController.Animator.SetCurrentActionTypeUpperBody(ActionType);
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		Animator animator = AnimationController.Animator;
		AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(1);
		if (animatorStateInfo.fullPathHash != AnimationState.NameHash && animator.IsInTransition(1))
		{
			animatorStateInfo = animator.GetNextAnimatorStateInfo(1);
		}
		if (animatorStateInfo.fullPathHash == AnimationState.NameHash)
		{
			float num = Mathf.Min(animatorStateInfo.length * FadeInTimeFactor, 0.2f);
			float num2 = Mathf.Min(animatorStateInfo.length * FadeOutTimeFactor, 1f);
			float num3 = animatorStateInfo.normalizedTime * animatorStateInfo.length;
			float weight = 1f;
			if (num3 < num)
			{
				weight = (((Fade & EFade.In) == 0) ? 1f : Interpolate.Ease01(Interpolate.EaseType.EaseInQuad, num3 / num));
			}
			else if (num3 > animatorStateInfo.length - num2)
			{
				weight = (((Fade & EFade.Out) == 0) ? 1f : Interpolate.Ease01(Interpolate.EaseType.EaseOutQuad, (animatorStateInfo.length - num3) / num2));
			}
			animator.SetLayerWeight(1, weight);
		}
		else if (Fade == EFade.InOut)
		{
			animator.SetLayerWeight(1, 0f);
		}
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		if ((Fade & EFade.Out) != 0)
		{
			AnimationController.Animator.SetLayerWeight(1, 0f);
		}
	}

	public override void OnDispose()
	{
		base.OnDispose();
		AnimationController.StateMachine.StateEntered -= OnStateEntered;
		AnimationController.StateMachine.StateExited -= OnStateExited;
	}
}
