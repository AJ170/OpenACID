using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "ETutorialCategory")]
	public enum ETutorialCategory
	{
		[ProtoEnum(Name = "TutCatIntroMission", Value = 0)]
		TutCatIntroMission = 0,
		[ProtoEnum(Name = "TutCatAssassinCreation", Value = 1)]
		TutCatAssassinCreation = 1,
		[ProtoEnum(Name = "TutCatGuideEraSelect", Value = 2)]
		TutCatGuideEraSelect = 2,
		[ProtoEnum(Name = "TutCatGuideShop", Value = 5)]
		TutCatGuideShop = 5,
		[ProtoEnum(Name = "TutCatMs25MysteryBox", Value = 6)]
		TutCatMs25MysteryBox = 6,
		[ProtoEnum(Name = "TutCatMs25Equip", Value = 7)]
		TutCatMs25Equip = 7,
		[ProtoEnum(Name = "TutCatMs25SkillPoints", Value = 8)]
		TutCatMs25SkillPoints = 8,
		[ProtoEnum(Name = "TutCatMs25Forge", Value = 9)]
		TutCatMs25Forge = 9,
		[ProtoEnum(Name = "TutCatMs26Codex", Value = 10)]
		TutCatMs26Codex = 10,
		[ProtoEnum(Name = "TutCatMs26HeroicMode", Value = 11)]
		TutCatMs26HeroicMode = 11,
		[ProtoEnum(Name = "TutCatMs26Contracts", Value = 12)]
		TutCatMs26Contracts = 12,
		[ProtoEnum(Name = "TutCatMs26Modules", Value = 13)]
		TutCatMs26Modules = 13,
		[ProtoEnum(Name = "TutCatMs26QuestIntro", Value = 14)]
		TutCatMs26QuestIntro = 14,
		[ProtoEnum(Name = "TutCatMs26QuestRewards", Value = 15)]
		TutCatMs26QuestRewards = 15,
		[ProtoEnum(Name = "TutCatMs26Hirelings", Value = 16)]
		TutCatMs26Hirelings = 16
	}
}
