using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.UI.State;
using SharedBaseLib.Serialization;
using UbiCore.Utils;

namespace Assets.Scripts.Database
{
	public class CTADefinitionUnlockNewHeroicRank : CTADefinitionBase
	{
		public int MinRank { get; set; }

		public CTADefinitionUnlockNewHeroicRank(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
			base.CheckOnProfileUpdate = true;
		}

		public static void UpdateHeroicRankShown()
		{
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			UbiCore.Utils.Singleton<LocalStorage>.Instance.CTACounterSet(LocalStorage.CTACounterHeroicRank, profile.maxRankIdx);
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			MinRank = _s.SerializeInt("MinRank", MinRank);
		}

		protected override EResult CheckTrigger()
		{
			LocalStorage instance = UbiCore.Utils.Singleton<LocalStorage>.Instance;
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			int num = instance.CTACounterGet(LocalStorage.CTACounterHeroicRank);
			int maxRankIdx = profile.maxRankIdx;
			if (maxRankIdx < MinRank || num == maxRankIdx)
			{
				return EResult.Proceed;
			}
			instance.CTACounterSet(LocalStorage.CTACounterHeroicRank, maxRankIdx);
			return EResult.Fullfilled;
		}
	}
}
