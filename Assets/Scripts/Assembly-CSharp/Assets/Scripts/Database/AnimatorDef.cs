using AssetBundleData;
using Database;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Database
{
	[Database("AnimatorDefinition")]
	public class AnimatorDef : DBEntry
	{
		public UnityResource AnimatorPrefab { get; set; }

		public AnimatorDef(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
			AnimatorPrefab = new UnityResource(_assetBundleInfo);
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			AnimatorPrefab.Serialize("AnimatorPrefab", _s, base.AssetBundleInfo);
		}

		public override void ClearCache()
		{
			AnimatorPrefab.ClearCache();
		}
	}
}
