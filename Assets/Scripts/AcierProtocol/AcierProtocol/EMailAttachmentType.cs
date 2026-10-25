using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EMailAttachmentType")]
	public enum EMailAttachmentType
	{
		[ProtoEnum(Name = "MailAttachmentTypeNone", Value = 1)]
		MailAttachmentTypeNone = 1,
		[ProtoEnum(Name = "MailAttachmentTypeFriendRequest", Value = 2)]
		MailAttachmentTypeFriendRequest = 2,
		[ProtoEnum(Name = "MailAttachmentTypeGift", Value = 3)]
		MailAttachmentTypeGift = 3,
		[ProtoEnum(Name = "MailAttachmentTypeShareMission", Value = 4)]
		MailAttachmentTypeShareMission = 4,
		[ProtoEnum(Name = "MailAttachmentTypeProgressionEvents", Value = 5)]
		MailAttachmentTypeProgressionEvents = 5,
		[ProtoEnum(Name = "MailAttachmentTypeHirelingUsageStatistics", Value = 6)]
		MailAttachmentTypeHirelingUsageStatistics = 6
	}
}
