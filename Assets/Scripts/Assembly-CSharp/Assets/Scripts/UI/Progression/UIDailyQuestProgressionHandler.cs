using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;
using SharedBaseLib.Core;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Progression
{
	public class UIDailyQuestProgressionHandler
	{
		private class LastProgressionStatus
		{
			public int RankIdx;

			public ulong ExpiresAtTimeStamp;

			public int[] QuestProgress;
		}

		private readonly List<LastProgressionStatus> m_lastCheckedChanges = new List<LastProgressionStatus>();

		public void OnEnterMission()
		{
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate -= new SimpleDelegate.CallbackFunction(OnProfileUpdate);
		}

		public void OnEnterAnimusFromStartUp()
		{
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate += new SimpleDelegate.CallbackFunction(OnProfileUpdate);
		}

		public void OnEnterAnimusAfterMission()
		{
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate += new SimpleDelegate.CallbackFunction(OnProfileUpdate);
			CheckForChangedEvents(Singleton<StateManager>.Instance.ProfileManager.LastProfileDataBeforeMission);
		}

		private void OnProfileUpdate(params object[] _params)
		{
			if (Main.Instance.SceneLoader.InAnimus)
			{
				CheckForChangedEvents(Singleton<StateManager>.Instance.ProfileManager.PreviousProfileData);
			}
		}

		private void CheckForChangedEvents(LastProfileData _lastData)
		{
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			if (profileManager.Profile.progressionQuests == null || _lastData.ProgressionQuests == null || !Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.DailyQuestsEntitlementGUID))
			{
				return;
			}
			ProgressionQuests progressionQuests = profileManager.Profile.progressionQuests;
			for (int i = 0; i < progressionQuests.rankQuests.Length; i++)
			{
				ProgressionQuests.RankQuests rankQuests = progressionQuests.rankQuests[i];
				if (m_lastCheckedChanges.Count <= i)
				{
					m_lastCheckedChanges.Add(null);
				}
				else if (m_lastCheckedChanges[i] != null && m_lastCheckedChanges[i].ExpiresAtTimeStamp != rankQuests.expiresAtTimeStamp)
				{
					m_lastCheckedChanges[i] = null;
				}
				if (m_lastCheckedChanges[i] == null)
				{
					m_lastCheckedChanges[i] = new LastProgressionStatus
					{
						RankIdx = rankQuests.rankIdx,
						ExpiresAtTimeStamp = rankQuests.expiresAtTimeStamp,
						QuestProgress = new int[rankQuests.questChain.Length]
					};
				}
				ProgressionQuests.RankQuests rankQuests2 = ((i >= _lastData.ProgressionQuests.rankQuests.Length) ? null : _lastData.ProgressionQuests.rankQuests[i]);
				if (rankQuests2 != null && rankQuests2.expiresAtTimeStamp != rankQuests.expiresAtTimeStamp)
				{
					continue;
				}
				for (int j = 0; j < rankQuests.questChain.Length; j++)
				{
					ProgressionQuests.RankQuests.Quest quest = rankQuests.questChain[j];
					uint num = ((rankQuests2 != null) ? rankQuests2.questChain[j].progress : 0u);
					if (quest.progress > num && (m_lastCheckedChanges[i].QuestProgress[j] == 0 || m_lastCheckedChanges[i].QuestProgress[j] != quest.progress))
					{
						string id = rankQuests.rankIdx + "_" + j;
						UINotificationPopup subSystemByType = UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UINotificationPopup>();
						subSystemByType.CreateQuestNotificatioPopup(id, quest, num);
						m_lastCheckedChanges[i].QuestProgress[j] = (int)quest.progress;
					}
				}
			}
		}
	}
}
