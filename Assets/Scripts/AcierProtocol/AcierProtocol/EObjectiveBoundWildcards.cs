using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EObjectiveBoundWildcards")]
	public enum EObjectiveBoundWildcards
	{
		[ProtoEnum(Name = "Count_EObjectiveBoundWildcards", Value = 1)]
		Count_EObjectiveBoundWildcards = 1,
		[ProtoEnum(Name = "TimeInSeconds_EObjectiveBoundWildcards", Value = 2)]
		TimeInSeconds_EObjectiveBoundWildcards = 2,
		[ProtoEnum(Name = "UnitGuids_EObjectiveBoundWildcards", Value = 3)]
		UnitGuids_EObjectiveBoundWildcards = 3,
		[ProtoEnum(Name = "ChallengerCount_EObjectiveBoundWildcards", Value = 4)]
		ChallengerCount_EObjectiveBoundWildcards = 4,
		[ProtoEnum(Name = "StaticObjectGuid_EObjectiveBoundWildcards", Value = 5)]
		StaticObjectGuid_EObjectiveBoundWildcards = 5,
		[ProtoEnum(Name = "SkillName_EObjectiveBoundWildcards", Value = 6)]
		SkillName_EObjectiveBoundWildcards = 6,
		[ProtoEnum(Name = "Notoriety_EObjectiveBoundWildcards", Value = 7)]
		Notoriety_EObjectiveBoundWildcards = 7,
		[ProtoEnum(Name = "Range_EObjectiveBoundWildcards", Value = 8)]
		Range_EObjectiveBoundWildcards = 8,
		[ProtoEnum(Name = "Available_EObjectiveBoundWildcards", Value = 9)]
		Available_EObjectiveBoundWildcards = 9
	}
}
