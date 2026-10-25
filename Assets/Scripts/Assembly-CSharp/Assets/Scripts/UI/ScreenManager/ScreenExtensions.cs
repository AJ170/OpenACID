using Assets.Scripts.UI.Screens.Popup;

namespace Assets.Scripts.UI.ScreenManager
{
	public static class ScreenExtensions
	{
		public static void ShowPopUpBareScreen(this UIScreenManager _manager, PopUpConfiguration _config)
		{
			UIPopup uIPopup = _manager.GetScreen(EScreenID.UIPopUpBare) as UIPopup;
			if (uIPopup != null)
			{
				uIPopup.Configure(_config);
				if (_manager.CurrentModalScreen != uIPopup)
				{
					_manager.ShowPopup(uIPopup, uIPopup.InTweener);
				}
			}
		}

		public static void ShowPopUpScreen(this UIScreenManager _manager, PopUpConfiguration _config)
		{
			UIPopup uIPopup = _manager.GetScreen(EScreenID.UIPopUp) as UIPopup;
			if (uIPopup != null)
			{
				uIPopup.Configure(_config);
				if (_manager.CurrentModalScreen != uIPopup)
				{
					_manager.ShowPopup(uIPopup, uIPopup.InTweener);
				}
			}
		}

		public static void ShowLoadingOverlay(this UIScreenManager _manager, LoadingOverlayConfiguration _config)
		{
			UILoadingOverlay uILoadingOverlay = _manager.GetScreen(EScreenID.LoadingOverlay) as UILoadingOverlay;
			if (uILoadingOverlay != null)
			{
				uILoadingOverlay.Configure(_config);
				_manager.ShowPopup(uILoadingOverlay, uILoadingOverlay.InTweener);
			}
		}

		public static bool IsPopupOpen(this UIScreenManager _manager)
		{
			UIScreen currentModalScreen = _manager.CurrentModalScreen;
			return currentModalScreen != null && currentModalScreen.ScreenId == EScreenID.UIPopUp;
		}
	}
}
