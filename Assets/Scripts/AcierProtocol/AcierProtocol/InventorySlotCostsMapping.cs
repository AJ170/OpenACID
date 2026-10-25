using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "InventorySlotCostsMapping")]
	public class InventorySlotCostsMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Entry")]
		public class Entry : IExtensible
		{
			private uint _purchaseIndex;

			private Resources _costs;

			private uint _amount;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "purchaseIndex", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public uint purchaseIndex
			{
				get
				{
					return _purchaseIndex;
				}
				set
				{
					_purchaseIndex = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "costs", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public Resources costs
			{
				get
				{
					return _costs;
				}
				set
				{
					_costs = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "amount", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public uint amount
			{
				get
				{
					return _amount;
				}
				set
				{
					_amount = value;
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
