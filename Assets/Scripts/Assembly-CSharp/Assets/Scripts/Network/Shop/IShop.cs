namespace Assets.Scripts.Network.Shop
{
	public interface IShop
	{
		void SetNativeGameObjectName(string _gameObjectName);

		void SetApplicationUsername(string _applicationUsername);

		void RequestProductFromStore(int _requestToken, string[] productIDs, int productCount);

		void BuyProduct(int _requestToken, string _productIDToBuy);

		void RestoreCompletedTransactions();

		void RemoveTransactionFromQueue(string _transactionId);

		void RecoverPurchases();
	}
}
