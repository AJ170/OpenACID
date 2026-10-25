using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionPhase")]
	public class MissionPhase : IExtensible
	{
		private MissionObjective[] _missionObjectives = new MissionObjective[0];

		private DifficultyConfigurationInteger[] _modifierCount = new DifficultyConfigurationInteger[0];

		private ModifierObjective[] _modifiers = new ModifierObjective[0];

		private long _phase;

		private long _phaseNextFail;

		private long _phaseNextSuccess;

		private float _budgetWeight;

		private EObjectiveType _trackerIcon;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "missionObjectives", DataFormat = DataFormat.Default)]
		public MissionObjective[] missionObjectives
		{
			get
			{
				return _missionObjectives;
			}
			set
			{
				Array.Resize(ref _missionObjectives, value.Length);
				value.CopyTo(_missionObjectives, 0);
			}
		}

		[ProtoMember(2, Name = "modifierCount", DataFormat = DataFormat.Default)]
		public DifficultyConfigurationInteger[] modifierCount
		{
			get
			{
				return _modifierCount;
			}
			set
			{
				Array.Resize(ref _modifierCount, value.Length);
				value.CopyTo(_modifierCount, 0);
			}
		}

		[ProtoMember(3, Name = "modifiers", DataFormat = DataFormat.Default)]
		public ModifierObjective[] modifiers
		{
			get
			{
				return _modifiers;
			}
			set
			{
				Array.Resize(ref _modifiers, value.Length);
				value.CopyTo(_modifiers, 0);
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "phase", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long phase
		{
			get
			{
				return _phase;
			}
			set
			{
				_phase = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "phaseNextFail", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long phaseNextFail
		{
			get
			{
				return _phaseNextFail;
			}
			set
			{
				_phaseNextFail = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "phaseNextSuccess", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long phaseNextSuccess
		{
			get
			{
				return _phaseNextSuccess;
			}
			set
			{
				_phaseNextSuccess = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "budgetWeight", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float budgetWeight
		{
			get
			{
				return _budgetWeight;
			}
			set
			{
				_budgetWeight = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "trackerIcon", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EObjectiveType.Deliver_EObjectiveType)]
		public EObjectiveType trackerIcon
		{
			get
			{
				return _trackerIcon;
			}
			set
			{
				_trackerIcon = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
