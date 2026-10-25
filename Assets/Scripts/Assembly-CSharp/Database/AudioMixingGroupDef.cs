using System.Collections.Generic;
using AssetBundleData;
using SharedBaseLib.Serialization;

namespace Database
{
	[Database("AudioMixingGroupDefinition")]
	public class AudioMixingGroupDef : DBEntry, IDuckingInfoListCarrier
	{
		public class DuckingInfo : DataSerializableBase
		{
			public string MixingGroupGuidString { get; set; }

			public GameID MixingGroup { get; set; }

			public float Threshold { get; set; }

			public float VolumeReduceTo { get; set; }

			public float FadeInTime { get; set; }

			public float FadeStayTime { get; set; }

			public float FadeOutTime { get; set; }

			public DuckingInfo()
			{
				Threshold = 5f;
				VolumeReduceTo = 0.6f;
				FadeInTime = 1f;
				FadeOutTime = 1f;
				FadeStayTime = 0f;
			}

			public DuckingInfo(DuckingInfo _other)
			{
				Threshold = _other.Threshold;
				VolumeReduceTo = _other.VolumeReduceTo;
				FadeInTime = _other.FadeInTime;
				FadeOutTime = _other.FadeOutTime;
				FadeStayTime = _other.FadeStayTime;
				MixingGroup = _other.MixingGroup;
				MixingGroupGuidString = _other.MixingGroupGuidString;
			}

			protected override void SerializeInternal(DataSerializerBase _s, object _customData = null)
			{
				MixingGroupGuidString = _s.SerializeString("MixingGroupGuidString", MixingGroupGuidString);
				Threshold = _s.SerializeFloat("Threshold", Threshold);
				VolumeReduceTo = _s.SerializeFloat("VolumeReduceTo", VolumeReduceTo);
				FadeInTime = _s.SerializeFloat("FadeInTime", FadeInTime);
				FadeStayTime = _s.SerializeFloat("FadeStayTime", FadeStayTime);
				FadeOutTime = _s.SerializeFloat("FadeOutTime", FadeOutTime);
			}

			public override string ToString()
			{
				return string.Format("MixingGroupGuidString: {0}, MixingGroup: {1}", MixingGroupGuidString, MixingGroup);
			}
		}

		public float Volume { get; set; }

		public List<DuckingInfo> DuckingInfos { get; set; }

		public AudioMixingGroupDef(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			Volume = _s.SerializeFloat("Volume", Volume);
			DuckingInfos = _s.SerializeList("DuckingInfos", DuckingInfos);
		}
	}
}
