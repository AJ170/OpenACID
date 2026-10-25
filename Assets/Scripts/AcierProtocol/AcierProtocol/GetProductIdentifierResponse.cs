using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "GetProductIdentifierResponse")]
	public class GetProductIdentifierResponse : IExtensible
	{
		private ShopTable _shopTable;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "shopTable", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ShopTable shopTable
		{
			get
			{
				return _shopTable;
			}
			set
			{
				_shopTable = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
