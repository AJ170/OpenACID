using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "ECompare")]
	public enum ECompare
	{
		[ProtoEnum(Name = "Less", Value = 1)]
		Less = 1,
		[ProtoEnum(Name = "Equal", Value = 2)]
		Equal = 2,
		[ProtoEnum(Name = "LessEqual", Value = 3)]
		LessEqual = 3,
		[ProtoEnum(Name = "Greater", Value = 4)]
		Greater = 4,
		[ProtoEnum(Name = "Unequal", Value = 5)]
		Unequal = 5,
		[ProtoEnum(Name = "GreaterEqual", Value = 6)]
		GreaterEqual = 6
	}
}
