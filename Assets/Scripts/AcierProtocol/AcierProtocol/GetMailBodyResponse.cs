using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "GetMailBodyResponse")]
	public class GetMailBodyResponse : IExtensible
	{
		private MailBody _mailBody;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "mailBody", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MailBody mailBody
		{
			get
			{
				return _mailBody;
			}
			set
			{
				_mailBody = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
