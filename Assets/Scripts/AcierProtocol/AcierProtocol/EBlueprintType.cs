using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EBlueprintType")]
	public enum EBlueprintType
	{
		[ProtoEnum(Name = "BlueprintWeapon", Value = 1)]
		BlueprintWeapon = 1,
		[ProtoEnum(Name = "BlueprintBoots", Value = 2)]
		BlueprintBoots = 2,
		[ProtoEnum(Name = "BlueprintLegs", Value = 3)]
		BlueprintLegs = 3,
		[ProtoEnum(Name = "BlueprintChest", Value = 4)]
		BlueprintChest = 4,
		[ProtoEnum(Name = "BlueprintArms", Value = 5)]
		BlueprintArms = 5,
		[ProtoEnum(Name = "BlueprintShoulder", Value = 6)]
		BlueprintShoulder = 6,
		[ProtoEnum(Name = "BlueprintInsignia", Value = 7)]
		BlueprintInsignia = 7
	}
}
