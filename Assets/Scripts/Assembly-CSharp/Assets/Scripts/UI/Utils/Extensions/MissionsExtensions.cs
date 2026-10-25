using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class MissionsExtensions
	{
		public static bool IsEqual(this MissionCollection.RankMissions _rankMissions, MissionCollection.RankMissions _otherRankMissions)
		{
			if (_rankMissions.rankIdx == _otherRankMissions.rankIdx && _rankMissions.missions.Length == _otherRankMissions.missions.Length)
			{
				for (int i = 0; i < _rankMissions.missions.Length; i++)
				{
					if (_rankMissions.missions[i] != _otherRankMissions.missions[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public static void GetMissionsByHeroicRank(this MissionCollection.RankMissions _rankMissions, ref List<AcierProtocol.Mission> _missionList)
		{
			for (int i = 0; i < _rankMissions.missions.Length; i++)
			{
				_missionList.Add(_rankMissions.missions[i]);
			}
		}

		public static AcierProtocol.Mission GetMissionByID(this MissionCollection.RankMissions _rankMissions, string _missionId)
		{
			if (_rankMissions != null)
			{
				return _rankMissions.missions.Find((AcierProtocol.Mission x) => x.id.ToLower() == _missionId.ToLower());
			}
			return null;
		}

		public static AcierProtocol.Mission GetMissionByBaseName(this MissionCollection.RankMissions _rankMissions, string _baseName)
		{
			if (_rankMissions != null)
			{
				return _rankMissions.missions.Find((AcierProtocol.Mission x) => x.baseName.ToLower() == _baseName.ToLower());
			}
			return null;
		}

		public static bool IsValidMissionId(this MissionCollection _missionCollection, string _missionId)
		{
			if (!string.IsNullOrEmpty(_missionId) && _missionCollection != null && _missionCollection.assassinMissions != null)
			{
				for (int i = 0; i < _missionCollection.assassinMissions.Length; i++)
				{
					MissionCollection.AssassinMissions assassinMissions = _missionCollection.assassinMissions[i];
					for (int j = 0; j < assassinMissions.rankMissions.Length; j++)
					{
						AcierProtocol.Mission[] missions = assassinMissions.rankMissions[j].missions;
						for (int k = 0; k < missions.Length; k++)
						{
							if (missions[k].id == _missionId)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public static string GetFirstAvailableMissionFromCollection(this MissionCollection _missionCollection)
		{
			if (_missionCollection != null && _missionCollection.assassinMissions != null)
			{
				for (int i = 0; i < _missionCollection.assassinMissions.Length; i++)
				{
					MissionCollection.AssassinMissions assassinMissions = _missionCollection.assassinMissions[i];
					for (int j = 0; j < assassinMissions.rankMissions.Length; j++)
					{
						AcierProtocol.Mission[] missions = assassinMissions.rankMissions[j].missions;
						if (missions.Length > 0)
						{
							return missions[0].id;
						}
					}
				}
			}
			return string.Empty;
		}

		public static MissionCollection.AssassinMissions GetMissionsByAssassin(this MissionCollection _missionCollection, string _assassinId)
		{
			return _missionCollection.assassinMissions.Find((MissionCollection.AssassinMissions x) => x.assassinId == _assassinId);
		}

		public static MissionCollection.RankMissions GetMissionsByAssassinAndRank(this MissionCollection.AssassinMissions _assassinMissions, string _assassinId, int _rank)
		{
			return _assassinMissions.rankMissions.Find((MissionCollection.RankMissions x) => x.rankIdx == _rank);
		}

		public static bool HasStoryMissionBeenWonBefore(string _baseName)
		{
			bool flag = false;
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			StoryMissionRewardMapping.Entry missionReward = MissionInfoExtensions.GetMissionReward(_baseName);
			if (missionReward != null)
			{
				for (int i = 0; i < missionReward.completionRewards.Length; i++)
				{
					if (flag)
					{
						break;
					}
					StoryMissionRewardMapping.Entry.RankEntry rankEntry = missionReward.completionRewards[i];
					for (int j = 0; j < rankEntry.lootTemplates.Length; j++)
					{
						LootTemplate lootTemplate = rankEntry.lootTemplates[j];
						if (lootTemplate.entitlementTemplate != null && profileManager.LastProfileDataBeforeMission.HasEntitlement(lootTemplate.entitlementTemplate.gameIdGuid))
						{
							flag = true;
							break;
						}
					}
				}
			}
			return flag;
		}
	}
}
