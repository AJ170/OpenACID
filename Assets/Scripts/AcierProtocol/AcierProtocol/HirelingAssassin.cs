using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "HirelingAssassin")]
	public class HirelingAssassin : IExtensible
	{
		[ProtoContract(Name = "EHirelingType")]
		public enum EHirelingType
		{
			[ProtoEnum(Name = "Random", Value = 1)]
			Random = 1,
			[ProtoEnum(Name = "RandomGenerated", Value = 2)]
			RandomGenerated = 2,
			[ProtoEnum(Name = "Friend", Value = 3)]
			Friend = 3
		}

		private string _userIdHash = "";

		private EHirelingType _hirelingType;

		private string _userName = "";

		private string _humanoidGuid = "";

		private string _headGuid = "";

		private Assassin.EClassTypeId _classTypeId = Assassin.EClassTypeId.None;

		private string _hirelingId = "";

		private int _level;

		private string _weaponGuid = "";

		private string _colorSetting = "";

		private int _rankIdx;

		private AssassinName _assassinName;

		private int _itemPower;

		private string _insigniaGuid = "";

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "userIdHash", DataFormat = DataFormat.Default)]
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

		[ProtoMember(3, IsRequired = true, Name = "hirelingType", DataFormat = DataFormat.TwosComplement)]
		public EHirelingType hirelingType
		{
			get
			{
				return _hirelingType;
			}
			set
			{
				_hirelingType = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "userName", DataFormat = DataFormat.Default)]
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

		[ProtoMember(6, IsRequired = false, Name = "humanoidGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string humanoidGuid
		{
			get
			{
				return _humanoidGuid;
			}
			set
			{
				_humanoidGuid = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "headGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string headGuid
		{
			get
			{
				return _headGuid;
			}
			set
			{
				_headGuid = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "classTypeId", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(Assassin.EClassTypeId.None)]
		public Assassin.EClassTypeId classTypeId
		{
			get
			{
				return _classTypeId;
			}
			set
			{
				_classTypeId = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "hirelingId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string hirelingId
		{
			get
			{
				return _hirelingId;
			}
			set
			{
				_hirelingId = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
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

		[ProtoMember(11, IsRequired = false, Name = "weaponGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string weaponGuid
		{
			get
			{
				return _weaponGuid;
			}
			set
			{
				_weaponGuid = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "colorSetting", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string colorSetting
		{
			get
			{
				return _colorSetting;
			}
			set
			{
				_colorSetting = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "rankIdx", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int rankIdx
		{
			get
			{
				return _rankIdx;
			}
			set
			{
				_rankIdx = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "assassinName", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public AssassinName assassinName
		{
			get
			{
				return _assassinName;
			}
			set
			{
				_assassinName = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "itemPower", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int itemPower
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

		[ProtoMember(16, IsRequired = false, Name = "insigniaGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string insigniaGuid
		{
			get
			{
				return _insigniaGuid;
			}
			set
			{
				_insigniaGuid = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
