using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "LangKey")]
	public class LangKey : IExtensible
	{
		private int _neutral;

		private int _female;

		private int _male;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = true, Name = "neutral", DataFormat = DataFormat.TwosComplement)]
		public int neutral
		{
			get
			{
				return _neutral;
			}
			set
			{
				_neutral = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "female", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int female
		{
			get
			{
				return _female;
			}
			set
			{
				_female = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "male", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int male
		{
			get
			{
				return _male;
			}
			set
			{
				_male = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
