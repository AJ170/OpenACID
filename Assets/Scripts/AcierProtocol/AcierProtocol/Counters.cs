using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "Counters")]
	public class Counters : IExtensible
	{
		private Counter[] _counters = new Counter[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "counters", DataFormat = DataFormat.Default)]
		public Counter[] counters
		{
			get
			{
				return _counters;
			}
			set
			{
				Array.Resize(ref _counters, value.Length);
				value.CopyTo(_counters, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
