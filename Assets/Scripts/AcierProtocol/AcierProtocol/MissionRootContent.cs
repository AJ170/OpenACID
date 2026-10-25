using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionRootContent")]
	public class MissionRootContent : IExtensible
	{
		private MissionBaseContent _MissionBaseContent;

		private EEra _Age;

		private string _BriefingText = "";

		private int _CrowdDensity;

		private string _DebriefingText = "";

		private int _DifficultyLevel;

		private int _Rank;

		private string _GUID = "";

		private string _BaseName = "";

		private bool _Hidden;

		private int _MaxCrowdEntities;

		private EMissionType _MissionType;

		private EMood _Mood;

		private string _Name = "";

		private EObjectiveType _ObjectiveType;

		private ERegion _Region;

		private string _Scene = "";

		private string _GenerationTime = "";

		private string _MissionUrl = "";

		private string _PlayerAssassinGUID = "";

		private string _PlayerHirelingGUID = "";

		private EMissionKind _MissionKind;

		private IExtension extensionObject;

		[ProtoMember(20, IsRequired = false, Name = "MissionBaseContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionBaseContent MissionBaseContent
		{
			get
			{
				return _MissionBaseContent;
			}
			set
			{
				_MissionBaseContent = value;
			}
		}

		[ProtoMember(19, IsRequired = false, Name = "Age", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EEra.EEra_None)]
		public EEra Age
		{
			get
			{
				return _Age;
			}
			set
			{
				_Age = value;
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "BriefingText", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string BriefingText
		{
			get
			{
				return _BriefingText;
			}
			set
			{
				_BriefingText = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "CrowdDensity", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int CrowdDensity
		{
			get
			{
				return _CrowdDensity;
			}
			set
			{
				_CrowdDensity = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "DebriefingText", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string DebriefingText
		{
			get
			{
				return _DebriefingText;
			}
			set
			{
				_DebriefingText = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "DifficultyLevel", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int DifficultyLevel
		{
			get
			{
				return _DifficultyLevel;
			}
			set
			{
				_DifficultyLevel = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "Rank", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int Rank
		{
			get
			{
				return _Rank;
			}
			set
			{
				_Rank = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "GUID", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string GUID
		{
			get
			{
				return _GUID;
			}
			set
			{
				_GUID = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "BaseName", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string BaseName
		{
			get
			{
				return _BaseName;
			}
			set
			{
				_BaseName = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "Hidden", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool Hidden
		{
			get
			{
				return _Hidden;
			}
			set
			{
				_Hidden = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "MaxCrowdEntities", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int MaxCrowdEntities
		{
			get
			{
				return _MaxCrowdEntities;
			}
			set
			{
				_MaxCrowdEntities = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "MissionType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EMissionType.Animus_EMissionType)]
		public EMissionType MissionType
		{
			get
			{
				return _MissionType;
			}
			set
			{
				_MissionType = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "Mood", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EMood.Daytime)]
		public EMood Mood
		{
			get
			{
				return _Mood;
			}
			set
			{
				_Mood = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "Name", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				_Name = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "ObjectiveType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EObjectiveType.Deliver_EObjectiveType)]
		public EObjectiveType ObjectiveType
		{
			get
			{
				return _ObjectiveType;
			}
			set
			{
				_ObjectiveType = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "Region", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(ERegion.Animus)]
		public ERegion Region
		{
			get
			{
				return _Region;
			}
			set
			{
				_Region = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "Scene", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string Scene
		{
			get
			{
				return _Scene;
			}
			set
			{
				_Scene = value;
			}
		}

		[ProtoMember(18, IsRequired = false, Name = "GenerationTime", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string GenerationTime
		{
			get
			{
				return _GenerationTime;
			}
			set
			{
				_GenerationTime = value;
			}
		}

		[ProtoMember(21, IsRequired = false, Name = "MissionUrl", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string MissionUrl
		{
			get
			{
				return _MissionUrl;
			}
			set
			{
				_MissionUrl = value;
			}
		}

		[ProtoMember(22, IsRequired = false, Name = "PlayerAssassinGUID", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string PlayerAssassinGUID
		{
			get
			{
				return _PlayerAssassinGUID;
			}
			set
			{
				_PlayerAssassinGUID = value;
			}
		}

		[ProtoMember(23, IsRequired = false, Name = "PlayerHirelingGUID", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string PlayerHirelingGUID
		{
			get
			{
				return _PlayerHirelingGUID;
			}
			set
			{
				_PlayerHirelingGUID = value;
			}
		}

		[ProtoMember(24, IsRequired = false, Name = "MissionKind", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EMissionKind.Escort_EMissionKind)]
		public EMissionKind MissionKind
		{
			get
			{
				return _MissionKind;
			}
			set
			{
				_MissionKind = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
