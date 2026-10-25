using AssetBundleData;
using SharedBaseLib.Serialization;
using UnityEngine;

namespace Database
{
	[Database("ColorDefinition")]
	public class ColorDef : DBEntry
	{
		public Color Color0;

		public Color Color1;

		public ColorDef(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			Color0 = _s.SerializeColor("Color0", Color0);
			Color1 = _s.SerializeColor("Color1", Color1);
		}
	}
}
