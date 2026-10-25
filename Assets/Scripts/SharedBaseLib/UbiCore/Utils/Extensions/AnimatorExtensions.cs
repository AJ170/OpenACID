using UnityEngine;

namespace UbiCore.Utils.Extensions
{
	public static class AnimatorExtensions
	{
		public static bool IsInState(this Animator _anim, int _state, bool _includeTransition = true, int _layer = 0)
		{
			if (_anim.GetCurrentAnimatorStateInfo(_layer).fullPathHash == _state)
			{
				return true;
			}
			if (_anim.IsInTransition(_layer) && _includeTransition && _anim.GetNextAnimatorStateInfo(_layer).fullPathHash == _state)
			{
				return true;
			}
			return false;
		}

		public static bool IsInTaggedState(this Animator _anim, int _tag, bool _includeTransition = true, int _layer = 0)
		{
			if (_anim.GetCurrentAnimatorStateInfo(_layer).tagHash == _tag)
			{
				return true;
			}
			if (_anim.IsInTransition(_layer) && _includeTransition && _anim.GetNextAnimatorStateInfo(_layer).tagHash == _tag)
			{
				return true;
			}
			return false;
		}

		public static float NormalizedTime(this Animator _anim, int _layer = 0)
		{
			return _anim.GetCurrentAnimatorStateInfo(_layer).normalizedTime;
		}
	}
}
