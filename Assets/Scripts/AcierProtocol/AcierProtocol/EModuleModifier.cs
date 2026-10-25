using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EModuleModifier")]
	public enum EModuleModifier
	{
		[ProtoEnum(Name = "ClientModifier", Value = 0)]
		ClientModifier = 0,
		[ProtoEnum(Name = "SCReward", Value = 1)]
		SCReward = 1,
		[ProtoEnum(Name = "XPReward", Value = 2)]
		XPReward = 2,
		[ProtoEnum(Name = "ItemFind", Value = 3)]
		ItemFind = 3,
		[ProtoEnum(Name = "SkillTrainingTimeFactor", Value = 4)]
		SkillTrainingTimeFactor = 4,
		[ProtoEnum(Name = "HTReward", Value = 5)]
		HTReward = 5
	}
}
