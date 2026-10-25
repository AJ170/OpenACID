using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "EntitlementInventory")]
	public class EntitlementInventory : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Entry")]
		public class Entry : IExtensible
		{
			private string _gameIdGuid = "";

			private long _unlockDate;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "gameIdGuid", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string gameIdGuid
			{
				get
				{
					return _gameIdGuid;
				}
				set
				{
					_gameIdGuid = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "unlockDate", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public long unlockDate
			{
				get
				{
					return _unlockDate;
				}
				set
				{
					_unlockDate = value;
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
