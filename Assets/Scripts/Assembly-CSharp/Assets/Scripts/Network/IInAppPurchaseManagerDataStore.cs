using System;
using BB.Mobile.Net;

namespace Assets.Scripts.Network
{
	public interface IInAppPurchaseManagerDataStore
	{
		void GetProductPricings(string[] _productIds, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null, int _callDepth = 5);

		void PurchaseProductFromShop(string _productId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null, int _callDepth = 0);
	}
}
