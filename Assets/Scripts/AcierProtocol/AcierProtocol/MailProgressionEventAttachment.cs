using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MailProgressionEventAttachment")]
	public class MailProgressionEventAttachment : IExtensible
	{
		private int _eventHeaderLocaKey;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "eventHeaderLocaKey", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int eventHeaderLocaKey
		{
			get
			{
				return _eventHeaderLocaKey;
			}
			set
			{
				_eventHeaderLocaKey = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
