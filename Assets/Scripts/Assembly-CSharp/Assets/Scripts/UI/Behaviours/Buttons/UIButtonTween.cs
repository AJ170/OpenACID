using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours.Buttons
{
	[RequireComponent(typeof(UIExtendedButton))]
	public class UIButtonTween : MonoBehaviour
	{
		[SerializeField]
		private BBTargetedTween m_tween;

		private UIExtendedButton m_button;

		protected UIExtendedButton Button
		{
			get
			{
				return m_button;
			}
			private set
			{
				m_button = value;
			}
		}

		private void Awake()
		{
			Button = GetComponent<UIExtendedButton>();
			UpdateConfig();
		}

		private void OnEnable()
		{
			EventDelegate.Add(m_button.onStateChange, OnStateChange);
			EventDelegate.Add(m_button.onVisualsUpdated, UpdateConfig);
			EventDelegate.Add(m_tween.onFinished, OnTweenFinished);
			UpdateConfig();
		}

		private void OnDisable()
		{
			EventDelegate.Remove(m_button.onStateChange, OnStateChange);
			EventDelegate.Remove(m_button.onVisualsUpdated, UpdateConfig);
			EventDelegate.Remove(m_tween.onFinished, OnTweenFinished);
		}

		private void OnStateChange()
		{
			DoOnStateChanged();
		}

		private void OnTweenFinished()
		{
			Button.UpdateVisuals();
			DoOnTweenFinished();
		}

		private void UpdateConfig()
		{
			DoUpdateConfig();
		}

		protected T GetTweenAs<T>() where T : BBTargetedTween
		{
			return m_tween as T;
		}

		protected virtual void DoOnStateChanged()
		{
			if (Button.state == UIButtonColor.State.Pressed)
			{
				m_tween.ResetToBeginning();
				m_tween.PlayForward();
			}
			else
			{
				m_tween.PlayReverse();
			}
		}

		protected virtual void DoOnTweenFinished()
		{
		}

		protected virtual void DoUpdateConfig()
		{
			ButtonConfiguration value;
			if (UIDataMapper.ButtonConfigurations.TryGetValue((int)m_button.Type, out value))
			{
				m_tween.duration = value.TransitionDuration;
			}
		}
	}
}
