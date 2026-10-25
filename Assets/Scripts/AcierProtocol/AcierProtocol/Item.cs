using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "Item")]
	public class Item : IExtensible
	{
		[ProtoContract(Name = "EItemCategory")]
		public enum EItemCategory
		{
			[ProtoEnum(Name = "None", Value = 1)]
			None = 1,
			[ProtoEnum(Name = "Sword", Value = 2)]
			Sword = 2,
			[ProtoEnum(Name = "Blunt", Value = 3)]
			Blunt = 3,
			[ProtoEnum(Name = "Dagger", Value = 4)]
			Dagger = 4,
			[ProtoEnum(Name = "Polearm", Value = 5)]
			Polearm = 5,
			[ProtoEnum(Name = "TwoHanded", Value = 6)]
			TwoHanded = 6,
			[ProtoEnum(Name = "DualWield", Value = 7)]
			DualWield = 7,
			[ProtoEnum(Name = "Boots", Value = 8)]
			Boots = 8,
			[ProtoEnum(Name = "Legs", Value = 9)]
			Legs = 9,
			[ProtoEnum(Name = "Chest", Value = 10)]
			Chest = 10,
			[ProtoEnum(Name = "Arms", Value = 11)]
			Arms = 11,
			[ProtoEnum(Name = "Shoulder", Value = 12)]
			Shoulder = 12,
			[ProtoEnum(Name = "Insignia", Value = 13)]
			Insignia = 13,
			[ProtoEnum(Name = "Outfit", Value = 14)]
			Outfit = 14
		}

		private IExtension extensionObject;

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
