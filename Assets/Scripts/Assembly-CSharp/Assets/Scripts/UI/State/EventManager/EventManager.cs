using System;
using AcierProtocol;
using Assets.Scripts.UI.State.EventManager.HppyCourCategoryManager;
using SharedBaseLib.Core;
using UbiCore.Utils;

namespace Assets.Scripts.UI.State.EventManager
{
	public class EventManager
	{
		private readonly BBSimpleList<EventManagerBase> m_eventManagers = new BBSimpleList<EventManagerBase>();

		private bool m_updateEventsOnTimerReset;

		public SimpleDelegate OnChanged;

		public EventManager()
		{
			Timer instance = UbiCore.Utils.Singleton<Timer>.Instance;
			instance.OnReset = (Timer.OnResetHandler)Delegate.Combine(instance.OnReset, new Timer.OnResetHandler(OnTimerReset));
			OnChanged = new SimpleDelegate(this, new Type[1] { typeof(EventManagerBase) });
			OnUpdateEvents();
		}

		public static bool IsShopEventType(EEventType _type)
		{
			return _type == EEventType.Event_Shop_Category_Visibility || _type == EEventType.Event_Shop_Item_Visibility || _type == EEventType.Event_Shop_Item_Percentage || _type == EEventType.Event_Shop_Category_Percentage || _type == EEventType.Event_Shop_Ad_Banner || _type == EEventType.Event_Shop_Hard_Currency_Payout_Increase || _type == EEventType.Event_Shop_Category_Payout_Increase;
		}

		public void ForceUpdate()
		{
			UpdateFrame(0f);
		}

		public EventManagerBase GetEventManger(EEventType _type)
		{
			EventManagerBase eventManagerBase = null;
			if (m_eventManagers.Count <= (int)_type)
			{
				while (m_eventManagers.Count <= (int)_type)
				{
					m_eventManagers.Add(null);
				}
			}
			eventManagerBase = m_eventManagers[(int)_type];
			if (eventManagerBase == null)
			{
				switch (_type)
				{
				case EEventType.Event_Shop_Category_Visibility:
					eventManagerBase = new EventCategoryVisibilityManager();
					break;
				case EEventType.Event_Shop_Item_Visibility:
					eventManagerBase = new EventShopBundleVisibilityManager();
					break;
				case EEventType.Event_Shop_Item_Percentage:
					eventManagerBase = new EventShopBundlePercentageManager();
					break;
				case EEventType.Event_Shop_Category_Percentage:
					eventManagerBase = new EventCategoryPercentageManager();
					break;
				case EEventType.Event_Shop_Ad_Banner:
					eventManagerBase = new EventShopAdvertisementsManager();
					break;
				case EEventType.Event_Shop_Hard_Currency_Payout_Increase:
					eventManagerBase = new EventShopBundleHappyHourManager();
					break;
				case EEventType.Event_Shop_Category_Payout_Increase:
					eventManagerBase = new EventShopCategoryHappyHourManager();
					break;
				case EEventType.Event_Progression_Challenges:
					eventManagerBase = new EventProgressionChallengesManager();
					break;
				case EEventType.Event_Flavor:
					eventManagerBase = new EventFlavorThemeManager();
					break;
				default:
					return null;
				}
				eventManagerBase.OnChanged += new SimpleDelegate.CallbackFunction(OnBaseManagerChanged);
				m_eventManagers[(int)_type] = eventManagerBase;
			}
			return eventManagerBase;
		}

		private void OnBaseManagerChanged(object[] _params)
		{
			OnChanged.Trigger(_params[0]);
		}

		public void OnUpdateEvents()
		{
			m_updateEventsOnTimerReset = true;
		}

		private void OnTimerReset()
		{
			UbiCore.Utils.Singleton<Timer>.Instance.AddCallback(UpdateFrame, 1f, true);
		}

		private bool UpdateFrame(float _deltaTime)
		{
			if (m_updateEventsOnTimerReset)
			{
				m_updateEventsOnTimerReset = false;
				UpdateEvents();
			}
			for (int i = 0; i < m_eventManagers.Count; i++)
			{
				EventManagerBase eventManagerBase = m_eventManagers[i];
				if (eventManagerBase != null)
				{
					eventManagerBase.Update();
				}
			}
			return true;
		}

		private void UpdateEvents()
		{
			for (int i = 0; i < m_eventManagers.Count; i++)
			{
				EventManagerBase eventManagerBase = m_eventManagers[i];
				if (eventManagerBase != null)
				{
					eventManagerBase.StartProcessEvents();
				}
			}
			Events events = Singleton<StateManager>.Instance.TemplateManager.Events;
			if (events != null && events.events != null)
			{
				for (int j = 0; j < events.events.Length; j++)
				{
					Event @event = events.events[j];
					EventManagerBase eventManger = GetEventManger(@event.type);
					if (eventManger != null)
					{
						eventManger.ProcessEvent(@event);
					}
				}
			}
			for (int k = 0; k < m_eventManagers.Count; k++)
			{
				EventManagerBase eventManagerBase2 = m_eventManagers[k];
				if (eventManagerBase2 != null)
				{
					eventManagerBase2.EndProcessEvents();
				}
			}
		}
	}
}
