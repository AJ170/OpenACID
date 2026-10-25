using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "AuthAccountRequest")]
	public class AuthAccountRequest : IExtensible
	{
		private string _username = "";

		private string _password = "";

		private TrackingDeviceInfo _trackingDeviceInfo;

		private TrackingAppInfo _trackingAppInfo;

		private string _ubiFederatedId = "";

		private string _ubiPushToken = "";

		private ELanguages _locale;

		private bool _firstAuth;

		private TrackingEvent[] _trackingEvents = new TrackingEvent[0];

		private string _signedData = "";

		private string _signature = "";

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "username", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string username
		{
			get
			{
				return _username;
			}
			set
			{
				_username = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "password", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string password
		{
			get
			{
				return _password;
			}
			set
			{
				_password = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "trackingDeviceInfo", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public TrackingDeviceInfo trackingDeviceInfo
		{
			get
			{
				return _trackingDeviceInfo;
			}
			set
			{
				_trackingDeviceInfo = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "trackingAppInfo", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public TrackingAppInfo trackingAppInfo
		{
			get
			{
				return _trackingAppInfo;
			}
			set
			{
				_trackingAppInfo = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "ubiFederatedId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string ubiFederatedId
		{
			get
			{
				return _ubiFederatedId;
			}
			set
			{
				_ubiFederatedId = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "ubiPushToken", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string ubiPushToken
		{
			get
			{
				return _ubiPushToken;
			}
			set
			{
				_ubiPushToken = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "locale", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(ELanguages.BlueByte)]
		public ELanguages locale
		{
			get
			{
				return _locale;
			}
			set
			{
				_locale = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "firstAuth", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool firstAuth
		{
			get
			{
				return _firstAuth;
			}
			set
			{
				_firstAuth = value;
			}
		}

		[ProtoMember(9, Name = "trackingEvents", DataFormat = DataFormat.Default)]
		public TrackingEvent[] trackingEvents
		{
			get
			{
				return _trackingEvents;
			}
			set
			{
				Array.Resize(ref _trackingEvents, value.Length);
				value.CopyTo(_trackingEvents, 0);
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "signedData", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string signedData
		{
			get
			{
				return _signedData;
			}
			set
			{
				_signedData = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "signature", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string signature
		{
			get
			{
				return _signature;
			}
			set
			{
				_signature = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
