using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "RerollQuestRewardRequest")]
	public class RerollQuestRewardRequest : IExtensible
	{
		private int _rankIdx;

		private EResourceType _paymentResource = EResourceType.HardCurrency;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "rankIdx", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int rankIdx
		{
			get
			{
				return _rankIdx;
			}
			set
			{
				_rankIdx = value;
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
