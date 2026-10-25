using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ProcessMailResponse")]
	public class ProcessMailResponse : IExtensible
	{
		private bool _removeMail;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "removeMail", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool removeMail
		{
			get
			{
				return _removeMail;
			}
			set
			{
				_removeMail = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
