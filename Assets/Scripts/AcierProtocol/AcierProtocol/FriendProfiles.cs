using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "FriendProfiles")]
	public class FriendProfiles : IExtensible
	{
		private FriendProfile[] _friendProfiles = new FriendProfile[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "friendProfiles", DataFormat = DataFormat.Default)]
		public FriendProfile[] friendProfiles
		{
			get
			{
				return _friendProfiles;
			}
			set
			{
				Array.Resize(ref _friendProfiles, value.Length);
				value.CopyTo(_friendProfiles, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
