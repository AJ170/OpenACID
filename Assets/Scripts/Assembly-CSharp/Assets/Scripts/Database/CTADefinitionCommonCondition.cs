using AssetBundleData;
using SharedBaseLib.Serialization;
using UbiCore.Utils;

namespace Assets.Scripts.Database
{
	public class CTADefinitionCommonCondition : CTADefinitionBase
	{
		public CTADefinitionCommonCondition(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
		}

		public override EResult CheckTriggerBase()
		{
			return CheckTrigger();
		}

		protected override EResult CheckTrigger()
		{
			LocalStorage instance = UbiCore.Utils.Singleton<LocalStorage>.Instance;
			if (instance.CTACounterGet(LocalStorage.CTACounterMission) - instance.CTACounterGet(LocalStorage.CTACounterMissionLastCTA) < base.MinMissions)
			{
				return EResult.Abort;
			}
			return EResult.Proceed;
		}
	}
}
