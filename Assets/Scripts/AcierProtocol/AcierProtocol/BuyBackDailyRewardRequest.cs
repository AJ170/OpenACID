using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "BuyBackDailyRewardRequest")]
	public class BuyBackDailyRewardRequest : IExtensible
	{
		private bool _buy;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "buy", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool buy
		{
			get
			{
				return _buy;
			}
			set
			{
				_buy = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
