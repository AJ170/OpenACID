using AcierProtocol;
using Assets.Scripts.UI.MVC.Models.Events;
using Assets.Scripts.UI.State.EventManager.Data;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;

namespace Assets.Scripts.UI.State.EventManager
{
	public class EventProgressionChallengesManager : EventManagerBase
	{
		public static EventProgressionChallengesManager Instance
		{
			get
			{
				return (EventProgressionChallengesManager)Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Progression_Challenges);
			}
		}

		public override EEventType ManagerType
		{
			get
			{
				return EEventType.Event_Progression_Challenges;
			}
		}

		public override void ProcessEvent(Event _event)
		{
			EventProgressionChallenges eventProgressionChallenges = new EventProgressionChallenges(_event);
			if (IsContainingChangedEvent(m_oldEvents, eventProgressionChallenges))
			{
				m_hasChanged = true;
			}
			m_Events.Add(eventProgressionChallenges);
		}

		public override void Update()
		{
			bool flag = false;
			for (int i = 0; i < m_Events.Count; i++)
			{
				EventProgressionChallenges eventProgressionChallenges = (EventProgressionChallenges)m_Events[i];
				bool lastValidState = eventProgressionChallenges.LastValidState;
				if (lastValidState != eventProgressionChallenges.Validate())
				{
					flag = true;
				}
			}
			if (flag)
			{
				OnChanged.Trigger(this);
			}
		}

		public UIEventModel GetEventModel()
		{
			UIEventModel uIEventModel = new UIEventModel();
			uIEventModel.EventProgressionChallenges = GetFirstValidEventProgressionEvent();
			return uIEventModel;
		}

		public bool HasValidProgressionEvent()
		{
			return GetFirstValidEventProgressionEvent() != null;
		}

		public EventProgressionChallenges GetFirstValidEventProgressionEvent()
		{
			if (!Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.FeatureAvailableWeeklyEventsGUID))
			{
				return null;
			}
			for (int i = 0; i < m_Events.Count; i++)
			{
				EventProgressionChallenges eventProgressionChallenges = (EventProgressionChallenges)m_Events[i];
				if (eventProgressionChallenges.LastValidState && eventProgressionChallenges.EventProgressionData.HasProgressDataForEvent())
				{
					return eventProgressionChallenges;
				}
			}
			return null;
		}

		private bool IsContainingChangedEvent(BBList<EventBase> _events, EventProgressionChallenges _event)
		{
			if (_events == null)
			{
				return false;
			}
			for (int i = 0; i < _events.Count; i++)
			{
				EventProgressionChallenges eventProgressionChallenges = (EventProgressionChallenges)_events[i];
				if (eventProgressionChallenges.Event.startTimeUTC == _event.Event.startTimeUTC && eventProgressionChallenges.Event.endTimeUTC == _event.Event.endTimeUTC && eventProgressionChallenges.Validate() != _event.Validate())
				{
					return true;
				}
			}
			return false;
		}
	}
}
