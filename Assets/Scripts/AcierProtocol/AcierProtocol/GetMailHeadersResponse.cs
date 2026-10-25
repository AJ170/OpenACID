using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "GetMailHeadersResponse")]
	public class GetMailHeadersResponse : IExtensible
	{
		private MailHeaders _headers;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "headers", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MailHeaders headers
		{
			get
			{
				return _headers;
			}
			set
			{
				_headers = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
