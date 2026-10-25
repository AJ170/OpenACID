using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionAlternatives")]
	public class MissionAlternatives : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Entry")]
		public class Entry : IExtensible
		{
			private string _basename = "";

			private int _seed;

			private int _objectiveLangKey;

			private int _missionDescription;

			private int _missionFlavorTextLoca;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "basename", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string basename
			{
				get
				{
					return _basename;
				}
				set
				{
					_basename = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "seed", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int seed
			{
				get
				{
					return _seed;
				}
				set
				{
					_seed = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "objectiveLangKey", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int objectiveLangKey
			{
				get
				{
					return _objectiveLangKey;
				}
				set
				{
					_objectiveLangKey = value;
				}
			}

			[ProtoMember(4, IsRequired = false, Name = "missionDescription", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int missionDescription
			{
				get
				{
					return _missionDescription;
				}
				set
				{
					_missionDescription = value;
				}
			}

			[ProtoMember(5, IsRequired = false, Name = "missionFlavorTextLoca", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int missionFlavorTextLoca
			{
				get
				{
					return _missionFlavorTextLoca;
				}
				set
				{
					_missionFlavorTextLoca = value;
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
