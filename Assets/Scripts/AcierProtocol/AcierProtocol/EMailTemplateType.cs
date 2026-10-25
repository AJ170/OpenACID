using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EMailTemplateType")]
	public enum EMailTemplateType
	{
		[ProtoEnum(Name = "WelcomeMail", Value = 1)]
		WelcomeMail = 1,
		[ProtoEnum(Name = "ShopTutorialMail", Value = 2)]
		ShopTutorialMail = 2,
		[ProtoEnum(Name = "GiftMail", Value = 3)]
		GiftMail = 3,
		[ProtoEnum(Name = "FriendRewardMail", Value = 4)]
		FriendRewardMail = 4,
		[ProtoEnum(Name = "FriendNoRewardMail", Value = 5)]
		FriendNoRewardMail = 5,
		[ProtoEnum(Name = "FullInventoryMail", Value = 6)]
		FullInventoryMail = 6,
		[ProtoEnum(Name = "FullMailbox", Value = 7)]
		FullMailbox = 7,
		[ProtoEnum(Name = "FriendRequestRejected", Value = 8)]
		FriendRequestRejected = 8,
		[ProtoEnum(Name = "FriendRequestRewardReceived", Value = 9)]
		FriendRequestRewardReceived = 9,
		[ProtoEnum(Name = "FriendRequestNoRewardReceived", Value = 10)]
		FriendRequestNoRewardReceived = 10,
		[ProtoEnum(Name = "GiftMailRejected", Value = 11)]
		GiftMailRejected = 11,
		[ProtoEnum(Name = "SupportItem", Value = 12)]
		SupportItem = 12,
		[ProtoEnum(Name = "SupportNoItem", Value = 13)]
		SupportNoItem = 13,
		[ProtoEnum(Name = "AchievementItem", Value = 14)]
		AchievementItem = 14,
		[ProtoEnum(Name = "MissionRewardItem", Value = 15)]
		MissionRewardItem = 15,
		[ProtoEnum(Name = "DailyRewardItem", Value = 16)]
		DailyRewardItem = 16,
		[ProtoEnum(Name = "ShareMission", Value = 17)]
		ShareMission = 17,
		[ProtoEnum(Name = "ShareMissionAcceptedReward", Value = 18)]
		ShareMissionAcceptedReward = 18,
		[ProtoEnum(Name = "ShareMissionBeatReward", Value = 19)]
		ShareMissionBeatReward = 19,
		[ProtoEnum(Name = "ShareMissionBeatSenderReward", Value = 20)]
		ShareMissionBeatSenderReward = 20,
		[ProtoEnum(Name = "ShareMissionAcceptedRewardForReceiver", Value = 21)]
		ShareMissionAcceptedRewardForReceiver = 21,
		[ProtoEnum(Name = "MysteryBoxRewardItem", Value = 22)]
		MysteryBoxRewardItem = 22,
		[ProtoEnum(Name = "EventRewardItem", Value = 23)]
		EventRewardItem = 23,
		[ProtoEnum(Name = "AppleExclusiveWinterOutfit", Value = 24)]
		AppleExclusiveWinterOutfit = 24,
		[ProtoEnum(Name = "ShopPurchaseFullInventoryMail", Value = 25)]
		ShopPurchaseFullInventoryMail = 25,
		[ProtoEnum(Name = "DecryptionMigrationMail", Value = 101)]
		DecryptionMigrationMail = 101,
		[ProtoEnum(Name = "ModulesMigrationMail", Value = 102)]
		ModulesMigrationMail = 102,
		[ProtoEnum(Name = "InventoryMigrationMail", Value = 103)]
		InventoryMigrationMail = 103,
		[ProtoEnum(Name = "AchievementMigrationMail", Value = 104)]
		AchievementMigrationMail = 104,
		[ProtoEnum(Name = "BetaRewardMigrationMail", Value = 105)]
		BetaRewardMigrationMail = 105,
		[ProtoEnum(Name = "AdColonyRewardMail", Value = 106)]
		AdColonyRewardMail = 106,
		[ProtoEnum(Name = "MS31MigrationMail", Value = 107)]
		MS31MigrationMail = 107,
		[ProtoEnum(Name = "HirelingRewardMail", Value = 108)]
		HirelingRewardMail = 108,
		[ProtoEnum(Name = "FriendRequestAcceptedMail", Value = 109)]
		FriendRequestAcceptedMail = 109,
		[ProtoEnum(Name = "FriendRequestReceivedMail", Value = 110)]
		FriendRequestReceivedMail = 110
	}
}
