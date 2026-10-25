using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.Tracking;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup.CallToActionPopup;
using Assets.Scripts.UI.Screens.ShopScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager;
using Assets.Scripts.UI.State.EventManager.Data;
using SharedBaseLib.Serialization;
using UbiCore.Utils;

namespace Assets.Scripts.Database
{
	public class CTADefinitionShopEvent : CTADefinitionBase
	{
		public int WaitHours { get; set; }

		public CTADefinitionShopEvent(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			WaitHours = _s.SerializeInt("WaitHours", WaitHours);
		}

		protected override EResult CheckTrigger()
		{
			LocalStorage instance = UbiCore.Utils.Singleton<LocalStorage>.Instance;
			if (!instance.CTACounterCheckTimeDiff(LocalStorage.CTACounterLastShopEvent, WaitHours * 3600))
			{
				return EResult.Proceed;
			}
			EventShopAdvertisementsManager eventShopAdvertisementsManager = (EventShopAdvertisementsManager)Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Shop_Ad_Banner);
			EventShopAdvertisement firstShopCTAAdvertisementShownInShopByPriority = eventShopAdvertisementsManager.GetFirstShopCTAAdvertisementShownInShopByPriority();
			if (firstShopCTAAdvertisementShownInShopByPriority == null)
			{
				return EResult.Proceed;
			}
			instance.CTACounterSetTimestamp(LocalStorage.CTACounterLastShopEvent);
			return EResult.Fullfilled;
		}

		public override void ShowPopup()
		{
			EventShopAdvertisementsManager eventShopAdvertisementsManager = (EventShopAdvertisementsManager)Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Shop_Ad_Banner);
			EventShopAdvertisement firstShopCTAAdvertisementShownInShopByPriority = eventShopAdvertisementsManager.GetFirstShopCTAAdvertisementShownInShopByPriority();
			if (firstShopCTAAdvertisementShownInShopByPriority == null)
			{
				return;
			}
			if (!string.IsNullOrEmpty(firstShopCTAAdvertisementShownInShopByPriority.Template.ShopProductId))
			{
				UIShopDetails shopDetails = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails;
				ShopItemNode shopItemById = shopDetails.CategoryRootNode.GetShopItemById(firstShopCTAAdvertisementShownInShopByPriority.Template.ShopProductId);
				if (shopItemById != null && shopItemById.ShopBundle.bundleValue > 0)
				{
					TrackingManager.OpenShopBundles("CTA", shopItemById.ShopBundle.id);
					EventDelegate.Add(Singleton<UIScreenManager>.Instance.SceneData.ItemBundlePreview.OnCloseButton, OnClosePopup, true);
					EventDelegate.Add(Singleton<UIScreenManager>.Instance.SceneData.ItemBundlePreview.OnAcceptedButton, OnAccepted, true);
					Singleton<UIScreenManager>.Instance.SceneData.ItemBundlePreview.OpenPreviewByShopItem(shopItemById.ShopBundle, LocalizationManager.Instance.Get(7980), string.Empty);
					return;
				}
			}
			EventDelegate.Add(Singleton<UIScreenManager>.Instance.SceneData.EventBundlePreview.OnCloseButton, OnClosePopup, true);
			EventDelegate.Add(Singleton<UIScreenManager>.Instance.SceneData.EventBundlePreview.OnAcceptedButton, OnAccepted, true);
			Singleton<UIScreenManager>.Instance.SceneData.EventBundlePreview.OpenPreviewByEvent(firstShopCTAAdvertisementShownInShopByPriority);
		}

		private void OnClosePopup()
		{
			CallToActionPopup.Proceed(this, false, true);
		}

		private void OnAccepted()
		{
			CallToActionPopup.Proceed(this, true, false);
		}
	}
}
