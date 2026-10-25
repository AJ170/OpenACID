using System.Collections.Generic;
using Assets.Scripts.UI.TweenHandling;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours
{
	public class UITweenToggle : MonoBehaviour
	{
		[SerializeField]
		private TweenHandler m_tweenHandlerForward;

		[SerializeField]
		private TweenHandler m_tweenHandlerReverse;

		[SerializeField]
		private UIToggle m_toggle;

		[SerializeField]
		private float m_duration;

		private void Awake()
		{
			List<TweenHandler.TweenData> tweenRefContainer = m_tweenHandlerForward.TweenRefContainer;
			for (int i = 0; i < tweenRefContainer.Count; i++)
			{
				tweenRefContainer[i].Tween.duration = m_duration;
			}
			tweenRefContainer = m_tweenHandlerReverse.TweenRefContainer;
			for (int j = 0; j < tweenRefContainer.Count; j++)
			{
				tweenRefContainer[j].Tween.duration = m_duration;
			}
			m_toggle.optionCanBeNone = true;
		}

		private void OnEnable()
		{
			m_tweenHandlerForward.ResetToBeginning();
			m_tweenHandlerReverse.ResetToBeginning();
			OnToggle();
			EnableListeners();
		}

		private void OnDisable()
		{
			m_tweenHandlerForward.ResetToBeginning();
			m_tweenHandlerReverse.ResetToBeginning();
			DisableListeners();
		}

		private void OnDestroy()
		{
			DisableListeners();
		}

		private void EnableListeners()
		{
			EventDelegate.Add(m_toggle.onChange, OnToggle);
		}

		private void DisableListeners()
		{
			EventDelegate.Remove(m_toggle.onChange, OnToggle);
		}

		private void OnToggle()
		{
			if (m_toggle.value)
			{
				PlayForward();
			}
			else
			{
				PlayBackwards();
			}
		}

		private void PlayForward()
		{
			m_tweenHandlerForward.ResetToBeginning();
			m_tweenHandlerForward.StartTweeningForward();
		}

		private void PlayBackwards()
		{
			m_tweenHandlerReverse.ResetToBeginning();
			m_tweenHandlerReverse.StartTweeningForward();
		}
	}
}
