using AcierProtocol;
using Assets.Scripts.UI.State.EventManager.Data;
using SharedBaseLib.Core;

namespace Assets.Scripts.UI.State.EventManager
{
	public class EventCategoryPercentageManager : EventManagerBase
	{
		public override EEventType ManagerType
		{
			get
			{
				return EEventType.Event_Shop_Category_Percentage;
			}
		}

		public override void ProcessEvent(Event _event)
		{
			EventCategoryPercentage eventCategoryPercentage = new EventCategoryPercentage(_event);
			if (IsContainingChangedEvent(m_oldEvents, eventCategoryPercentage))
			{
				m_hasChanged = true;
			}
			m_Events.Add(eventCategoryPercentage);
		}

		public override void Update()
		{
			bool flag = false;
			for (int i = 0; i < m_Events.Count; i++)
			{
				EventCategoryPercentage eventCategoryPercentage = (EventCategoryPercentage)m_Events[i];
				bool lastValidState = eventCategoryPercentage.LastValidState;
				if (lastValidState != eventCategoryPercentage.Validate())
				{
					flag = true;
				}
			}
			if (flag)
			{
				OnChanged.Trigger(this);
			}
		}

		public BBList<EventCategoryPercentage> GetCategoryPercentageEventsByCategory(EShopCategory _category)
		{
			BBList<EventCategoryPercentage> bBList = new BBList<EventCategoryPercentage>();
			for (int i = 0; i < m_Events.Count; i++)
			{
				EventCategoryPercentage eventCategoryPercentage = (EventCategoryPercentage)m_Events[i];
				if (eventCategoryPercentage.ShopCategory == _category)
				{
					bBList.Add(eventCategoryPercentage);
				}
			}
			return bBList;
		}

		private bool IsContainingChangedEvent(BBList<EventBase> _events, EventCategoryPercentage _event)
		{
			if (_events == null)
			{
				return false;
			}
			for (int i = 0; i < _events.Count; i++)
			{
				EventCategoryPercentage eventCategoryPercentage = (EventCategoryPercentage)_events[i];
				if (eventCategoryPercentage.ShopCategory == _event.ShopCategory && eventCategoryPercentage.Event.startTimeUTC == _event.Event.startTimeUTC && eventCategoryPercentage.Event.endTimeUTC == _event.Event.endTimeUTC && eventCategoryPercentage.Validate() != _event.Validate())
				{
					return true;
				}
			}
			return false;
		}
	}
}
