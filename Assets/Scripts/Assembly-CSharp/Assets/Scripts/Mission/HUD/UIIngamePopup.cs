using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Ingame.Behaviours;
using Assets.Scripts.UI.Localization;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.HUD
{
	public class UIIngamePopup : UIBase
	{
		private const float c_closeInputWait = 0.5f;

		private bool m_waitForTap;

		private UIIngamePopupBehavior m_uiIngamePopupBehavior;

		private InfoEvent.ETextAnnotationMode m_mode;

		private float m_closeInputWaitClock;

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.IngamePopup;
			}
		}

		public override UIBaseBehaviour Details
		{
			get
			{
				return m_uiIngamePopupBehavior;
			}
		}

		public override bool IsVisible
		{
			get
			{
				return m_uiIngamePopupBehavior != null && m_uiIngamePopupBehavior.IsVisible;
			}
		}

		public bool IsFullyVisible
		{
			get
			{
				return m_uiIngamePopupBehavior != null && m_uiIngamePopupBehavior.FullyVisible;
			}
		}

		public bool IsClosedableByGamepad
		{
			get
			{
				return IsVisible && m_mode == InfoEvent.ETextAnnotationMode.PopupOpenConfirm && !OptionsScreenHelper.IsOptionsScreen();
			}
		}

		public override bool IsOnUi(Vector2 _position)
		{
			return m_waitForTap;
		}

		public override void SetVisible(bool _value)
		{
			m_uiIngamePopupBehavior.transform.gameObject.SetActive(_value);
		}

		public override void OnSceneLoaded()
		{
			base.OnSceneLoaded();
			if (m_uiIngamePopupBehavior == null)
			{
				UIBaseBehaviour widgetByType = UbiCore.Utils.Singleton<UIScreenWidgetManager>.Instance.GetWidgetByType(ScreenWidgetType);
				if (widgetByType != null)
				{
					m_uiIngamePopupBehavior = widgetByType as UIIngamePopupBehavior;
				}
				base.IsInitialized = true;
			}
		}

		public override void OnSceneUnload()
		{
			base.OnSceneUnload();
			m_uiIngamePopupBehavior = null;
			m_waitForTap = false;
			base.IsInitialized = false;
		}

		public override void OnMissionLoaded()
		{
			base.OnMissionLoaded();
			if (base.IsInitialized && UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface != null)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.AddListener<InfoEvent>(OnSendInfoEvent);
			}
		}

		public override void OnMissionUnload()
		{
			base.OnMissionUnload();
			if (UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface != null)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.RemoveListener<InfoEvent>(OnSendInfoEvent);
			}
			m_uiIngamePopupBehavior.HidePopups();
		}

		private void OnSendInfoEvent(InfoEvent _infoEvent)
		{
			if (base.HasMissionEnded)
			{
				return;
			}
			InfoEvent.ETextAnnotationMode annotationMode = _infoEvent.AnnotationMode;
			switch (annotationMode)
			{
			case InfoEvent.ETextAnnotationMode.PopupOpenConfirm:
			case InfoEvent.ETextAnnotationMode.PopupOpenExternal:
			{
				string message = UIOnScreenMessageBehaviour.GetMessage(_infoEvent);
				string header = UIOnScreenMessageBehaviour.GetHeader(_infoEvent);
				if (!string.IsNullOrEmpty(message))
				{
					m_waitForTap = true;
					UbiCore.Utils.Singleton<Timer>.Instance.Pause(Timer.EPauseState.PauseDialog);
					if (annotationMode == InfoEvent.ETextAnnotationMode.PopupOpenConfirm)
					{
						m_closeInputWaitClock = 0.5f;
						string button = LocalizationManager.Instance.Get(3791);
						m_uiIngamePopupBehavior.ShowPopup(header, message, button, OnPopupDismissed);
					}
					else
					{
						m_closeInputWaitClock = 0f;
						m_uiIngamePopupBehavior.ShowPopup(header, message, null, null);
					}
					m_mode = annotationMode;
				}
				break;
			}
			case InfoEvent.ETextAnnotationMode.PopupCloseExternal:
				m_closeInputWaitClock = 0f;
				OnPopupDismissed();
				break;
			}
		}

		public void OnPopupDismissed()
		{
			OnPopupDismissed(null);
		}

		private bool OnPopupDismissed(object _userData)
		{
			if (m_closeInputWaitClock > 0f)
			{
				return false;
			}
			m_waitForTap = false;
			m_mode = InfoEvent.ETextAnnotationMode.Invalid;
			UbiCore.Utils.Singleton<Timer>.Instance.Unpause();
			m_uiIngamePopupBehavior.HidePopups();
			return true;
		}

		public override void OnUIAlignmentChanged(bool _isRightHanded)
		{
			base.OnUIAlignmentChanged(_isRightHanded);
			if (m_uiIngamePopupBehavior != null)
			{
				m_uiIngamePopupBehavior.OnUIAlignmentChanged(_isRightHanded);
			}
		}

		public override void UpdateFrame(float _deltaTime)
		{
			base.UpdateFrame(_deltaTime);
			if (m_closeInputWaitClock > 0f && IsFullyVisible)
			{
				m_closeInputWaitClock -= UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime;
			}
		}
	}
}
