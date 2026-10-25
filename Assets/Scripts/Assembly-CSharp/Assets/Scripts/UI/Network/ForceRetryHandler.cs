using System;
using Assets.Scripts.Network.Error;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;

namespace Assets.Scripts.UI.Network
{
	public class ForceRetryHandler : IUIErrorHandler
	{
		public void Handle(AcierErrorException _error, UINetworkAdaptor.DataStoreRequest _request, AsyncCallback _callback, bool _isBlocked)
		{
			PopUpButtonConfiguration[] buttons = new PopUpButtonConfiguration[1]
			{
				new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
					Label = LocalizationManager.Instance.Get(1199),
					Data = _request,
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
						if (_isBlocked)
						{
							UINetworkAdaptor.BlockedNetworkRequest(_request, this, _callback);
						}
						else
						{
							UINetworkAdaptor.NetworkRequest(_request, this, _callback);
						}
					}
				}
			};
			PopUpConfiguration defaultErrorBody = UIErrorHelper.GetDefaultErrorBody(_error);
			defaultErrorBody.Buttons = buttons;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(defaultErrorBody);
		}
	}
}
