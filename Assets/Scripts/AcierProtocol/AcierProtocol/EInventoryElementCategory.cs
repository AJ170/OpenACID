using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EInventoryElementCategory")]
	public enum EInventoryElementCategory
	{
		[ProtoEnum(Name = "InventoryItem", Value = 1)]
		InventoryItem = 1,
		[ProtoEnum(Name = "InventoryModule", Value = 2)]
		InventoryModule = 2,
		[ProtoEnum(Name = "InventoryBlueprint", Value = 3)]
		InventoryBlueprint = 3,
		[ProtoEnum(Name = "InventoryMysteryBox", Value = 4)]
		InventoryMysteryBox = 4,
		[ProtoEnum(Name = "InventoryCustom", Value = 5)]
		InventoryCustom = 5
	}
}
