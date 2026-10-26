using AcierProtocol;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;

namespace Assets.Scripts.UI.Notifications
{
	public class UICampaignEvaluator : UINewsEvaluatorBase
	{
		public override ENewsType Type
		{
			get
			{
				return ENewsType.Campaign;
			}
		}

		public override void Init()
		{
			base.Init();
			if (Assets.Scripts.Offline.OfflineMode.Enabled)
			{
				// Campaigns come from the server statics.
				return;
			}
			OnProfileUpdated();
		}

		public override void Update()
		{
		}

		protected override void OnProfileUpdated(params object[] _params)
		{
			base.OnProfileUpdated(_params);
			CampaignCatalog.Campaign[] campaigns = Singleton<StateManager>.Instance.TemplateManager.CampaignCatalog.campaigns;
			foreach (CampaignCatalog.Campaign campaign in campaigns)
			{
				for (int j = 0; j < campaign.missions.Length; j++)
				{
					MissionsAtLocation missionsAtLocation = campaign.missions[j];
					for (int k = 0; k < missionsAtLocation.missions.Length; k++)
					{
						MissionInfo missionInfo = missionsAtLocation.missions[k];
						if (missionInfo.IsUnlocked() && !missionInfo.IsTutorialMission())
						{
							StoryMissionRewardMapping.Entry missionReward = missionInfo.GetMissionReward();
							StoryMissionRewardMapping.Entry.RankEntry rankNormalRewards = missionReward.GetRankNormalRewards(Singleton<StateManager>.Instance.MissionSelectionManager.CampaignHeroicMissionRank);
							if (rankNormalRewards != null && !rankNormalRewards.lootTemplates.HasEntitlementsFomLootTemplates())
							{
								base.Value = true;
								return;
							}
						}
					}
				}
			}
			base.Value = false;
		}
	}
}
