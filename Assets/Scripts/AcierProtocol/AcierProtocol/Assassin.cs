using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "Assassin")]
	public class Assassin : IExtensible
	{
		[ProtoContract(Name = "EClassTypeId")]
		public enum EClassTypeId
		{
			[ProtoEnum(Name = "None", Value = 1)]
			None = 1,
			[ProtoEnum(Name = "Berserker", Value = 2)]
			Berserker = 2,
			[ProtoEnum(Name = "Shadowblade", Value = 3)]
			Shadowblade = 3,
			[ProtoEnum(Name = "Trickster", Value = 4)]
			Trickster = 4,
			[ProtoEnum(Name = "Thief", Value = 5)]
			Thief = 5
		}

		private string _id;

		private int _skillPoints;

		private EClassTypeId _classTypeId = EClassTypeId.None;

		private EClassTypeId _extendedClassTypeId = EClassTypeId.None;

		private int _level;

		private long _xp;

		private long _hitPoints;

		private long _strength;

		private long _precision;

		private long _baseWeapon;

		private long _armoreRating;

		private long _incognito;

		private long _evasion;

		private string _humanoidGuid = "";

		private string _weaponGuid = "";

		private string _headGuid = "";

		private EquippedElement[] _equippedElements = new EquippedElement[0];

		private AssassinSkillTiers _baseClassskillTiers;

		private AssassinSkillTiers _extendedClassskillTiers;

		private AssassinSkillInTraining _assassinSkillInTraining;

		private SkillWheel _skillWheel;

		private int _pointsToSpendForExtendedClass;

		private string _colorSetting = "";

		private HirelingAssassins _hirelingAssassins;

		private EnergyBar _energyBar;

		private AssassinName _assassinName;

		private long _itemPower;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = true, Name = "id", DataFormat = DataFormat.Default)]
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

		[ProtoMember(3, IsRequired = false, Name = "skillPoints", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int skillPoints
		{
			get
			{
				return _skillPoints;
			}
			set
			{
				_skillPoints = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "classTypeId", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EClassTypeId.None)]
		public EClassTypeId classTypeId
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

		[ProtoMember(5, IsRequired = false, Name = "extendedClassTypeId", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EClassTypeId.None)]
		public EClassTypeId extendedClassTypeId
		{
			get
			{
				return _extendedClassTypeId;
			}
			set
			{
				_extendedClassTypeId = value;
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

		[ProtoMember(7, IsRequired = false, Name = "xp", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long xp
		{
			get
			{
				return _xp;
			}
			set
			{
				_xp = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "hitPoints", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long hitPoints
		{
			get
			{
				return _hitPoints;
			}
			set
			{
				_hitPoints = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "strength", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long strength
		{
			get
			{
				return _strength;
			}
			set
			{
				_strength = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "precision", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long precision
		{
			get
			{
				return _precision;
			}
			set
			{
				_precision = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "baseWeapon", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long baseWeapon
		{
			get
			{
				return _baseWeapon;
			}
			set
			{
				_baseWeapon = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "armoreRating", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long armoreRating
		{
			get
			{
				return _armoreRating;
			}
			set
			{
				_armoreRating = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "incognito", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long incognito
		{
			get
			{
				return _incognito;
			}
			set
			{
				_incognito = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "evasion", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long evasion
		{
			get
			{
				return _evasion;
			}
			set
			{
				_evasion = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "humanoidGuid", DataFormat = DataFormat.Default)]
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

		[ProtoMember(16, IsRequired = false, Name = "weaponGuid", DataFormat = DataFormat.Default)]
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

		[ProtoMember(17, IsRequired = false, Name = "headGuid", DataFormat = DataFormat.Default)]
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

		[ProtoMember(18, Name = "equippedElements", DataFormat = DataFormat.Default)]
		public EquippedElement[] equippedElements
		{
			get
			{
				return _equippedElements;
			}
			set
			{
				Array.Resize(ref _equippedElements, value.Length);
				value.CopyTo(_equippedElements, 0);
			}
		}

		[ProtoMember(20, IsRequired = false, Name = "baseClassskillTiers", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public AssassinSkillTiers baseClassskillTiers
		{
			get
			{
				return _baseClassskillTiers;
			}
			set
			{
				_baseClassskillTiers = value;
			}
		}

		[ProtoMember(21, IsRequired = false, Name = "extendedClassskillTiers", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public AssassinSkillTiers extendedClassskillTiers
		{
			get
			{
				return _extendedClassskillTiers;
			}
			set
			{
				_extendedClassskillTiers = value;
			}
		}

		[ProtoMember(22, IsRequired = false, Name = "assassinSkillInTraining", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public AssassinSkillInTraining assassinSkillInTraining
		{
			get
			{
				return _assassinSkillInTraining;
			}
			set
			{
				_assassinSkillInTraining = value;
			}
		}

		[ProtoMember(23, IsRequired = false, Name = "skillWheel", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public SkillWheel skillWheel
		{
			get
			{
				return _skillWheel;
			}
			set
			{
				_skillWheel = value;
			}
		}

		[ProtoMember(24, IsRequired = false, Name = "pointsToSpendForExtendedClass", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int pointsToSpendForExtendedClass
		{
			get
			{
				return _pointsToSpendForExtendedClass;
			}
			set
			{
				_pointsToSpendForExtendedClass = value;
			}
		}

		[ProtoMember(25, IsRequired = false, Name = "colorSetting", DataFormat = DataFormat.Default)]
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

		[ProtoMember(27, IsRequired = false, Name = "hirelingAssassins", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public HirelingAssassins hirelingAssassins
		{
			get
			{
				return _hirelingAssassins;
			}
			set
			{
				_hirelingAssassins = value;
			}
		}

		[ProtoMember(29, IsRequired = false, Name = "energyBar", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public EnergyBar energyBar
		{
			get
			{
				return _energyBar;
			}
			set
			{
				_energyBar = value;
			}
		}

		[ProtoMember(30, IsRequired = false, Name = "assassinName", DataFormat = DataFormat.Default)]
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

		[ProtoMember(31, IsRequired = false, Name = "itemPower", DataFormat = DataFormat.TwosComplement)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
