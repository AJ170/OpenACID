using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "RandomNumberBlob")]
	public class RandomNumberBlob : IExtensible
	{
		private int[] _randomData = new int[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "randomData", DataFormat = DataFormat.TwosComplement)]
		public int[] randomData
		{
			get
			{
				return _randomData;
			}
			set
			{
				Array.Resize(ref _randomData, value.Length);
				value.CopyTo(_randomData, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
