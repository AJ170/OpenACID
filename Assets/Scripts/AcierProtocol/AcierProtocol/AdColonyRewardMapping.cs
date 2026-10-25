using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "AdColonyRewardMapping")]
	public class AdColonyRewardMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "RankEntry")]
		public class RankEntry : IExtensible
		{
			[Serializable]
			[ProtoContract(Name = "RarityWeightEntry")]
			public class RarityWeightEntry : IExtensible
			{
				private Quality.EQuality _quality = Quality.EQuality.Mundane;

				private double _weight;

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

				[ProtoMember(2, IsRequired = false, Name = "weight", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(0.0)]
				public double weight
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

			private LootFilter _lootFilter;

			private RarityWeightEntry[] _rarityWeightEntries = new RarityWeightEntry[0];

			private int _rankIdx;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "lootFilter", DataFormat = DataFormat.Default)]
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

			[ProtoMember(2, Name = "rarityWeightEntries", DataFormat = DataFormat.Default)]
			public RarityWeightEntry[] rarityWeightEntries
			{
				get
				{
					return _rarityWeightEntries;
				}
				set
				{
					Array.Resize(ref _rarityWeightEntries, value.Length);
					value.CopyTo(_rarityWeightEntries, 0);
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "rankIdx", DataFormat = DataFormat.TwosComplement)]
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

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private RankEntry[] _rankEntries = new RankEntry[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "rankEntries", DataFormat = DataFormat.Default)]
		public RankEntry[] rankEntries
		{
			get
			{
				return _rankEntries;
			}
			set
			{
				Array.Resize(ref _rankEntries, value.Length);
				value.CopyTo(_rankEntries, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
