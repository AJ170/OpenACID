using AcierProtocol;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Tracking;
using Assets.Scripts.UI.MVC.Controllers.Banner;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.MissionScreen;
using Assets.Scripts.UI.Screens.ShopScreen.Advertisement;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager;
using Assets.Scripts.UI.State.EventManager.Data;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using Database;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.ShopScreen
{
	public class UIShopAdvertisementDetails : UIShopBaseItem
	{
		private static readonly Color32 s_defaultbackgroundColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		[SerializeField]
		[CheckAssigned]
		private GameObject m_onSelectedTweensGameObject;

		[CheckAssigned]
		[SerializeField]
		private GameObject m_onFadeOutTweensGameObject;

		[CheckAssigned]
		[SerializeField]
		private BBTargetedTween m_pressedTween;

		[SerializeField]
		[CheckAssigned]
		private UITexture m_backgroundTexture;

		[SerializeField]
		[CheckAssigned]
		private UIShopAdStandardDetails m_standardDetails;

		[SerializeField]
		[CheckAssigned]
		private UIHappyHourAdvertisementDetails m_happyHourDetails;

		[CheckAssigned]
		[SerializeField]
		private UIShopAdEventDetails m_eventDetails;

		[CheckAssigned]
		[SerializeField]
		private UIBannerShopAdvertisementController m_bannerShopAdertisementController;

		private bool m_isActive;

		private bool m_isAllowedToBeTapped = true;

		private ShopItemNode m_itemNodeData;

		private EventShopAdvertisement m_shopEvent;

		private IShopAdBaseDetails m_currentActive;

		private EBannerType m_bannerType;

		public override EShopNodeType ShopDataNodeType
		{
			get
			{
				return EShopNodeType.Advertisement;
			}
		}

		public BBTargetedTween[] SelectTweens { get; private set; }

		public BBTargetedTween[] FadeOutTweens { get; private set; }

		public bool IsAllowedToBeTapped
		{
			get
			{
				return m_isAllowedToBeTapped;
			}
			set
			{
				m_isAllowedToBeTapped = value;
			}
		}

		public bool IsActive
		{
			get
			{
				return m_isActive;
			}
			set
			{
				m_isActive = value;
				base.gameObject.SetActive(m_isActive);
			}
		}

		public EBannerType LayoutType { get; set; }

		public void InstantShowItem()
		{
			m_onEnterTweensGameObject.GetComponent<BBTargetedTween>().SetTweenToEnd();
		}

		public override void UpdateData(ShopBaseNode _baseNode)
		{
			ShopHappyHour shopHappyHour = _baseNode as ShopHappyHour;
			if (shopHappyHour != null)
			{
				EventShopAdvertisementsManager eventShopAdvertisementsManager = (EventShopAdvertisementsManager)Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Shop_Ad_Banner);
				EventShopAdvertisement happyHourAdvertisementByCategory = eventShopAdvertisementsManager.GetHappyHourAdvertisementByCategory(_baseNode.ParentNode.Category);
				m_bannerShopAdertisementController.IsAllowedToBeTapped = false;
				UpdateAdvertisementData(happyHourAdvertisementByCategory, EBannerType.Advertisement1X2);
			}
		}

		public void UpdateAdvertisementData(EventShopAdvertisement _shopEvent, EBannerType _bannerType)
		{
			base.UpdateData(null);
			m_shopEvent = _shopEvent;
			m_bannerType = _bannerType;
			EventShopBannerTemplate template = m_shopEvent.Template;
			ActivateColliders(_shopEvent != null && IsAllowedToBeTapped);
			UIShopDetails shopDetails = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails;
			m_itemNodeData = shopDetails.CategoryRootNode.GetShopItemById(template.ShopProductId);
			bool isActive = false;
			bool isActive2 = false;
			bool isActive3 = false;
			if (template.IsHappyHourBanner())
			{
				isActive3 = true;
				m_currentActive = m_happyHourDetails;
			}
			else if (template.AdvertisementBannerLayoutType == EAdvertisementBannerType.DirectPurchase)
			{
				isActive = true;
				m_currentActive = m_eventDetails;
			}
			else
			{
				isActive2 = true;
				m_currentActive = m_standardDetails;
			}
			m_currentActive.UpdateDetails(m_shopEvent, m_bannerShopAdertisementController.LayoutType);
			m_eventDetails.IsActive = isActive;
			m_standardDetails.IsActive = isActive2;
			m_happyHourDetails.IsActive = isActive3;
			UpdateTexture();
			base.MainContainer.gameObject.SetActive(true);
			IsActive = true;
		}

		public void Clear()
		{
			IsActive = false;
			m_itemNodeData = null;
			m_shopEvent = null;
			m_currentActive = null;
		}

		public void GetSelectTargets(out ShopCategoryNode _nextCategory, out ShopBaseNode _selectItem)
		{
			ShopCategoryNode shopCategoryNode = null;
			ShopBaseNode shopBaseNode = null;
			if (m_itemNodeData != null)
			{
				shopCategoryNode = m_itemNodeData.ParentNode;
				shopBaseNode = m_itemNodeData;
			}
			else
			{
				UIShopDetails shopDetails = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails;
				shopCategoryNode = shopDetails.CategoryRootNode.GetShopCategoryByType(m_shopEvent.Template.ShopCategory);
			}
			_nextCategory = shopCategoryNode;
			_selectItem = shopBaseNode;
		}

		protected override void OnAwake()
		{
			SelectTweens = m_onSelectedTweensGameObject.GetComponents<BBTargetedTween>();
			FadeOutTweens = m_onFadeOutTweensGameObject.GetComponents<BBTargetedTween>();
		}

		protected override void OnDestroyItem()
		{
			base.OnDestroyItem();
			Clear();
			FadeOutTweens = null;
			SelectTweens = null;
		}

		protected override void OnItemPress(bool _pressed)
		{
			if (_pressed)
			{
				m_pressedTween.StartTweenPlayForward();
			}
			else
			{
				m_pressedTween.SetTweenToBeginning();
			}
		}

		protected override void OnItemDragStart()
		{
			if (base.IsSelected)
			{
				m_pressedTween.StartTweenPlayReverse();
			}
		}

		protected override void OnItemClicked()
		{
			if (base.IsSelected && Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.ShopEntitlementGUID))
			{
				EventDelegate.Execute(OnSelected);
				OnClickedtoRedirect(m_shopEvent);
				m_bannerShopAdertisementController.OnNavigateToScreen();
			}
		}

		private void Update()
		{
			if (IsActive && m_currentActive != null)
			{
				m_currentActive.UpdateDynamicData();
			}
		}

		private void UpdateTexture()
		{
			Color widgetColor = UIItemUtil.GetWidgetColor(m_shopEvent.Template.GlowLayerColorGameId);
			GameID backgroundGameId = m_shopEvent.Template.GetBackgroundGameId(m_bannerType);
			if (!backgroundGameId.Valid())
			{
				return;
			}
			UIItemUtil.UpdateUITexture(backgroundGameId, m_backgroundTexture);
			Color32 _qualityColor = s_defaultbackgroundColor;
			if (m_shopEvent.Template.IsUsingTintableBackground())
			{
				_qualityColor = widgetColor;
				if (m_itemNodeData != null)
				{
					UIUtil.UpdateShopItemBannerQuality(m_itemNodeData.ShopBundle, ref _qualityColor);
				}
			}
			m_backgroundTexture.color = _qualityColor;
		}

		private static void OnClickedtoRedirect(EventShopAdvertisement _event)
		{
			string triggeredFrom = "ShopBanner";
			UIShopDetails shopDetails = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails;
			ShopItemNode shopItemById = shopDetails.CategoryRootNode.GetShopItemById(_event.Template.ShopProductId);
			Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.Show();
			bool flag = Singleton<UIScreenManager>.Instance.CurrentScreenId == EScreenID.NewsFeedScreen;
			if (flag)
			{
				triggeredFrom = "NewsFeedShopBanner";
				UINotificationPopup subSystemByType = UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UINotificationPopup>();
				subSystemByType.IsShowingNotificationPaused = false;
				UbiCore.Utils.Singleton<UiMain>.Instance.FlowHandler.OnEnterAnimusAtStartup();
				if (Main.Instance.IsNetworkEnabled && !UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialFinished && UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.SetFirstAvailableStep() && Singleton<UIScreenManager>.Instance.CurrentScreen.ScreenId != EScreenID.NewsFeedScreen)
				{
					return;
				}
			}
			ShopBundle shopProductShopBundle = _event.GetShopProductShopBundle();
			CampaignCatalog.Campaign _campaign = null;
			if (shopProductShopBundle != null && shopProductShopBundle.IsAnUnlockCampaignBundle(ref _campaign))
			{
				TrackingManager.OpenShopBundles(triggeredFrom, shopProductShopBundle.id);
				if (flag)
				{
					UIMissionListCampaigns.OpenAndSelectCampaignsItem(_campaign);
				}
				Singleton<UIScreenManager>.Instance.SceneData.UnlockMissionBundlePreview.OpenPreviewByEvent(_campaign);
			}
			else if (shopItemById != null)
			{
				TrackingManager.OpenShopBundles(triggeredFrom, shopItemById.ShopBundle.id);
				EventDelegate.Add(shopDetails.OnItemAutoFocused, delegate
				{
					if (UIShopDetails.s_itemAutoFocusSuccessful)
					{
						ShopItemNode shopItemById2 = shopDetails.CategoryRootNode.GetShopItemById(_event.Template.ShopProductId);
						if (shopItemById2 != null)
						{
							if (shopItemById2.ShopBundle.bundleValue > 0)
							{
								Singleton<UIScreenManager>.Instance.SceneData.ItemBundlePreview.OpenPreviewByShopItem(shopItemById2.ShopBundle, string.Empty, string.Empty);
							}
							else
							{
								Singleton<UIScreenManager>.Instance.SceneData.EventBundlePreview.OpenPreviewByEvent(_event);
							}
						}
					}
				}, true);
				shopDetails.OpenAndSetFocusOnItem(shopItemById, true);
			}
			else
			{
				UIShopDetails.OpenAndSetFocusOnItem(_event.Template.ShopCategory);
			}
		}
	}
}
