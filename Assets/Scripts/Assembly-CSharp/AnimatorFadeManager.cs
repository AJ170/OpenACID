using Assets.Scripts.Animation;
using SharedBaseLib.Core;
using UnityEngine;

public class AnimatorFadeManager
{
	private class Transition
	{
		public float Duration { get; private set; }

		public float NormalizedTime { get; private set; }

		public Transition(float _duration, float _normalizedTime)
		{
			Duration = _duration;
			NormalizedTime = _normalizedTime;
		}
	}

	private class TransitionFoot : Transition
	{
		public int StateLeft { get; private set; }

		public int StateRight { get; private set; }

		public TransitionFoot(float _duration, float _normalizedTime, int _stateLeft, int _stateRight)
			: base(_duration, _normalizedTime)
		{
			StateLeft = _stateLeft;
			StateRight = _stateRight;
		}
	}

	private static readonly BBDictionary<int, Transition> Transitions;

	static AnimatorFadeManager()
	{
		Transitions = new BBDictionary<int, Transition>();
		Transitions.Add(AnimationStateHashes.State_Stunned.NameHash, new TransitionFoot(0.1f, 0f, AnimationStateHashes.State_Stunned_Footl.NameHash, AnimationStateHashes.State_Stunned_Footr.NameHash));
	}

	public static void GoInstant(Animator _animator, int _stateHash, int _layer)
	{
		_animator.Play(_stateHash, _layer);
	}

	public static void GoFade(Animator _animator, int _stateHash, int _layer)
	{
		Transition value;
		if (Transitions.TryGetValue(_stateHash, out value))
		{
			TransitionFoot transitionFoot = value as TransitionFoot;
			if (transitionFoot != null)
			{
				_stateHash = ((!(_animator.GetFloat(AnimatorParameters.FootRfront) < 0.5f)) ? transitionFoot.StateRight : transitionFoot.StateLeft);
			}
			Debug.Log("CrossFade:" + _animator.name);
			_animator.CrossFade(_stateHash, value.Duration, _layer, value.NormalizedTime);
		}
	}
}
