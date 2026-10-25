using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionData")]
	public class MissionData : IExtensible
	{
		private string _fullName = "";

		private string _baseName = "";

		private ERegion _region;

		private EEra _era;

		private EMood _mood;

		private string _objective = "";

		private int _budget;

		private int _difficulty;

		private int _rank;

		private int _seed;

		private EMissionKind _missionKind;

		private EObjectiveType _baseObjectiveType;

		private LangKey _objectiveLangKey;

		private LangKey _missionDescription;

		private LangKey _missionShortDescription;

		private LangKey _missionFlavorTextLoca;

		private MissionFile _mission;

		private MissionGoals _missionGoals;

		private string _campaignId = "";

		private int _storyMissionSortIndex;

		private string[] _reqEntitlements = new string[0];

		private string _regionAppearanceOption = "";

		private string _missionFileContent = "";

		private string _logFileContent = "";

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "fullName", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string fullName
		{
			get
			{
				return _fullName;
			}
			set
			{
				_fullName = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "baseName", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string baseName
		{
			get
			{
				return _baseName;
			}
			set
			{
				_baseName = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "region", DataFormat = DataFormat.TwosComplement)]
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

		[ProtoMember(4, IsRequired = false, Name = "era", DataFormat = DataFormat.TwosComplement)]
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

		[ProtoMember(5, IsRequired = false, Name = "mood", DataFormat = DataFormat.TwosComplement)]
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

		[ProtoMember(6, IsRequired = false, Name = "objective", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string objective
		{
			get
			{
				return _objective;
			}
			set
			{
				_objective = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "budget", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int budget
		{
			get
			{
				return _budget;
			}
			set
			{
				_budget = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "difficulty", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int difficulty
		{
			get
			{
				return _difficulty;
			}
			set
			{
				_difficulty = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "rank", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int rank
		{
			get
			{
				return _rank;
			}
			set
			{
				_rank = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "seed", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int seed
		{
			get
			{
				return _seed;
			}
			set
			{
				_seed = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "missionKind", DataFormat = DataFormat.TwosComplement)]
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

		[ProtoMember(12, IsRequired = false, Name = "baseObjectiveType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EObjectiveType.Deliver_EObjectiveType)]
		public EObjectiveType baseObjectiveType
		{
			get
			{
				return _baseObjectiveType;
			}
			set
			{
				_baseObjectiveType = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "objectiveLangKey", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LangKey objectiveLangKey
		{
			get
			{
				return _objectiveLangKey;
			}
			set
			{
				_objectiveLangKey = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "missionDescription", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LangKey missionDescription
		{
			get
			{
				return _missionDescription;
			}
			set
			{
				_missionDescription = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "missionShortDescription", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LangKey missionShortDescription
		{
			get
			{
				return _missionShortDescription;
			}
			set
			{
				_missionShortDescription = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "missionFlavorTextLoca", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LangKey missionFlavorTextLoca
		{
			get
			{
				return _missionFlavorTextLoca;
			}
			set
			{
				_missionFlavorTextLoca = value;
			}
		}

		[ProtoMember(17, IsRequired = false, Name = "mission", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionFile mission
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

		[ProtoMember(18, IsRequired = false, Name = "missionGoals", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionGoals missionGoals
		{
			get
			{
				return _missionGoals;
			}
			set
			{
				_missionGoals = value;
			}
		}

		[ProtoMember(50, IsRequired = false, Name = "campaignId", DataFormat = DataFormat.Default)]
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

		[ProtoMember(51, IsRequired = false, Name = "storyMissionSortIndex", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int storyMissionSortIndex
		{
			get
			{
				return _storyMissionSortIndex;
			}
			set
			{
				_storyMissionSortIndex = value;
			}
		}

		[ProtoMember(52, Name = "reqEntitlements", DataFormat = DataFormat.Default)]
		public string[] reqEntitlements
		{
			get
			{
				return _reqEntitlements;
			}
			set
			{
				Array.Resize(ref _reqEntitlements, value.Length);
				value.CopyTo(_reqEntitlements, 0);
			}
		}

		[ProtoMember(53, IsRequired = false, Name = "regionAppearanceOption", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string regionAppearanceOption
		{
			get
			{
				return _regionAppearanceOption;
			}
			set
			{
				_regionAppearanceOption = value;
			}
		}

		[ProtoMember(70, IsRequired = false, Name = "missionFileContent", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string missionFileContent
		{
			get
			{
				return _missionFileContent;
			}
			set
			{
				_missionFileContent = value;
			}
		}

		[ProtoMember(71, IsRequired = false, Name = "logFileContent", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string logFileContent
		{
			get
			{
				return _logFileContent;
			}
			set
			{
				_logFileContent = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
