using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MailBody")]
	public class MailBody : IExtensible
	{
		private string _bodyId = "";

		private string _body = "";

		private MailAttachment _attachment;

		private LangKey _bodyLangKey;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "bodyId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string bodyId
		{
			get
			{
				return _bodyId;
			}
			set
			{
				_bodyId = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "body", DataFormat = DataFormat.Default)]
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

		[ProtoMember(3, IsRequired = false, Name = "attachment", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MailAttachment attachment
		{
			get
			{
				return _attachment;
			}
			set
			{
				_attachment = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "bodyLangKey", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LangKey bodyLangKey
		{
			get
			{
				return _bodyLangKey;
			}
			set
			{
				_bodyLangKey = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
