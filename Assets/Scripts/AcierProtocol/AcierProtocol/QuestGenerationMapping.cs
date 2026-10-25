using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "QuestGenerationMapping")]
	public class QuestGenerationMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "MinMaxRegion")]
		public class MinMaxRegion : IExtensible
		{
			private uint _minValue;

			private uint _maxValue;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "minValue", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public uint minValue
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

			[ProtoMember(2, IsRequired = false, Name = "maxValue", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public uint maxValue
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

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "Strategy")]
		public class Strategy : IExtensible
		{
			[Serializable]
			[ProtoContract(Name = "Template")]
			public class Template : IExtensible
			{
				[Serializable]
				[ProtoContract(Name = "StoryCondition")]
				public class StoryCondition : IExtensible
				{
					private string _campaignId = "";

					private string _missionName = "";

					private IExtension extensionObject;

					[ProtoMember(1, IsRequired = false, Name = "campaignId", DataFormat = DataFormat.Default)]
					[DefaultValue("")]
					public string campaignId
					{
						get
						{
							return _campaignId;
						}
						set
						{
							_campaignId = value;
						}
					}

					[ProtoMember(2, IsRequired = false, Name = "missionName", DataFormat = DataFormat.Default)]
					[DefaultValue("")]
					public string missionName
					{
						get
						{
							return _missionName;
						}
						set
						{
							_missionName = value;
						}
					}

					IExtension IExtensible.GetExtensionObject(bool createIfMissing)
					{
						return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
					}
				}

				private ERegion[] _regions = new ERegion[0];

				private EEra[] _eras = new EEra[0];

				private EMood[] _moods = new EMood[0];

				private int[] _difficulties = new int[0];

				private EMissionKind[] _missionKinds = new EMissionKind[0];

				private EObjectiveType[] _baseObjectiveTypes = new EObjectiveType[0];

				private StoryCondition[] _storyConditions = new StoryCondition[0];

				private IExtension extensionObject;

				[ProtoMember(1, Name = "regions", DataFormat = DataFormat.TwosComplement)]
				public ERegion[] regions
				{
					get
					{
						return _regions;
					}
					set
					{
						Array.Resize(ref _regions, value.Length);
						value.CopyTo(_regions, 0);
					}
				}

				[ProtoMember(2, Name = "eras", DataFormat = DataFormat.TwosComplement)]
				public EEra[] eras
				{
					get
					{
						return _eras;
					}
					set
					{
						Array.Resize(ref _eras, value.Length);
						value.CopyTo(_eras, 0);
					}
				}

				[ProtoMember(3, Name = "moods", DataFormat = DataFormat.TwosComplement)]
				public EMood[] moods
				{
					get
					{
						return _moods;
					}
					set
					{
						Array.Resize(ref _moods, value.Length);
						value.CopyTo(_moods, 0);
					}
				}

				[ProtoMember(4, Name = "difficulties", DataFormat = DataFormat.TwosComplement)]
				public int[] difficulties
				{
					get
					{
						return _difficulties;
					}
					set
					{
						Array.Resize(ref _difficulties, value.Length);
						value.CopyTo(_difficulties, 0);
					}
				}

				[ProtoMember(5, Name = "missionKinds", DataFormat = DataFormat.TwosComplement)]
				public EMissionKind[] missionKinds
				{
					get
					{
						return _missionKinds;
					}
					set
					{
						Array.Resize(ref _missionKinds, value.Length);
						value.CopyTo(_missionKinds, 0);
					}
				}

				[ProtoMember(6, Name = "baseObjectiveTypes", DataFormat = DataFormat.TwosComplement)]
				public EObjectiveType[] baseObjectiveTypes
				{
					get
					{
						return _baseObjectiveTypes;
					}
					set
					{
						Array.Resize(ref _baseObjectiveTypes, value.Length);
						value.CopyTo(_baseObjectiveTypes, 0);
					}
				}

				[ProtoMember(7, Name = "storyConditions", DataFormat = DataFormat.Default)]
				public StoryCondition[] storyConditions
				{
					get
					{
						return _storyConditions;
					}
					set
					{
						Array.Resize(ref _storyConditions, value.Length);
						value.CopyTo(_storyConditions, 0);
					}
				}

				IExtension IExtensible.GetExtensionObject(bool createIfMissing)
				{
					return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
				}
			}

			[Serializable]
			[ProtoContract(Name = "Rarity")]
			public class Rarity : IExtensible
			{
				private float _weight;

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

				IExtension IExtensible.GetExtensionObject(bool createIfMissing)
				{
					return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
				}
			}

			[Serializable]
			[ProtoContract(Name = "QualityWeight")]
			public class QualityWeight : IExtensible
			{
				private Quality.EQuality _quality = Quality.EQuality.Mundane;

				private float _weight;

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

				[ProtoMember(2, IsRequired = false, Name = "weight", DataFormat = DataFormat.FixedSize)]
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

			private string _strategyId = "";

			private Template _template;

			private Rarity _slotRarity;

			private MinMaxRegion[] _slotLocations = new MinMaxRegion[0];

			private MinMaxRegion _timesToComplete;

			private int[] _heroicIndices = new int[0];

			private LootFilter _reward;

			private int _rewardMultiplier = 1;

			private int _descriptionLocaKey;

			private QualityWeight[] _qualityWeights = new QualityWeight[0];

			private LootMasterTable.LootMacroTableRange.Range _range;

			private int _titleLocaKey;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "strategyId", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string strategyId
			{
				get
				{
					return _strategyId;
				}
				set
				{
					_strategyId = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "template", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public Template template
			{
				get
				{
					return _template;
				}
				set
				{
					_template = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "slotRarity", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public Rarity slotRarity
			{
				get
				{
					return _slotRarity;
				}
				set
				{
					_slotRarity = value;
				}
			}

			[ProtoMember(4, Name = "slotLocations", DataFormat = DataFormat.Default)]
			public MinMaxRegion[] slotLocations
			{
				get
				{
					return _slotLocations;
				}
				set
				{
					Array.Resize(ref _slotLocations, value.Length);
					value.CopyTo(_slotLocations, 0);
				}
			}

			[ProtoMember(5, IsRequired = false, Name = "timesToComplete", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public MinMaxRegion timesToComplete
			{
				get
				{
					return _timesToComplete;
				}
				set
				{
					_timesToComplete = value;
				}
			}

			[ProtoMember(6, Name = "heroicIndices", DataFormat = DataFormat.TwosComplement)]
			public int[] heroicIndices
			{
				get
				{
					return _heroicIndices;
				}
				set
				{
					Array.Resize(ref _heroicIndices, value.Length);
					value.CopyTo(_heroicIndices, 0);
				}
			}

			[ProtoMember(7, IsRequired = false, Name = "reward", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public LootFilter reward
			{
				get
				{
					return _reward;
				}
				set
				{
					_reward = value;
				}
			}

			[ProtoMember(12, IsRequired = false, Name = "rewardMultiplier", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(1)]
			public int rewardMultiplier
			{
				get
				{
					return _rewardMultiplier;
				}
				set
				{
					_rewardMultiplier = value;
				}
			}

			[ProtoMember(8, IsRequired = false, Name = "descriptionLocaKey", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int descriptionLocaKey
			{
				get
				{
					return _descriptionLocaKey;
				}
				set
				{
					_descriptionLocaKey = value;
				}
			}

			[ProtoMember(9, Name = "qualityWeights", DataFormat = DataFormat.Default)]
			public QualityWeight[] qualityWeights
			{
				get
				{
					return _qualityWeights;
				}
				set
				{
					Array.Resize(ref _qualityWeights, value.Length);
					value.CopyTo(_qualityWeights, 0);
				}
			}

			[ProtoMember(10, IsRequired = false, Name = "range", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public LootMasterTable.LootMacroTableRange.Range range
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

			[ProtoMember(11, IsRequired = false, Name = "titleLocaKey", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int titleLocaKey
			{
				get
				{
					return _titleLocaKey;
				}
				set
				{
					_titleLocaKey = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "Configuration")]
		public class Configuration : IExtensible
		{
			private MinMaxRegion _chainLength;

			private int[] _heroicIndices = new int[0];

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "chainLength", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public MinMaxRegion chainLength
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

			[ProtoMember(2, Name = "heroicIndices", DataFormat = DataFormat.TwosComplement)]
			public int[] heroicIndices
			{
				get
				{
					return _heroicIndices;
				}
				set
				{
					Array.Resize(ref _heroicIndices, value.Length);
					value.CopyTo(_heroicIndices, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private Strategy[] _strategies = new Strategy[0];

		private Configuration[] _configurations = new Configuration[0];

		private uint _maxNumberOfRerolls = 1u;

		private Resources _questsRerollCosts;

		private Resources _rankQuestRegenerationCosts;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "strategies", DataFormat = DataFormat.Default)]
		public Strategy[] strategies
		{
			get
			{
				return _strategies;
			}
			set
			{
				Array.Resize(ref _strategies, value.Length);
				value.CopyTo(_strategies, 0);
			}
		}

		[ProtoMember(2, Name = "configurations", DataFormat = DataFormat.Default)]
		public Configuration[] configurations
		{
			get
			{
				return _configurations;
			}
			set
			{
				Array.Resize(ref _configurations, value.Length);
				value.CopyTo(_configurations, 0);
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "maxNumberOfRerolls", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(1L)]
		public uint maxNumberOfRerolls
		{
			get
			{
				return _maxNumberOfRerolls;
			}
			set
			{
				_maxNumberOfRerolls = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "questsRerollCosts", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Resources questsRerollCosts
		{
			get
			{
				return _questsRerollCosts;
			}
			set
			{
				_questsRerollCosts = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "rankQuestRegenerationCosts", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Resources rankQuestRegenerationCosts
		{
			get
			{
				return _rankQuestRegenerationCosts;
			}
			set
			{
				_rankQuestRegenerationCosts = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
