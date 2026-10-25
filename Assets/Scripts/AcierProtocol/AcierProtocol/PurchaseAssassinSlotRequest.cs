using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "PurchaseAssassinSlotRequest")]
	public class PurchaseAssassinSlotRequest : IExtensible
	{
		private int _alreadyPurchasedAssassinSlots;

		private EResourceType _paymentResource = EResourceType.HardCurrency;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "alreadyPurchasedAssassinSlots", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int alreadyPurchasedAssassinSlots
		{
			get
			{
				return _alreadyPurchasedAssassinSlots;
			}
			set
			{
				_alreadyPurchasedAssassinSlots = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "paymentResource", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EResourceType.HardCurrency)]
		public EResourceType paymentResource
		{
			get
			{
				return _paymentResource;
			}
			set
			{
				_paymentResource = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
