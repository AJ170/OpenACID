using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "ELimitType")]
	public enum ELimitType
	{
		[ProtoEnum(Name = "Roof", Value = 0)]
		Roof = 0,
		[ProtoEnum(Name = "Ground", Value = 1)]
		Ground = 1,
		[ProtoEnum(Name = "Hiding", Value = 2)]
		Hiding = 2,
		[ProtoEnum(Name = "HotspotHaystack", Value = 3)]
		HotspotHaystack = 3,
		[ProtoEnum(Name = "HotspotBench", Value = 4)]
		HotspotBench = 4,
		[ProtoEnum(Name = "HotspotRooftopGarden", Value = 5)]
		HotspotRooftopGarden = 5,
		[ProtoEnum(Name = "HotspotCivilianCover", Value = 6)]
		HotspotCivilianCover = 6,
		[ProtoEnum(Name = "HotspotDoctor", Value = 7)]
		HotspotDoctor = 7,
		[ProtoEnum(Name = "HotspotCaptives", Value = 8)]
		HotspotCaptives = 8,
		[ProtoEnum(Name = "HotspotChests", Value = 9)]
		HotspotChests = 9,
		[ProtoEnum(Name = "HotspotPigeonCoup", Value = 10)]
		HotspotPigeonCoup = 10,
		[ProtoEnum(Name = "HotspotCourtesans", Value = 11)]
		HotspotCourtesans = 11,
		[ProtoEnum(Name = "HotspotThieves", Value = 12)]
		HotspotThieves = 12,
		[ProtoEnum(Name = "Fleeing", Value = 13)]
		Fleeing = 13,
		[ProtoEnum(Name = "Skills", Value = 14)]
		Skills = 14,
		[ProtoEnum(Name = "Combat", Value = 15)]
		Combat = 15,
		[ProtoEnum(Name = "NotorietyAnonymous", Value = 16)]
		NotorietyAnonymous = 16,
		[ProtoEnum(Name = "NotorietyExposed", Value = 17)]
		NotorietyExposed = 17,
		[ProtoEnum(Name = "NotorietyAlterted", Value = 18)]
		NotorietyAlterted = 18,
		[ProtoEnum(Name = "LowestBaseHealthPercent", Value = 19)]
		LowestBaseHealthPercent = 19,
		[ProtoEnum(Name = "KilledUnitsInCombat", Value = 20)]
		KilledUnitsInCombat = 20,
		[ProtoEnum(Name = "KilledUnitsTotal", Value = 21)]
		KilledUnitsTotal = 21,
		[ProtoEnum(Name = "KilledUnitsByAssassination", Value = 22)]
		KilledUnitsByAssassination = 22,
		[ProtoEnum(Name = "LostCaptives", Value = 23)]
		LostCaptives = 23,
		[ProtoEnum(Name = "SuccessfullyParried", Value = 24)]
		SuccessfullyParried = 24,
		[ProtoEnum(Name = "SwitchedTarget", Value = 25)]
		SwitchedTarget = 25,
		[ProtoEnum(Name = "ExecutedKillMoves", Value = 26)]
		ExecutedKillMoves = 26,
		[ProtoEnum(Name = "SuccessfullHirelingAttacks", Value = 27)]
		SuccessfullHirelingAttacks = 27
	}
}
