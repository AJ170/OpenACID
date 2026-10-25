using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "PurchaseInventorySlotRequest")]
	public class PurchaseInventorySlotRequest : IExtensible
	{
		private int _alreadyPurchasedInventorySlots;

		private EResourceType _paymentResource = EResourceType.HardCurrency;

		private int _amount;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "alreadyPurchasedInventorySlots", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int alreadyPurchasedInventorySlots
		{
			get
			{
				return _alreadyPurchasedInventorySlots;
			}
			set
			{
				_alreadyPurchasedInventorySlots = value;
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

		[ProtoMember(3, IsRequired = false, Name = "amount", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int amount
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
}
