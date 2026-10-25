using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EServerEnvironment")]
	public enum EServerEnvironment
	{
		[ProtoEnum(Name = "Trunk", Value = 0)]
		Trunk = 0,
		[ProtoEnum(Name = "Trunk2", Value = 1)]
		Trunk2 = 1,
		[ProtoEnum(Name = "Stable", Value = 2)]
		Stable = 2,
		[ProtoEnum(Name = "External", Value = 3)]
		External = 3
	}
}
