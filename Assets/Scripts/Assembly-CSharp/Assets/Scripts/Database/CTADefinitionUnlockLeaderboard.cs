using AssetBundleData;
using SharedBaseLib.Serialization;
using UbiCore.Utils;

namespace Assets.Scripts.Database
{
	public class CTADefinitionUnlockLeaderboard : CTADefinitionBase
	{
		public int WaitHours { get; set; }

		public CTADefinitionUnlockLeaderboard(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
			base.CheckOnProfileUpdate = true;
		}

		public static void UpdateLeaderboardLastShown()
		{
			UbiCore.Utils.Singleton<LocalStorage>.Instance.CTACounterSetTimestamp(LocalStorage.CTACounterLastLeaderboardShown);
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			WaitHours = _s.SerializeInt("WaitHours", WaitHours);
		}

		protected override EResult CheckTrigger()
		{
			LocalStorage instance = UbiCore.Utils.Singleton<LocalStorage>.Instance;
			if (!instance.CTACounterCheckTimeDiff(LocalStorage.CTACounterLastLeaderboardShown, WaitHours * 3600))
			{
				return EResult.Proceed;
			}
			instance.CTACounterSetTimestamp(LocalStorage.CTACounterLastLeaderboardShown);
			return EResult.Fullfilled;
		}
	}
}
