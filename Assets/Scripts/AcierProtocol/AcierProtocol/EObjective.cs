using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EObjective")]
	public enum EObjective
	{
		[ProtoEnum(Name = "Deliver", Value = 0)]
		Deliver = 0,
		[ProtoEnum(Name = "DeliverNotoriety", Value = 1)]
		DeliverNotoriety = 1,
		[ProtoEnum(Name = "GetInRangeStatic", Value = 2)]
		GetInRangeStatic = 2,
		[ProtoEnum(Name = "GetInRangeMobile", Value = 17)]
		GetInRangeMobile = 17,
		[ProtoEnum(Name = "Kill", Value = 3)]
		Kill = 3,
		[ProtoEnum(Name = "KillEnemyType", Value = 4)]
		KillEnemyType = 4,
		[ProtoEnum(Name = "NotorietyObjective", Value = 5)]
		NotorietyObjective = 5,
		[ProtoEnum(Name = "NpcInteraction", Value = 6)]
		NpcInteraction = 6,
		[ProtoEnum(Name = "Parkour", Value = 7)]
		Parkour = 7,
		[ProtoEnum(Name = "TimeLimitObjective", Value = 8)]
		TimeLimitObjective = 8,
		[ProtoEnum(Name = "Loot", Value = 9)]
		Loot = 9,
		[ProtoEnum(Name = "BecomeAnonymous", Value = 11)]
		BecomeAnonymous = 11,
		[ProtoEnum(Name = "StayInRangeStatic", Value = 12)]
		StayInRangeStatic = 12,
		[ProtoEnum(Name = "StayInRangeMobile", Value = 13)]
		StayInRangeMobile = 13,
		[ProtoEnum(Name = "LimitObjective", Value = 14)]
		LimitObjective = 14,
		[ProtoEnum(Name = "SkillLimit", Value = 15)]
		SkillLimit = 15,
		[ProtoEnum(Name = "Escape", Value = 16)]
		Escape = 16,
		[ProtoEnum(Name = "Identify", Value = 18)]
		Identify = 18,
		[ProtoEnum(Name = "Escort", Value = 19)]
		Escort = 19,
		[ProtoEnum(Name = "GetToNextStart", Value = 20)]
		GetToNextStart = 20,
		[ProtoEnum(Name = "Spawn", Value = 21)]
		Spawn = 21,
		[ProtoEnum(Name = "Void", Value = 22)]
		Void = 22,
		[ProtoEnum(Name = "FreeCaptives", Value = 10)]
		FreeCaptives = 10
	}
}
