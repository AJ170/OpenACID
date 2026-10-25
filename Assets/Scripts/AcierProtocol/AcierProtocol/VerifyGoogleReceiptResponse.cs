using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "VerifyGoogleReceiptResponse")]
	public class VerifyGoogleReceiptResponse : IExtensible
	{
		private string _purchaseToken = "";

		private string _ubiTransactionId = "";

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "purchaseToken", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string purchaseToken
		{
			get
			{
				return _purchaseToken;
			}
			set
			{
				_purchaseToken = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "ubiTransactionId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string ubiTransactionId
		{
			get
			{
				return _ubiTransactionId;
			}
			set
			{
				_ubiTransactionId = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
