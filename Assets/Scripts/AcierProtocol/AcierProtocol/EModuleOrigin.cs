using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EModuleOrigin")]
	public enum EModuleOrigin
	{
		[ProtoEnum(Name = "ModuleOrigin_DailyReward", Value = 0)]
		ModuleOrigin_DailyReward = 0,
		[ProtoEnum(Name = "ModuleOrigin_Mission", Value = 1)]
		ModuleOrigin_Mission = 1,
		[ProtoEnum(Name = "ModuleOrigin_Achievement", Value = 2)]
		ModuleOrigin_Achievement = 2,
		[ProtoEnum(Name = "ModuleOrigin_PlayerGift", Value = 3)]
		ModuleOrigin_PlayerGift = 3,
		[ProtoEnum(Name = "ModuleOrigin_Shop", Value = 4)]
		ModuleOrigin_Shop = 4,
		[ProtoEnum(Name = "ModuleOrigin_Support", Value = 5)]
		ModuleOrigin_Support = 5,
		[ProtoEnum(Name = "ModuleOrigin_System", Value = 6)]
		ModuleOrigin_System = 6,
		[ProtoEnum(Name = "ModuleOrigin_Decryption", Value = 7)]
		ModuleOrigin_Decryption = 7,
		[ProtoEnum(Name = "ModuleOrigin_Invalid", Value = 8)]
		ModuleOrigin_Invalid = 8,
		[ProtoEnum(Name = "ModuleOrigin_ProgressionQuest", Value = 18)]
		ModuleOrigin_ProgressionQuest = 18,
		[ProtoEnum(Name = "ModuleOrigin_Events", Value = 19)]
		ModuleOrigin_Events = 19,
		[ProtoEnum(Name = "Origin_Facebook", Value = 9)]
		Origin_Facebook = 9,
		[ProtoEnum(Name = "Origin_HirelingPopup", Value = 10)]
		Origin_HirelingPopup = 10,
		[ProtoEnum(Name = "Origin_FriendScreen", Value = 11)]
		Origin_FriendScreen = 11,
		[ProtoEnum(Name = "Origin_SocialScreen", Value = 12)]
		Origin_SocialScreen = 12,
		[ProtoEnum(Name = "Origin_CharacterCreation", Value = 13)]
		Origin_CharacterCreation = 13,
		[ProtoEnum(Name = "Origin_RollSimulation", Value = 14)]
		Origin_RollSimulation = 14,
		[ProtoEnum(Name = "Origin_Migration", Value = 15)]
		Origin_Migration = 15,
		[ProtoEnum(Name = "Origin_MysteryBox", Value = 16)]
		Origin_MysteryBox = 16,
		[ProtoEnum(Name = "Origin_Forge", Value = 17)]
		Origin_Forge = 17,
		[ProtoEnum(Name = "Origin_AdColonyReward", Value = 20)]
		Origin_AdColonyReward = 20,
		[ProtoEnum(Name = "Origin_UPlay", Value = 21)]
		Origin_UPlay = 21,
		[ProtoEnum(Name = "Origin_Weibo", Value = 22)]
		Origin_Weibo = 22
	}
}
