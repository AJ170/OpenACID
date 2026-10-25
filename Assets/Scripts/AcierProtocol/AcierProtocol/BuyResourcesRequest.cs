using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "BuyResourcesRequest")]
	public class BuyResourcesRequest : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Entry")]
		public class Entry : IExtensible
		{
			private EResourceType _resType = EResourceType.HardCurrency;

			private long _amount;

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

			[ProtoMember(2, IsRequired = false, Name = "amount", DataFormat = DataFormat.TwosComplement)]
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

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private Entry[] _entries = new Entry[0];

		private EModuleOrigin _origin;

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

		[ProtoMember(2, IsRequired = false, Name = "origin", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EModuleOrigin.ModuleOrigin_DailyReward)]
		public EModuleOrigin origin
		{
			get
			{
				return _origin;
			}
			set
			{
				_origin = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
