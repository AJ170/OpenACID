using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "QualitySettingsMapping")]
	public class QualitySettingsMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Entry")]
		public class Entry : IExtensible
		{
			private string _deviceId = "";

			private QualitySettings _qualitySettings;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "deviceId", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string deviceId
			{
				get
				{
					return _deviceId;
				}
				set
				{
					_deviceId = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "qualitySettings", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public QualitySettings qualitySettings
			{
				get
				{
					return _qualitySettings;
				}
				set
				{
					_qualitySettings = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private Entry[] _entries = new Entry[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "entries", DataFormat = DataFormat.Default)]
		public Entry[] entries
		{
			get
			{
				return _entries;
			}
			set
			{
				Array.Resize(ref _entries, value.Length);
				value.CopyTo(_entries, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
