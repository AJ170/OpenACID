using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EModuleType")]
	public enum EModuleType
	{
		[ProtoEnum(Name = "DamageAlgorithm", Value = 0)]
		DamageAlgorithm = 0,
		[ProtoEnum(Name = "ThreatBroadcasting", Value = 1)]
		ThreatBroadcasting = 1,
		[ProtoEnum(Name = "SecurityPack", Value = 2)]
		SecurityPack = 2,
		[ProtoEnum(Name = "AvoidanceProtocol", Value = 3)]
		AvoidanceProtocol = 3,
		[ProtoEnum(Name = "RewardUpgrade", Value = 4)]
		RewardUpgrade = 4,
		[ProtoEnum(Name = "Overclocking", Value = 5)]
		Overclocking = 5,
		[ProtoEnum(Name = "MemoryExpansion", Value = 6)]
		MemoryExpansion = 6,
		[ProtoEnum(Name = "Module_Damage", Value = 7)]
		Module_Damage = 7,
		[ProtoEnum(Name = "Module_Defense", Value = 8)]
		Module_Defense = 8,
		[ProtoEnum(Name = "Module_Stealth", Value = 9)]
		Module_Stealth = 9,
		[ProtoEnum(Name = "Module_SC", Value = 10)]
		Module_SC = 10,
		[ProtoEnum(Name = "Module_XP", Value = 11)]
		Module_XP = 11,
		[ProtoEnum(Name = "Module_HT", Value = 12)]
		Module_HT = 12
	}
}
