using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionUnlockMapping")]
	public class MissionUnlockMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Entry")]
		public class Entry : IExtensible
		{
			private string _mission = "";

			private string[] _reqEntitlements = new string[0];

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "mission", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string mission
			{
				get
				{
					return _mission;
				}
				set
				{
					_mission = value;
				}
			}

			[ProtoMember(2, Name = "reqEntitlements", DataFormat = DataFormat.Default)]
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
