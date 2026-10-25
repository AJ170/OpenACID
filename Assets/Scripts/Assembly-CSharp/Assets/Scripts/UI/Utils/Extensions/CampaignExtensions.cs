using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Tutorial;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using Database;
using SharedBaseLib.Core;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class CampaignExtensions
	{
		public static CampaignCatalog.Campaign GetCampaignById(this CampaignCatalog _campaigns, string _campaignId)
		{
			CampaignCatalog.Campaign[] campaigns = _campaigns.campaigns;
			foreach (CampaignCatalog.Campaign campaign in campaigns)
			{
				if (campaign.id.Equals(_campaignId))
				{
					return campaign;
				}
			}
			return null;
		}

		public static CampaignCatalog.Campaign GetCampaignByMissionId(this CampaignCatalog _campaigns, string _missionId)
		{
			CampaignCatalog.Campaign[] campaigns = _campaigns.campaigns;
			for (int i = 0; i < campaigns.Length; i++)
			{
				MissionInfo missionById = campaigns[i].GetMissionById(_missionId);
				if (missionById != null)
				{
					return campaigns[i];
				}
			}
			return null;
		}

		public static bool CanUnlockByShopItems(this CampaignCatalog.Campaign _campaign)
		{
			return _campaign.GetShopItemsByCampaignId().Count > 0;
		}

		public static BBList<ShopBundle> GetShopItemsByCampaignId(this CampaignCatalog.Campaign _campaign)
		{
			ShopTable shopTable = Singleton<StateManager>.Instance.TemplateManager.ShopTable;
			return shopTable.GetShopBundleByEntitlementsPayout(_campaign.reqEntitlements);
		}

		public static void GetSortedCampaigns(this CampaignCatalog _campaigns, ref List<CampaignCatalog.Campaign> _campaignList)
		{
			CampaignCatalog.Campaign[] campaigns = _campaigns.campaigns;
			foreach (CampaignCatalog.Campaign campaign in campaigns)
			{
				if (!campaign.hidden)
				{
					_campaignList.Add(campaign);
				}
			}
			_campaignList.Sort((CampaignCatalog.Campaign _campaignA, CampaignCatalog.Campaign _campaignB) => _campaignA.sortIndex.CompareTo(_campaignB.sortIndex));
		}

		public static MissionInfo GetMissionById(this CampaignCatalog _campaigns, string _missionId)
		{
			CampaignCatalog.Campaign[] campaigns = _campaigns.campaigns;
			for (int i = 0; i < campaigns.Length; i++)
			{
				MissionInfo missionById = campaigns[i].GetMissionById(_missionId);
				if (missionById != null)
				{
					return missionById;
				}
			}
			return null;
		}

		public static LocationSet GetLocationByMissionInfo(this CampaignCatalog _campaigns, string _missionId)
		{
			for (int i = 0; i < _campaigns.campaigns.Length; i++)
			{
				CampaignCatalog.Campaign campaign = _campaigns.campaigns[i];
				LocationSet locationByMissionInfo = campaign.GetLocationByMissionInfo(_missionId);
				if (locationByMissionInfo != null)
				{
					return locationByMissionInfo;
				}
			}
			return null;
		}

		public static void FixLocalStorageSeenMissionProgression(this CampaignCatalog _campaigns)
		{
			for (int i = 0; i < _campaigns.campaigns.Length; i++)
			{
				List<MissionInfo> _missionList = new List<MissionInfo>();
				_campaigns.campaigns[i].GetSortedMissionList(ref _missionList);
				for (int j = 0; j < _missionList.Count; j++)
				{
					MissionInfo missionInfo = _missionList[j];
					if (j == 0)
					{
						UbiCore.Utils.Singleton<LocalStorage>.Instance.SetMissionProgressUnlocked(missionInfo.baseName, true);
					}
					else if (missionInfo.IsTutorialMission())
					{
						UbiCore.Utils.Singleton<LocalStorage>.Instance.SetMissionProgressCompleted(missionInfo.baseName, true);
						UbiCore.Utils.Singleton<LocalStorage>.Instance.SetMissionProgressUnlocked(missionInfo.baseName, true);
					}
				}
			}
		}

		public static bool HasUnlockAnimationBeenPlayed(this CampaignCatalog.Campaign _campaign)
		{
			for (int i = 0; i < _campaign.reqEntitlements.Length; i++)
			{
				if (!UbiCore.Utils.Singleton<LocalStorage>.Instance.WasEntitlementShown(_campaign.reqEntitlements[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static bool IsCampaignUnlocked(this CampaignCatalog.Campaign _campaign)
		{
			if (_campaign.id == TutorialHelper.TutorialCampaignId && !Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.AnimusCampaignVisibleTutorialsGUID))
			{
				return false;
			}
			return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlements(_campaign.reqEntitlements);
		}

		public static CampaignDefinition GetCampaignDefinition(this CampaignCatalog.Campaign _campaign)
		{
			GameID gameID = Main.Instance.GameDB.FindGameIdFromGuid(_campaign.id);
			return Main.Instance.GameDB.GetItem<CampaignDefinition>(gameID);
		}

		public static RegionDefinition GetRegionDefinitionForCampaign(this CampaignCatalog.Campaign _campaign)
		{
			if (_campaign.missions.Length > 0)
			{
				LocationSet location = _campaign.missions[0].location;
				return location.GetRegionDefinition();
			}
			return null;
		}

		public static void GetSortedMissionList(this CampaignCatalog.Campaign _campaign, ref List<MissionInfo> _missionList)
		{
			MissionsAtLocation[] missions = _campaign.missions;
			foreach (MissionsAtLocation missionsAtLocation in missions)
			{
				for (int j = 0; j < missionsAtLocation.missions.Length; j++)
				{
					_missionList.Add(missionsAtLocation.missions[j]);
				}
			}
			_missionList.Sort((MissionInfo _missionInfoA, MissionInfo _missionInfoB) => _missionInfoA.sortIndex.CompareTo(_missionInfoB.sortIndex));
		}

		public static MissionInfo GetMissionById(this CampaignCatalog.Campaign _campaign, string _missionId)
		{
			MissionsAtLocation[] missions = _campaign.missions;
			foreach (MissionsAtLocation missionsAtLocation in missions)
			{
				for (int j = 0; j < missionsAtLocation.missions.Length; j++)
				{
					MissionInfo missionInfo = missionsAtLocation.missions[j];
					if (missionInfo.baseName == _missionId)
					{
						return missionInfo;
					}
				}
			}
			return null;
		}

		public static LocationSet GetLocationByMissionInfo(this CampaignCatalog.Campaign _campaign, string _missionId)
		{
			MissionsAtLocation[] missions = _campaign.missions;
			foreach (MissionsAtLocation missionsAtLocation in missions)
			{
				for (int j = 0; j < missionsAtLocation.missions.Length; j++)
				{
					MissionInfo missionInfo = missionsAtLocation.missions[j];
					if (missionInfo.baseName == _missionId)
					{
						return missionsAtLocation.location;
					}
				}
			}
			return null;
		}
	}
}
