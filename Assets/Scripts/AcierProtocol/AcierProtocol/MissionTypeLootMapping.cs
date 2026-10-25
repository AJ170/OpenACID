using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionTypeLootMapping")]
	public class MissionTypeLootMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Entry")]
		public class Entry : IExtensible
		{
			private EMissionKind _missionKind;

			private LootFilter[] _lootFilters = new LootFilter[0];

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "missionKind", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EMissionKind.Escort_EMissionKind)]
			public EMissionKind missionKind
			{
				get
				{
					return _missionKind;
				}
				set
				{
					_missionKind = value;
				}
			}

			[ProtoMember(2, Name = "lootFilters", DataFormat = DataFormat.Default)]
			public LootFilter[] lootFilters
			{
				get
				{
					return _lootFilters;
				}
				set
				{
					Array.Resize(ref _lootFilters, value.Length);
					value.CopyTo(_lootFilters, 0);
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
