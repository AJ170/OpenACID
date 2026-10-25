using AcierProtocol;
using Assets.Scripts.UI.Utils.Extensions.Shop;

namespace Assets.Scripts.UI.State.EventManager.Data
{
	public class EventShopAdvertisement : EventBase
	{
		private readonly EventShopBannerTemplate m_template = new EventShopBannerTemplate();

		public EventShopBannerTemplate Template
		{
			get
			{
				return m_template;
			}
		}

		public EventShopAdvertisement(Event _event)
			: base(_event)
		{
		}

		public bool CanShowBannerInNewsFeed()
		{
			return CanShowBanner() && Template.ShowInNewsFeed;
		}

		public bool CanShowBannerInShop()
		{
			return CanShowBanner() && Template.ShowInShop;
		}

		public bool IsAffectedItemShownInShop()
		{
			return Template.IsAffectedItemShownInShop();
		}

		public ShopBundle GetShopProductShopBundle()
		{
			return Template.GetShopProductShopBundle();
		}

		public bool IsUsingTintableBackground()
		{
			return Template.IsUsingTintableBackground();
		}

		public bool IsShopItemUnlocked()
		{
			return Template.IsShopItemUnlocked();
		}

		protected override void OnProcessEvent(Event _event)
		{
			Template.ProcessEvent(_event.properties);
		}

		private bool CanShowBanner()
		{
			if (base.LastValidState && IsAffectedItemShownInShop())
			{
				ShopBundle shopProductShopBundle = GetShopProductShopBundle();
				if (shopProductShopBundle == null || !shopProductShopBundle.IsAlreadyOwned())
				{
					return true;
				}
			}
			return false;
		}
	}
}
