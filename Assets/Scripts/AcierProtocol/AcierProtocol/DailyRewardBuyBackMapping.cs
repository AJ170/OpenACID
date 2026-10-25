using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "DailyRewardBuyBackMapping")]
	public class DailyRewardBuyBackMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Entry")]
		public class Entry : IExtensible
		{
			private int _missedDays;

			private Resources _cost;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "missedDays", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int missedDays
			{
				get
				{
					return _missedDays;
				}
				set
				{
					_missedDays = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "cost", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public Resources cost
			{
				get
				{
					return _cost;
				}
				set
				{
					_cost = value;
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
