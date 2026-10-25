using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EFadeMode")]
	public enum EFadeMode
	{
		[ProtoEnum(Name = "Simple_EFadeMode", Value = 0)]
		Simple_EFadeMode = 0,
		[ProtoEnum(Name = "Fancy_EFadeMode", Value = 1)]
		Fancy_EFadeMode = 1,
		[ProtoEnum(Name = "Raster_EFadeMode", Value = 2)]
		Raster_EFadeMode = 2
	}
}
