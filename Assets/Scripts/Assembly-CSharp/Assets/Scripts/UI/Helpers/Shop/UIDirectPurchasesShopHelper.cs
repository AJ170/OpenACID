using System;
using AcierProtocol;
using Assets.Scripts.Network;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.Scripts.UI.Utils.Extensions.Shop;

namespace Assets.Scripts.UI.Helpers.Shop
{
	public class UIDirectPurchasesShopHelper
	{
		public Action<ProductPricings> OnPricingListUpdated;

		private bool m_isCheckingItemsShownInShopOnly;

		private bool m_isWaitingforPriceingList;

		private bool m_isExtraBlockerUsed;

		private UIRequestToken m_productPricingsRequestToken;

		public bool IsWaitingForNetworkCall
		{
			get
			{
				return m_productPricingsRequestToken != null;
			}
		}

		public void Clear()
		{
			if (m_productPricingsRequestToken != null)
			{
				m_productPricingsRequestToken.Abandoned = true;
			}
			m_productPricingsRequestToken = null;
		}

		public void UpdatePricingList(bool _checkItemsShownInShopOnly, bool _updateSilently = true, IUIErrorHandler _errorHandler = null)
		{
			if (m_isWaitingforPriceingList && m_isCheckingItemsShownInShopOnly == _checkItemsShownInShopOnly)
			{
				if (!_updateSilently && !m_isExtraBlockerUsed)
				{
					m_isExtraBlockerUsed = true;
					Singleton<UIScreenManager>.Instance.ShowLoadingOverlay(LoadingOverlayConfiguration.Default);
				}
				return;
			}
			if (m_isExtraBlockerUsed)
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			}
			m_isExtraBlockerUsed = false;
			m_isWaitingforPriceingList = true;
			m_isCheckingItemsShownInShopOnly = _checkItemsShownInShopOnly;
			ShopTable directPurchasesShopTable = Singleton<StateManager>.Instance.TemplateManager.DirectPurchasesShopTable;
			string[] productIds = directPurchasesShopTable.GetAllDirectPurchaseProductIds(m_isCheckingItemsShownInShopOnly);
			if (m_productPricingsRequestToken != null)
			{
				m_productPricingsRequestToken.Abandoned = true;
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			}
			if (_updateSilently)
			{
				m_productPricingsRequestToken = UINetworkAdaptor.NetworkRequest(delegate(AsyncCallback _result)
				{
					Singleton<StateManager>.Instance.ProfileManager.GetProductPricings(productIds, _result);
				}, _errorHandler, OnProductPricingsReceived);
			}
			else
			{
				m_productPricingsRequestToken = UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _result)
				{
					Singleton<StateManager>.Instance.ProfileManager.GetProductPricings(productIds, _result);
				}, _errorHandler, OnProductPricingsReceived);
			}
		}

		private void OnProductPricingsReceived(IAsyncResult _asyncResult)
		{
			if (m_isExtraBlockerUsed)
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			}
			m_isCheckingItemsShownInShopOnly = false;
			m_isExtraBlockerUsed = false;
			m_isWaitingforPriceingList = false;
			m_productPricingsRequestToken = null;
			ProductPricings obj = null;
			if (!_asyncResult.IsError())
			{
				AsyncDataResult<ProductPricings> asyncDataResult = (AsyncDataResult<ProductPricings>)_asyncResult;
				obj = asyncDataResult.Result();
			}
			Action<ProductPricings> onPricingListUpdated = OnPricingListUpdated;
			if (onPricingListUpdated != null)
			{
				onPricingListUpdated(obj);
			}
		}
	}
}
