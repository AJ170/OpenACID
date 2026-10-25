using AssetBundleData;
using SharedBaseLib.Serialization;

namespace Database
{
	[Database("SoundDefinition")]
	public class SoundDef : DBEntry
	{
		public enum EAudioImporterLoadType
		{
			DecompressOnLoad = 0,
			CompressedInMemory = 1,
			StreamFromDisc = 2
		}

		public UnityResource SoundResource { get; set; }

		public SoundDef(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
			SoundResource = new UnityResource(_assetBundleInfo);
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			SoundResource.Serialize("SoundResource", _s, base.AssetBundleInfo);
		}

		public override void ClearCache()
		{
			SoundResource.ClearCache();
		}
	}
}
