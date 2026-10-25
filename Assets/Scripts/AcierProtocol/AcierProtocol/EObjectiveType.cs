using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EObjectiveType")]
	public enum EObjectiveType
	{
		[ProtoEnum(Name = "Deliver_EObjectiveType", Value = 0)]
		Deliver_EObjectiveType = 0,
		[ProtoEnum(Name = "Kill_EObjectiveType", Value = 1)]
		Kill_EObjectiveType = 1,
		[ProtoEnum(Name = "Find_EObjectiveType", Value = 2)]
		Find_EObjectiveType = 2,
		[ProtoEnum(Name = "Loot_EObjectiveType", Value = 3)]
		Loot_EObjectiveType = 3,
		[ProtoEnum(Name = "Plant_EObjectiveType", Value = 4)]
		Plant_EObjectiveType = 4,
		[ProtoEnum(Name = "Escape_EObjectiveType", Value = 5)]
		Escape_EObjectiveType = 5,
		[ProtoEnum(Name = "Assasinate_EObjectiveType", Value = 6)]
		Assasinate_EObjectiveType = 6,
		[ProtoEnum(Name = "Story_EObjectiveType", Value = 7)]
		Story_EObjectiveType = 7,
		[ProtoEnum(Name = "UnknownTarget_EObjectiveType", Value = 8)]
		UnknownTarget_EObjectiveType = 8,
		[ProtoEnum(Name = "Follow_EObjectiveType", Value = 9)]
		Follow_EObjectiveType = 9,
		[ProtoEnum(Name = "Free_EObjectiveType", Value = 10)]
		Free_EObjectiveType = 10,
		[ProtoEnum(Name = "Reach_EObjectiveType", Value = 11)]
		Reach_EObjectiveType = 11,
		[ProtoEnum(Name = "Tutorial_EObjectiveType", Value = 12)]
		Tutorial_EObjectiveType = 12,
		[ProtoEnum(Name = "None_EObjectiveType", Value = 42)]
		None_EObjectiveType = 42
	}
}
