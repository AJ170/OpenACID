using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ProductPricing")]
	public class ProductPricing : IExtensible
	{
		private string _productId = "";

		private string _productPrice = "";

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "productId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string productId
		{
			get
			{
				return _productId;
			}
			set
			{
				_productId = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "productPrice", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string productPrice
		{
			get
			{
				return _productPrice;
			}
			set
			{
				_productPrice = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
