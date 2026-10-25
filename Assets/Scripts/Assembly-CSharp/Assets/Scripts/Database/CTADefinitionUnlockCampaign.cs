using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.MissionScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using SharedBaseLib.Serialization;
using UbiCore.Utils;

namespace Assets.Scripts.Database
{
	public class CTADefinitionUnlockCampaign : CTADefinitionBase
	{
		public GameID CampaignGameId { get; protected set; }

		public int WaitHours { get; set; }

		public CTADefinitionUnlockCampaign(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			CampaignGameId = SerializeGameID("CampaignGameId", _s, this, CampaignGameId);
			WaitHours = _s.SerializeInt("WaitHours", WaitHours);
		}

		protected override EResult CheckTrigger()
		{
			LocalStorage instance = UbiCore.Utils.Singleton<LocalStorage>.Instance;
			if (!instance.CTACounterCheckTimeDiff(LocalStorage.CTACounterLastUnlockCampaign, WaitHours * 3600))
			{
				return EResult.Proceed;
			}
			if (!string.IsNullOrEmpty(base.Entitlement) && !Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(base.Entitlement))
			{
				return EResult.Proceed;
			}
			CampaignCatalog.Campaign unlockableCampaign = GetUnlockableCampaign();
			if (unlockableCampaign == null)
			{
				return EResult.Proceed;
			}
			instance.CTACounterSetTimestamp(LocalStorage.CTACounterLastUnlockCampaign);
			return EResult.Fullfilled;
		}

		public override void RedirectToScreen()
		{
			CampaignCatalog.Campaign unlockableCampaign = GetUnlockableCampaign();
			if (unlockableCampaign != null)
			{
				UIMissionListCampaigns.OpenAndSelectCampaignsItem(unlockableCampaign);
				Singleton<UIScreenManager>.Instance.SceneData.UnlockMissionBundlePreview.OpenPreviewByEvent(unlockableCampaign);
			}
		}

		private CampaignCatalog.Campaign GetUnlockableCampaign()
		{
			CampaignCatalog.Campaign campaign = null;
			CampaignCatalog campaignCatalog = Singleton<StateManager>.Instance.TemplateManager.CampaignCatalog;
			for (int i = 0; i < campaignCatalog.campaigns.Length; i++)
			{
				CampaignCatalog.Campaign campaign2 = campaignCatalog.campaigns[i];
				if (campaign2.IsCampaignUnlocked() || !campaign2.CanUnlockByShopItems())
				{
					continue;
				}
				if (campaign == null)
				{
					campaign = campaign2;
				}
				else if (CampaignGameId != GameID.Invalid)
				{
					string _guid = campaign2.id;
					int num = GameID.ComputeGuidHash(ref _guid);
					if (num == CampaignGameId.GetHashCode())
					{
						campaign = campaign2;
						break;
					}
				}
			}
			return campaign;
		}
	}
}
