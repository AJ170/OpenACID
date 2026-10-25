using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionDefinition")]
	public class MissionDefinition : IExtensible
	{
		private MissionPhase[] _phases = new MissionPhase[0];

		private GlobalMissionParameter[] _globalParameters = new GlobalMissionParameter[0];

		private ChallengeObjective[] _challenges = new ChallengeObjective[0];

		private DifficultyConfigurationInteger[] _challengeCount = new DifficultyConfigurationInteger[0];

		private ModifierObjective[] _modifiers = new ModifierObjective[0];

		private DifficultyConfigurationInteger[] _modifierCount = new DifficultyConfigurationInteger[0];

		private ChallengeObjective[] _forcedChallenges = new ChallengeObjective[0];

		private EObjective _baseType;

		private EObjectiveType _iconType;

		private string _objectiveKind = "";

		private int[] _locaGroup = new int[0];

		private int[] _storyLocaGroup = new int[0];

		private EMissionKind _missionKind;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "phases", DataFormat = DataFormat.Default)]
		public MissionPhase[] phases
		{
			get
			{
				return _phases;
			}
			set
			{
				Array.Resize(ref _phases, value.Length);
				value.CopyTo(_phases, 0);
			}
		}

		[ProtoMember(8, Name = "globalParameters", DataFormat = DataFormat.Default)]
		public GlobalMissionParameter[] globalParameters
		{
			get
			{
				return _globalParameters;
			}
			set
			{
				Array.Resize(ref _globalParameters, value.Length);
				value.CopyTo(_globalParameters, 0);
			}
		}

		[ProtoMember(9, Name = "challenges", DataFormat = DataFormat.Default)]
		public ChallengeObjective[] challenges
		{
			get
			{
				return _challenges;
			}
			set
			{
				Array.Resize(ref _challenges, value.Length);
				value.CopyTo(_challenges, 0);
			}
		}

		[ProtoMember(10, Name = "challengeCount", DataFormat = DataFormat.Default)]
		public DifficultyConfigurationInteger[] challengeCount
		{
			get
			{
				return _challengeCount;
			}
			set
			{
				Array.Resize(ref _challengeCount, value.Length);
				value.CopyTo(_challengeCount, 0);
			}
		}

		[ProtoMember(11, Name = "modifiers", DataFormat = DataFormat.Default)]
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

		[ProtoMember(12, Name = "modifierCount", DataFormat = DataFormat.Default)]
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

		[ProtoMember(14, Name = "forcedChallenges", DataFormat = DataFormat.Default)]
		public ChallengeObjective[] forcedChallenges
		{
			get
			{
				return _forcedChallenges;
			}
			set
			{
				Array.Resize(ref _forcedChallenges, value.Length);
				value.CopyTo(_forcedChallenges, 0);
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "baseType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EObjective.Deliver)]
		public EObjective baseType
		{
			get
			{
				return _baseType;
			}
			set
			{
				_baseType = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "iconType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EObjectiveType.Deliver_EObjectiveType)]
		public EObjectiveType iconType
		{
			get
			{
				return _iconType;
			}
			set
			{
				_iconType = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "objectiveKind", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string objectiveKind
		{
			get
			{
				return _objectiveKind;
			}
			set
			{
				_objectiveKind = value;
			}
		}

		[ProtoMember(4, Name = "locaGroup", DataFormat = DataFormat.TwosComplement)]
		public int[] locaGroup
		{
			get
			{
				return _locaGroup;
			}
			set
			{
				Array.Resize(ref _locaGroup, value.Length);
				value.CopyTo(_locaGroup, 0);
			}
		}

		[ProtoMember(5, Name = "storyLocaGroup", DataFormat = DataFormat.TwosComplement)]
		public int[] storyLocaGroup
		{
			get
			{
				return _storyLocaGroup;
			}
			set
			{
				Array.Resize(ref _storyLocaGroup, value.Length);
				value.CopyTo(_storyLocaGroup, 0);
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "missionKind", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EMissionKind.Escort_EMissionKind)]
		public EMissionKind missionKind
		{
			get
			{
				return _missionKind;
			}
			set
			{
				_missionKind = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
