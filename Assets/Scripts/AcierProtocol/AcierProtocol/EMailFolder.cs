using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EMailFolder")]
	public enum EMailFolder
	{
		[ProtoEnum(Name = "MailFolderNews", Value = 0)]
		MailFolderNews = 0,
		[ProtoEnum(Name = "MailFolderSocial", Value = 1)]
		MailFolderSocial = 1
	}
}
