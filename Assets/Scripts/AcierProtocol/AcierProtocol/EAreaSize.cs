using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EAreaSize")]
	public enum EAreaSize
	{
		[ProtoEnum(Name = "Small", Value = 0)]
		Small = 0,
		[ProtoEnum(Name = "Medium", Value = 1)]
		Medium = 1,
		[ProtoEnum(Name = "Large", Value = 2)]
		Large = 2,
		[ProtoEnum(Name = "Full", Value = 3)]
		Full = 3
	}
}
