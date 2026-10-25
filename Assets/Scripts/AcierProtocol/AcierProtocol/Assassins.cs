using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "Assassins")]
	public class Assassins : IExtensible
	{
		private Assassin[] _assassins = new Assassin[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "assassins", DataFormat = DataFormat.Default)]
		public Assassin[] assassins
		{
			get
			{
				return _assassins;
			}
			set
			{
				Array.Resize(ref _assassins, value.Length);
				value.CopyTo(_assassins, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
