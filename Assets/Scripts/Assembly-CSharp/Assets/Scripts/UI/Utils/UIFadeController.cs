using Assets.Scripts.UI.NGUIExtensions.Tweens;
using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	public class UIFadeController : MonoBehaviour
	{
		public enum EState
		{
			On = 0,
			Off = 1
		}

		[SerializeField]
		private BBTweenAlpha m_tween;

		public void Set(float _from, float _to, float _duration)
		{
			m_tween.from = _from;
			m_tween.to = _to;
			m_tween.duration = _duration;
		}

		public void SetState(EState _state, bool _instant)
		{
			switch (_state)
			{
			case EState.On:
				if (_instant)
				{
					m_tween.SetTweenToEnd();
				}
				else
				{
					m_tween.StartTweenPlayForward();
				}
				break;
			case EState.Off:
				if (_instant)
				{
					m_tween.SetTweenToBeginning();
				}
				else
				{
					m_tween.StartTweenPlayReverse();
				}
				break;
			}
		}

		private void Awake()
		{
		}
	}
}
