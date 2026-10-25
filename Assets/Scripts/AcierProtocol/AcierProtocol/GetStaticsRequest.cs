using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "GetStaticsRequest")]
	public class GetStaticsRequest : IExtensible
	{
		private long _staticsVersion;

		private EShopPortal _shopPortalId = EShopPortal.All;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "staticsVersion", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long staticsVersion
		{
			get
			{
				return _staticsVersion;
			}
			set
			{
				_staticsVersion = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "shopPortalId", DataFormat = DataFormat.TwosComplement)]
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
