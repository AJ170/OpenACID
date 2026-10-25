using AcierProtocol;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class UIButtonHeaderManager : MonoBehaviour
	{
		[SerializeField]
		private UIButtonHeader[] m_bottomBarButtons;

		[SerializeField]
		private UIButtonHeader m_eventButton;

		private UIButtonHeader m_lastButton;

		private UIFlowQueueItem m_onActivateButtonCompleted;

		public UIButtonHeader[] AllBottomBarButtons
		{
			get
			{
				return m_bottomBarButtons;
			}
		}

		public UIButtonHeader LastButton
		{
			get
			{
				return m_lastButton;
			}
		}

		public void ActivateRemote(GameObject _button)
		{
			UIButtonHeader component = _button.GetComponent<UIButtonHeader>();
			if (component != null)
			{
				PressButton(component);
			}
		}

		public void OnPress()
		{
			GameObject gameObject = UIEventTrigger.current.gameObject;
			if (gameObject != null)
			{
				UIButtonHeader component = gameObject.GetComponent<UIButtonHeader>();
				if (component != null)
				{
					PressButton(component);
				}
			}
		}

		public UIButtonHeader GetButtonByHeaderType(UIButtonHeader.EHeaderType _eHeader)
		{
			if (m_eventButton != null && m_eventButton.HeaderType == _eHeader)
			{
				return m_eventButton;
			}
			return m_bottomBarButtons.Find((UIButtonHeader _header) => _header.HeaderType == _eHeader);
		}

		public void ActivateBottomBarButtons(UIFlowQueueItem _onActivateButtonCompleted)
		{
			m_onActivateButtonCompleted = _onActivateButtonCompleted;
			for (int i = 0; i < m_bottomBarButtons.Length; i++)
			{
				UIButtonHeader uIButtonHeader = m_bottomBarButtons[i];
				UIButtonHeaderEvaluator component = uIButtonHeader.GetComponent<UIButtonHeaderEvaluator>();
				EventDelegate.Add(uIButtonHeader.OnUnlockChangeCompleted, OnUnlockChangeCompleted, true);
				component.IsActive = true;
			}
		}

		private void OnUnlockChangeCompleted()
		{
			if (m_onActivateButtonCompleted != null)
			{
				m_onActivateButtonCompleted();
			}
		}

		private void OnDestroy()
		{
			m_lastButton = null;
			for (int i = 0; i < m_bottomBarButtons.Length; i++)
			{
				if (m_bottomBarButtons[i] != null)
				{
					EventDelegate.Remove(m_bottomBarButtons[i].OnUnlockChangeCompleted, OnUnlockChangeCompleted);
				}
			}
		}

		private void Update()
		{
			EScreenID currentScreenId = Singleton<UIScreenManager>.Instance.CurrentScreenId;
			UIButtonHeader.EHeaderType buttonHeaderTypeByScreen = UIButtonHeaderUtil.GetButtonHeaderTypeByScreen(currentScreenId);
			if (buttonHeaderTypeByScreen == UIButtonHeader.EHeaderType.Invalid)
			{
				return;
			}
			if (m_eventButton != null && m_eventButton.HeaderType == buttonHeaderTypeByScreen)
			{
				PressButton(m_eventButton);
				return;
			}
			for (int i = 0; i < m_bottomBarButtons.Length; i++)
			{
				UIButtonHeader uIButtonHeader = m_bottomBarButtons[i];
				if (uIButtonHeader.HeaderType == buttonHeaderTypeByScreen)
				{
					PressButton(uIButtonHeader);
				}
			}
		}

		private void PressButton(UIButtonHeader _nextButton)
		{
			if (!(m_lastButton != null) || m_lastButton.HeaderType != _nextButton.HeaderType)
			{
				if (m_lastButton != null)
				{
					m_lastButton.DeselectButton();
				}
				m_lastButton = _nextButton;
				if (m_lastButton != null)
				{
					m_lastButton.SelectButton();
				}
			}
		}
	}
}
