using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MailFriendRequest")]
	public class MailFriendRequest : IExtensible
	{
		private string _userName = "";

		private string _userId = "";

		private bool _withReward;

		private int _userlevel;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "userName", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string userName
		{
			get
			{
				return _userName;
			}
			set
			{
				_userName = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "userId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string userId
		{
			get
			{
				return _userId;
			}
			set
			{
				_userId = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "withReward", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool withReward
		{
			get
			{
				return _withReward;
			}
			set
			{
				_withReward = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "userlevel", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int userlevel
		{
			get
			{
				return _userlevel;
			}
			set
			{
				_userlevel = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
