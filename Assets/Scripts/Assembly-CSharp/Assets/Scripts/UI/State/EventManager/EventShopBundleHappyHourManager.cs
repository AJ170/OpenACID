using AcierProtocol;
using Assets.Scripts.UI.State.EventManager.Data;
using SharedBaseLib.Core;

namespace Assets.Scripts.UI.State.EventManager
{
	public class EventShopBundleHappyHourManager : EventManagerBase
	{
		public override EEventType ManagerType
		{
			get
			{
				return EEventType.Event_Shop_Hard_Currency_Payout_Increase;
			}
		}

		public override void ProcessEvent(Event _event)
		{
			EventShopCategoryHappyHour eventShopCategoryHappyHour = new EventShopCategoryHappyHour(_event);
			bool flag = HasInOldEvents(eventShopCategoryHappyHour);
			m_hasChanged |= !flag;
			m_Events.Add(eventShopCategoryHappyHour);
		}

		public BBList<EventShopCategoryHappyHour> GetHappyHourEvents(string _shopBundleName)
		{
			BBList<EventShopCategoryHappyHour> bBList = new BBList<EventShopCategoryHappyHour>();
			for (int i = 0; i < m_Events.Count; i++)
			{
				EventShopCategoryHappyHour eventShopCategoryHappyHour = (EventShopCategoryHappyHour)m_Events[i];
				if (eventShopCategoryHappyHour.ShopBundleName == _shopBundleName)
				{
					bBList.Add(eventShopCategoryHappyHour);
				}
			}
			return bBList;
		}

		public BBList<EventShopCategoryHappyHour> GetHappyHourEventsbyByCategory(EShopCategory _category)
		{
			BBList<EventShopCategoryHappyHour> bBList = new BBList<EventShopCategoryHappyHour>();
			for (int i = 0; i < m_Events.Count; i++)
			{
				EventShopCategoryHappyHour eventShopCategoryHappyHour = (EventShopCategoryHappyHour)m_Events[i];
				if (eventShopCategoryHappyHour.ShopCategory == _category)
				{
					bBList.Add(eventShopCategoryHappyHour);
				}
			}
			return bBList;
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
