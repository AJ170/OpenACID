using System.Collections.Generic;
using AssetBundleData;
using SharedBaseLib.Serialization;

namespace Database
{
	[Database("EffectPackage")]
	public class EffectPackage : DBEntry
	{
		public List<EffectPackageElement> Elements { get; set; }

		public List<EffectState> States { get; set; }

		public EffectPackage(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			Elements = _s.SerializeList("Elements", Elements, base.AssetBundleInfo);
			States = _s.SerializeList("States", States);
		}

		public override void ClearCache()
		{
			foreach (EffectPackageElement element in Elements)
			{
				element.PreFab.ClearCache();
			}
		}
	}
}
