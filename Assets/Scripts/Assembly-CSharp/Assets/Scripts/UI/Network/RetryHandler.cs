using System;
using Assets.Scripts.Network.Error;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;

namespace Assets.Scripts.UI.Network
{
	public class RetryHandler : IUIErrorHandler
	{
		private readonly Action m_ok;

		private readonly StringId m_okLocaId;

		public RetryHandler(Action _okCallback = null, int _okLocaId = 0)
		{
			m_ok = _okCallback;
			m_okLocaId = ((!(_okLocaId != StringId.Invalid)) ? 1780 : _okLocaId);
		}

		public void Handle(AcierErrorException _error, UINetworkAdaptor.DataStoreRequest _request, AsyncCallback _callback, bool _isBlocked)
		{
			PopUpButtonConfiguration[] buttons = new PopUpButtonConfiguration[2]
			{
				new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
					Label = LocalizationManager.Instance.Get(m_okLocaId),
					OnButtonPressed = OnOkClicked
				},
				new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
					Label = LocalizationManager.Instance.Get(1199),
					Data = _request,
					OnButtonPressed = delegate
					{
						OnRetryClicked(_request, _callback, _isBlocked);
					}
				}
			};
			PopUpConfiguration defaultErrorBody = UIErrorHelper.GetDefaultErrorBody(_error);
			defaultErrorBody.Buttons = buttons;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(defaultErrorBody);
		}

		private void OnOkClicked(object _)
		{
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			if (m_ok != null)
			{
				m_ok();
			}
		}

		private void OnRetryClicked(UINetworkAdaptor.DataStoreRequest _request, AsyncCallback _callback, bool _isBlocked)
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
}
