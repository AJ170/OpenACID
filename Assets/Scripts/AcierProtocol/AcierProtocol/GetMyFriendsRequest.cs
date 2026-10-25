using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "GetMyFriendsRequest")]
	public class GetMyFriendsRequest : IExtensible
	{
		private bool _useCached = true;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "useCached", DataFormat = DataFormat.Default)]
		[DefaultValue(true)]
		public bool useCached
		{
			get
			{
				return _useCached;
			}
			set
			{
				_useCached = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
