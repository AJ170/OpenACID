using System;
using Assets.Scripts.Network.Error;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Utils;

namespace Assets.Scripts.UI.Network
{
	public class UIBlockHandler : IUIErrorHandler
	{
		public void Handle(AcierErrorException _error, UINetworkAdaptor.DataStoreRequest _request, AsyncCallback _callback, bool _isBlocked)
		{
			PopUpButtonConfiguration[] buttons = new PopUpButtonConfiguration[1]
			{
				new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
					Label = LocalizationManager.Instance.Get(1780),
					OnButtonPressed = delegate
					{
						UIUtil.QuitApplication();
					}
				}
			};
			PopUpConfiguration defaultErrorBody = UIErrorHelper.GetDefaultErrorBody(_error);
			defaultErrorBody.Buttons = buttons;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(defaultErrorBody);
		}
	}
}
