using AssetBundleData;
using SharedBaseLib.Serialization;
using UbiCore.Utils;

namespace Assets.Scripts.Database
{
	public class CTADefinitionAdColony : CTADefinitionBase
	{
		public int WaitHours { get; set; }

		public CTADefinitionAdColony(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			WaitHours = _s.SerializeInt("WaitHours", WaitHours);
		}

		protected override EResult CheckTrigger()
		{
			LocalStorage instance = UbiCore.Utils.Singleton<LocalStorage>.Instance;
			if (!AdColonyManager.IsVideoAvailable)
			{
				return EResult.Proceed;
			}
			if (!instance.CTACounterCheckTimeDiff(LocalStorage.CTACounterLastAdColony, WaitHours * 3600))
			{
				return EResult.Proceed;
			}
			return EResult.Fullfilled;
		}

		public override void RedirectToScreen()
		{
			if (AdColonyManager.IsAvailable)
			{
				AdColonyManager.Instance.ShowAdColonyVideoWithCallback(AdColonyManager.ERewardType.MysteryBox, true);
			}
			base.RedirectToScreen();
		}
	}
}
