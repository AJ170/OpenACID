using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Progression
{
	public class UIEventChallengeProgressionHandler
	{
		private class LastProgressionStatus
		{
			public ECounterType CounterType;

			public long LastValue;
		}

		private string m_lastEventId = string.Empty;

		private readonly List<LastProgressionStatus> m_lastCheckedChanges = new List<LastProgressionStatus>();

		public void OnEnterMission()
		{
			EventDelegate.Remove(Singleton<StateManager>.Instance.ProfileManager.onCountersUpdate, OnEventProgressionChanged);
		}

		public void OnEnterAnimusFromStartUp()
		{
			EventDelegate.Add(Singleton<StateManager>.Instance.ProfileManager.onCountersUpdate, OnEventProgressionChanged);
		}

		public void OnEnterAnimusAfterMission()
		{
			EventDelegate.Add(Singleton<StateManager>.Instance.ProfileManager.onCountersUpdate, OnEventProgressionChanged);
			CheckForChangedEvents(Singleton<StateManager>.Instance.ProfileManager.LastProfileDataBeforeMission);
		}

		private void OnEventProgressionChanged()
		{
			if (Main.Instance.SceneLoader.InAnimus)
			{
				CheckForChangedEvents(Singleton<StateManager>.Instance.ProfileManager.PreviousProfileData);
			}
		}

		private void CheckForChangedEvents(LastProfileData _lastData)
		{
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			if (profileManager.Profile.eventsProgress == null || !Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.FeatureAvailableWeeklyEventsGUID))
			{
				return;
			}
			string mainEventId = profileManager.Profile.eventsProgress.mainEventId;
			if (mainEventId != m_lastEventId)
			{
				m_lastCheckedChanges.Clear();
			}
			m_lastEventId = mainEventId;
			EventsProgress.ChallengeEvent[] challengeEvents = profileManager.Profile.eventsProgress.challengeEvents;
			foreach (EventsProgress.ChallengeEvent challengeEvent in challengeEvents)
			{
				long previousChallengeCount = challengeEvent.GetPreviousChallengeCount(_lastData);
				long challengeCount = challengeEvent.GetChallengeCount();
				if (previousChallengeCount >= 0 && challengeCount >= 0)
				{
					LastProgressionStatus lastShownEventChangeData = GetLastShownEventChangeData(challengeEvent.challengeCounter);
					if (lastShownEventChangeData.LastValue != challengeCount && challengeCount != previousChallengeCount)
					{
						UINotificationPopup subSystemByType = UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UINotificationPopup>();
						subSystemByType.CreateEventNotificatioPopup(challengeEvent, previousChallengeCount);
						lastShownEventChangeData.LastValue = challengeCount;
					}
				}
			}
		}

		private LastProgressionStatus GetLastShownEventChangeData(ECounterType _challengeCounter)
		{
			LastProgressionStatus lastProgressionStatus = null;
			for (int i = 0; i < m_lastCheckedChanges.Count; i++)
			{
				if (m_lastCheckedChanges[i].CounterType == _challengeCounter)
				{
					lastProgressionStatus = m_lastCheckedChanges[i];
				}
			}
			if (lastProgressionStatus == null)
			{
				LastProgressionStatus lastProgressionStatus2 = new LastProgressionStatus();
				lastProgressionStatus2.CounterType = _challengeCounter;
				lastProgressionStatus2.LastValue = 0L;
				lastProgressionStatus = lastProgressionStatus2;
				m_lastCheckedChanges.Add(lastProgressionStatus);
			}
			return lastProgressionStatus;
		}
	}
}
