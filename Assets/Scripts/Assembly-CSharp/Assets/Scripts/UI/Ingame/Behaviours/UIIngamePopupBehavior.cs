using Assets.Scripts.UI.Screens.Popup;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours
{
	public class UIIngamePopupBehavior : UIBaseBehaviour
	{
		[SerializeField]
		private UISlantedPopup m_leftPopup;

		[SerializeField]
		private UISlantedPopup m_rightPopup;

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.IngamePopup;
			}
		}

		public void ShowPopup(string _header, string _text, string _button, UISlantedPopup.SlantedPopupEventDelegateWithResult _callback)
		{
			UISlantedPopup uISlantedPopup = m_leftPopup;
			if (base.IsRightHandedAlignment)
			{
				uISlantedPopup = m_rightPopup;
			}
			uISlantedPopup.gameObject.SetActive(true);
			uISlantedPopup.SetHeaderText(_header);
			uISlantedPopup.SetDescriptionText(_text);
			uISlantedPopup.SetButtonText(_button);
			uISlantedPopup.SetCallback(_callback);
			uISlantedPopup.MoveIn();
		}

		public void HidePopups()
		{
			if (m_leftPopup != null && m_leftPopup.gameObject.activeSelf && m_leftPopup.State == UISlantedPopup.EState.OnScreen)
			{
				m_leftPopup.MoveOut();
			}
			if (m_rightPopup != null && m_rightPopup.gameObject.activeSelf && m_rightPopup.State == UISlantedPopup.EState.OnScreen)
			{
				m_rightPopup.MoveOut();
			}
		}

		public override void SetVisibility(bool _visible)
		{
			base.SetVisibility(_visible);
			if (_visible)
			{
				base.gameObject.SetActive(true);
			}
		}

		private void Awake()
		{
			m_leftPopup.OnMoveOutFinished += OnMoveOutFinished;
			m_rightPopup.OnMoveOutFinished += OnMoveOutFinished;
		}

		private void OnDestroy()
		{
			if (m_leftPopup != null)
			{
				m_leftPopup.OnMoveOutFinished -= OnMoveOutFinished;
			}
			if (m_rightPopup != null)
			{
				m_rightPopup.OnMoveOutFinished -= OnMoveOutFinished;
			}
		}

		private void OnMoveOutFinished(UISlantedPopup _popup)
		{
			_popup.gameObject.SetActive(false);
		}
	}
}
