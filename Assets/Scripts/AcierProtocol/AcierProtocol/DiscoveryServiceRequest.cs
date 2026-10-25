using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "DiscoveryServiceRequest")]
	public class DiscoveryServiceRequest : IExtensible
	{
		private string _acierClientBuildVersion = "";

		private EPlatform _platform;

		private uint _acierClientProtocolVersion;

		private EServerEnvironment _environment;

		private string _deviceIdentifier = "";

		private TrackingDeviceInfo _deviceInfoTrack;

		private TrackingAppInfo _appInfoTrack;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "acierClientBuildVersion", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string acierClientBuildVersion
		{
			get
			{
				return _acierClientBuildVersion;
			}
			set
			{
				_acierClientBuildVersion = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "platform", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EPlatform.AppleDevice)]
		public EPlatform platform
		{
			get
			{
				return _platform;
			}
			set
			{
				_platform = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "acierClientProtocolVersion", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public uint acierClientProtocolVersion
		{
			get
			{
				return _acierClientProtocolVersion;
			}
			set
			{
				_acierClientProtocolVersion = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "environment", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EServerEnvironment.Trunk)]
		public EServerEnvironment environment
		{
			get
			{
				return _environment;
			}
			set
			{
				_environment = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "deviceIdentifier", DataFormat = DataFormat.Default)]
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
