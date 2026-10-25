using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MysteryBoxMapping")]
	public class MysteryBoxMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Entry")]
		public class Entry : IExtensible
		{
			private Quality.EQuality _quality = Quality.EQuality.Mundane;

			private EMysteryBoxType _type = EMysteryBoxType.MysteryBoxItemType;

			private Resource _overspendingCost;

			private int _chainLength;

			private LootFilter _lootFilter;

			private Quality.EQuality _bonusQuality = Quality.EQuality.Mundane;

			private LootFilter _bonusLootFilter;

			private EFeature _feature = EFeature.Feature_LocationNormalAccess;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "quality", DataFormat = DataFormat.TwosComplement)]
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

			[ProtoMember(2, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
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

			[ProtoMember(3, IsRequired = false, Name = "overspendingCost", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public Resource overspendingCost
			{
				get
				{
					return _overspendingCost;
				}
				set
				{
					_overspendingCost = value;
				}
			}

			[ProtoMember(4, IsRequired = false, Name = "chainLength", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int chainLength
			{
				get
				{
					return _chainLength;
				}
				set
				{
					_chainLength = value;
				}
			}

			[ProtoMember(5, IsRequired = false, Name = "lootFilter", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public LootFilter lootFilter
			{
				get
				{
					return _lootFilter;
				}
				set
				{
					_lootFilter = value;
				}
			}

			[ProtoMember(6, IsRequired = false, Name = "bonusQuality", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(Quality.EQuality.Mundane)]
			public Quality.EQuality bonusQuality
			{
				get
				{
					return _bonusQuality;
				}
				set
				{
					_bonusQuality = value;
				}
			}

			[ProtoMember(7, IsRequired = false, Name = "bonusLootFilter", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public LootFilter bonusLootFilter
			{
				get
				{
					return _bonusLootFilter;
				}
				set
				{
					_bonusLootFilter = value;
				}
			}

			[ProtoMember(8, IsRequired = false, Name = "feature", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EFeature.Feature_LocationNormalAccess)]
			public EFeature feature
			{
				get
				{
					return _feature;
				}
				set
				{
					_feature = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private Entry[] _entries = new Entry[0];

		private float _chanceCurrentAssassinLevel;

		private float _chanceMaxAssassinLevel;

		private float _currentAssassinLevelLowerOffset;

		private float _currentAssassinLevelHigherOffset;

		private float _maxAssassinLevelLowerOffset;

		private float _maxAssassinLevelHigherOffset;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "entries", DataFormat = DataFormat.Default)]
		public Entry[] entries
		{
			get
			{
				return _entries;
			}
			set
			{
				Array.Resize(ref _entries, value.Length);
				value.CopyTo(_entries, 0);
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "chanceCurrentAssassinLevel", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float chanceCurrentAssassinLevel
		{
			get
			{
				return _chanceCurrentAssassinLevel;
			}
			set
			{
				_chanceCurrentAssassinLevel = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "chanceMaxAssassinLevel", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float chanceMaxAssassinLevel
		{
			get
			{
				return _chanceMaxAssassinLevel;
			}
			set
			{
				_chanceMaxAssassinLevel = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "currentAssassinLevelLowerOffset", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float currentAssassinLevelLowerOffset
		{
			get
			{
				return _currentAssassinLevelLowerOffset;
			}
			set
			{
				_currentAssassinLevelLowerOffset = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "currentAssassinLevelHigherOffset", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float currentAssassinLevelHigherOffset
		{
			get
			{
				return _currentAssassinLevelHigherOffset;
			}
			set
			{
				_currentAssassinLevelHigherOffset = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "maxAssassinLevelLowerOffset", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float maxAssassinLevelLowerOffset
		{
			get
			{
				return _maxAssassinLevelLowerOffset;
			}
			set
			{
				_maxAssassinLevelLowerOffset = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "maxAssassinLevelHigherOffset", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float maxAssassinLevelHigherOffset
		{
			get
			{
				return _maxAssassinLevelHigherOffset;
			}
			set
			{
				_maxAssassinLevelHigherOffset = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
