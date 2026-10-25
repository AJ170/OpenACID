using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "TwitterHashTagEntity")]
	public class TwitterHashTagEntity : IExtensible
	{
		private string _text = "";

		private int _start;

		private int _end;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "text", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "start", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int start
		{
			get
			{
				return _start;
			}
			set
			{
				_start = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "end", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int end
		{
			get
			{
				return _end;
			}
			set
			{
				_end = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
