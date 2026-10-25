using System.Collections.Generic;
using AssetBundleData;
using SharedBaseLib.Serialization;

namespace Database
{
	[Database("AudioMixStateDefinition")]
	public class AudioMixStateDef : DBEntry, IDuckingInfoListCarrier
	{
		public List<AudioMixingGroupDef.DuckingInfo> DuckingInfos { get; set; }

		public bool UseDefault { get; set; }

		public AudioMixingGroupDef.DuckingInfo DefaultState { get; set; }

		public AudioMixStateDef(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
			DuckingInfos = new List<AudioMixingGroupDef.DuckingInfo>();
			DefaultState = new AudioMixingGroupDef.DuckingInfo();
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			UseDefault = _s.SerializeBool("UseDefault", UseDefault);
			DefaultState.Serialize("DefaultState", _s);
			DuckingInfos = _s.SerializeList("DuckingInfos", DuckingInfos);
		}

		public override string ToString()
		{
			return string.Format("DefaultState: {0}, UseDefault: {1}, DuckingInfos: {2}", DefaultState, UseDefault, DuckingInfos);
		}
	}
}
