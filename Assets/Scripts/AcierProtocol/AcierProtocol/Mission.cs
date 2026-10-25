using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "Mission")]
	public class Mission : IExtensible
	{
		private string _id;

		private ERuntimeMissionType _type = ERuntimeMissionType.RandomMission;

		private LocationSet _locationSet;

		private EObjectiveType _objectiveBaseType;

		private LangKey _objectiveLangKey;

		private int _rank;

		private string _baseName = "";

		private LangKey _missionDescription;

		private LangKey _missionFlavorTextLoca;

		private string _objectiveType = "";

		private bool _wasWon;

		private EMissionKind _missionKind;

		private int _difficulty;

		private bool _eliteMission;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = true, Name = "id", DataFormat = DataFormat.Default)]
		public string id
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(ERuntimeMissionType.RandomMission)]
		public ERuntimeMissionType type
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

		[ProtoMember(3, IsRequired = false, Name = "locationSet", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LocationSet locationSet
		{
			get
			{
				return _locationSet;
			}
			set
			{
				_locationSet = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "objectiveBaseType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EObjectiveType.Deliver_EObjectiveType)]
		public EObjectiveType objectiveBaseType
		{
			get
			{
				return _objectiveBaseType;
			}
			set
			{
				_objectiveBaseType = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "objectiveLangKey", DataFormat = DataFormat.Default)]
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

		[ProtoMember(7, IsRequired = false, Name = "rank", DataFormat = DataFormat.TwosComplement)]
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

		[ProtoMember(8, IsRequired = false, Name = "baseName", DataFormat = DataFormat.Default)]
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

		[ProtoMember(9, IsRequired = false, Name = "missionDescription", DataFormat = DataFormat.Default)]
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

		[ProtoMember(10, IsRequired = false, Name = "missionFlavorTextLoca", DataFormat = DataFormat.Default)]
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

		[ProtoMember(11, IsRequired = false, Name = "objectiveType", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string objectiveType
		{
			get
			{
				return _objectiveType;
			}
			set
			{
				_objectiveType = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "wasWon", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool wasWon
		{
			get
			{
				return _wasWon;
			}
			set
			{
				_wasWon = value;
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

		[ProtoMember(16, IsRequired = false, Name = "difficulty", DataFormat = DataFormat.TwosComplement)]
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

		[ProtoMember(17, IsRequired = false, Name = "eliteMission", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool eliteMission
		{
			get
			{
				return _eliteMission;
			}
			set
			{
				_eliteMission = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
