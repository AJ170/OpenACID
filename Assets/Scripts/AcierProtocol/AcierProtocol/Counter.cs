using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "Counter")]
	public class Counter : IExtensible
	{
		private long _num;

		private long _ECounterTypeOrdinal;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "num", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long num
		{
			get
			{
				return _num;
			}
			set
			{
				_num = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "ECounterTypeOrdinal", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long ECounterTypeOrdinal
		{
			get
			{
				return _ECounterTypeOrdinal;
			}
			set
			{
				_ECounterTypeOrdinal = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
