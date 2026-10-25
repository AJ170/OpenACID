using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionLootScalingMapping")]
	public class MissionLootScalingMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "MissionFlavourEntry")]
		public class MissionFlavourEntry : IExtensible
		{
			[Serializable]
			[ProtoContract(Name = "RarityWeighting")]
			public class RarityWeighting : IExtensible
			{
				[Serializable]
				[ProtoContract(Name = "Entry")]
				public class Entry : IExtensible
				{
					private int _rankIdx;

					private Quality.EQuality _quality = Quality.EQuality.Mundane;

					private float _weight;

					private IExtension extensionObject;

					[ProtoMember(1, IsRequired = false, Name = "rankIdx", DataFormat = DataFormat.TwosComplement)]
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

					[ProtoMember(2, IsRequired = false, Name = "quality", DataFormat = DataFormat.TwosComplement)]
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

					[ProtoMember(3, IsRequired = false, Name = "weight", DataFormat = DataFormat.FixedSize)]
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

					IExtension IExtensible.GetExtensionObject(bool createIfMissing)
					{
						return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
					}
				}

				private Entry[] _entries = new Entry[0];

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

				IExtension IExtensible.GetExtensionObject(bool createIfMissing)
				{
					return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
				}
			}

			private string _mission = "";

			private string _flavour = "";

			private RarityWeighting _rarityWeighting;

			private RarityWeighting _rarityWeightingElite;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "mission", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string mission
			{
				get
				{
					return _mission;
				}
				set
				{
					_mission = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "flavour", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string flavour
			{
				get
				{
					return _flavour;
				}
				set
				{
					_flavour = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "rarityWeighting", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public RarityWeighting rarityWeighting
			{
				get
				{
					return _rarityWeighting;
				}
				set
				{
					_rarityWeighting = value;
				}
			}

			[ProtoMember(4, IsRequired = false, Name = "rarityWeightingElite", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public RarityWeighting rarityWeightingElite
			{
				get
				{
					return _rarityWeightingElite;
				}
				set
				{
					_rarityWeightingElite = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private MissionFlavourEntry[] _entries = new MissionFlavourEntry[0];

		private string _defaultFlavour = "Default";

		private float _upDiffExp = 0.65f;

		private int _levelOffsetMax1 = 1;

		private int _levelOffsetMax2 = 2;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "entries", DataFormat = DataFormat.Default)]
		public MissionFlavourEntry[] entries
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

		[ProtoMember(2, IsRequired = false, Name = "defaultFlavour", DataFormat = DataFormat.Default)]
		[DefaultValue("Default")]
		public string defaultFlavour
		{
			get
			{
				return _defaultFlavour;
			}
			set
			{
				_defaultFlavour = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "upDiffExp", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0.65f)]
		public float upDiffExp
		{
			get
			{
				return _upDiffExp;
			}
			set
			{
				_upDiffExp = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "levelOffsetMax1", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(1)]
		public int levelOffsetMax1
		{
			get
			{
				return _levelOffsetMax1;
			}
			set
			{
				_levelOffsetMax1 = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "levelOffsetMax2", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(2)]
		public int levelOffsetMax2
		{
			get
			{
				return _levelOffsetMax2;
			}
			set
			{
				_levelOffsetMax2 = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
