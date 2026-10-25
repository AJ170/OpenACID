using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "DailyRewardMapping")]
	public class DailyRewardMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "WeightedLoot")]
		public class WeightedLoot : IExtensible
		{
			private float _weight;

			private LootTemplate[] _lootTemplates = new LootTemplate[0];

			private string _id = "";

			private string _itemDefGuid = "";

			private string _colorDefGuid = "";

			private LangKey _nameLangKey;

			private LangKey _descLangKey;

			private Quality.EQuality _quality = Quality.EQuality.Mundane;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "weight", DataFormat = DataFormat.FixedSize)]
			[DefaultValue(0f)]
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

			[ProtoMember(2, Name = "lootTemplates", DataFormat = DataFormat.Default)]
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

			[ProtoMember(3, IsRequired = false, Name = "id", DataFormat = DataFormat.Default)]
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

			[ProtoMember(5, IsRequired = false, Name = "colorDefGuid", DataFormat = DataFormat.Default)]
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

			[ProtoMember(6, IsRequired = false, Name = "nameLangKey", DataFormat = DataFormat.Default)]
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

			[ProtoMember(7, IsRequired = false, Name = "descLangKey", DataFormat = DataFormat.Default)]
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

			[ProtoMember(8, IsRequired = false, Name = "quality", DataFormat = DataFormat.TwosComplement)]
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
		[ProtoContract(Name = "DailyRewardTable")]
		public class DailyRewardTable : IExtensible
		{
			private int _day;

			private WeightedLoot[] _lootTable = new WeightedLoot[0];

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "day", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int day
			{
				get
				{
					return _day;
				}
				set
				{
					_day = value;
				}
			}

			[ProtoMember(2, Name = "lootTable", DataFormat = DataFormat.Default)]
			public WeightedLoot[] lootTable
			{
				get
				{
					return _lootTable;
				}
				set
				{
					Array.Resize(ref _lootTable, value.Length);
					value.CopyTo(_lootTable, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private DailyRewardTable[] _dailyRewards = new DailyRewardTable[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "dailyRewards", DataFormat = DataFormat.Default)]
		public DailyRewardTable[] dailyRewards
		{
			get
			{
				return _dailyRewards;
			}
			set
			{
				Array.Resize(ref _dailyRewards, value.Length);
				value.CopyTo(_dailyRewards, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
