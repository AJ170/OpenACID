using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MaxFriendsMapping")]
	public class MaxFriendsMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Entry")]
		public class Entry : IExtensible
		{
			private int _level;

			private int _maxFriends;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int level
			{
				get
				{
					return _level;
				}
				set
				{
					_level = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "maxFriends", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int maxFriends
			{
				get
				{
					return _maxFriends;
				}
				set
				{
					_maxFriends = value;
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
