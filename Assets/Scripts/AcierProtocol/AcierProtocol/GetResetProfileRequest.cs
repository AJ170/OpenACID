using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "GetResetProfileRequest")]
	public class GetResetProfileRequest : IExtensible
	{
		private IExtension extensionObject;

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
