using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "GetProductIdentifierRequest")]
	public class GetProductIdentifierRequest : IExtensible
	{
		private EShopPortal _shopPortalId = EShopPortal.All;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "shopPortalId", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EShopPortal.All)]
		public EShopPortal shopPortalId
		{
			get
			{
				return _shopPortalId;
			}
			set
			{
				_shopPortalId = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
