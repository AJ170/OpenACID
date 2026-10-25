using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ProductPricings")]
	public class ProductPricings : IExtensible
	{
		private ProductPricing[] _prices = new ProductPricing[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "prices", DataFormat = DataFormat.Default)]
		public ProductPricing[] prices
		{
			get
			{
				return _prices;
			}
			set
			{
				Array.Resize(ref _prices, value.Length);
				value.CopyTo(_prices, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
