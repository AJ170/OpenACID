using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ProcessMailRequest")]
	public class ProcessMailRequest : IExtensible
	{
		private string _mailId = "";

		private bool _accept;

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

		[ProtoMember(2, IsRequired = false, Name = "accept", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool accept
		{
			get
			{
				return _accept;
			}
			set
			{
				_accept = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
