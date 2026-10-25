using System;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using SharedBaseLib.Core;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Helpers.Shop
{
	public class UIBuyShopItemHelper
	{
		public delegate void PurchaseHandler();

		public delegate void PurchaseEventHandler(PurchaseHandler e);

		public Action OnBuyProcessEnded;

		public Action OnGotoShopEnded;

		public SimpleDelegate OnPurchasedEvent;

		private ShopBundle m_shopBundleRef;

		private bool m_wasSentToInventory;

		private Resource m_productCost;

		private bool m_isInProgressOfBuyingItem;

		private bool m_isBuyingFromShop;

		public ShopBundle LastShopBundleBeforeShop { get; set; }

		public bool IsBuyingItem
		{
			get
			{
				return m_isInProgressOfBuyingItem;
			}
		}

		public bool IsShopHelperPopupOpen
		{
			get
			{
				return m_shopBundleRef != null;
			}
		}

		public Resource ProductCost
		{
			get
			{
				return m_productCost;
			}
		}

		public ShopBundle ShopBundleRef
		{
			get
			{
				return m_shopBundleRef;
			}
		}

		public bool WasLastPurchesSentToInventory
		{
			get
			{
				return m_wasSentToInventory;
			}
		}

		public event PurchaseEventHandler PurchaseEvent;

		public void Init()
		{
			OnPurchasedEvent = new SimpleDelegate(this, new Type[3]
			{
				typeof(string),
				typeof(bool),
				typeof(bool)
			});
		}

		public void Destroy()
		{
			OnPurchasedEvent = null;
			LastShopBundleBeforeShop = null;
		}

		public void BuyItem(ShopBundle _shopBundle, Resource _cost, bool _isBuyingFromShop = true)
		{
			Reset();
			m_isBuyingFromShop = _isBuyingFromShop;
			m_shopBundleRef = _shopBundle;
			m_productCost = _cost;
			TryBuyProduct();
		}

		public void Reset()
		{
			m_shopBundleRef = null;
			m_productCost = null;
			m_wasSentToInventory = false;
		}

		private void TryBuyProduct()
		{
			m_wasSentToInventory = m_shopBundleRef.HasInventorySpaceForBundle();
			if (m_shopBundleRef.IsDirectPurchasesBundle())
			{
				OnClosePopup();
				m_isInProgressOfBuyingItem = true;
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_buy);
				UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _result)
				{
					Singleton<StateManager>.Instance.ProfileManager.PurchaseProductFromShop(m_shopBundleRef.houstonId, _result);
				}, null, OnShopHCItemPurchased);
			}
			else
			{
				UIBuyHelper.DetermineAllowPurchaseShopBundle(m_productCost, m_shopBundleRef, OnPurchaseAllowedCallback, OnGoToShopOnClose, OnPopupCanceled);
			}
		}

		private void OnPopupCanceled()
		{
			Reset();
		}

		private void OnClosePopup()
		{
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
		}

		private void OnGoToShopOnClose()
		{
			LastShopBundleBeforeShop = m_shopBundleRef;
			Action onGotoShopEnded = OnGotoShopEnded;
			if (onGotoShopEnded != null)
			{
				onGotoShopEnded();
			}
		}

		private void OnPurchaseAllowedCallback()
		{
			m_isInProgressOfBuyingItem = true;
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_buy);
			if (this.PurchaseEvent != null)
			{
				this.PurchaseEvent(OnPurchase);
			}
			else
			{
				OnPurchase();
			}
		}

		private void OnPurchase()
		{
			if (m_shopBundleRef.HasWalletPayoutResource())
			{
				Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.WalletHandler.ProfileSyncEnabled = false;
			}
			Resources cost = new Resources();
			cost.resources = cost.resources.Add(m_productCost);
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _result)
			{
				Singleton<StateManager>.Instance.ProfileManager.PurchaseInGameShopItem(cost, m_shopBundleRef.id, _result);
			}, null, OnShopItemPurchased);
		}

		private void OnShopHCItemPurchased(IAsyncResult _asyncResult)
		{
			m_isInProgressOfBuyingItem = false;
			if (!_asyncResult.IsError())
			{
				Singleton<UIScreenManager>.Instance.SceneData.ShopDetails.SetDisabledShopItems(m_shopBundleRef.id);
			}
			Action onBuyProcessEnded = OnBuyProcessEnded;
			if (onBuyProcessEnded != null)
			{
				onBuyProcessEnded();
			}
			Reset();
		}

		private void OnShopItemPurchased(IAsyncResult _asyncResult)
		{
			m_isInProgressOfBuyingItem = false;
			UIWalletHandler walletHandler = Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.WalletHandler;
			if (!_asyncResult.IsError())
			{
				if (m_productCost != null && !walletHandler.ProfileSyncEnabled)
				{
					walletHandler.SubtractAmount(m_productCost.type, m_productCost.amount);
				}
				if (OnPurchasedEvent != null)
				{
					OnPurchasedEvent.Trigger(m_shopBundleRef.id, m_wasSentToInventory, m_isBuyingFromShop);
				}
			}
			else
			{
				walletHandler.ProfileSyncEnabled = true;
			}
			Action onBuyProcessEnded = OnBuyProcessEnded;
			if (onBuyProcessEnded != null)
			{
				onBuyProcessEnded();
			}
			Reset();
		}
	}
}
