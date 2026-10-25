using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "LeaderBoardUser")]
	public class LeaderBoardUser : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "GeneratedLeaderBoardAssassin")]
		public class GeneratedLeaderBoardAssassin : IExtensible
		{
			private int _damage;

			private int _health;

			private int _armorRating;

			private Assassin.EClassTypeId _primaryClass = Assassin.EClassTypeId.None;

			private Assassin.EClassTypeId _secondaryClass = Assassin.EClassTypeId.None;

			private string _outfitId = "";

			private long _itemPower;

			private string _headDefGuid = "";

			private string _colorDefGuid = "";

			private long _eventScore;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "damage", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int damage
			{
				get
				{
					return _damage;
				}
				set
				{
					_damage = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "health", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int health
			{
				get
				{
					return _health;
				}
				set
				{
					_health = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "armorRating", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int armorRating
			{
				get
				{
					return _armorRating;
				}
				set
				{
					_armorRating = value;
				}
			}

			[ProtoMember(4, IsRequired = false, Name = "primaryClass", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(Assassin.EClassTypeId.None)]
			public Assassin.EClassTypeId primaryClass
			{
				get
				{
					return _primaryClass;
				}
				set
				{
					_primaryClass = value;
				}
			}

			[ProtoMember(5, IsRequired = false, Name = "secondaryClass", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(Assassin.EClassTypeId.None)]
			public Assassin.EClassTypeId secondaryClass
			{
				get
				{
					return _secondaryClass;
				}
				set
				{
					_secondaryClass = value;
				}
			}

			[ProtoMember(6, IsRequired = false, Name = "outfitId", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string outfitId
			{
				get
				{
					return _outfitId;
				}
				set
				{
					_outfitId = value;
				}
			}

			[ProtoMember(7, IsRequired = false, Name = "itemPower", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public long itemPower
			{
				get
				{
					return _itemPower;
				}
				set
				{
					_itemPower = value;
				}
			}

			[ProtoMember(8, IsRequired = false, Name = "headDefGuid", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string headDefGuid
			{
				get
				{
					return _headDefGuid;
				}
				set
				{
					_headDefGuid = value;
				}
			}

			[ProtoMember(9, IsRequired = false, Name = "colorDefGuid", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string colorDefGuid
			{
				get
				{
					return _colorDefGuid;
				}
				set
				{
					_colorDefGuid = value;
				}
			}

			[ProtoMember(10, IsRequired = false, Name = "eventScore", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public long eventScore
			{
				get
				{
					return _eventScore;
				}
				set
				{
					_eventScore = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private int _position;

		private string _userName = "";

		private string _userIdHash = "";

		private long _score;

		private bool _friend;

		private bool _currentUser;

		private Assassin _assassin;

		private int _rank;

		private GeneratedLeaderBoardAssassin _generatedLeaderBoardAssassin;

		private string _userId = "";

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "position", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int position
		{
			get
			{
				return _position;
			}
			set
			{
				_position = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "userName", DataFormat = DataFormat.Default)]
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

		[ProtoMember(3, IsRequired = false, Name = "userIdHash", DataFormat = DataFormat.Default)]
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

		[ProtoMember(4, IsRequired = false, Name = "score", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long score
		{
			get
			{
				return _score;
			}
			set
			{
				_score = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "friend", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool friend
		{
			get
			{
				return _friend;
			}
			set
			{
				_friend = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "currentUser", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool currentUser
		{
			get
			{
				return _currentUser;
			}
			set
			{
				_currentUser = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "assassin", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Assassin assassin
		{
			get
			{
				return _assassin;
			}
			set
			{
				_assassin = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "rank", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int rank
		{
			get
			{
				return _rank;
			}
			set
			{
				_rank = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "generatedLeaderBoardAssassin", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public GeneratedLeaderBoardAssassin generatedLeaderBoardAssassin
		{
			get
			{
				return _generatedLeaderBoardAssassin;
			}
			set
			{
				_generatedLeaderBoardAssassin = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "userId", DataFormat = DataFormat.Default)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
