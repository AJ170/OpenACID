using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "GetMyFriendsResponse")]
	public class GetMyFriendsResponse : IExtensible
	{
		private FriendProfiles _friends;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "friends", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public FriendProfiles friends
		{
			get
			{
				return _friends;
			}
			set
			{
				_friends = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
