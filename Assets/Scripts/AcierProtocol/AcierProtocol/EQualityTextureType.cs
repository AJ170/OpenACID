using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EQualityTextureType")]
	public enum EQualityTextureType
	{
		[ProtoEnum(Name = "Generic", Value = 0)]
		Generic = 0,
		[ProtoEnum(Name = "DXT", Value = 1)]
		DXT = 1,
		[ProtoEnum(Name = "ETC2", Value = 2)]
		ETC2 = 2
	}
}
