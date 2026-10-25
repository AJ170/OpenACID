using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "TrackingDeviceInfo")]
	public class TrackingDeviceInfo : IExtensible
	{
		private string _device = "";

		private string _os = "";

		private string _deviceLanguage = "";

		private string _graphicsDeviceName = "";

		private long _systemMemorySize;

		private string _processorType = "";

		private EQualityTextureType _textureType;

		private EPlatform _platform;

		private bool _tv;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "device", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string device
		{
			get
			{
				return _device;
			}
			set
			{
				_device = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "os", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string os
		{
			get
			{
				return _os;
			}
			set
			{
				_os = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "deviceLanguage", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string deviceLanguage
		{
			get
			{
				return _deviceLanguage;
			}
			set
			{
				_deviceLanguage = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "graphicsDeviceName", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string graphicsDeviceName
		{
			get
			{
				return _graphicsDeviceName;
			}
			set
			{
				_graphicsDeviceName = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "systemMemorySize", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long systemMemorySize
		{
			get
			{
				return _systemMemorySize;
			}
			set
			{
				_systemMemorySize = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "processorType", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string processorType
		{
			get
			{
				return _processorType;
			}
			set
			{
				_processorType = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "textureType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EQualityTextureType.Generic)]
		public EQualityTextureType textureType
		{
			get
			{
				return _textureType;
			}
			set
			{
				_textureType = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "platform", DataFormat = DataFormat.TwosComplement)]
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

		[ProtoMember(9, IsRequired = false, Name = "tv", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool tv
		{
			get
			{
				return _tv;
			}
			set
			{
				_tv = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
