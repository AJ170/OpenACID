using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "OpenMysteryBoxResponse")]
	public class OpenMysteryBoxResponse : IExtensible
	{
		private MysteryBoxChain _mysteryBoxChain;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "mysteryBoxChain", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MysteryBoxChain mysteryBoxChain
		{
			get
			{
				return _mysteryBoxChain;
			}
			set
			{
				_mysteryBoxChain = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
