using System;
using Assets.Scripts.Network.Error;
using Assets.Scripts.UI.ScreenManager;

namespace Assets.Scripts.UI.Network
{
	public class DefaultHandler : IUIErrorHandler
	{
		private readonly EventDelegate.Callback m_callback;

		public DefaultHandler(EventDelegate.Callback _callback = null)
		{
			m_callback = _callback;
		}

		public void Handle(AcierErrorException _error, UINetworkAdaptor.DataStoreRequest _request, AsyncCallback _callback, bool _isBlocked)
		{
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(UIErrorHelper.GetDefaultPopUpConfiguration(_error, m_callback));
		}
	}
}
