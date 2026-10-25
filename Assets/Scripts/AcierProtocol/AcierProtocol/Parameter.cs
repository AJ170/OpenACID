using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "Parameter")]
	public class Parameter : IExtensible
	{
		private float _weight;

		private EObjectiveParameter _name;

		private ParameterType _type = ParameterType.Integer_ParameterType;

		private DifficultyConfigurationInteger[] _difficultyValuesInteger = new DifficultyConfigurationInteger[0];

		private DifficultyConfigurationFloat[] _difficultyValuesFloat = new DifficultyConfigurationFloat[0];

		private DifficultyConfigurationDouble[] _difficultyValuesDouble = new DifficultyConfigurationDouble[0];

		private DifficultyConfigurationString[] _difficultyValuesString = new DifficultyConfigurationString[0];

		private DifficultyConfigurationEAlertLevel[] _difficultyValuesEAlertLevel = new DifficultyConfigurationEAlertLevel[0];

		private DifficultyConfigurationECompare[] _difficultyValuesECompare = new DifficultyConfigurationECompare[0];

		private DifficultyConfigurationELimitType[] _difficultyValuesELimitType = new DifficultyConfigurationELimitType[0];

		private DifficultyConfigurationEObjectSubCategory[] _difficultyValuesEObjectSubCategory = new DifficultyConfigurationEObjectSubCategory[0];

		private DifficultyConfigurationBoolean[] _difficultyValuesBoolean = new DifficultyConfigurationBoolean[0];

		private DifficultyConfigurationETemplateTargetKind[] _difficultyValuesETemplateTargetKind = new DifficultyConfigurationETemplateTargetKind[0];

		private DifficultyConfigurationENpcInteractionType[] _difficultyValuesENpcInteractionType = new DifficultyConfigurationENpcInteractionType[0];

		private DifficultyConfigurationEMissionAreaType[] _difficultyValuesEMissionAreaType = new DifficultyConfigurationEMissionAreaType[0];

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

		[ProtoMember(2, IsRequired = false, Name = "name", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EObjectiveParameter.DoNotKill)]
		public EObjectiveParameter name
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

		[ProtoMember(3, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(ParameterType.Integer_ParameterType)]
		public ParameterType type
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

		[ProtoMember(4, Name = "difficultyValuesInteger", DataFormat = DataFormat.Default)]
		public DifficultyConfigurationInteger[] difficultyValuesInteger
		{
			get
			{
				return _difficultyValuesInteger;
			}
			set
			{
				Array.Resize(ref _difficultyValuesInteger, value.Length);
				value.CopyTo(_difficultyValuesInteger, 0);
			}
		}

		[ProtoMember(5, Name = "difficultyValuesFloat", DataFormat = DataFormat.Default)]
		public DifficultyConfigurationFloat[] difficultyValuesFloat
		{
			get
			{
				return _difficultyValuesFloat;
			}
			set
			{
				Array.Resize(ref _difficultyValuesFloat, value.Length);
				value.CopyTo(_difficultyValuesFloat, 0);
			}
		}

		[ProtoMember(6, Name = "difficultyValuesDouble", DataFormat = DataFormat.Default)]
		public DifficultyConfigurationDouble[] difficultyValuesDouble
		{
			get
			{
				return _difficultyValuesDouble;
			}
			set
			{
				Array.Resize(ref _difficultyValuesDouble, value.Length);
				value.CopyTo(_difficultyValuesDouble, 0);
			}
		}

		[ProtoMember(7, Name = "difficultyValuesString", DataFormat = DataFormat.Default)]
		public DifficultyConfigurationString[] difficultyValuesString
		{
			get
			{
				return _difficultyValuesString;
			}
			set
			{
				Array.Resize(ref _difficultyValuesString, value.Length);
				value.CopyTo(_difficultyValuesString, 0);
			}
		}

		[ProtoMember(8, Name = "difficultyValuesEAlertLevel", DataFormat = DataFormat.Default)]
		public DifficultyConfigurationEAlertLevel[] difficultyValuesEAlertLevel
		{
			get
			{
				return _difficultyValuesEAlertLevel;
			}
			set
			{
				Array.Resize(ref _difficultyValuesEAlertLevel, value.Length);
				value.CopyTo(_difficultyValuesEAlertLevel, 0);
			}
		}

		[ProtoMember(9, Name = "difficultyValuesECompare", DataFormat = DataFormat.Default)]
		public DifficultyConfigurationECompare[] difficultyValuesECompare
		{
			get
			{
				return _difficultyValuesECompare;
			}
			set
			{
				Array.Resize(ref _difficultyValuesECompare, value.Length);
				value.CopyTo(_difficultyValuesECompare, 0);
			}
		}

		[ProtoMember(10, Name = "difficultyValuesELimitType", DataFormat = DataFormat.Default)]
		public DifficultyConfigurationELimitType[] difficultyValuesELimitType
		{
			get
			{
				return _difficultyValuesELimitType;
			}
			set
			{
				Array.Resize(ref _difficultyValuesELimitType, value.Length);
				value.CopyTo(_difficultyValuesELimitType, 0);
			}
		}

		[ProtoMember(11, Name = "difficultyValuesEObjectSubCategory", DataFormat = DataFormat.Default)]
		public DifficultyConfigurationEObjectSubCategory[] difficultyValuesEObjectSubCategory
		{
			get
			{
				return _difficultyValuesEObjectSubCategory;
			}
			set
			{
				Array.Resize(ref _difficultyValuesEObjectSubCategory, value.Length);
				value.CopyTo(_difficultyValuesEObjectSubCategory, 0);
			}
		}

		[ProtoMember(12, Name = "difficultyValuesBoolean", DataFormat = DataFormat.Default)]
		public DifficultyConfigurationBoolean[] difficultyValuesBoolean
		{
			get
			{
				return _difficultyValuesBoolean;
			}
			set
			{
				Array.Resize(ref _difficultyValuesBoolean, value.Length);
				value.CopyTo(_difficultyValuesBoolean, 0);
			}
		}

		[ProtoMember(13, Name = "difficultyValuesETemplateTargetKind", DataFormat = DataFormat.Default)]
		public DifficultyConfigurationETemplateTargetKind[] difficultyValuesETemplateTargetKind
		{
			get
			{
				return _difficultyValuesETemplateTargetKind;
			}
			set
			{
				Array.Resize(ref _difficultyValuesETemplateTargetKind, value.Length);
				value.CopyTo(_difficultyValuesETemplateTargetKind, 0);
			}
		}

		[ProtoMember(14, Name = "difficultyValuesENpcInteractionType", DataFormat = DataFormat.Default)]
		public DifficultyConfigurationENpcInteractionType[] difficultyValuesENpcInteractionType
		{
			get
			{
				return _difficultyValuesENpcInteractionType;
			}
			set
			{
				Array.Resize(ref _difficultyValuesENpcInteractionType, value.Length);
				value.CopyTo(_difficultyValuesENpcInteractionType, 0);
			}
		}

		[ProtoMember(15, Name = "difficultyValuesEMissionAreaType", DataFormat = DataFormat.Default)]
		public DifficultyConfigurationEMissionAreaType[] difficultyValuesEMissionAreaType
		{
			get
			{
				return _difficultyValuesEMissionAreaType;
			}
			set
			{
				Array.Resize(ref _difficultyValuesEMissionAreaType, value.Length);
				value.CopyTo(_difficultyValuesEMissionAreaType, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
