using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveBase")]
	public class ObjectiveBase : IExtensible
	{
		private int _id;

		private int _sharedId;

		private int _sharedIdSecondary;

		private bool _spawnInPreviousPhase;

		private bool _despawnUnitsOnTermination;

		private bool _unitSpawnProvider;

		private EObjective _objectiveType;

		private Parameter[] _parameters = new Parameter[0];

		private int[] _descriptionLocaGroup = new int[0];

		private int[] _shortDescriptionLocaGroup = new int[0];

		private int[] _secondaryDescriptionLocaGroup = new int[0];

		private EMapIconType _mapIcon;

		private int[] _loadoutLocaGroup = new int[0];

		private bool _isHiddenInTracker;

		private bool _isOptional;

		private bool _isHiddenInUi;

		private IExtension extensionObject;

		[ProtoMember(14, IsRequired = false, Name = "id", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int id
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

		[ProtoMember(1, IsRequired = false, Name = "sharedId", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int sharedId
		{
			get
			{
				return _sharedId;
			}
			set
			{
				_sharedId = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "sharedIdSecondary", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int sharedIdSecondary
		{
			get
			{
				return _sharedIdSecondary;
			}
			set
			{
				_sharedIdSecondary = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "spawnInPreviousPhase", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool spawnInPreviousPhase
		{
			get
			{
				return _spawnInPreviousPhase;
			}
			set
			{
				_spawnInPreviousPhase = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "despawnUnitsOnTermination", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool despawnUnitsOnTermination
		{
			get
			{
				return _despawnUnitsOnTermination;
			}
			set
			{
				_despawnUnitsOnTermination = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "unitSpawnProvider", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool unitSpawnProvider
		{
			get
			{
				return _unitSpawnProvider;
			}
			set
			{
				_unitSpawnProvider = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "objectiveType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EObjective.Deliver)]
		public EObjective objectiveType
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

		[ProtoMember(7, Name = "parameters", DataFormat = DataFormat.Default)]
		public Parameter[] parameters
		{
			get
			{
				return _parameters;
			}
			set
			{
				Array.Resize(ref _parameters, value.Length);
				value.CopyTo(_parameters, 0);
			}
		}

		[ProtoMember(8, Name = "descriptionLocaGroup", DataFormat = DataFormat.TwosComplement)]
		public int[] descriptionLocaGroup
		{
			get
			{
				return _descriptionLocaGroup;
			}
			set
			{
				Array.Resize(ref _descriptionLocaGroup, value.Length);
				value.CopyTo(_descriptionLocaGroup, 0);
			}
		}

		[ProtoMember(9, Name = "shortDescriptionLocaGroup", DataFormat = DataFormat.TwosComplement)]
		public int[] shortDescriptionLocaGroup
		{
			get
			{
				return _shortDescriptionLocaGroup;
			}
			set
			{
				Array.Resize(ref _shortDescriptionLocaGroup, value.Length);
				value.CopyTo(_shortDescriptionLocaGroup, 0);
			}
		}

		[ProtoMember(17, Name = "secondaryDescriptionLocaGroup", DataFormat = DataFormat.TwosComplement)]
		public int[] secondaryDescriptionLocaGroup
		{
			get
			{
				return _secondaryDescriptionLocaGroup;
			}
			set
			{
				Array.Resize(ref _secondaryDescriptionLocaGroup, value.Length);
				value.CopyTo(_secondaryDescriptionLocaGroup, 0);
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "mapIcon", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EMapIconType.Invalid_EMapIconType)]
		public EMapIconType mapIcon
		{
			get
			{
				return _mapIcon;
			}
			set
			{
				_mapIcon = value;
			}
		}

		[ProtoMember(11, Name = "loadoutLocaGroup", DataFormat = DataFormat.TwosComplement)]
		public int[] loadoutLocaGroup
		{
			get
			{
				return _loadoutLocaGroup;
			}
			set
			{
				Array.Resize(ref _loadoutLocaGroup, value.Length);
				value.CopyTo(_loadoutLocaGroup, 0);
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "isHiddenInTracker", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool isHiddenInTracker
		{
			get
			{
				return _isHiddenInTracker;
			}
			set
			{
				_isHiddenInTracker = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "isOptional", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool isOptional
		{
			get
			{
				return _isOptional;
			}
			set
			{
				_isOptional = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "isHiddenInUi", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool isHiddenInUi
		{
			get
			{
				return _isHiddenInUi;
			}
			set
			{
				_isHiddenInUi = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
