using System.Security.Cryptography;
using System.Text;
using AcierProtocol;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using UnityEngine;

namespace Assets.Scripts.Network.Shop
{
	public class ShopAndroid : IShop
	{
		private AndroidJavaObject m_bridgeManager = new AndroidJavaObject("com.acid.plugins.hc.ArtBridge");

		private string m_androidGameObjectName = string.Empty;

		private string m_applicationUsername = string.Empty;

		public void SetNativeGameObjectName(string _gameObjectName)
		{
			m_androidGameObjectName = _gameObjectName;
			m_bridgeManager.Call("SetUp", m_androidGameObjectName);
		}

		public void SetApplicationUsername(string _applicationUsername)
		{
			m_applicationUsername = _applicationUsername;
		}

		public void RequestProductFromStore(int _requestToken, string[] _productIDs, int productCount = 0)
		{
			m_bridgeManager.Call("RequestItems", true, _requestToken, _productIDs);
		}

		public void BuyProduct(int _requestToken, string _productIDToBuy)
		{
			m_bridgeManager.Call("BuyProduct", _productIDToBuy, _requestToken, GenerateRandomCargo());
		}

		public void RestoreCompletedTransactions()
		{
			m_bridgeManager.Call("RequestRecover");
		}

		public void RemoveTransactionFromQueue(string _purchaseToken)
		{
			m_bridgeManager.Call("ConsumePurchase", _purchaseToken);
		}

		public void RecoverPurchases()
		{
			ShopTable directPurchasesShopTable = Singleton<StateManager>.Instance.TemplateManager.DirectPurchasesShopTable;
			string[] allDirectPurchaseProductIds = directPurchasesShopTable.GetAllDirectPurchaseProductIds(false);
			if (allDirectPurchaseProductIds != null)
			{
				m_bridgeManager.Call("RequestItems", false, 0, allDirectPurchaseProductIds);
			}
		}

		private string SHA1HashStringForUTF8String(string plainText)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(plainText);
			SHA1CryptoServiceProvider sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();
			byte[] bytes2 = sHA1CryptoServiceProvider.ComputeHash(bytes);
			return HexStringFromBytes(bytes2);
		}

		private string HexStringFromBytes(byte[] bytes)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in bytes)
			{
				string value = b.ToString("x2");
				stringBuilder.Append(value);
			}
			return stringBuilder.ToString();
		}

		private string GenerateRandomCargo()
		{
			string text = "abcdefghijklmnopqrstuvwxyz0123456789#+@&$ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			string text2 = string.Empty;
			for (int i = 0; i <= 9; i++)
			{
				int startIndex = Random.Range(0, text.Length - 1);
				text2 += text.Substring(startIndex, 1);
			}
			return SHA1HashStringForUTF8String(m_applicationUsername + text2);
		}

		private void onDestroy()
		{
			if (m_bridgeManager != null)
			{
				m_bridgeManager.Call("Destroy");
			}
		}
	}
}
