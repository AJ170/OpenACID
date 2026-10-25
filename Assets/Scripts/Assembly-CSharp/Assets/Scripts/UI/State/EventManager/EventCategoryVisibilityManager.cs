using AcierProtocol;
using Assets.Scripts.UI.State.EventManager.Data;

namespace Assets.Scripts.UI.State.EventManager
{
	public class EventCategoryVisibilityManager : EventManagerBase
	{
		public override EEventType ManagerType
		{
			get
			{
				return EEventType.Event_Shop_Category_Visibility;
			}
		}

		public override void ProcessEvent(Event _event)
		{
			EventCategoryVisibility eventCategoryVisibility = new EventCategoryVisibility(_event);
			bool flag = HasInOldEvents(eventCategoryVisibility);
			m_hasChanged |= !flag;
			m_Events.Add(eventCategoryVisibility);
		}

		public override void Update()
		{
			bool flag = false;
			for (int i = 0; i < m_Events.Count; i++)
			{
				EventCategoryVisibility eventCategoryVisibility = (EventCategoryVisibility)m_Events[i];
				bool lastValidState = eventCategoryVisibility.LastValidState;
				if (lastValidState != eventCategoryVisibility.Validate())
				{
					flag = true;
				}
			}
			if (flag)
			{
				OnChanged.Trigger(this);
			}
		}

		public EventCategoryVisibility GetCategoryVisibilityEvent(EShopCategory _category)
		{
			for (int i = 0; i < m_Events.Count; i++)
			{
				EventCategoryVisibility eventCategoryVisibility = (EventCategoryVisibility)m_Events[i];
				if (eventCategoryVisibility.ShopCategory == _category && eventCategoryVisibility.Validate())
				{
					return eventCategoryVisibility;
				}
			}
			return null;
		}

		public bool IsCategoryVisible(EShopCategory _category)
		{
			for (int i = 0; i < m_Events.Count; i++)
			{
				EventCategoryVisibility eventCategoryVisibility = (EventCategoryVisibility)m_Events[i];
				if (eventCategoryVisibility.ShopCategory == _category && eventCategoryVisibility.Validate())
				{
					return eventCategoryVisibility.IsVisible;
				}
			}
			return true;
		}
	}
}
