using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "EventTemplate")]
	public class EventTemplate : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "PropertyDefinition")]
		public class PropertyDefinition : IExtensible
		{
			[ProtoContract(Name = "EParameterType")]
			public enum EParameterType
			{
				[ProtoEnum(Name = "PARAM_FLOAT", Value = 0)]
				PARAM_FLOAT = 0,
				[ProtoEnum(Name = "PARAM_STRING", Value = 1)]
				PARAM_STRING = 1,
				[ProtoEnum(Name = "PARAM_INT", Value = 2)]
				PARAM_INT = 2,
				[ProtoEnum(Name = "PARAM_DATETIME", Value = 3)]
				PARAM_DATETIME = 3,
				[ProtoEnum(Name = "PARAM_BOOLEAN", Value = 4)]
				PARAM_BOOLEAN = 4,
				[ProtoEnum(Name = "PARAM_DATE", Value = 5)]
				PARAM_DATE = 5,
				[ProtoEnum(Name = "PARAM_ENUM", Value = 6)]
				PARAM_ENUM = 6
			}

			[ProtoContract(Name = "EUsageType")]
			public enum EUsageType
			{
				[ProtoEnum(Name = "USAGE_PROHIBITED", Value = 0)]
				USAGE_PROHIBITED = 0,
				[ProtoEnum(Name = "USAGE_OPTIONAL", Value = 1)]
				USAGE_OPTIONAL = 1,
				[ProtoEnum(Name = "USAGE_REQUIRED", Value = 2)]
				USAGE_REQUIRED = 2
			}

			private EParameterType _type;

			private EUsageType _usage;

			private bool _changeable;

			private string _name = "";

			private string _defaultValue = "";

			private long _minValue = -1L;

			private long _maxValue = -1L;

			private long _stepSize = -1L;

			private string[] _validValues = new string[0];

			private string _description = "";

			private string[] _validIds = new string[0];

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EParameterType.PARAM_FLOAT)]
			public EParameterType type
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

			[ProtoMember(2, IsRequired = false, Name = "usage", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EUsageType.USAGE_PROHIBITED)]
			public EUsageType usage
			{
				get
				{
					return _usage;
				}
				set
				{
					_usage = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "changeable", DataFormat = DataFormat.Default)]
			[DefaultValue(false)]
			public bool changeable
			{
				get
				{
					return _changeable;
				}
				set
				{
					_changeable = value;
				}
			}

			[ProtoMember(4, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string name
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

			[ProtoMember(5, IsRequired = false, Name = "defaultValue", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string defaultValue
			{
				get
				{
					return _defaultValue;
				}
				set
				{
					_defaultValue = value;
				}
			}

			[ProtoMember(6, IsRequired = false, Name = "minValue", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(-1L)]
			public long minValue
			{
				get
				{
					return _minValue;
				}
				set
				{
					_minValue = value;
				}
			}

			[ProtoMember(7, IsRequired = false, Name = "maxValue", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(-1L)]
			public long maxValue
			{
				get
				{
					return _maxValue;
				}
				set
				{
					_maxValue = value;
				}
			}

			[ProtoMember(8, IsRequired = false, Name = "stepSize", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(-1L)]
			public long stepSize
			{
				get
				{
					return _stepSize;
				}
				set
				{
					_stepSize = value;
				}
			}

			[ProtoMember(9, Name = "validValues", DataFormat = DataFormat.Default)]
			public string[] validValues
			{
				get
				{
					return _validValues;
				}
				set
				{
					Array.Resize(ref _validValues, value.Length);
					value.CopyTo(_validValues, 0);
				}
			}

			[ProtoMember(10, IsRequired = false, Name = "description", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string description
			{
				get
				{
					return _description;
				}
				set
				{
					_description = value;
				}
			}

			[ProtoMember(11, Name = "validIds", DataFormat = DataFormat.Default)]
			public string[] validIds
			{
				get
				{
					return _validIds;
				}
				set
				{
					Array.Resize(ref _validIds, value.Length);
					value.CopyTo(_validIds, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private EEventType _type;

		private string _name = "";

		private string _description = "";

		private PropertyDefinition[] _propertyDefinitions = new PropertyDefinition[0];

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EEventType.Event_Shop_Category_Visibility)]
		public EEventType type
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

		[ProtoMember(2, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string name
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

		[ProtoMember(3, IsRequired = false, Name = "description", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string description
		{
			get
			{
				return _description;
			}
			set
			{
				_description = value;
			}
		}

		[ProtoMember(4, Name = "propertyDefinitions", DataFormat = DataFormat.Default)]
		public PropertyDefinition[] propertyDefinitions
		{
			get
			{
				return _propertyDefinitions;
			}
			set
			{
				Array.Resize(ref _propertyDefinitions, value.Length);
				value.CopyTo(_propertyDefinitions, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
