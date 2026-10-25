using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EInventorySlot")]
	public enum EInventorySlot
	{
		[ProtoEnum(Name = "SlotWeapon", Value = 1)]
		SlotWeapon = 1,
		[ProtoEnum(Name = "SlotBoots", Value = 2)]
		SlotBoots = 2,
		[ProtoEnum(Name = "SlotLegs", Value = 3)]
		SlotLegs = 3,
		[ProtoEnum(Name = "SlotChest", Value = 4)]
		SlotChest = 4,
		[ProtoEnum(Name = "SlotArms", Value = 5)]
		SlotArms = 5,
		[ProtoEnum(Name = "SlotShoulder", Value = 6)]
		SlotShoulder = 6,
		[ProtoEnum(Name = "SlotInsignia", Value = 7)]
		SlotInsignia = 7,
		[ProtoEnum(Name = "SlotOutfit", Value = 8)]
		SlotOutfit = 8,
		[ProtoEnum(Name = "SlotModule", Value = 9)]
		SlotModule = 9
	}
}
