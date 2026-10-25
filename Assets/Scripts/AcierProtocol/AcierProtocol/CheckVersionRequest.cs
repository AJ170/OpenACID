using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "CheckVersionRequest")]
	public class CheckVersionRequest : IExtensible
	{
		private uint _protocolVersion;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = true, Name = "protocolVersion", DataFormat = DataFormat.TwosComplement)]
		public uint protocolVersion
		{
			get
			{
				return _protocolVersion;
			}
			set
			{
				_protocolVersion = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
