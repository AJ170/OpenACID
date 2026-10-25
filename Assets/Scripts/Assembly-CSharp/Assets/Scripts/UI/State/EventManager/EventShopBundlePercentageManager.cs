using AcierProtocol;
using Assets.Scripts.UI.State.EventManager.Data;
using SharedBaseLib.Core;

namespace Assets.Scripts.UI.State.EventManager
{
	public class EventShopBundlePercentageManager : EventManagerBase
	{
		public override EEventType ManagerType
		{
			get
			{
				return EEventType.Event_Shop_Item_Percentage;
			}
		}

		public override void ProcessEvent(Event _event)
		{
			EventShopBundlePercentage eventShopBundlePercentage = new EventShopBundlePercentage(_event);
			if (!IsContainingChangedEvent(m_oldEvents, eventShopBundlePercentage))
			{
				m_hasChanged = true;
			}
			m_Events.Add(eventShopBundlePercentage);
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

		public BBList<EventShopBundlePercentage> GetShopBundlePercentageEvents(string _shopBundleId)
		{
			BBList<EventShopBundlePercentage> bBList = new BBList<EventShopBundlePercentage>();
			for (int i = 0; i < m_Events.Count; i++)
			{
				EventShopBundlePercentage eventShopBundlePercentage = (EventShopBundlePercentage)m_Events[i];
				if (eventShopBundlePercentage.ShopBundleId == _shopBundleId)
				{
					bBList.Add(eventShopBundlePercentage);
				}
			}
			return bBList;
		}

		private bool IsContainingChangedEvent(BBList<EventBase> _events, EventShopBundlePercentage _event)
		{
			if (_events == null)
			{
				return false;
			}
			for (int i = 0; i < _events.Count; i++)
			{
				EventShopBundlePercentage eventShopBundlePercentage = (EventShopBundlePercentage)_events[i];
				if (eventShopBundlePercentage.ShopBundleId == _event.ShopBundleId && eventShopBundlePercentage.Event.startTimeUTC == _event.Event.startTimeUTC && eventShopBundlePercentage.Event.endTimeUTC == _event.Event.endTimeUTC && eventShopBundlePercentage.Validate() != _event.Validate())
				{
					return true;
				}
			}
			return false;
		}
	}
}
