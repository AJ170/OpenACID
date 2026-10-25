using System;
using System.Collections;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class AnimationExtensions
	{
		public static void SampleAnimation(this UnityEngine.Animation _animation, float _factor01, string _stateName = "")
		{
			_factor01 = Mathf.Clamp01(_factor01);
			foreach (AnimationState item in _animation)
			{
				if (string.IsNullOrEmpty(_stateName) || _stateName == item.name)
				{
					float speed = item.speed;
					item.speed = 0f;
					item.normalizedTime = _factor01;
					item.weight = 1f;
					item.enabled = true;
					_animation.Sample();
					item.enabled = false;
					item.speed = speed;
				}
			}
		}

		public static void UpdateAnimationSpeed(this UnityEngine.Animation _animation, float _speedBoost = 1f)
		{
			foreach (AnimationState item in _animation)
			{
				item.speed = 1f / _speedBoost;
				if (UbiCore.Utils.Singleton<Timer>.IsAvailable && UbiCore.Utils.Singleton<Timer>.Instance.timeScale > 0f)
				{
					item.speed /= UbiCore.Utils.Singleton<Timer>.Instance.timeScale;
				}
			}
		}

		public static void PlayFirstClip(this UnityEngine.Animation _animation)
		{
			IEnumerator enumerator = _animation.GetEnumerator();
			try
			{
				if (enumerator.MoveNext())
				{
					AnimationState animationState = (AnimationState)enumerator.Current;
					_animation.Play(animationState.name);
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}

		public static void RemoveAnimationClip(this UnityEngine.Animation _animation)
		{
			foreach (AnimationState item in _animation)
			{
				_animation.RemoveClip(item.name);
			}
		}
	}
}
