using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "LootMasterTable")]
	public class LootMasterTable : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "LootMacroTableRange")]
		public class LootMacroTableRange : IExtensible
		{
			[Serializable]
			[ProtoContract(Name = "Range")]
			public class Range : IExtensible
			{
				private int _min;

				private int _max;

				private IExtension extensionObject;

				[ProtoMember(1, IsRequired = true, Name = "min", DataFormat = DataFormat.TwosComplement)]
				public int min
				{
					get
					{
						return _min;
					}
					set
					{
						_min = value;
					}
				}

				[ProtoMember(2, IsRequired = true, Name = "max", DataFormat = DataFormat.TwosComplement)]
				public int max
				{
					get
					{
						return _max;
					}
					set
					{
						_max = value;
					}
				}

				IExtension IExtensible.GetExtensionObject(bool createIfMissing)
				{
					return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
				}
			}

			[Serializable]
			[ProtoContract(Name = "LootMacroTable")]
			public class LootMacroTable : IExtensible
			{
				[Serializable]
				[ProtoContract(Name = "LootTemplateTableRef")]
				public class LootTemplateTableRef : IExtensible
				{
					private string _lootTemplateTableName;

					private float _weight;

					private IExtension extensionObject;

					[ProtoMember(1, IsRequired = true, Name = "lootTemplateTableName", DataFormat = DataFormat.Default)]
					public string lootTemplateTableName
					{
						get
						{
							return _lootTemplateTableName;
						}
						set
						{
							_lootTemplateTableName = value;
						}
					}

					[ProtoMember(2, IsRequired = true, Name = "weight", DataFormat = DataFormat.FixedSize)]
					public float weight
					{
						get
						{
							return _weight;
						}
						set
						{
							_weight = value;
						}
					}

					IExtension IExtensible.GetExtensionObject(bool createIfMissing)
					{
						return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
					}
				}

				private string _name = "";

				private LootTypes _lootTypes;

				private LootTemplateTableRef[] _lootTemplateTableRefs = new LootTemplateTableRef[0];

				private LocationSet _location;

				private EFeature[] _features = new EFeature[0];

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

				[ProtoMember(2, IsRequired = false, Name = "lootTypes", DataFormat = DataFormat.Default)]
				[DefaultValue(null)]
				public LootTypes lootTypes
				{
					get
					{
						return _lootTypes;
					}
					set
					{
						_lootTypes = value;
					}
				}

				[ProtoMember(3, Name = "lootTemplateTableRefs", DataFormat = DataFormat.Default)]
				public LootTemplateTableRef[] lootTemplateTableRefs
				{
					get
					{
						return _lootTemplateTableRefs;
					}
					set
					{
						Array.Resize(ref _lootTemplateTableRefs, value.Length);
						value.CopyTo(_lootTemplateTableRefs, 0);
					}
				}

				[ProtoMember(4, IsRequired = false, Name = "location", DataFormat = DataFormat.Default)]
				[DefaultValue(null)]
				public LocationSet location
				{
					get
					{
						return _location;
					}
					set
					{
						_location = value;
					}
				}

				[ProtoMember(5, Name = "features", DataFormat = DataFormat.TwosComplement)]
				public EFeature[] features
				{
					get
					{
						return _features;
					}
					set
					{
						Array.Resize(ref _features, value.Length);
						value.CopyTo(_features, 0);
					}
				}

				IExtension IExtensible.GetExtensionObject(bool createIfMissing)
				{
					return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
				}
			}

			private Range _range;

			private LootMacroTable[] _lootMacroTables = new LootMacroTable[0];

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = true, Name = "range", DataFormat = DataFormat.Default)]
			public Range range
			{
				get
				{
					return _range;
				}
				set
				{
					_range = value;
				}
			}

			[ProtoMember(2, Name = "lootMacroTables", DataFormat = DataFormat.Default)]
			public LootMacroTable[] lootMacroTables
			{
				get
				{
					return _lootMacroTables;
				}
				set
				{
					Array.Resize(ref _lootMacroTables, value.Length);
					value.CopyTo(_lootMacroTables, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "LootTemplateTable")]
		public class LootTemplateTable : IExtensible
		{
			[Serializable]
			[ProtoContract(Name = "LootTemplateRef")]
			public class LootTemplateRef : IExtensible
			{
				private string _lootTemplateName;

				private float _weight;

				private IExtension extensionObject;

				[ProtoMember(1, IsRequired = true, Name = "lootTemplateName", DataFormat = DataFormat.Default)]
				public string lootTemplateName
				{
					get
					{
						return _lootTemplateName;
					}
					set
					{
						_lootTemplateName = value;
					}
				}

				[ProtoMember(2, IsRequired = true, Name = "weight", DataFormat = DataFormat.FixedSize)]
				public float weight
				{
					get
					{
						return _weight;
					}
					set
					{
						_weight = value;
					}
				}

				IExtension IExtensible.GetExtensionObject(bool createIfMissing)
				{
					return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
				}
			}

			private string _name;

			private LootTemplateRef[] _lootTemplateRefs = new LootTemplateRef[0];

			private Quality.EQuality _quality = Quality.EQuality.Mundane;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = true, Name = "name", DataFormat = DataFormat.Default)]
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

			[ProtoMember(2, Name = "lootTemplateRefs", DataFormat = DataFormat.Default)]
			public LootTemplateRef[] lootTemplateRefs
			{
				get
				{
					return _lootTemplateRefs;
				}
				set
				{
					Array.Resize(ref _lootTemplateRefs, value.Length);
					value.CopyTo(_lootTemplateRefs, 0);
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

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "ItemModTable")]
		public class ItemModTable : IExtensible
		{
			[Serializable]
			[ProtoContract(Name = "ItemMod")]
			public class ItemMod : IExtensible
			{
				private string _skillGuid;

				private int _minValue;

				private int _maxValue;

				private float _weight;

				private bool _plainItemPower;

				private IExtension extensionObject;

				[ProtoMember(1, IsRequired = true, Name = "skillGuid", DataFormat = DataFormat.Default)]
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

				[ProtoMember(2, IsRequired = true, Name = "minValue", DataFormat = DataFormat.TwosComplement)]
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

				[ProtoMember(3, IsRequired = true, Name = "maxValue", DataFormat = DataFormat.TwosComplement)]
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

				[ProtoMember(4, IsRequired = true, Name = "weight", DataFormat = DataFormat.FixedSize)]
				public float weight
				{
					get
					{
						return _weight;
					}
					set
					{
						_weight = value;
					}
				}

				[ProtoMember(5, IsRequired = false, Name = "plainItemPower", DataFormat = DataFormat.Default)]
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

				IExtension IExtensible.GetExtensionObject(bool createIfMissing)
				{
					return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
				}
			}

			private string _name;

			private ItemMod[] _itemMods = new ItemMod[0];

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = true, Name = "name", DataFormat = DataFormat.Default)]
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

			[ProtoMember(2, Name = "itemMods", DataFormat = DataFormat.Default)]
			public ItemMod[] itemMods
			{
				get
				{
					return _itemMods;
				}
				set
				{
					Array.Resize(ref _itemMods, value.Length);
					value.CopyTo(_itemMods, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private LootMacroTableRange[] _lootMacroTableRanges = new LootMacroTableRange[0];

		private LootTemplateTable[] _lootTemplateTables = new LootTemplateTable[0];

		private ItemModTable[] _itemModTables = new ItemModTable[0];

		private LootTemplate[] _lootTemplates = new LootTemplate[0];

		private long _version;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "lootMacroTableRanges", DataFormat = DataFormat.Default)]
		public LootMacroTableRange[] lootMacroTableRanges
		{
			get
			{
				return _lootMacroTableRanges;
			}
			set
			{
				Array.Resize(ref _lootMacroTableRanges, value.Length);
				value.CopyTo(_lootMacroTableRanges, 0);
			}
		}

		[ProtoMember(2, Name = "lootTemplateTables", DataFormat = DataFormat.Default)]
		public LootTemplateTable[] lootTemplateTables
		{
			get
			{
				return _lootTemplateTables;
			}
			set
			{
				Array.Resize(ref _lootTemplateTables, value.Length);
				value.CopyTo(_lootTemplateTables, 0);
			}
		}

		[ProtoMember(3, Name = "itemModTables", DataFormat = DataFormat.Default)]
		public ItemModTable[] itemModTables
		{
			get
			{
				return _itemModTables;
			}
			set
			{
				Array.Resize(ref _itemModTables, value.Length);
				value.CopyTo(_itemModTables, 0);
			}
		}

		[ProtoMember(4, Name = "lootTemplates", DataFormat = DataFormat.Default)]
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

		[ProtoMember(5, IsRequired = false, Name = "version", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long version
		{
			get
			{
				return _version;
			}
			set
			{
				_version = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
