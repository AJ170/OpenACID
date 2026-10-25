using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "LocationUnlockMappings")]
	public class LocationUnlockMappings : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Entry")]
		public class Entry : IExtensible
		{
			private LocationSet _location;

			private string _sceneName = "";

			private string[] _reqEntitlements = new string[0];

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "location", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public LocationSet location
			{
				get
				{
					return _location;
				}
				set
				{
					_location = value;
				}
			}

			[ProtoMember(11, IsRequired = false, Name = "sceneName", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string sceneName
			{
				get
				{
					return _sceneName;
				}
				set
				{
					_sceneName = value;
				}
			}

			[ProtoMember(12, Name = "reqEntitlements", DataFormat = DataFormat.Default)]
			public string[] reqEntitlements
			{
				get
				{
					return _reqEntitlements;
				}
				set
				{
					Array.Resize(ref _reqEntitlements, value.Length);
					value.CopyTo(_reqEntitlements, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private Entry[] _entries = new Entry[0];

		private long _version;

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

		[ProtoMember(2, IsRequired = false, Name = "version", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long version
		{
			get
			{
				return _version;
			}
			set
			{
				_version = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
