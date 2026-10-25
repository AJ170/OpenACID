using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "VerifyReceiptRequest")]
	public class VerifyReceiptRequest : IExtensible
	{
		private string _receiptData = "";

		private string _transactionId = "";

		private string _currencyCode = "";

		private string _costDecimal = "";

		private string _formattedPrice = "";

		private string _storeCountry = "";

		private string _deviceId = "";

		private string _originalTransactionId = "";

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "receiptData", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string receiptData
		{
			get
			{
				return _receiptData;
			}
			set
			{
				_receiptData = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "transactionId", DataFormat = DataFormat.Default)]
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

		[ProtoMember(3, IsRequired = false, Name = "currencyCode", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string currencyCode
		{
			get
			{
				return _currencyCode;
			}
			set
			{
				_currencyCode = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "costDecimal", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string costDecimal
		{
			get
			{
				return _costDecimal;
			}
			set
			{
				_costDecimal = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "formattedPrice", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string formattedPrice
		{
			get
			{
				return _formattedPrice;
			}
			set
			{
				_formattedPrice = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "storeCountry", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string storeCountry
		{
			get
			{
				return _storeCountry;
			}
			set
			{
				_storeCountry = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "deviceId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string deviceId
		{
			get
			{
				return _deviceId;
			}
			set
			{
				_deviceId = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "originalTransactionId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string originalTransactionId
		{
			get
			{
				return _originalTransactionId;
			}
			set
			{
				_originalTransactionId = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
