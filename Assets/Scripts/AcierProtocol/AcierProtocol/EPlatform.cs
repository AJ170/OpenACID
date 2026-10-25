using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EPlatform")]
	public enum EPlatform
	{
		[ProtoEnum(Name = "AppleDevice", Value = 0)]
		AppleDevice = 0,
		[ProtoEnum(Name = "GoogleDevice", Value = 1)]
		GoogleDevice = 1,
		[ProtoEnum(Name = "Unknown", Value = 2)]
		Unknown = 2
	}
}
