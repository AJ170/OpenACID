using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "PerformanceTrack")]
	public class PerformanceTrack : IExtensible
	{
		private string _deviceModelName = "";

		private string _environmentName = "";

		private float _minFps;

		private float _maxFps;

		private float _avgFps;

		private TrackingDeviceInfo _deviceInfoTrack;

		private TrackingAppInfo _appInfoTrack;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "deviceModelName", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string deviceModelName
		{
			get
			{
				return _deviceModelName;
			}
			set
			{
				_deviceModelName = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "environmentName", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string environmentName
		{
			get
			{
				return _environmentName;
			}
			set
			{
				_environmentName = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "minFps", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float minFps
		{
			get
			{
				return _minFps;
			}
			set
			{
				_minFps = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "maxFps", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float maxFps
		{
			get
			{
				return _maxFps;
			}
			set
			{
				_maxFps = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "avgFps", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float avgFps
		{
			get
			{
				return _avgFps;
			}
			set
			{
				_avgFps = value;
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
