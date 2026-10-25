using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "Resource")]
	public class Resource : IExtensible
	{
		private long _amount;

		private EResourceType _type = EResourceType.HardCurrency;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "amount", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long amount
		{
			get
			{
				return _amount;
			}
			set
			{
				_amount = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EResourceType.HardCurrency)]
		public EResourceType type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
