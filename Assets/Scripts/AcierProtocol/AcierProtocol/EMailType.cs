using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EMailType")]
	public enum EMailType
	{
		[ProtoEnum(Name = "MailTypePlayer2Player", Value = 0)]
		MailTypePlayer2Player = 0,
		[ProtoEnum(Name = "MailTypeSystem", Value = 1)]
		MailTypeSystem = 1,
		[ProtoEnum(Name = "MailTypeMission", Value = 2)]
		MailTypeMission = 2
	}
}
