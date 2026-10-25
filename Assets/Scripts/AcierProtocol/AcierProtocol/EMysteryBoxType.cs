using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EMysteryBoxType")]
	public enum EMysteryBoxType
	{
		[ProtoEnum(Name = "MysteryBoxItemType", Value = 1)]
		MysteryBoxItemType = 1,
		[ProtoEnum(Name = "MysteryBoxRessourceType", Value = 2)]
		MysteryBoxRessourceType = 2,
		[ProtoEnum(Name = "MysteryBoxMiscType", Value = 3)]
		MysteryBoxMiscType = 3,
		[ProtoEnum(Name = "MysteryBoxItemType_Tutorial", Value = 4)]
		MysteryBoxItemType_Tutorial = 4,
		[ProtoEnum(Name = "MysteryBoxBootyChestType", Value = 5)]
		MysteryBoxBootyChestType = 5,
		[ProtoEnum(Name = "MysteryBoxMovieType", Value = 6)]
		MysteryBoxMovieType = 6
	}
}
