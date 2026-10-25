using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "GetMailHeadersRequest")]
	public class GetMailHeadersRequest : IExtensible
	{
		private long _lastTimestamp;

		private int _maxHeaders;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "lastTimestamp", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long lastTimestamp
		{
			get
			{
				return _lastTimestamp;
			}
			set
			{
				_lastTimestamp = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "maxHeaders", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int maxHeaders
		{
			get
			{
				return _maxHeaders;
			}
			set
			{
				_maxHeaders = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
