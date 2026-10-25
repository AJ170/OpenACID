using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EMovementSpeed")]
	public enum EMovementSpeed
	{
		[ProtoEnum(Name = "WalkSlow", Value = 0)]
		WalkSlow = 0,
		[ProtoEnum(Name = "Walk", Value = 1)]
		Walk = 1,
		[ProtoEnum(Name = "WalkFast", Value = 2)]
		WalkFast = 2,
		[ProtoEnum(Name = "Run", Value = 3)]
		Run = 3,
		[ProtoEnum(Name = "Sprint", Value = 4)]
		Sprint = 4,
		[ProtoEnum(Name = "Invalid_EMovementSpeed", Value = 10)]
		Invalid_EMovementSpeed = 10
	}
}
