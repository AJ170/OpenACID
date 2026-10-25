using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ShopBundle")]
	public class ShopBundle : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Payout")]
		public class Payout : IExtensible
		{
			private int _premiumTime;

			private LootTemplate[] _lootTemplates = new LootTemplate[0];

			private IExtension extensionObject;

			[ProtoMember(2, IsRequired = false, Name = "premiumTime", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int premiumTime
			{
				get
				{
					return _premiumTime;
				}
				set
				{
					_premiumTime = value;
				}
			}

			[ProtoMember(5, Name = "lootTemplates", DataFormat = DataFormat.Default)]
			public LootTemplate[] lootTemplates
			{
				get
				{
					return _lootTemplates;
				}
				set
				{
					Array.Resize(ref _lootTemplates, value.Length);
					value.CopyTo(_lootTemplates, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private string _id = "";

		private Resources _costs;

		private Payout _payout;

		private string _itemDefGuid = "";

		private LangKey _nameLangKey;

		private LangKey _descLangKey;

		private bool _enabled;

		private EShopCategory _category = EShopCategory.Shopcat_HcPackage;

		private uint _position;

		private string _colorDefGuid = "";

		private string _name = "";

		private Quality.EQuality _quality = Quality.EQuality.Mundane;

		private LootTemplate _shopPreview;

		private bool _isShopPreviewEnabled;

		private string _shopPreviewColorDefGuid = "";

		private string[] _reqEntitlementGuids = new string[0];

		private LangKey _shortDescLangKey;

		private EShopProductLabel _shopProductLabel;

		private int _bundleValue;

		private bool _displayPayoutsStacked;

		private string _houstonId = "";

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "id", DataFormat = DataFormat.Default)]
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

		[ProtoMember(2, IsRequired = false, Name = "costs", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Resources costs
		{
			get
			{
				return _costs;
			}
			set
			{
				_costs = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "payout", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Payout payout
		{
			get
			{
				return _payout;
			}
			set
			{
				_payout = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "itemDefGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string itemDefGuid
		{
			get
			{
				return _itemDefGuid;
			}
			set
			{
				_itemDefGuid = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "nameLangKey", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LangKey nameLangKey
		{
			get
			{
				return _nameLangKey;
			}
			set
			{
				_nameLangKey = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "descLangKey", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LangKey descLangKey
		{
			get
			{
				return _descLangKey;
			}
			set
			{
				_descLangKey = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "enabled", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool enabled
		{
			get
			{
				return _enabled;
			}
			set
			{
				_enabled = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "category", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EShopCategory.Shopcat_HcPackage)]
		public EShopCategory category
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

		[ProtoMember(10, IsRequired = false, Name = "position", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public uint position
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

		[ProtoMember(11, IsRequired = false, Name = "colorDefGuid", DataFormat = DataFormat.Default)]
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

		[ProtoMember(12, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
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

		[ProtoMember(13, IsRequired = false, Name = "quality", DataFormat = DataFormat.TwosComplement)]
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

		[ProtoMember(14, IsRequired = false, Name = "shopPreview", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LootTemplate shopPreview
		{
			get
			{
				return _shopPreview;
			}
			set
			{
				_shopPreview = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "isShopPreviewEnabled", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool isShopPreviewEnabled
		{
			get
			{
				return _isShopPreviewEnabled;
			}
			set
			{
				_isShopPreviewEnabled = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "shopPreviewColorDefGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string shopPreviewColorDefGuid
		{
			get
			{
				return _shopPreviewColorDefGuid;
			}
			set
			{
				_shopPreviewColorDefGuid = value;
			}
		}

		[ProtoMember(17, Name = "reqEntitlementGuids", DataFormat = DataFormat.Default)]
		public string[] reqEntitlementGuids
		{
			get
			{
				return _reqEntitlementGuids;
			}
			set
			{
				Array.Resize(ref _reqEntitlementGuids, value.Length);
				value.CopyTo(_reqEntitlementGuids, 0);
			}
		}

		[ProtoMember(18, IsRequired = false, Name = "shortDescLangKey", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LangKey shortDescLangKey
		{
			get
			{
				return _shortDescLangKey;
			}
			set
			{
				_shortDescLangKey = value;
			}
		}

		[ProtoMember(19, IsRequired = false, Name = "shopProductLabel", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EShopProductLabel.NoLabel)]
		public EShopProductLabel shopProductLabel
		{
			get
			{
				return _shopProductLabel;
			}
			set
			{
				_shopProductLabel = value;
			}
		}

		[ProtoMember(20, IsRequired = false, Name = "bundleValue", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int bundleValue
		{
			get
			{
				return _bundleValue;
			}
			set
			{
				_bundleValue = value;
			}
		}

		[ProtoMember(23, IsRequired = false, Name = "displayPayoutsStacked", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool displayPayoutsStacked
		{
			get
			{
				return _displayPayoutsStacked;
			}
			set
			{
				_displayPayoutsStacked = value;
			}
		}

		[ProtoMember(24, IsRequired = false, Name = "houstonId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string houstonId
		{
			get
			{
				return _houstonId;
			}
			set
			{
				_houstonId = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
