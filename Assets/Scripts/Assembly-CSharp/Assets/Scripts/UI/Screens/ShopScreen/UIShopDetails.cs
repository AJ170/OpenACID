using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Tracking;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.Helpers.Shop;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager;
using Assets.Scripts.UI.State.EventManager.Data;
using Assets.Scripts.UI.State.ProfileManager;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.ShopScreen
{
	public class UIShopDetails : UIScreenChild
	{
		public class DisabledShopItems
		{
			public string ShopItemId;

			public float DisabledDelay;
		}

		public static string s_flipOnFocusId = string.Empty;

		public static bool s_itemAutoFocusSuccessful;

		private static readonly StringId s_shopTitleName = 670;

		private static int s_setFocusOnCategoryOnActivate;

		private static string s_setFocusOnItemOnActivate;

		[SerializeField]
		private UIExtendedLabel m_shopBranchName;

		[SerializeField]
		private UIShopScrollers m_shopScrollers;

		[SerializeField]
		private UIWidget m_shopBranchContainer;

		[SerializeField]
		private UIWidget m_heroicBranchContainer;

		[HideInInspector]
		public List<EventDelegate> OnDirectPurchasesItemUpdated = new List<EventDelegate>();

		[HideInInspector]
		public List<EventDelegate> OnItemAutoFocused = new List<EventDelegate>();

		[HideInInspector]
		public List<EventDelegate> OnShopCategoryChanged = new List<EventDelegate>();

		private ShopCategoryNode m_rootCategoryContainer;

		private ShopCategoryNode m_currentCategoryContainer;

		private ShopCategoryNode m_nextCategoryContainer;

		private ShopCategoryNode m_categoryAfterProductPricingsRecived;

		private UIButtonHeader m_shopMenuButton;

		private UIBuyShopItemHelper m_buyShopItemHelper;

		private ShopBaseNode m_setFocusOnItemAfterPriceUpdate;

		private bool m_waitingForPurchaseEnd;

		private bool m_reloadShopOnLateUpdate;

		private bool m_showShopChangedAlertMsg;

		private ProductPricings m_productPricings;

		private Id<TrackPoint> m_currentTrack = Id<TrackPoint>.Invalid;

		private readonly List<DisabledShopItems> m_blockedShopItems = new List<DisabledShopItems>();

		public UIShopCategoriesScroller CategoryScroller
		{
			get
			{
				return m_shopScrollers.CategoryScroller;
			}
		}

		public UIShopItemScroller ItemScroller
		{
			get
			{
				return m_shopScrollers.ItemScroller;
			}
		}

		public List<UIShopBaseItem> CategoryItems
		{
			get
			{
				return m_shopScrollers.CategoryScroller.Items;
			}
		}

		public ShopCategoryNode CategoryRootNode
		{
			get
			{
				if (m_rootCategoryContainer == null)
				{
					ShopTable shopTable = Singleton<StateManager>.Instance.TemplateManager.ShopTable;
					m_rootCategoryContainer = shopTable.GetShopCatalog();
				}
				return m_rootCategoryContainer;
			}
		}

		public ShopCategoryNode CurrentCategoryContainer
		{
			get
			{
				return m_currentCategoryContainer;
			}
		}

		public ShopCategoryNode CategoryAfterProductPricingsRecived
		{
			get
			{
				return m_categoryAfterProductPricingsRecived;
			}
		}

		public ShopCategoryNode NextCategoryContainer
		{
			get
			{
				return m_nextCategoryContainer;
			}
			set
			{
				m_nextCategoryContainer = value;
				OnChangingCategory(m_nextCategoryContainer);
			}
		}

		public UIBuyShopItemHelper BuyShopItemHelper
		{
			get
			{
				return m_buyShopItemHelper;
			}
		}

		public void OpenAndSetFocusOnItem(ShopBaseNode _node, bool _openOnEnter = false)
		{
			ShopItemNode shopItemNode = (ShopItemNode)_node;
			string text = ((shopItemNode == null) ? string.Empty : shopItemNode.ShopBundle.id);
			s_flipOnFocusId = ((!_openOnEnter) ? string.Empty : text);
			if (Singleton<UIScreenManager>.Instance.CurrentScreenId != EScreenID.ShopScreen)
			{
				s_setFocusOnItemOnActivate = text;
				Singleton<UIScreenManager>.Instance.ChangeScreen(EScreenID.ShopScreen, new UITweenHandle(null, null));
			}
			else
			{
				SetFocusOnItem(_node);
			}
		}

		public static void OpenAndSetFocusOnItem(EShopCategory _category)
		{
			if (Singleton<UIScreenManager>.Instance.CurrentScreenId != EScreenID.ShopScreen)
			{
				s_setFocusOnCategoryOnActivate = (int)_category;
				Singleton<UIScreenManager>.Instance.ChangeScreen(EScreenID.ShopScreen, new UITweenHandle(null, null));
				return;
			}
			UIShopDetails shopDetails = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails;
			if (shopDetails != null)
			{
				shopDetails.OpenCategory(_category);
			}
		}

		public void OpenCategory(EShopCategory _category)
		{
			m_shopScrollers.ResetScroller();
			ShopCategoryNode shopCategoryByType = CategoryRootNode.GetShopCategoryByType(_category);
			if (shopCategoryByType != null)
			{
				if (shopCategoryByType.HasDirectPurchasesItem())
				{
					NextCategoryContainer = shopCategoryByType;
					UpdateDirectPurchasesShopList(shopCategoryByType);
				}
				else
				{
					SetCurrentCategory(shopCategoryByType, true);
				}
			}
		}

		public void SetFocusOnItem(ShopBaseNode _node)
		{
			if (_node == null)
			{
				return;
			}
			if (_node.ParentNode != null && _node.DataType == EShopDataType.DirectPurchasItem)
			{
				ShopCategoryNode parentNode = _node.ParentNode;
				if (parentNode != null && parentNode.HasDirectPurchasesItem())
				{
					m_setFocusOnItemAfterPriceUpdate = _node;
					UpdateDirectPurchasesShopList(parentNode);
					return;
				}
			}
			m_shopScrollers.ResetScroller();
			SetCurrentCategory(_node.ParentNode, true);
			int childIndexByNode = CurrentCategoryContainer.GetChildIndexByNode(_node);
			m_shopScrollers.SetFocusOnChildByIndex(childIndexByNode);
			s_itemAutoFocusSuccessful = true;
			EventDelegate.Execute(OnItemAutoFocused);
		}

		public void UpdateDirectPurchasesShopList(ShopCategoryNode _categoryNode)
		{
			m_categoryAfterProductPricingsRecived = _categoryNode;
			UIDirectPurchasesShopHelper directPurchasesShopHelper = UbiCore.Utils.Singleton<UiMain>.Instance.DirectPurchasesShopHelper;
			if (directPurchasesShopHelper.IsWaitingForNetworkCall)
			{
				Singleton<UIScreenManager>.Instance.ShowLoadingOverlay(LoadingOverlayConfiguration.Default);
			}
			else if (m_productPricings == null)
			{
				directPurchasesShopHelper.UpdatePricingList(true, false, UINetworkAdaptor.ShopDirectPurchaseHandler);
			}
			else
			{
				UpdateCurrentCategoryProducts();
			}
		}

		public void SetNextToCurrentCategory()
		{
			if (NextCategoryContainer != null)
			{
				SetCurrentCategory(NextCategoryContainer, false);
			}
		}

		public override void OnBackKey()
		{
			if (m_currentCategoryContainer.Category != CategoryRootNode.Category)
			{
				SetCurrentCategory(CategoryRootNode, true);
			}
			else
			{
				GenericAndroidBackKey.LoadMissionScreen();
			}
		}

		public void SetCurrentCategory(ShopCategoryNode _nextCategory, bool _instant)
		{
			if (m_showShopChangedAlertMsg)
			{
				m_showShopChangedAlertMsg = false;
				UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(6436));
			}
			if (m_currentCategoryContainer != null && m_currentTrack != Id<TrackPoint>.Invalid)
			{
				TrackingManager.CloseShopCategoryTrackPoint(m_currentTrack, m_currentCategoryContainer.Category);
			}
			m_nextCategoryContainer = null;
			m_currentCategoryContainer = _nextCategory;
			m_currentTrack = TrackingManager.OpenShopCategoryTrackPoint(_nextCategory.Category);
			OnChangingCategory(CurrentCategoryContainer);
			m_shopScrollers.UpdateShopLayout(CurrentCategoryContainer, _instant);
			EventDelegate.Execute(OnShopCategoryChanged);
		}

		public void SetDisabledShopItems(string _itemId, float _delay = 3f)
		{
			DisabledShopItems disabledShopItems = GetDisabledShopItems(_itemId);
			if (disabledShopItems == null)
			{
				DisabledShopItems disabledShopItems2 = new DisabledShopItems();
				disabledShopItems2.ShopItemId = _itemId;
				disabledShopItems = disabledShopItems2;
				m_blockedShopItems.Add(disabledShopItems);
			}
			disabledShopItems.DisabledDelay = _delay;
		}

		public DisabledShopItems GetDisabledShopItems(string _itemId)
		{
			for (int i = 0; i < m_blockedShopItems.Count; i++)
			{
				DisabledShopItems disabledShopItems = m_blockedShopItems[i];
				if (disabledShopItems.ShopItemId == _itemId)
				{
					return disabledShopItems;
				}
			}
			return null;
		}

		public void UpdateBlockedShopItems()
		{
			for (int num = m_blockedShopItems.Count - 1; num >= 0; num--)
			{
				DisabledShopItems disabledShopItems = m_blockedShopItems[num];
				disabledShopItems.DisabledDelay -= RealTime.deltaTime;
				if (disabledShopItems.DisabledDelay <= 0f)
				{
					m_blockedShopItems.RemoveAt(num);
				}
			}
		}

		protected override void OnScreenInit()
		{
			m_buyShopItemHelper = new UIBuyShopItemHelper();
			m_buyShopItemHelper.Init();
			EventDelegate.Add(m_screen.onActivate, OnScreenActivate);
			EventDelegate.Add(m_screen.onDeActivate, OnScreenDeActivate);
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			profileManager.onDirectPurchasedVerifyed = (Action<string>)Delegate.Combine(profileManager.onDirectPurchasedVerifyed, new Action<string>(OnDirectPurchasedVerifyed));
			Singleton<UIScreenManager>.Instance.SceneData.ShopDetails = this;
			m_shopScrollers.Init();
		}

		protected override void OnScreenDestroy()
		{
			if (Singleton<UIScreenManager>.Instance.SceneData != null)
			{
				Singleton<UIScreenManager>.Instance.SceneData.ShopDetails = null;
			}
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			profileManager.onDirectPurchasedVerifyed = (Action<string>)Delegate.Remove(profileManager.onDirectPurchasedVerifyed, new Action<string>(OnDirectPurchasedVerifyed));
			if (m_buyShopItemHelper != null)
			{
				m_buyShopItemHelper.Destroy();
			}
			m_buyShopItemHelper = null;
			if ((bool)m_screen)
			{
				EventDelegate.Remove(m_screen.onActivate, OnScreenActivate);
				EventDelegate.Remove(m_screen.onDeActivate, OnScreenDeActivate);
			}
			m_shopScrollers.CategoryScroller.OnScreenDestroy();
		}

		private void OnDestroy()
		{
			s_setFocusOnItemOnActivate = string.Empty;
			OnDirectPurchasesItemUpdated.Clear();
			OnItemAutoFocused.Clear();
			m_rootCategoryContainer = null;
			m_currentCategoryContainer = null;
			m_nextCategoryContainer = null;
			m_categoryAfterProductPricingsRecived = null;
			m_setFocusOnItemAfterPriceUpdate = null;
			m_shopMenuButton = null;
		}

		private void LateUpdate()
		{
			if (m_reloadShopOnLateUpdate)
			{
				m_reloadShopOnLateUpdate = false;
				ReloadShop();
			}
		}

		private void OnShopDiscountEventChanged(params object[] _params)
		{
			UIBuyShopItemHelper buyShopItemHelper = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails.BuyShopItemHelper;
			if (!buyShopItemHelper.IsBuyingItem && buyShopItemHelper.IsShopHelperPopupOpen)
			{
				Resource productCost = buyShopItemHelper.ProductCost;
				Resource finalDiscountedResourceCost = buyShopItemHelper.ShopBundleRef.GetFinalDiscountedResourceCost();
				if (finalDiscountedResourceCost.amount != productCost.amount)
				{
					Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
					UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(5736));
				}
			}
		}

		private void OnShopItemEventChanged(params object[] _params)
		{
			m_reloadShopOnLateUpdate = true;
		}

		private void OnShopCategoryEventChanged(params object[] _params)
		{
			m_reloadShopOnLateUpdate = true;
		}

		private void OnAdvertisementbannerEventChanged(params object[] _params)
		{
			ShopCategoryNode shopCategoryNode = m_nextCategoryContainer ?? m_currentCategoryContainer;
			if (CategoryRootNode == shopCategoryNode)
			{
				m_reloadShopOnLateUpdate = true;
			}
		}

		private void OnHappyHourEventChanged(params object[] _params)
		{
			ShopCategoryNode shopCategoryNode = m_nextCategoryContainer ?? m_currentCategoryContainer;
			if (CategoryRootNode == shopCategoryNode)
			{
				m_reloadShopOnLateUpdate = true;
			}
		}

		private void OnCategoryPayoutIncreasedEventChanged(params object[] _params)
		{
			ShopCategoryNode shopCategoryNode = m_nextCategoryContainer ?? m_currentCategoryContainer;
			if (CategoryRootNode == shopCategoryNode || shopCategoryNode == null || shopCategoryNode.HasDirectPurchasesItem())
			{
				m_reloadShopOnLateUpdate = true;
			}
		}

		private void ReloadShop()
		{
			if (m_waitingForPurchaseEnd)
			{
				return;
			}
			UIBuyShopItemHelper buyShopItemHelper = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails.BuyShopItemHelper;
			if (buyShopItemHelper.IsBuyingItem)
			{
				m_waitingForPurchaseEnd = true;
				buyShopItemHelper.OnBuyProcessEnded = (Action)Delegate.Combine(buyShopItemHelper.OnBuyProcessEnded, new Action(OnBuyProcessEnded));
				return;
			}
			ShopCategoryNode shopCategoryNode = m_nextCategoryContainer ?? m_currentCategoryContainer;
			bool flag = CategoryRootNode == shopCategoryNode;
			EShopCategory category = shopCategoryNode.Category;
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			if (m_nextCategoryContainer != null)
			{
				m_shopScrollers.ClearCallbacks();
			}
			m_rootCategoryContainer = null;
			m_showShopChangedAlertMsg = true;
			if (!flag && CategoryRootNode.GetShopCategoryByType(category) != null)
			{
				OpenCategory(category);
				return;
			}
			SetCurrentCategory(CategoryRootNode, false);
			m_shopScrollers.ResetScroller();
		}

		private void OnBuyProcessEnded()
		{
			UIBuyShopItemHelper buyShopItemHelper = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails.BuyShopItemHelper;
			buyShopItemHelper.OnBuyProcessEnded = (Action)Delegate.Remove(buyShopItemHelper.OnBuyProcessEnded, new Action(OnBuyProcessEnded));
			m_waitingForPurchaseEnd = false;
			m_reloadShopOnLateUpdate = true;
		}

		private void OnScreenActivate()
		{
			GenericBackButton backButton = Singleton<UIScreenManager>.Instance.SceneData.BackButton;
			EventDelegate.Add(backButton.OnBeforeTapBack, OnBeforeTapBack);
			UIButtonHeaderManager buttonHeaderManager = Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.ButtonHeaderManager;
			if (buttonHeaderManager != null)
			{
				m_shopMenuButton = buttonHeaderManager.GetButtonByHeaderType(UIButtonHeader.EHeaderType.Shop);
				EventDelegate.Add(m_shopMenuButton.ScreenChanger.onTrigger, OnShopButtonTriggerd);
			}
			m_rootCategoryContainer = null;
			if (s_setFocusOnCategoryOnActivate > 0)
			{
				m_shopScrollers.HideScrollers();
				OpenAndSetFocusOnItem((EShopCategory)s_setFocusOnCategoryOnActivate);
			}
			else if (!string.IsNullOrEmpty(s_setFocusOnItemOnActivate))
			{
				m_shopScrollers.HideScrollers();
				ShopItemNode shopItemById = CategoryRootNode.GetShopItemById(s_setFocusOnItemOnActivate);
				OpenAndSetFocusOnItem(shopItemById, !string.IsNullOrEmpty(s_flipOnFocusId));
			}
			else
			{
				SetCurrentCategory(CategoryRootNode, false);
			}
			m_shopScrollers.ResetScroller();
			s_setFocusOnItemOnActivate = string.Empty;
			s_setFocusOnCategoryOnActivate = 0;
			EventManager eventManager = Singleton<StateManager>.Instance.EventManager;
			eventManager.GetEventManger(EEventType.Event_Shop_Item_Visibility).OnChanged += new SimpleDelegate.CallbackFunction(OnShopItemEventChanged);
			eventManager.GetEventManger(EEventType.Event_Shop_Category_Visibility).OnChanged += new SimpleDelegate.CallbackFunction(OnShopCategoryEventChanged);
			eventManager.GetEventManger(EEventType.Event_Shop_Category_Percentage).OnChanged += new SimpleDelegate.CallbackFunction(OnShopDiscountEventChanged);
			eventManager.GetEventManger(EEventType.Event_Shop_Item_Percentage).OnChanged += new SimpleDelegate.CallbackFunction(OnShopDiscountEventChanged);
			eventManager.GetEventManger(EEventType.Event_Shop_Ad_Banner).OnChanged += new SimpleDelegate.CallbackFunction(OnAdvertisementbannerEventChanged);
			eventManager.GetEventManger(EEventType.Event_Shop_Hard_Currency_Payout_Increase).OnChanged += new SimpleDelegate.CallbackFunction(OnHappyHourEventChanged);
			eventManager.GetEventManger(EEventType.Event_Shop_Category_Payout_Increase).OnChanged += new SimpleDelegate.CallbackFunction(OnCategoryPayoutIncreasedEventChanged);
			UIDirectPurchasesShopHelper directPurchasesShopHelper = UbiCore.Utils.Singleton<UiMain>.Instance.DirectPurchasesShopHelper;
			directPurchasesShopHelper.OnPricingListUpdated = (Action<ProductPricings>)Delegate.Combine(directPurchasesShopHelper.OnPricingListUpdated, new Action<ProductPricings>(OnPricingListUpdated));
			if (!directPurchasesShopHelper.IsWaitingForNetworkCall)
			{
				directPurchasesShopHelper.UpdatePricingList(true, true, UINetworkAdaptor.ShopDirectPurchaseHandler);
			}
			LocalStorage instance = UbiCore.Utils.Singleton<LocalStorage>.Instance;
			instance.SHOPRemoveAll();
			Array values = Enum.GetValues(typeof(EEventType));
			for (int i = 0; i < values.Length; i++)
			{
				EEventType eEventType = (EEventType)(int)values.GetValue(i);
				EventManagerBase eventManger = eventManager.GetEventManger(eEventType);
				if (eventManger != null)
				{
					ReadOnlyCollection<EventBase> events = eventManger.Events;
					for (int j = 0; j < events.Count; j++)
					{
						instance.SHOPAddValue(eEventType, events[j].CRC32);
					}
				}
			}
		}

		private void OnScreenDeActivate()
		{
			m_buyShopItemHelper.LastShopBundleBeforeShop = null;
			m_currentCategoryContainer = null;
			m_nextCategoryContainer = null;
			m_categoryAfterProductPricingsRecived = null;
			m_setFocusOnItemAfterPriceUpdate = null;
			m_productPricings = null;
			m_rootCategoryContainer = null;
			s_setFocusOnItemOnActivate = string.Empty;
			s_setFocusOnCategoryOnActivate = 0;
			if (m_shopMenuButton != null)
			{
				EventDelegate.Remove(m_shopMenuButton.ScreenChanger.onTrigger, OnShopButtonTriggerd);
			}
			m_shopMenuButton = null;
			if (Singleton<UIScreenManager>.Instance.SceneData != null)
			{
				GenericBackButton backButton = Singleton<UIScreenManager>.Instance.SceneData.BackButton;
				if (backButton != null)
				{
					EventDelegate.Remove(backButton.OnBeforeTapBack, OnBeforeTapBack);
				}
			}
			OnDirectPurchasesItemUpdated.Clear();
			EventManager eventManager = Singleton<StateManager>.Instance.EventManager;
			eventManager.GetEventManger(EEventType.Event_Shop_Item_Visibility).OnChanged -= new SimpleDelegate.CallbackFunction(OnShopItemEventChanged);
			eventManager.GetEventManger(EEventType.Event_Shop_Category_Visibility).OnChanged -= new SimpleDelegate.CallbackFunction(OnShopCategoryEventChanged);
			eventManager.GetEventManger(EEventType.Event_Shop_Category_Percentage).OnChanged -= new SimpleDelegate.CallbackFunction(OnShopDiscountEventChanged);
			eventManager.GetEventManger(EEventType.Event_Shop_Item_Percentage).OnChanged -= new SimpleDelegate.CallbackFunction(OnShopDiscountEventChanged);
			eventManager.GetEventManger(EEventType.Event_Shop_Ad_Banner).OnChanged -= new SimpleDelegate.CallbackFunction(OnAdvertisementbannerEventChanged);
			eventManager.GetEventManger(EEventType.Event_Shop_Hard_Currency_Payout_Increase).OnChanged -= new SimpleDelegate.CallbackFunction(OnHappyHourEventChanged);
			eventManager.GetEventManger(EEventType.Event_Shop_Category_Payout_Increase).OnChanged -= new SimpleDelegate.CallbackFunction(OnCategoryPayoutIncreasedEventChanged);
			if (UbiCore.Utils.Singleton<UiMain>.IsAvailable)
			{
				UIDirectPurchasesShopHelper directPurchasesShopHelper = UbiCore.Utils.Singleton<UiMain>.Instance.DirectPurchasesShopHelper;
				if (directPurchasesShopHelper != null)
				{
					directPurchasesShopHelper.OnPricingListUpdated = (Action<ProductPricings>)Delegate.Remove(directPurchasesShopHelper.OnPricingListUpdated, new Action<ProductPricings>(OnPricingListUpdated));
					directPurchasesShopHelper.Clear();
				}
			}
		}

		private void OnDirectPurchasedVerifyed(string _productid)
		{
			SetDisabledShopItems(_productid, 0f);
			if (!base.gameObject.activeInHierarchy || !m_currentCategoryContainer.HasDirectPurchasesItem())
			{
				return;
			}
			UIShopItemDetails uIShopItemDetails = (UIShopItemDetails)ItemScroller.GetShopBaseItemByHustonId(_productid);
			if (uIShopItemDetails != null)
			{
				if (uIShopItemDetails.ShopBundle.HasWalletPayoutResource())
				{
					Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.WalletHandler.ProfileSyncEnabled = false;
				}
				bool flag = uIShopItemDetails.ShopBundle.HasInventorySpaceForBundle();
				m_buyShopItemHelper.OnPurchasedEvent.Trigger(uIShopItemDetails.ShopItemId, flag, true);
			}
		}

		private void OnChangingCategory(ShopCategoryNode _node)
		{
			m_screen.ShowBackButton = _node.ParentNode != null;
			UIButtonHeader buttonByHeaderType = Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.ButtonHeaderManager.GetButtonByHeaderType(UIButtonHeader.EHeaderType.Shop);
			UIButtonHeaderEvaluator component = buttonByHeaderType.GetComponent<UIButtonHeaderEvaluator>();
			if (component != null)
			{
				component.ForceEvaluat();
			}
			bool flag = _node.Category != EShopCategory.Shopcat_Heroic;
			if (flag)
			{
				StringId nameKey = s_shopTitleName;
				if (_node.NameKey != null)
				{
					nameKey = _node.NameKey;
				}
				m_shopBranchName.LocalizationKey = nameKey;
			}
			m_shopBranchContainer.gameObject.SetActive(flag);
			m_heroicBranchContainer.gameObject.SetActive(!flag);
		}

		private void OnShopButtonTriggerd()
		{
			SetCurrentCategory(CategoryRootNode, false);
		}

		private void OnBeforeTapBack()
		{
			if (m_nextCategoryContainer != null)
			{
				m_shopScrollers.OnTapBack();
			}
			bool flag = false;
			GenericBackButton backButton = Singleton<UIScreenManager>.Instance.SceneData.BackButton;
			if (m_buyShopItemHelper.LastShopBundleBeforeShop != null)
			{
				ShopItemNode shopItemById = CategoryRootNode.GetShopItemById(m_buyShopItemHelper.LastShopBundleBeforeShop.id);
				if (shopItemById != null)
				{
					SetFocusOnItem(shopItemById);
					flag = true;
				}
				m_buyShopItemHelper.LastShopBundleBeforeShop = null;
			}
			if (!flag && CurrentCategoryContainer.ParentNode != null)
			{
				flag = true;
				SetFocusOnItem(CurrentCategoryContainer);
			}
			backButton.StopBackTapEvent = flag;
		}

		private void UpdateCurrentCategoryProducts()
		{
			m_categoryAfterProductPricingsRecived.UpdateAvailableItemsByProductPricings(m_productPricings);
			if (m_categoryAfterProductPricingsRecived.ItemCount == 0)
			{
				SetCurrentCategory(CategoryRootNode, false);
				if (m_setFocusOnItemAfterPriceUpdate != null)
				{
					s_itemAutoFocusSuccessful = false;
					EventDelegate.Execute(OnItemAutoFocused);
				}
				m_setFocusOnItemAfterPriceUpdate = null;
				return;
			}
			if (OnDirectPurchasesItemUpdated.Count > 0)
			{
				EventDelegate.Execute(OnDirectPurchasesItemUpdated);
			}
			else
			{
				SetCurrentCategory(m_categoryAfterProductPricingsRecived, true);
				int focusOnChildByIndex = 0;
				if (m_setFocusOnItemAfterPriceUpdate != null)
				{
					focusOnChildByIndex = CurrentCategoryContainer.GetChildIndexByNode(m_setFocusOnItemAfterPriceUpdate);
				}
				m_shopScrollers.SetFocusOnChildByIndex(focusOnChildByIndex);
			}
			if (m_setFocusOnItemAfterPriceUpdate != null)
			{
				s_itemAutoFocusSuccessful = true;
				EventDelegate.Execute(OnItemAutoFocused);
			}
			m_setFocusOnItemAfterPriceUpdate = null;
			m_categoryAfterProductPricingsRecived = null;
		}

		private void OnPricingListUpdated(ProductPricings _productPricings)
		{
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			m_productPricings = _productPricings;
			if (m_categoryAfterProductPricingsRecived != null)
			{
				UpdateCurrentCategoryProducts();
			}
		}
	}
}
