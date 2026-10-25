using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MailHeaders")]
	public class MailHeaders : IExtensible
	{
		private MailHeader[] _headers = new MailHeader[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "headers", DataFormat = DataFormat.Default)]
		public MailHeader[] headers
		{
			get
			{
				return _headers;
			}
			set
			{
				Array.Resize(ref _headers, value.Length);
				value.CopyTo(_headers, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
