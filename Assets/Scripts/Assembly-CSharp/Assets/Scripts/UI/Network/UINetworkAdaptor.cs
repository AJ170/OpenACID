using System;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network;
using Assets.Scripts.Network.Error;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Network
{
	public static class UINetworkAdaptor
	{
		public delegate void DataStoreRequest(AsyncCallback _ar);

		private static IUIErrorHandler s_default;

		private static IUIErrorHandler s_retry;

		private static IUIErrorHandler s_forceRetry;

		private static IUIErrorHandler s_blockedHandler;

		private static IUIErrorHandler s_shopDirectPurchaseHandler;

		public static IUIErrorHandler DefaultHandler
		{
			get
			{
				return s_default ?? (s_default = new DefaultHandler());
			}
		}

		public static IUIErrorHandler RetryHandler
		{
			get
			{
				return s_retry ?? (s_retry = new RetryHandler());
			}
		}

		public static IUIErrorHandler ForceRetryHandler
		{
			get
			{
				return s_forceRetry ?? (s_forceRetry = new ForceRetryHandler());
			}
		}

		public static IUIErrorHandler ShopDirectPurchaseHandler
		{
			get
			{
				return s_shopDirectPurchaseHandler ?? (s_shopDirectPurchaseHandler = new ShopDirectPurchaseHandler());
			}
		}

		private static IUIErrorHandler BlockedHandler
		{
			get
			{
				return s_blockedHandler ?? (s_blockedHandler = new UIBlockHandler());
			}
		}

		public static UIRequestToken BlockedNetworkRequest(DataStoreRequest _request, IUIErrorHandler _errorHandler = null, AsyncCallback _callback = null)
		{
			return NetworkRequestImpl(_request, true, _errorHandler, _callback);
		}

		public static UIRequestToken NetworkRequest(DataStoreRequest _request, IUIErrorHandler _errorHandler = null, AsyncCallback _callback = null)
		{
			return NetworkRequestImpl(_request, false, _errorHandler, _callback);
		}

		private static UIRequestToken NetworkRequestImpl(DataStoreRequest _request, bool _blocked, IUIErrorHandler _errorHandler = null, AsyncCallback _callback = null)
		{
			UIRequestToken token = new UIRequestToken();
			if (_blocked)
			{
				Singleton<UIScreenManager>.Instance.ShowLoadingOverlay(LoadingOverlayConfiguration.Default);
			}
			_request(delegate(IAsyncResult _asyncResult)
			{
				AsyncErrorResult asyncErrorResult = _asyncResult as AsyncErrorResult;
				if (_blocked)
				{
					Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
				}
				try
				{
					if (_callback != null && !token.Abandoned)
					{
						_callback(_asyncResult);
					}
				}
				catch (Exception)
				{
				}
				if (asyncErrorResult != null && asyncErrorResult.Exception != null && !token.Abandoned)
				{
					AcierErrorException ex2 = asyncErrorResult.Exception as AcierErrorException;
					if (ex2 != null)
					{
						IUIErrorHandler iUIErrorHandler = _errorHandler;
						if (_errorHandler == null)
						{
							IUIErrorHandler iUIErrorHandler2;
							if (UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialActive && UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsBasicTutorialFinished)
							{
								IUIErrorHandler forceRetryHandler = ForceRetryHandler;
								iUIErrorHandler2 = forceRetryHandler;
							}
							else
							{
								iUIErrorHandler2 = DefaultHandler;
							}
							iUIErrorHandler = iUIErrorHandler2;
						}
						if (ex2.RestartRequired())
						{
							iUIErrorHandler = BlockedHandler;
						}
						iUIErrorHandler.Handle(ex2, _request, _callback, _blocked);
					}
				}
			});
			return token;
		}
	}
}
