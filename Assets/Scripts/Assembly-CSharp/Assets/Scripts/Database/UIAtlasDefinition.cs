using AssetBundleData;
using Database;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Database
{
	[Database("UIAtlasDefinition")]
	public class UIAtlasDefinition : DBEntry
	{
		public UnityResource Atlas { get; set; }

		public bool IntegrateToStandalone { get; set; }

		public UIAtlasDefinition(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
			Atlas = new UnityResource(_assetBundleInfo);
			IntegrateToStandalone = false;
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			Atlas.Serialize("Atlas", _s, base.AssetBundleInfo);
			IntegrateToStandalone = _s.SerializeBool("IntegrateToStandalone", IntegrateToStandalone);
		}

		public override void ClearCache()
		{
			Atlas.ClearCache();
		}
	}
}
