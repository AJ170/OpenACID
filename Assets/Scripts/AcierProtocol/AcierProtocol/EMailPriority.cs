using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EMailPriority")]
	public enum EMailPriority
	{
		[ProtoEnum(Name = "MailPriorityLow", Value = 0)]
		MailPriorityLow = 0,
		[ProtoEnum(Name = "MailPriorityMedium", Value = 1)]
		MailPriorityMedium = 1,
		[ProtoEnum(Name = "MailPriorityHigh", Value = 2)]
		MailPriorityHigh = 2
	}
}
