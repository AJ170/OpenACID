using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "RemoveFriendRequest")]
	public class RemoveFriendRequest : IExtensible
	{
		private string _friendId = "";

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "friendId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string friendId
		{
			get
			{
				return _friendId;
			}
			set
			{
				_friendId = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
