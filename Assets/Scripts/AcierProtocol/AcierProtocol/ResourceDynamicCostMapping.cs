using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ResourceDynamicCostMapping")]
	public class ResourceDynamicCostMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "CostEntry")]
		public class CostEntry : IExtensible
		{
			private uint _purchaseIndex;

			private Resources _costs;

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

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "DynamicCostEntry")]
		public class DynamicCostEntry : IExtensible
		{
			private EResourceType _resourceType = EResourceType.HardCurrency;

			private CostEntry[] _entries = new CostEntry[0];

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "resourceType", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EResourceType.HardCurrency)]
			public EResourceType resourceType
			{
				get
				{
					return _resourceType;
				}
				set
				{
					_resourceType = value;
				}
			}

			[ProtoMember(2, Name = "entries", DataFormat = DataFormat.Default)]
			public CostEntry[] entries
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

		private DynamicCostEntry[] _dynamicCostEntries = new DynamicCostEntry[0];

		private long _version;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "dynamicCostEntries", DataFormat = DataFormat.Default)]
		public DynamicCostEntry[] dynamicCostEntries
		{
			get
			{
				return _dynamicCostEntries;
			}
			set
			{
				Array.Resize(ref _dynamicCostEntries, value.Length);
				value.CopyTo(_dynamicCostEntries, 0);
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
