using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "Event")]
	public class Event : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Property")]
		public class Property : IExtensible
		{
			private string _name = "";

			private string _value = "";

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
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

			[ProtoMember(2, IsRequired = false, Name = "value", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string value
			{
				get
				{
					return _value;
				}
				set
				{
					_value = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "LootProperty")]
		public class LootProperty : IExtensible
		{
			private string _name = "";

			private LootTemplate _value;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
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

			[ProtoMember(2, IsRequired = false, Name = "value", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public LootTemplate value
			{
				get
				{
					return _value;
				}
				set
				{
					_value = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private EEventType _type;

		private string _name = "";

		private long _startTimeUTC;

		private long _endTimeUTC;

		private Property[] _properties = new Property[0];

		private LootProperty[] _lootProperties = new LootProperty[0];

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

		[ProtoMember(3, IsRequired = false, Name = "startTimeUTC", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long startTimeUTC
		{
			get
			{
				return _startTimeUTC;
			}
			set
			{
				_startTimeUTC = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "endTimeUTC", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long endTimeUTC
		{
			get
			{
				return _endTimeUTC;
			}
			set
			{
				_endTimeUTC = value;
			}
		}

		[ProtoMember(5, Name = "properties", DataFormat = DataFormat.Default)]
		public Property[] properties
		{
			get
			{
				return _properties;
			}
			set
			{
				Array.Resize(ref _properties, value.Length);
				value.CopyTo(_properties, 0);
			}
		}

		[ProtoMember(6, Name = "lootProperties", DataFormat = DataFormat.Default)]
		public LootProperty[] lootProperties
		{
			get
			{
				return _lootProperties;
			}
			set
			{
				Array.Resize(ref _lootProperties, value.Length);
				value.CopyTo(_lootProperties, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
