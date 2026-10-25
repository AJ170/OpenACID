using System;
using AcierProtocol;
using Assets.Scripts.Network.Shop;
using BB.Mobile.Net;

namespace Assets.Scripts.Network
{
	public interface IShopDataStore : INativeShopMessageHandler
	{
		void Update();

		void SetGameObjectName(string _gameObjectName);

		void RecoverPurchases();

		void SetApplicationUsername(string _applicationUsername);

		void FinishTransaction(string _transactionId);

		IAsyncDataResult<ProductPricings> GetProductPricings(string[] _productIds, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void SendReceipt(string _productId, string _transactionId, string _originalTransactionId, string _receiptData, string _currencyCode, string _cost, string _formattedPrice, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void SendGoogleReceipt(string _productId, string _receiptSignature, string _receiptData, string _currencyCode, string _cost, string _formattedPrice, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<VerifyReceiptResponse> BuyProduct(string _productId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<VerifyReceiptResponse> VerifyReceipt(string _transactionId, string _originalTransactionId, string _receiptData, string _currencyCode, string _cost, string _formattedPrice, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<VerifyGoogleReceiptResponse> VerifyGoogleReceipt(string _receiptSignature, string _receiptData, string _currencyCode, string _cost, string _formattedPrice, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);
	}
}
