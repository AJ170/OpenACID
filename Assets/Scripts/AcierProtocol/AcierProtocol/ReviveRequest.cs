using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ReviveRequest")]
	public class ReviveRequest : IExtensible
	{
		private string _missionToken = "";

		private EResourceType _paymentResource = EResourceType.HardCurrency;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "missionToken", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string missionToken
		{
			get
			{
				return _missionToken;
			}
			set
			{
				_missionToken = value;
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
