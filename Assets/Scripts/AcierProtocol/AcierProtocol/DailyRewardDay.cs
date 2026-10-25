using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "DailyRewardDay")]
	public class DailyRewardDay : IExtensible
	{
		private int _day;

		private ShopBundle _shopBundle;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "day", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int day
		{
			get
			{
				return _day;
			}
			set
			{
				_day = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "shopBundle", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ShopBundle shopBundle
		{
			get
			{
				return _shopBundle;
			}
			set
			{
				_shopBundle = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
