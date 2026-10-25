using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "TrackingEvent")]
	public class TrackingEvent : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "TrackingParam")]
		public class TrackingParam : IExtensible
		{
			private string _key = "";

			private string _value = "";

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "key", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string key
			{
				get
				{
					return _key;
				}
				set
				{
					_key = value;
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

		private ETrackingEventType _eventType;

		private TrackingParam[] _trackingParams = new TrackingParam[0];

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "eventType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(ETrackingEventType.AppLaunch)]
		public ETrackingEventType eventType
		{
			get
			{
				return _eventType;
			}
			set
			{
				_eventType = value;
			}
		}

		[ProtoMember(2, Name = "trackingParams", DataFormat = DataFormat.Default)]
		public TrackingParam[] trackingParams
		{
			get
			{
				return _trackingParams;
			}
			set
			{
				Array.Resize(ref _trackingParams, value.Length);
				value.CopyTo(_trackingParams, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
