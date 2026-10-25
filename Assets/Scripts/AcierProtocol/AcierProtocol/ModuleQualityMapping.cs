using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ModuleQualityMapping")]
	public class ModuleQualityMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Entry")]
		public class Entry : IExtensible
		{
			private Quality.EQuality _moduleQuality = Quality.EQuality.Mundane;

			private Quality.EQuality _displayQuality = Quality.EQuality.Mundane;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "moduleQuality", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(Quality.EQuality.Mundane)]
			public Quality.EQuality moduleQuality
			{
				get
				{
					return _moduleQuality;
				}
				set
				{
					_moduleQuality = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "displayQuality", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(Quality.EQuality.Mundane)]
			public Quality.EQuality displayQuality
			{
				get
				{
					return _displayQuality;
				}
				set
				{
					_displayQuality = value;
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
