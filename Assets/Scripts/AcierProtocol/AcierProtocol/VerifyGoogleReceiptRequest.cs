using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "VerifyGoogleReceiptRequest")]
	public class VerifyGoogleReceiptRequest : IExtensible
	{
		private string _inAppPurchaseData = "";

		private string _inAppPurchaseSignature = "";

		private string _currencyCode = "";

		private string _costDecimal = "";

		private string _formattedPrice = "";

		private string _storeCountry = "";

		private string _deviceId = "";

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "inAppPurchaseData", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string inAppPurchaseData
		{
			get
			{
				return _inAppPurchaseData;
			}
			set
			{
				_inAppPurchaseData = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "inAppPurchaseSignature", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string inAppPurchaseSignature
		{
			get
			{
				return _inAppPurchaseSignature;
			}
			set
			{
				_inAppPurchaseSignature = value;
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
