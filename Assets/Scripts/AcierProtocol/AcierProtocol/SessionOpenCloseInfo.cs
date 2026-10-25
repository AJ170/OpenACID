using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "SessionOpenCloseInfo")]
	public class SessionOpenCloseInfo : IExtensible
	{
		private string _type = "";

		private long _openTime;

		private long _closeTime;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "type", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string type
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

		[ProtoMember(3, IsRequired = false, Name = "openTime", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long openTime
		{
			get
			{
				return _openTime;
			}
			set
			{
				_openTime = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "closeTime", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long closeTime
		{
			get
			{
				return _closeTime;
			}
			set
			{
				_closeTime = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
