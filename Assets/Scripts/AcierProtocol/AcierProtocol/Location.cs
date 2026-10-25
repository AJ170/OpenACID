using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "Location")]
	public class Location : IExtensible
	{
		private HotspotParameterConfiguration[] _hotspotConfig = new HotspotParameterConfiguration[0];

		private string[] _objectiveKinds = new string[0];

		private DifficultyConfigurationDouble[] _playerStartObjectiveMinDistance = new DifficultyConfigurationDouble[0];

		private PopulationConfiguration[] _populationConfig = new PopulationConfiguration[0];

		private DifficultyConfigurationDouble[] _populationMinDistance = new DifficultyConfigurationDouble[0];

		private DifficultyConfigurationInteger[] _spawnPointUnitCount = new DifficultyConfigurationInteger[0];

		private EEra _era;

		private EMood _mood;

		private ERegion _region;

		private int[] _generatedTitleLocaKeys = new int[0];

		private int[] _generatedBriefingLocaKeys = new int[0];

		private int[] _generatedDebriefingLocaKeys = new int[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "hotspotConfig", DataFormat = DataFormat.Default)]
		public HotspotParameterConfiguration[] hotspotConfig
		{
			get
			{
				return _hotspotConfig;
			}
			set
			{
				Array.Resize(ref _hotspotConfig, value.Length);
				value.CopyTo(_hotspotConfig, 0);
			}
		}

		[ProtoMember(2, Name = "objectiveKinds", DataFormat = DataFormat.Default)]
		public string[] objectiveKinds
		{
			get
			{
				return _objectiveKinds;
			}
			set
			{
				Array.Resize(ref _objectiveKinds, value.Length);
				value.CopyTo(_objectiveKinds, 0);
			}
		}

		[ProtoMember(3, Name = "playerStartObjectiveMinDistance", DataFormat = DataFormat.Default)]
		public DifficultyConfigurationDouble[] playerStartObjectiveMinDistance
		{
			get
			{
				return _playerStartObjectiveMinDistance;
			}
			set
			{
				Array.Resize(ref _playerStartObjectiveMinDistance, value.Length);
				value.CopyTo(_playerStartObjectiveMinDistance, 0);
			}
		}

		[ProtoMember(4, Name = "populationConfig", DataFormat = DataFormat.Default)]
		public PopulationConfiguration[] populationConfig
		{
			get
			{
				return _populationConfig;
			}
			set
			{
				Array.Resize(ref _populationConfig, value.Length);
				value.CopyTo(_populationConfig, 0);
			}
		}

		[ProtoMember(5, Name = "populationMinDistance", DataFormat = DataFormat.Default)]
		public DifficultyConfigurationDouble[] populationMinDistance
		{
			get
			{
				return _populationMinDistance;
			}
			set
			{
				Array.Resize(ref _populationMinDistance, value.Length);
				value.CopyTo(_populationMinDistance, 0);
			}
		}

		[ProtoMember(6, Name = "spawnPointUnitCount", DataFormat = DataFormat.Default)]
		public DifficultyConfigurationInteger[] spawnPointUnitCount
		{
			get
			{
				return _spawnPointUnitCount;
			}
			set
			{
				Array.Resize(ref _spawnPointUnitCount, value.Length);
				value.CopyTo(_spawnPointUnitCount, 0);
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "era", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EEra.EEra_None)]
		public EEra era
		{
			get
			{
				return _era;
			}
			set
			{
				_era = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "mood", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EMood.Daytime)]
		public EMood mood
		{
			get
			{
				return _mood;
			}
			set
			{
				_mood = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "region", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(ERegion.Animus)]
		public ERegion region
		{
			get
			{
				return _region;
			}
			set
			{
				_region = value;
			}
		}

		[ProtoMember(11, Name = "generatedTitleLocaKeys", DataFormat = DataFormat.TwosComplement)]
		public int[] generatedTitleLocaKeys
		{
			get
			{
				return _generatedTitleLocaKeys;
			}
			set
			{
				Array.Resize(ref _generatedTitleLocaKeys, value.Length);
				value.CopyTo(_generatedTitleLocaKeys, 0);
			}
		}

		[ProtoMember(12, Name = "generatedBriefingLocaKeys", DataFormat = DataFormat.TwosComplement)]
		public int[] generatedBriefingLocaKeys
		{
			get
			{
				return _generatedBriefingLocaKeys;
			}
			set
			{
				Array.Resize(ref _generatedBriefingLocaKeys, value.Length);
				value.CopyTo(_generatedBriefingLocaKeys, 0);
			}
		}

		[ProtoMember(13, Name = "generatedDebriefingLocaKeys", DataFormat = DataFormat.TwosComplement)]
		public int[] generatedDebriefingLocaKeys
		{
			get
			{
				return _generatedDebriefingLocaKeys;
			}
			set
			{
				Array.Resize(ref _generatedDebriefingLocaKeys, value.Length);
				value.CopyTo(_generatedDebriefingLocaKeys, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
