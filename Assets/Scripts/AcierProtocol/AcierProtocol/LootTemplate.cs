using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "LootTemplate")]
	public class LootTemplate : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "ItemTemplate")]
		public class ItemTemplate : IExtensible
		{
			[Serializable]
			[ProtoContract(Name = "ModStaticTemplate")]
			public class ModStaticTemplate : IExtensible
			{
				private string _skillGuid = "";

				private int _minValue;

				private int _maxValue;

				private bool _plainItemPower;

				private EModuleModifier _moduleModifier;

				private IExtension extensionObject;

				[ProtoMember(1, IsRequired = false, Name = "skillGuid", DataFormat = DataFormat.Default)]
				[DefaultValue("")]
				public string skillGuid
				{
					get
					{
						return _skillGuid;
					}
					set
					{
						_skillGuid = value;
					}
				}

				[ProtoMember(2, IsRequired = false, Name = "minValue", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(0)]
				public int minValue
				{
					get
					{
						return _minValue;
					}
					set
					{
						_minValue = value;
					}
				}

				[ProtoMember(3, IsRequired = false, Name = "maxValue", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(0)]
				public int maxValue
				{
					get
					{
						return _maxValue;
					}
					set
					{
						_maxValue = value;
					}
				}

				[ProtoMember(4, IsRequired = false, Name = "plainItemPower", DataFormat = DataFormat.Default)]
				[DefaultValue(false)]
				public bool plainItemPower
				{
					get
					{
						return _plainItemPower;
					}
					set
					{
						_plainItemPower = value;
					}
				}

				[ProtoMember(5, IsRequired = false, Name = "moduleModifier", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(EModuleModifier.ClientModifier)]
				public EModuleModifier moduleModifier
				{
					get
					{
						return _moduleModifier;
					}
					set
					{
						_moduleModifier = value;
					}
				}

				IExtension IExtensible.GetExtensionObject(bool createIfMissing)
				{
					return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
				}
			}

			private ModStaticTemplate[] _presetMods = new ModStaticTemplate[0];

			private string[] _modTableNames = new string[0];

			private int _maxModCount;

			private string _baseStatStr = "";

			private int _minBaseStatValue;

			private int _maxBaseStatValue;

			private Item.EItemCategory _category = Item.EItemCategory.None;

			private IExtension extensionObject;

			[ProtoMember(6, Name = "presetMods", DataFormat = DataFormat.Default)]
			public ModStaticTemplate[] presetMods
			{
				get
				{
					return _presetMods;
				}
				set
				{
					Array.Resize(ref _presetMods, value.Length);
					value.CopyTo(_presetMods, 0);
				}
			}

			[ProtoMember(7, Name = "modTableNames", DataFormat = DataFormat.Default)]
			public string[] modTableNames
			{
				get
				{
					return _modTableNames;
				}
				set
				{
					Array.Resize(ref _modTableNames, value.Length);
					value.CopyTo(_modTableNames, 0);
				}
			}

			[ProtoMember(8, IsRequired = false, Name = "maxModCount", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int maxModCount
			{
				get
				{
					return _maxModCount;
				}
				set
				{
					_maxModCount = value;
				}
			}

			[ProtoMember(9, IsRequired = false, Name = "baseStatStr", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string baseStatStr
			{
				get
				{
					return _baseStatStr;
				}
				set
				{
					_baseStatStr = value;
				}
			}

			[ProtoMember(10, IsRequired = false, Name = "minBaseStatValue", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int minBaseStatValue
			{
				get
				{
					return _minBaseStatValue;
				}
				set
				{
					_minBaseStatValue = value;
				}
			}

			[ProtoMember(11, IsRequired = false, Name = "maxBaseStatValue", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int maxBaseStatValue
			{
				get
				{
					return _maxBaseStatValue;
				}
				set
				{
					_maxBaseStatValue = value;
				}
			}

			[ProtoMember(12, IsRequired = false, Name = "category", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(Item.EItemCategory.None)]
			public Item.EItemCategory category
			{
				get
				{
					return _category;
				}
				set
				{
					_category = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "ModuleTemplate")]
		public class ModuleTemplate : IExtensible
		{
			private EDurationType _durationType = EDurationType.Time;

			private uint _duration;

			private Modification[] _skills = new Modification[0];

			private EModuleType _type;

			private uint _minAmount;

			private uint _maxAmount;

			private IExtension extensionObject;

			[ProtoMember(6, IsRequired = false, Name = "durationType", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EDurationType.Time)]
			public EDurationType durationType
			{
				get
				{
					return _durationType;
				}
				set
				{
					_durationType = value;
				}
			}

			[ProtoMember(7, IsRequired = false, Name = "duration", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public uint duration
			{
				get
				{
					return _duration;
				}
				set
				{
					_duration = value;
				}
			}

			[ProtoMember(8, Name = "skills", DataFormat = DataFormat.Default)]
			public Modification[] skills
			{
				get
				{
					return _skills;
				}
				set
				{
					Array.Resize(ref _skills, value.Length);
					value.CopyTo(_skills, 0);
				}
			}

			[ProtoMember(9, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EModuleType.DamageAlgorithm)]
			public EModuleType type
			{
				get
				{
					return _type;
				}
				set
				{
					_type = value;
				}
			}

			[ProtoMember(10, IsRequired = false, Name = "minAmount", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public uint minAmount
			{
				get
				{
					return _minAmount;
				}
				set
				{
					_minAmount = value;
				}
			}

			[ProtoMember(11, IsRequired = false, Name = "maxAmount", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public uint maxAmount
			{
				get
				{
					return _maxAmount;
				}
				set
				{
					_maxAmount = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "ResourceTemplate")]
		public class ResourceTemplate : IExtensible
		{
			private Resource _minResource;

			private Resource _maxResource;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "minResource", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public Resource minResource
			{
				get
				{
					return _minResource;
				}
				set
				{
					_minResource = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "maxResource", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public Resource maxResource
			{
				get
				{
					return _maxResource;
				}
				set
				{
					_maxResource = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "BlueprintTemplate")]
		public class BlueprintTemplate : IExtensible
		{
			private EBlueprintType _type = EBlueprintType.BlueprintWeapon;

			private uint _minAmount;

			private uint _maxAmount;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EBlueprintType.BlueprintWeapon)]
			public EBlueprintType type
			{
				get
				{
					return _type;
				}
				set
				{
					_type = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "minAmount", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public uint minAmount
			{
				get
				{
					return _minAmount;
				}
				set
				{
					_minAmount = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "maxAmount", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public uint maxAmount
			{
				get
				{
					return _maxAmount;
				}
				set
				{
					_maxAmount = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "MysteryBoxTemplate")]
		public class MysteryBoxTemplate : IExtensible
		{
			private EMysteryBoxType _type = EMysteryBoxType.MysteryBoxItemType;

			private uint _minAmount;

			private uint _maxAmount;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EMysteryBoxType.MysteryBoxItemType)]
			public EMysteryBoxType type
			{
				get
				{
					return _type;
				}
				set
				{
					_type = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "minAmount", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public uint minAmount
			{
				get
				{
					return _minAmount;
				}
				set
				{
					_minAmount = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "maxAmount", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public uint maxAmount
			{
				get
				{
					return _maxAmount;
				}
				set
				{
					_maxAmount = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "EntitlementTemplate")]
		public class EntitlementTemplate : IExtensible
		{
			private string _gameIdGuid = "";

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "gameIdGuid", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string gameIdGuid
			{
				get
				{
					return _gameIdGuid;
				}
				set
				{
					_gameIdGuid = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "CustomTemplate")]
		public class CustomTemplate : IExtensible
		{
			private ECustomLootTemplateType _type = ECustomLootTemplateType.LockPick;

			private uint _minAmount;

			private uint _maxAmount;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(ECustomLootTemplateType.LockPick)]
			public ECustomLootTemplateType type
			{
				get
				{
					return _type;
				}
				set
				{
					_type = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "minAmount", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public uint minAmount
			{
				get
				{
					return _minAmount;
				}
				set
				{
					_minAmount = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "maxAmount", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public uint maxAmount
			{
				get
				{
					return _maxAmount;
				}
				set
				{
					_maxAmount = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private string _name = "";

		private int _level;

		private Quality.EQuality _quality = Quality.EQuality.Mundane;

		private string _guid = "";

		private int[] _nameKeys = new int[0];

		private int[] _descKeys = new int[0];

		private int _itemPowerOverride;

		private string _id = "";

		private ItemTemplate _itemTemplate;

		private ModuleTemplate _moduleTemplate;

		private ResourceTemplate _resourceTemplate;

		private BlueprintTemplate _blueprintTemplate;

		private MysteryBoxTemplate _mysteryBoxTemplate;

		private EntitlementTemplate _entitlementTemplate;

		private CustomTemplate _customTemplate;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
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

		[ProtoMember(3, IsRequired = false, Name = "quality", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(Quality.EQuality.Mundane)]
		public Quality.EQuality quality
		{
			get
			{
				return _quality;
			}
			set
			{
				_quality = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "guid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string guid
		{
			get
			{
				return _guid;
			}
			set
			{
				_guid = value;
			}
		}

		[ProtoMember(5, Name = "nameKeys", DataFormat = DataFormat.TwosComplement)]
		public int[] nameKeys
		{
			get
			{
				return _nameKeys;
			}
			set
			{
				Array.Resize(ref _nameKeys, value.Length);
				value.CopyTo(_nameKeys, 0);
			}
		}

		[ProtoMember(6, Name = "descKeys", DataFormat = DataFormat.TwosComplement)]
		public int[] descKeys
		{
			get
			{
				return _descKeys;
			}
			set
			{
				Array.Resize(ref _descKeys, value.Length);
				value.CopyTo(_descKeys, 0);
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "itemPowerOverride", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int itemPowerOverride
		{
			get
			{
				return _itemPowerOverride;
			}
			set
			{
				_itemPowerOverride = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "id", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
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

		[ProtoMember(10, IsRequired = false, Name = "itemTemplate", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ItemTemplate itemTemplate
		{
			get
			{
				return _itemTemplate;
			}
			set
			{
				_itemTemplate = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "moduleTemplate", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ModuleTemplate moduleTemplate
		{
			get
			{
				return _moduleTemplate;
			}
			set
			{
				_moduleTemplate = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "resourceTemplate", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ResourceTemplate resourceTemplate
		{
			get
			{
				return _resourceTemplate;
			}
			set
			{
				_resourceTemplate = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "blueprintTemplate", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public BlueprintTemplate blueprintTemplate
		{
			get
			{
				return _blueprintTemplate;
			}
			set
			{
				_blueprintTemplate = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "mysteryBoxTemplate", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MysteryBoxTemplate mysteryBoxTemplate
		{
			get
			{
				return _mysteryBoxTemplate;
			}
			set
			{
				_mysteryBoxTemplate = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "entitlementTemplate", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public EntitlementTemplate entitlementTemplate
		{
			get
			{
				return _entitlementTemplate;
			}
			set
			{
				_entitlementTemplate = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "customTemplate", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public CustomTemplate customTemplate
		{
			get
			{
				return _customTemplate;
			}
			set
			{
				_customTemplate = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
