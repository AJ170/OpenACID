using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "InventoryElement")]
	public class InventoryElement : IExtensible
	{
		private string _id;

		private EInventoryElementCategory _category = EInventoryElementCategory.InventoryItem;

		private Quality.EQuality _quality = Quality.EQuality.Mundane;

		private string _guid = "";

		private LangKey _langKey;

		private LangKey _descriptionlangKey;

		private long _createdAt;

		private string _templateName = "";

		private int _stackSize;

		private string _debugRollCode = "";

		private bool _isEquipable;

		private string _technicalName = "";

		private ItemData _itemData;

		private ModuleData _moduleData;

		private BlueprintData _blueprintData;

		private MysteryBoxData _mysteryBoxData;

		private CustomLootData _customLootData;

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

		[ProtoMember(2, IsRequired = false, Name = "category", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EInventoryElementCategory.InventoryItem)]
		public EInventoryElementCategory category
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

		[ProtoMember(5, IsRequired = false, Name = "guid", DataFormat = DataFormat.Default)]
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

		[ProtoMember(6, IsRequired = false, Name = "langKey", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LangKey langKey
		{
			get
			{
				return _langKey;
			}
			set
			{
				_langKey = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "descriptionlangKey", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LangKey descriptionlangKey
		{
			get
			{
				return _descriptionlangKey;
			}
			set
			{
				_descriptionlangKey = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "createdAt", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long createdAt
		{
			get
			{
				return _createdAt;
			}
			set
			{
				_createdAt = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "templateName", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string templateName
		{
			get
			{
				return _templateName;
			}
			set
			{
				_templateName = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "stackSize", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int stackSize
		{
			get
			{
				return _stackSize;
			}
			set
			{
				_stackSize = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "debugRollCode", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string debugRollCode
		{
			get
			{
				return _debugRollCode;
			}
			set
			{
				_debugRollCode = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "isEquipable", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool isEquipable
		{
			get
			{
				return _isEquipable;
			}
			set
			{
				_isEquipable = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "technicalName", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string technicalName
		{
			get
			{
				return _technicalName;
			}
			set
			{
				_technicalName = value;
			}
		}

		[ProtoMember(20, IsRequired = false, Name = "itemData", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ItemData itemData
		{
			get
			{
				return _itemData;
			}
			set
			{
				_itemData = value;
			}
		}

		[ProtoMember(21, IsRequired = false, Name = "moduleData", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ModuleData moduleData
		{
			get
			{
				return _moduleData;
			}
			set
			{
				_moduleData = value;
			}
		}

		[ProtoMember(22, IsRequired = false, Name = "blueprintData", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public BlueprintData blueprintData
		{
			get
			{
				return _blueprintData;
			}
			set
			{
				_blueprintData = value;
			}
		}

		[ProtoMember(23, IsRequired = false, Name = "mysteryBoxData", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MysteryBoxData mysteryBoxData
		{
			get
			{
				return _mysteryBoxData;
			}
			set
			{
				_mysteryBoxData = value;
			}
		}

		[ProtoMember(24, IsRequired = false, Name = "customLootData", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public CustomLootData customLootData
		{
			get
			{
				return _customLootData;
			}
			set
			{
				_customLootData = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
