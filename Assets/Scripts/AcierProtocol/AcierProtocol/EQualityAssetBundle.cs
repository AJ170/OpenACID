using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EQualityAssetBundle")]
	public enum EQualityAssetBundle
	{
		[ProtoEnum(Name = "High", Value = 0)]
		High = 0,
		[ProtoEnum(Name = "Low", Value = 1)]
		Low = 1
	}
}
