using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "PopulationConfiguration")]
	public class PopulationConfiguration : IExtensible
	{
		private UnitData[] _units = new UnitData[0];

		private string _comment = "";

		private bool _fillerUnit;

		private ESpawnPointKind[] _supportedSpawnPointType = new ESpawnPointKind[0];

		private EChooseCondition[] _chooseCondition = new EChooseCondition[0];

		private string[] _supportedMissionNames = new string[0];

		private DifficultyConfigurationInteger[] _difficultyValues = new DifficultyConfigurationInteger[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "units", DataFormat = DataFormat.Default)]
		public UnitData[] units
		{
			get
			{
				return _units;
			}
			set
			{
				Array.Resize(ref _units, value.Length);
				value.CopyTo(_units, 0);
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "comment", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string comment
		{
			get
			{
				return _comment;
			}
			set
			{
				_comment = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "fillerUnit", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool fillerUnit
		{
			get
			{
				return _fillerUnit;
			}
			set
			{
				_fillerUnit = value;
			}
		}

		[ProtoMember(4, Name = "supportedSpawnPointType", DataFormat = DataFormat.TwosComplement)]
		public ESpawnPointKind[] supportedSpawnPointType
		{
			get
			{
				return _supportedSpawnPointType;
			}
			set
			{
				Array.Resize(ref _supportedSpawnPointType, value.Length);
				value.CopyTo(_supportedSpawnPointType, 0);
			}
		}

		[ProtoMember(5, Name = "chooseCondition", DataFormat = DataFormat.TwosComplement)]
		public EChooseCondition[] chooseCondition
		{
			get
			{
				return _chooseCondition;
			}
			set
			{
				Array.Resize(ref _chooseCondition, value.Length);
				value.CopyTo(_chooseCondition, 0);
			}
		}

		[ProtoMember(7, Name = "supportedMissionNames", DataFormat = DataFormat.Default)]
		public string[] supportedMissionNames
		{
			get
			{
				return _supportedMissionNames;
			}
			set
			{
				Array.Resize(ref _supportedMissionNames, value.Length);
				value.CopyTo(_supportedMissionNames, 0);
			}
		}

		[ProtoMember(6, Name = "difficultyValues", DataFormat = DataFormat.Default)]
		public DifficultyConfigurationInteger[] difficultyValues
		{
			get
			{
				return _difficultyValues;
			}
			set
			{
				Array.Resize(ref _difficultyValues, value.Length);
				value.CopyTo(_difficultyValues, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
