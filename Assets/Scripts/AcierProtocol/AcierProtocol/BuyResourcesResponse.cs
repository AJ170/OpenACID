using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "BuyResourcesResponse")]
	public class BuyResourcesResponse : IExtensible
	{
		private long _paidHardCurrency;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "paidHardCurrency", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long paidHardCurrency
		{
			get
			{
				return _paidHardCurrency;
			}
			set
			{
				_paidHardCurrency = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
