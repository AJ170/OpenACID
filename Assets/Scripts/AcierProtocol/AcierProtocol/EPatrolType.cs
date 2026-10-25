using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EPatrolType")]
	public enum EPatrolType
	{
		[ProtoEnum(Name = "Military", Value = 0)]
		Military = 0,
		[ProtoEnum(Name = "CevilianCouple", Value = 1)]
		CevilianCouple = 1,
		[ProtoEnum(Name = "CevilianNarrow", Value = 2)]
		CevilianNarrow = 2,
		[ProtoEnum(Name = "CevilianWide", Value = 3)]
		CevilianWide = 3,
		[ProtoEnum(Name = "CevilianHidingSpot", Value = 4)]
		CevilianHidingSpot = 4,
		[ProtoEnum(Name = "CourtesanHidingSpot", Value = 5)]
		CourtesanHidingSpot = 5,
		[ProtoEnum(Name = "CapturedNpcs", Value = 6)]
		CapturedNpcs = 6,
		[ProtoEnum(Name = "Template_EPatrolType", Value = 7)]
		Template_EPatrolType = 7,
		[ProtoEnum(Name = "Invalid_EPatrolType", Value = 10)]
		Invalid_EPatrolType = 10
	}
}
