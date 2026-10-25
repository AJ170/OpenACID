using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EPushNotificationType")]
	public enum EPushNotificationType
	{
		[ProtoEnum(Name = "SkillTrainingFinished", Value = 1)]
		SkillTrainingFinished = 1,
		[ProtoEnum(Name = "HirelingPicked", Value = 2)]
		HirelingPicked = 2,
		[ProtoEnum(Name = "ModuleExpired", Value = 3)]
		ModuleExpired = 3,
		[ProtoEnum(Name = "ModuleExpiresSoon", Value = 4)]
		ModuleExpiresSoon = 4,
		[ProtoEnum(Name = "FriendRequestReceived", Value = 5)]
		FriendRequestReceived = 5,
		[ProtoEnum(Name = "FriendRequestAccepted", Value = 6)]
		FriendRequestAccepted = 6,
		[ProtoEnum(Name = "SharedMissionInvitationReceived", Value = 7)]
		SharedMissionInvitationReceived = 7,
		[ProtoEnum(Name = "SharedMissionInvitationAccepted", Value = 8)]
		SharedMissionInvitationAccepted = 8,
		[ProtoEnum(Name = "NewFriendMail", Value = 9)]
		NewFriendMail = 9,
		[ProtoEnum(Name = "NewSystemMail", Value = 10)]
		NewSystemMail = 10,
		[ProtoEnum(Name = "PremiumAccountExpiresSoon", Value = 11)]
		PremiumAccountExpiresSoon = 11,
		[ProtoEnum(Name = "PremiumAccountExpired", Value = 12)]
		PremiumAccountExpired = 12,
		[ProtoEnum(Name = "ItemDiscountEventStarted", Value = 13)]
		ItemDiscountEventStarted = 13,
		[ProtoEnum(Name = "HardCurrencyHappyHourStarted", Value = 14)]
		HardCurrencyHappyHourStarted = 14,
		[ProtoEnum(Name = "ProgressionChallengesEventStarted", Value = 15)]
		ProgressionChallengesEventStarted = 15,
		[ProtoEnum(Name = "ProgressionChallengesEventAboutToEnd", Value = 16)]
		ProgressionChallengesEventAboutToEnd = 16
	}
}
