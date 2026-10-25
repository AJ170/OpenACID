using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "ERotationAtWaypoint")]
	public enum ERotationAtWaypoint
	{
		[ProtoEnum(Name = "None", Value = 0)]
		None = 0,
		[ProtoEnum(Name = "RotationOfWaypoint", Value = 1)]
		RotationOfWaypoint = 1,
		[ProtoEnum(Name = "LookingToCenter", Value = 2)]
		LookingToCenter = 2
	}
}
