using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "FriendProfile")]
	public class FriendProfile : IExtensible
	{
		private UserProfile _userProfile;

		private EFriendType _friendType;

		private string _id;

		private string _userIdHash = "";

		private string _userName = "";

		private int _level;

		private long _lastActivity;

		private long _maxItemPower;

		private int _maxRankIdx;

		private FriendAssassin[] _friendAssassin = new FriendAssassin[0];

		private int _numAssassins;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "userProfile", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public UserProfile userProfile
		{
			get
			{
				return _userProfile;
			}
			set
			{
				_userProfile = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "friendType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EFriendType.FriendTypeBidirectional)]
		public EFriendType friendType
		{
			get
			{
				return _friendType;
			}
			set
			{
				_friendType = value;
			}
		}

		[ProtoMember(3, IsRequired = true, Name = "id", DataFormat = DataFormat.Default)]
		public string id
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "userIdHash", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string userIdHash
		{
			get
			{
				return _userIdHash;
			}
			set
			{
				_userIdHash = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "userName", DataFormat = DataFormat.Default)]
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

		[ProtoMember(6, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int level
		{
			get
			{
				return _level;
			}
			set
			{
				_level = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "lastActivity", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long lastActivity
		{
			get
			{
				return _lastActivity;
			}
			set
			{
				_lastActivity = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "maxItemPower", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long maxItemPower
		{
			get
			{
				return _maxItemPower;
			}
			set
			{
				_maxItemPower = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "maxRankIdx", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int maxRankIdx
		{
			get
			{
				return _maxRankIdx;
			}
			set
			{
				_maxRankIdx = value;
			}
		}

		[ProtoMember(10, Name = "friendAssassin", DataFormat = DataFormat.Default)]
		public FriendAssassin[] friendAssassin
		{
			get
			{
				return _friendAssassin;
			}
			set
			{
				Array.Resize(ref _friendAssassin, value.Length);
				value.CopyTo(_friendAssassin, 0);
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "numAssassins", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int numAssassins
		{
			get
			{
				return _numAssassins;
			}
			set
			{
				_numAssassins = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
