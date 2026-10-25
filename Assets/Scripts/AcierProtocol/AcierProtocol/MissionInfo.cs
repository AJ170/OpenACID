using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionInfo")]
	public class MissionInfo : IExtensible
	{
		private EObjectiveType _objectiveBaseType;

		private string _baseName = "";

		private string _objective = "";

		private LangKey _objectiveLangKey;

		private int _missionDescription;

		private int _missionFlavorTextLoca;

		private string[] _reqEntitlements = new string[0];

		private int _sortIndex;

		private int _rank;

		private int _difficulty;

		private EMissionKind _missionKind;

		private string _regionAppearanceOption = "";

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "objectiveBaseType", DataFormat = DataFormat.TwosComplement)]
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

		[ProtoMember(3, IsRequired = false, Name = "objective", DataFormat = DataFormat.Default)]
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

		[ProtoMember(4, IsRequired = false, Name = "objectiveLangKey", DataFormat = DataFormat.Default)]
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

		[ProtoMember(5, IsRequired = false, Name = "missionDescription", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int missionDescription
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

		[ProtoMember(6, IsRequired = false, Name = "missionFlavorTextLoca", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int missionFlavorTextLoca
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

		[ProtoMember(7, Name = "reqEntitlements", DataFormat = DataFormat.Default)]
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

		[ProtoMember(8, IsRequired = false, Name = "sortIndex", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int sortIndex
		{
			get
			{
				return _sortIndex;
			}
			set
			{
				_sortIndex = value;
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

		[ProtoMember(10, IsRequired = false, Name = "difficulty", DataFormat = DataFormat.TwosComplement)]
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

		[ProtoMember(12, IsRequired = false, Name = "regionAppearanceOption", DataFormat = DataFormat.Default)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
