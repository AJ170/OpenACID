using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.State.EventManager.Data;

namespace Assets.Scripts.UI.State.EventManager
{
	public class EventShopBundleVisibilityManager : EventManagerBase
	{
		public override EEventType ManagerType
		{
			get
			{
				return EEventType.Event_Shop_Item_Visibility;
			}
		}

		public override void ProcessEvent(Event _event)
		{
			EventShopBundleVisibility eventShopBundleVisibility = new EventShopBundleVisibility(_event);
			bool flag = HasInOldEvents(eventShopBundleVisibility);
			m_hasChanged |= !flag;
			m_Events.Add(eventShopBundleVisibility);
		}

		public override void Update()
		{
			bool flag = false;
			for (int i = 0; i < m_Events.Count; i++)
			{
				EventShopBundleVisibility eventShopBundleVisibility = (EventShopBundleVisibility)m_Events[i];
				bool lastValidState = eventShopBundleVisibility.LastValidState;
				if (lastValidState != eventShopBundleVisibility.Validate())
				{
					flag = true;
				}
			}
			if (flag)
			{
				OnChanged.Trigger(this);
			}
		}

		public EventShopBundleVisibility GetShopBundleVisibilityEvent(string _shopBundleId)
		{
			for (int i = 0; i < m_Events.Count; i++)
			{
				EventShopBundleVisibility eventShopBundleVisibility = (EventShopBundleVisibility)m_Events[i];
				if (eventShopBundleVisibility.ShopBundleId == _shopBundleId && eventShopBundleVisibility.Validate())
				{
					return eventShopBundleVisibility;
				}
			}
			return null;
		}

		public Dictionary<string, int> GetItemOverrideSortIds()
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			for (int i = 0; i < m_Events.Count; i++)
			{
				EventShopBundleVisibility eventShopBundleVisibility = (EventShopBundleVisibility)m_Events[i];
				if (eventShopBundleVisibility.Position != 0 && eventShopBundleVisibility.LastValidState && !dictionary.ContainsKey(eventShopBundleVisibility.ShopBundleId))
				{
					dictionary.Add(eventShopBundleVisibility.ShopBundleId, eventShopBundleVisibility.Position);
				}
			}
			return dictionary;
		}
	}
}
