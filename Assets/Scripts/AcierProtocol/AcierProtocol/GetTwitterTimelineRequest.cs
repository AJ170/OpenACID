using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "GetTwitterTimelineRequest")]
	public class GetTwitterTimelineRequest : IExtensible
	{
		private ELanguages _language;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "language", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(ELanguages.BlueByte)]
		public ELanguages language
		{
			get
			{
				return _language;
			}
			set
			{
				_language = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
