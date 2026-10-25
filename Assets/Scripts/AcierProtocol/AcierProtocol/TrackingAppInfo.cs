using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "TrackingAppInfo")]
	public class TrackingAppInfo : IExtensible
	{
		private string _buildNumber = "";

		private int _serverProtocolVersion;

		private long _appLaunchTime;

		private string _appLanguage = "";

		private string _bundleVersion = "";

		private string _qualitySettingsName = "";

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "buildNumber", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string buildNumber
		{
			get
			{
				return _buildNumber;
			}
			set
			{
				_buildNumber = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "serverProtocolVersion", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int serverProtocolVersion
		{
			get
			{
				return _serverProtocolVersion;
			}
			set
			{
				_serverProtocolVersion = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "appLaunchTime", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long appLaunchTime
		{
			get
			{
				return _appLaunchTime;
			}
			set
			{
				_appLaunchTime = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "appLanguage", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string appLanguage
		{
			get
			{
				return _appLanguage;
			}
			set
			{
				_appLanguage = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "bundleVersion", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string bundleVersion
		{
			get
			{
				return _bundleVersion;
			}
			set
			{
				_bundleVersion = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "qualitySettingsName", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string qualitySettingsName
		{
			get
			{
				return _qualitySettingsName;
			}
			set
			{
				_qualitySettingsName = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
