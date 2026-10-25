using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.ShopScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using SharedBaseLib.Serialization;
using UbiCore.Utils;

namespace Assets.Scripts.Database
{
	public class CTADefinitionExceedingRank : CTADefinitionBase
	{
		public int WaitMissions { get; set; }

		public CTADefinitionExceedingRank(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			WaitMissions = _s.SerializeInt("WaitMissions", WaitMissions);
		}

		public override StringMapper GetBodyMapper()
		{
			int maxRankIdx = Singleton<StateManager>.Instance.ProfileManager.Profile.maxRankIdx;
			HeroicRankDescription value;
			UbiCore.Utils.Singleton<UIDataMapper>.Instance.HeroicRankDescription.TryGetValue(maxRankIdx, out value);
			string value2 = LocalizationManager.Instance.Get(value.NameId);
			StringMapper stringMapper = new StringMapper();
			stringMapper.Add("yourHighestRankTitle", value2);
			return stringMapper;
		}

		public override void RedirectToScreen()
		{
			HeroicRankMapping.Entry nextHeroicRankToUnlock = Singleton<StateManager>.Instance.TemplateManager.HeroicRankMapping.GetNextHeroicRankToUnlock();
			if (base.RedirectScreen == EScreenID.ShopScreen && nextHeroicRankToUnlock != null)
			{
				UIShopDetails shopDetails = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails;
				ShopBaseNode heroicRankShopBundleByEntitlement = shopDetails.CategoryRootNode.GetHeroicRankShopBundleByEntitlement(nextHeroicRankToUnlock.entitlement);
				shopDetails.OpenAndSetFocusOnItem(heroicRankShopBundleByEntitlement, true);
			}
			else if (base.RedirectScreen != EScreenID.Invalid)
			{
				Singleton<UIScreenManager>.Instance.ChangeScreen(base.RedirectScreen, new UITweenHandle(null, null));
			}
		}

		protected override EResult CheckTrigger()
		{
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			int level = profileManager.CurrentAssassin.level;
			int maxRankIdx = profileManager.Profile.maxRankIdx;
			HeroicRankMapping.Entry[] heroicRankMapping = Singleton<StateManager>.Instance.TemplateManager.HeroicRankMapping;
			if (maxRankIdx >= heroicRankMapping.Length - 1)
			{
				return EResult.Proceed;
			}
			HeroicRankMapping.Entry entry = heroicRankMapping[maxRankIdx];
			if (level <= entry.randomMissionsScalingLevelUpper)
			{
				return EResult.Proceed;
			}
			LocalStorage instance = UbiCore.Utils.Singleton<LocalStorage>.Instance;
			if (instance.CTACounterGet(LocalStorage.CTACounterExceedingRankNextAvail) <= WaitMissions)
			{
				return EResult.Proceed;
			}
			if (instance.CTACounterGet(LocalStorage.CTACounterExceedingRankCurrent) < maxRankIdx)
			{
				return EResult.Proceed;
			}
			instance.CTACounterSet(LocalStorage.CTACounterExceedingRankNextAvail, 0);
			return EResult.Fullfilled;
		}
	}
}
