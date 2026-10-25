using AcierProtocol;
using Assets.Scripts.UI.State.EventManager.Data;
using Assets.Scripts.UI.Utils.Extensions;

namespace Assets.Scripts.UI.State.EventManager
{
	public class EventShopAdvertisementsManager : EventManagerBase
	{
		public override EEventType ManagerType
		{
			get
			{
				return EEventType.Event_Shop_Ad_Banner;
			}
		}

		public override void ProcessEvent(Event _event)
		{
			EventShopAdvertisement eventShopAdvertisement = new EventShopAdvertisement(_event);
			bool flag = HasInOldEvents(eventShopAdvertisement);
			m_hasChanged |= !flag;
			m_Events.Add(eventShopAdvertisement);
		}

		public bool HasActiveNewsFeedAdvertisements()
		{
			return GetNumberOfActiveBannersInNewsFeed() > 0;
		}

		public bool HasActiveShopAdvertisements()
		{
			return GetNumberOfActiveBannersInShop() > 0;
		}

		public int GetNumberOfActiveBannersInNewsFeed()
		{
			int num = 0;
			for (int i = 0; i < base.Events.Count; i++)
			{
				EventShopAdvertisement eventShopAdvertisement = (EventShopAdvertisement)base.Events[i];
				if (eventShopAdvertisement.CanShowBannerInNewsFeed() && eventShopAdvertisement.Template.IsHappyHourBannerAndAllowedToBeShown())
				{
					num++;
				}
			}
			return num;
		}

		public int GetNumberOfActiveBannersInShop()
		{
			int num = 0;
			for (int i = 0; i < base.Events.Count; i++)
			{
				EventShopAdvertisement eventShopAdvertisement = (EventShopAdvertisement)base.Events[i];
				if (eventShopAdvertisement.CanShowBannerInShop() && eventShopAdvertisement.Template.IsHappyHourBannerAndAllowedToBeShown())
				{
					num++;
				}
			}
			return num;
		}

		public EventShopAdvertisement GetHappyHourAdvertisementByCategory(EShopCategory _category)
		{
			for (int i = 0; i < base.Events.Count; i++)
			{
				EventShopAdvertisement eventShopAdvertisement = (EventShopAdvertisement)base.Events[i];
				if (eventShopAdvertisement.Template.AdvertisementBannerLayoutType != EAdvertisementBannerType.HappyHours || eventShopAdvertisement.CanShowBannerInShop())
				{
					ShopBundle shopProductShopBundle = eventShopAdvertisement.GetShopProductShopBundle();
					if ((shopProductShopBundle != null && shopProductShopBundle.category == _category) || eventShopAdvertisement.Template.ShopCategory == _category)
					{
						return eventShopAdvertisement;
					}
				}
			}
			return null;
		}

		public EventShopAdvertisement GetFirstShopCTAAdvertisementShownInShopByPriority()
		{
			EventShopAdvertisement eventShopAdvertisement = null;
			for (int i = 0; i < base.Events.Count; i++)
			{
				EventShopAdvertisement eventShopAdvertisement2 = (EventShopAdvertisement)base.Events[i];
				if (eventShopAdvertisement2.Template.HighlightWithShopCta && eventShopAdvertisement2.CanShowBannerInShop() && eventShopAdvertisement2.IsShopItemUnlocked())
				{
					if (eventShopAdvertisement2.Template.HasValidShopProductId())
					{
						return eventShopAdvertisement2;
					}
					if (eventShopAdvertisement == null)
					{
						eventShopAdvertisement = eventShopAdvertisement2;
					}
				}
			}
			return eventShopAdvertisement;
		}

		public override void Update()
		{
			bool flag = false;
			for (int i = 0; i < m_Events.Count; i++)
			{
				EventBase eventBase = m_Events[i];
				bool lastValidState = eventBase.LastValidState;
				if (lastValidState != eventBase.Validate())
				{
					flag = true;
				}
			}
			if (flag)
			{
				OnChanged.Trigger(this);
			}
		}
	}
}
