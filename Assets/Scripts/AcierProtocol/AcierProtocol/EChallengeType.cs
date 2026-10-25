using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EChallengeType")]
	public enum EChallengeType
	{
		[ProtoEnum(Name = "Invalid_EChallengeType", Value = 0)]
		Invalid_EChallengeType = 0,
		[ProtoEnum(Name = "HealthLimit_EChallengeType", Value = 1)]
		HealthLimit_EChallengeType = 1,
		[ProtoEnum(Name = "KillTypeLimit_EChallengeType", Value = 2)]
		KillTypeLimit_EChallengeType = 2,
		[ProtoEnum(Name = "DontKillTypeLimit_EChallengeType", Value = 3)]
		DontKillTypeLimit_EChallengeType = 3,
		[ProtoEnum(Name = "NoAssassination_EChallengeType", Value = 4)]
		NoAssassination_EChallengeType = 4,
		[ProtoEnum(Name = "NoCombat_EChallengeType", Value = 5)]
		NoCombat_EChallengeType = 5,
		[ProtoEnum(Name = "NoDoctor_EChallengeType", Value = 6)]
		NoDoctor_EChallengeType = 6,
		[ProtoEnum(Name = "NoFleeing_EChallengeType", Value = 7)]
		NoFleeing_EChallengeType = 7,
		[ProtoEnum(Name = "NoHidingSpots_EChallengeType", Value = 8)]
		NoHidingSpots_EChallengeType = 8,
		[ProtoEnum(Name = "NoKill_EChallengeType", Value = 9)]
		NoKill_EChallengeType = 9,
		[ProtoEnum(Name = "NoSkills_EChallengeType", Value = 10)]
		NoSkills_EChallengeType = 10,
		[ProtoEnum(Name = "OnlyAssassinations_EChallengeType", Value = 11)]
		OnlyAssassinations_EChallengeType = 11,
		[ProtoEnum(Name = "StayAnonymous_EChallengeType", Value = 12)]
		StayAnonymous_EChallengeType = 12,
		[ProtoEnum(Name = "StayUndetected_EChallengeType", Value = 13)]
		StayUndetected_EChallengeType = 13,
		[ProtoEnum(Name = "TimeLimit_EChallengeType", Value = 14)]
		TimeLimit_EChallengeType = 14,
		[ProtoEnum(Name = "TreasureChest_EChallengeType", Value = 15)]
		TreasureChest_EChallengeType = 15
	}
}
