using System;
using Assets.Scripts.Network.Error;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.ShopScreen;

namespace Assets.Scripts.UI.Network
{
	public class ShopDirectPurchaseHandler : IUIErrorHandler
	{
		public void Handle(AcierErrorException _error, UINetworkAdaptor.DataStoreRequest _request, AsyncCallback _callback, bool _isBlocked)
		{
			if (Singleton<UIScreenManager>.Instance.CurrentScreenId == EScreenID.ShopScreen)
			{
				UIShopDetails shopDetails = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails;
				if (shopDetails.CategoryAfterProductPricingsRecived == null || !shopDetails.CategoryAfterProductPricingsRecived.HasDirectPurchasesItem())
				{
					return;
				}
			}
			UINetworkAdaptor.DefaultHandler.Handle(_error, _request, _callback, _isBlocked);
		}
	}
}
