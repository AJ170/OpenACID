using AssetBundleData;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Database
{
	public class CTADefinitionStarterPack : CTADefinitionBase
	{
		public int MinLevel { get; protected set; }

		public int MaxLevel { get; protected set; }

		public CTADefinitionStarterPack(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			MinLevel = _s.SerializeInt("MinLevel", MinLevel);
			MaxLevel = _s.SerializeInt("MaxLevel", MaxLevel);
		}

		protected override EResult CheckTrigger()
		{
			return EResult.Proceed;
		}
	}
}
