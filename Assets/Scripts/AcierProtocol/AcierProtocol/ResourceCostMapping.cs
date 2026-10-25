using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ResourceCostMapping")]
	public class ResourceCostMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Entry")]
		public class Entry : IExtensible
		{
			private long _amount;

			private double _exchange;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "amount", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public long amount
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

			[ProtoMember(2, IsRequired = false, Name = "exchange", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0.0)]
			public double exchange
			{
				get
				{
					return _exchange;
				}
				set
				{
					_exchange = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "Table")]
		public class Table : IExtensible
		{
			private EResourceType _resType = EResourceType.HardCurrency;

			private Entry[] _entries = new Entry[0];

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "resType", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EResourceType.HardCurrency)]
			public EResourceType resType
			{
				get
				{
					return _resType;
				}
				set
				{
					_resType = value;
				}
			}

			[ProtoMember(2, Name = "entries", DataFormat = DataFormat.Default)]
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

		[Serializable]
		[ProtoContract(Name = "ResourceToSoftCurrency")]
		public class ResourceToSoftCurrency : IExtensible
		{
			private EResourceType _resType = EResourceType.HardCurrency;

			private double _factor;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "resType", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EResourceType.HardCurrency)]
			public EResourceType resType
			{
				get
				{
					return _resType;
				}
				set
				{
					_resType = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "factor", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0.0)]
			public double factor
			{
				get
				{
					return _factor;
				}
				set
				{
					_factor = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private Table[] _tables = new Table[0];

		private ResourceToSoftCurrency[] _factorTable = new ResourceToSoftCurrency[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "tables", DataFormat = DataFormat.Default)]
		public Table[] tables
		{
			get
			{
				return _tables;
			}
			set
			{
				Array.Resize(ref _tables, value.Length);
				value.CopyTo(_tables, 0);
			}
		}

		[ProtoMember(2, Name = "factorTable", DataFormat = DataFormat.Default)]
		public ResourceToSoftCurrency[] factorTable
		{
			get
			{
				return _factorTable;
			}
			set
			{
				Array.Resize(ref _factorTable, value.Length);
				value.CopyTo(_factorTable, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
