using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EMissionKind")]
	public enum EMissionKind
	{
		[ProtoEnum(Name = "Escort_EMissionKind", Value = 0)]
		Escort_EMissionKind = 0,
		[ProtoEnum(Name = "IdentifyKill_EMissionKind", Value = 1)]
		IdentifyKill_EMissionKind = 1,
		[ProtoEnum(Name = "KillType_EMissionKind", Value = 2)]
		KillType_EMissionKind = 2,
		[ProtoEnum(Name = "FreeCaptives_EMissionKind", Value = 3)]
		FreeCaptives_EMissionKind = 3,
		[ProtoEnum(Name = "ContractKill_EMissionKind", Value = 4)]
		ContractKill_EMissionKind = 4,
		[ProtoEnum(Name = "Deliver_EMissionKind", Value = 5)]
		Deliver_EMissionKind = 5,
		[ProtoEnum(Name = "ParkourRace_EMissionKind", Value = 6)]
		ParkourRace_EMissionKind = 6,
		[ProtoEnum(Name = "Gather_EMissionKind", Value = 7)]
		Gather_EMissionKind = 7,
		[ProtoEnum(Name = "Loot_EMissionKind", Value = 8)]
		Loot_EMissionKind = 8,
		[ProtoEnum(Name = "Kill_EMissionKind", Value = 9)]
		Kill_EMissionKind = 9,
		[ProtoEnum(Name = "TailKill_EMissionKind", Value = 10)]
		TailKill_EMissionKind = 10,
		[ProtoEnum(Name = "Tail_EMissionKind", Value = 11)]
		Tail_EMissionKind = 11,
		[ProtoEnum(Name = "CrowHunt_EMissionKind", Value = 12)]
		CrowHunt_EMissionKind = 12
	}
}
