using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "Quality")]
	public class Quality : IExtensible
	{
		[ProtoContract(Name = "EQuality")]
		public enum EQuality
		{
			[ProtoEnum(Name = "Mundane", Value = 1)]
			Mundane = 1,
			[ProtoEnum(Name = "Common", Value = 2)]
			Common = 2,
			[ProtoEnum(Name = "Uncommon", Value = 3)]
			Uncommon = 3,
			[ProtoEnum(Name = "Rare", Value = 4)]
			Rare = 4,
			[ProtoEnum(Name = "Perfect", Value = 5)]
			Perfect = 5
		}

		private IExtension extensionObject;

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
