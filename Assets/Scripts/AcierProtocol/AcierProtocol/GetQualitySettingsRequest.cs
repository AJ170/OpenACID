using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "GetQualitySettingsRequest")]
	public class GetQualitySettingsRequest : IExtensible
	{
		private string _deviceIdentifier = "";

		private TrackingDeviceInfo _deviceInfoTrack;

		private TrackingAppInfo _appInfoTrack;

		private IExtension extensionObject;

		[ProtoMember(8, IsRequired = false, Name = "deviceIdentifier", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string deviceIdentifier
		{
			get
			{
				return _deviceIdentifier;
			}
			set
			{
				_deviceIdentifier = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "deviceInfoTrack", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public TrackingDeviceInfo deviceInfoTrack
		{
			get
			{
				return _deviceInfoTrack;
			}
			set
			{
				_deviceInfoTrack = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "appInfoTrack", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public TrackingAppInfo appInfoTrack
		{
			get
			{
				return _appInfoTrack;
			}
			set
			{
				_appInfoTrack = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
