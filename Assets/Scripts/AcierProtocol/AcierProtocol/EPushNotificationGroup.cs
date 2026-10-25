using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EPushNotificationGroup")]
	public enum EPushNotificationGroup
	{
		[ProtoEnum(Name = "SkillTrainingGroup", Value = 1)]
		SkillTrainingGroup = 1,
		[ProtoEnum(Name = "HirelingGroup", Value = 2)]
		HirelingGroup = 2,
		[ProtoEnum(Name = "BoostGroup", Value = 3)]
		BoostGroup = 3,
		[ProtoEnum(Name = "FriendGroup", Value = 4)]
		FriendGroup = 4,
		[ProtoEnum(Name = "SharedMissionGroup", Value = 5)]
		SharedMissionGroup = 5,
		[ProtoEnum(Name = "MailGroup", Value = 6)]
		MailGroup = 6,
		[ProtoEnum(Name = "EventGroup", Value = 7)]
		EventGroup = 7,
		[ProtoEnum(Name = "PremiumAccountGroup", Value = 8)]
		PremiumAccountGroup = 8
	}
}
