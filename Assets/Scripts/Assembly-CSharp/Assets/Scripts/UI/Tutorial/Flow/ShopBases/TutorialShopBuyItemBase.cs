using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.ShopScreen;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.ShopBases
{
	public abstract class TutorialShopBuyItemBase : TutorialFlowBase
	{
		[SerializeField]
		private FullscreenBlocker m_blocker;

		[SerializeField]
		private FullscreenBlocker m_itemsBlocker;

		[SerializeField]
		private UISlantedPopup m_textPopup;

		[SerializeField]
		private UIShopDetails m_shopDetails;

		protected UIShopItemDetails m_shopItemDetails;

		private UIDragScrollView m_shopItemDrag;

		protected void Awake()
		{
		}

		public override void Activate()
		{
			base.Activate();
			ActivateMenuBarBlockers(true);
			UnblockOptionsButton(m_blocker);
			m_textPopup.SetHeaderTextLocalized(GetHeaderText());
			m_textPopup.SetDescriptionTextLocalized(GetDescriptionText());
			m_textPopup.MoveIn();
			m_itemsBlocker.gameObject.SetActive(true);
			m_blocker.gameObject.SetActive(true);
			m_blocker.IncrementDepth();
			Singleton<UIScreenManager>.Instance.SceneData.ShopDetails.BuyShopItemHelper.OnPurchasedEvent += new SimpleDelegate.CallbackFunction(OnPurchased);
			m_shopItemDetails = (UIShopItemDetails)m_shopDetails.ItemScroller.GetShopBaseItemById(GetItemToBuy());
			UIWidget component = m_shopItemDetails.gameObject.GetComponent<UIWidget>();
			m_shopItemDrag = component.GetComponent<UIDragScrollView>();
			m_shopItemDrag.DragEnabled = false;
			UIWidget[] componentsInChildren = m_shopItemDetails.BuyButton.GetComponentsInChildren<UIWidget>(true);
			m_itemsBlocker.SetNonBlocked(componentsInChildren);
			m_itemsBlocker.IncrementDepth();
			CreateButtonHighlight(m_shopItemDetails.BuyButton, Vector3.zero);
			ActivateBackButton(false);
		}

		protected abstract string GetItemToBuy();

		public override void Deactivate()
		{
			if (IsActive)
			{
				base.Deactivate();
				ActivateMenuBarBlockers(false);
				Singleton<UIScreenManager>.Instance.SceneData.ShopDetails.BuyShopItemHelper.OnPurchasedEvent -= new SimpleDelegate.CallbackFunction(OnPurchased);
				m_shopItemDrag.DragEnabled = true;
				m_textPopup.MoveOut();
				m_itemsBlocker.gameObject.SetActive(false);
				m_itemsBlocker.DecrementDepth();
				m_blocker.gameObject.SetActive(false);
				m_blocker.DecrementDepth();
				ActivateBackButton(true);
			}
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			UISceneData sceneData = Singleton<UIScreenManager>.Instance.SceneData;
			if (sceneData != null && sceneData.ShopDetails != null && sceneData.ShopDetails.BuyShopItemHelper != null)
			{
				sceneData.ShopDetails.BuyShopItemHelper.OnPurchasedEvent -= new SimpleDelegate.CallbackFunction(OnPurchased);
			}
			m_shopItemDrag = null;
			m_shopItemDetails = null;
		}

		private void OnPurchased(object[] _params)
		{
			Singleton<UIScreenManager>.Instance.SceneData.ShopDetails.BuyShopItemHelper.OnPurchasedEvent -= new SimpleDelegate.CallbackFunction(OnPurchased);
			UIScreenManager instance = Singleton<UIScreenManager>.Instance;
			UIWalletHandler walletHandler = instance.SceneData.UIMenuBars.WalletHandler;
			walletHandler.ResetVirtualAddedAmounts();
			walletHandler.ProfileSyncEnabled = true;
			walletHandler.ForceUpdateResourceType(EResourceType.SoftCurrency);
			OnPurchasedInternal();
		}

		protected virtual void OnPurchasedInternal()
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.FinishCategory(Category, null);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.ShopScreen;
		}

		public abstract int GetHeaderText();

		public abstract int GetDescriptionText();
	}
}
