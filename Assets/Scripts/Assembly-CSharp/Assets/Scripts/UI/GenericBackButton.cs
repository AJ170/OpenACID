using System;
using System.Collections.Generic;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class GenericBackButton : MonoBehaviour
	{
		[SerializeField]
		private UIExtendedButton m_button;

		[SerializeField]
		private EScreenID m_previousScreenID;

		[HideInInspector]
		public List<EventDelegate> OnBeforeTapBack = new List<EventDelegate>();

		public bool StopBackTapEvent { get; set; }

		public void UpdateBackButtonVisibility()
		{
			UIScreen currentScreen = Singleton<UIScreenManager>.Instance.CurrentScreen;
			SetVisibility(currentScreen.ShowBackButton);
			m_previousScreenID = currentScreen.m_previousScreen;
		}

		private void SetVisibility(bool _status)
		{
			m_button.isEnabled = _status;
			base.transform.SetChildrensVisibility(_status);
		}

		private void Start()
		{
			EventDelegate.Add(Singleton<UIScreenManager>.Instance.onScreenChanged, UpdateBackButtonVisibility);
			OptionsMenuNavigation.OverlayMenuClosed = (OptionsMenuNavigation.OptionsMenuClosed)Delegate.Combine(OptionsMenuNavigation.OverlayMenuClosed, new OptionsMenuNavigation.OptionsMenuClosed(UpdateBackButtonVisibility));
		}

		private void OnDestroy()
		{
			OnBeforeTapBack.Clear();
			EventDelegate.Remove(Singleton<UIScreenManager>.Instance.onScreenChanged, UpdateBackButtonVisibility);
			OptionsMenuNavigation.OverlayMenuClosed = (OptionsMenuNavigation.OptionsMenuClosed)Delegate.Remove(OptionsMenuNavigation.OverlayMenuClosed, new OptionsMenuNavigation.OptionsMenuClosed(UpdateBackButtonVisibility));
		}

		private void OnClick()
		{
			EventDelegate.Execute(OnBeforeTapBack);
			EScreenID eScreenID = UIScreenChangerHelper.IsScreenChangeValid(m_previousScreenID);
			if (!StopBackTapEvent && eScreenID != EScreenID.Invalid)
			{
				Singleton<UIScreenManager>.Instance.ChangeScreen(eScreenID, new UITweenHandle(null, null));
			}
			StopBackTapEvent = false;
		}

		public void TriggerGoBack(EScreenID _screenId = EScreenID.Invalid)
		{
			if (_screenId == EScreenID.Invalid)
			{
				_screenId = m_previousScreenID;
			}
			if (!StopBackTapEvent && _screenId != EScreenID.Invalid)
			{
				Singleton<UIScreenManager>.Instance.ChangeScreen(_screenId, new UITweenHandle(null, null));
			}
			StopBackTapEvent = false;
		}
	}
}
