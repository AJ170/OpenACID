using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "VerifyReceiptResponse")]
	public class VerifyReceiptResponse : IExtensible
	{
		private string _transactionId = "";

		private string _ubiTransactionId = "";

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "transactionId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string transactionId
		{
			get
			{
				return _transactionId;
			}
			set
			{
				_transactionId = value;
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
