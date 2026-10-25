using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "GetMailBodyRequest")]
	public class GetMailBodyRequest : IExtensible
	{
		private string _mailId = "";

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "mailId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string mailId
		{
			get
			{
				return _mailId;
			}
			set
			{
				_mailId = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
