using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionConfigurations")]
	public class MissionConfigurations : IExtensible
	{
		private GlobalsMap _globalsMap;

		private LocationMap _locationMap;

		private MissionDefinitionMap _missionDefinitionMap;

		private InteractableObjectConfigurationData[] _interactableObjects = new InteractableObjectConfigurationData[0];

		private StaticObjectConfigurationData[] _staticObjects = new StaticObjectConfigurationData[0];

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "globalsMap", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public GlobalsMap globalsMap
		{
			get
			{
				return _globalsMap;
			}
			set
			{
				_globalsMap = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "locationMap", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LocationMap locationMap
		{
			get
			{
				return _locationMap;
			}
			set
			{
				_locationMap = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "missionDefinitionMap", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionDefinitionMap missionDefinitionMap
		{
			get
			{
				return _missionDefinitionMap;
			}
			set
			{
				_missionDefinitionMap = value;
			}
		}

		[ProtoMember(4, Name = "interactableObjects", DataFormat = DataFormat.Default)]
		public InteractableObjectConfigurationData[] interactableObjects
		{
			get
			{
				return _interactableObjects;
			}
			set
			{
				Array.Resize(ref _interactableObjects, value.Length);
				value.CopyTo(_interactableObjects, 0);
			}
		}

		[ProtoMember(5, Name = "staticObjects", DataFormat = DataFormat.Default)]
		public StaticObjectConfigurationData[] staticObjects
		{
			get
			{
				return _staticObjects;
			}
			set
			{
				Array.Resize(ref _staticObjects, value.Length);
				value.CopyTo(_staticObjects, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
