using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "SendMailRequest")]
	public class SendMailRequest : IExtensible
	{
		private string _receiverId = "";

		private string _subject = "";

		private string _body = "";

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "receiverId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string receiverId
		{
			get
			{
				return _receiverId;
			}
			set
			{
				_receiverId = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "subject", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string subject
		{
			get
			{
				return _subject;
			}
			set
			{
				_subject = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "body", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string body
		{
			get
			{
				return _body;
			}
			set
			{
				_body = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
