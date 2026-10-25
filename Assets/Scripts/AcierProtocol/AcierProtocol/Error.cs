using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "Error")]
	public class Error : IExtensible
	{
		private ErrorCategory _category;

		private ErrorCode _code;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = true, Name = "category", DataFormat = DataFormat.TwosComplement)]
		public ErrorCategory category
		{
			get
			{
				return _category;
			}
			set
			{
				_category = value;
			}
		}

		[ProtoMember(2, IsRequired = true, Name = "code", DataFormat = DataFormat.TwosComplement)]
		public ErrorCode code
		{
			get
			{
				return _code;
			}
			set
			{
				_code = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
