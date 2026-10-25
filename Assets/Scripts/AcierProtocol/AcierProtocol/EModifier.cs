using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EModifier")]
	public enum EModifier
	{
		[ProtoEnum(Name = "TimeLimit", Value = 0)]
		TimeLimit = 0,
		[ProtoEnum(Name = "NoRoof", Value = 1)]
		NoRoof = 1,
		[ProtoEnum(Name = "NoGround", Value = 2)]
		NoGround = 2,
		[ProtoEnum(Name = "NoHiding", Value = 3)]
		NoHiding = 3,
		[ProtoEnum(Name = "NoKill", Value = 4)]
		NoKill = 4,
		[ProtoEnum(Name = "NoAssassination", Value = 5)]
		NoAssassination = 5,
		[ProtoEnum(Name = "KeepNotoriety", Value = 6)]
		KeepNotoriety = 6,
		[ProtoEnum(Name = "NoFleeing", Value = 7)]
		NoFleeing = 7,
		[ProtoEnum(Name = "HPPercentage", Value = 8)]
		HPPercentage = 8,
		[ProtoEnum(Name = "NoSkills", Value = 9)]
		NoSkills = 9,
		[ProtoEnum(Name = "NoCombat", Value = 10)]
		NoCombat = 10,
		[ProtoEnum(Name = "OnlyAssassination", Value = 11)]
		OnlyAssassination = 11,
		[ProtoEnum(Name = "NoDoctor", Value = 12)]
		NoDoctor = 12
	}
}
