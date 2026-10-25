using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EMapIconType")]
	public enum EMapIconType
	{
		[ProtoEnum(Name = "Invalid_EMapIconType", Value = 0)]
		Invalid_EMapIconType = 0,
		[ProtoEnum(Name = "Target_EMapIconType", Value = 1)]
		Target_EMapIconType = 1,
		[ProtoEnum(Name = "Assassinate_EMapIconType", Value = 2)]
		Assassinate_EMapIconType = 2,
		[ProtoEnum(Name = "Codex_EMapIconType", Value = 3)]
		Codex_EMapIconType = 3,
		[ProtoEnum(Name = "Coins_EMapIconType", Value = 4)]
		Coins_EMapIconType = 4,
		[ProtoEnum(Name = "Courtesan_EMapIconType", Value = 5)]
		Courtesan_EMapIconType = 5,
		[ProtoEnum(Name = "Deliver_EMapIconType", Value = 6)]
		Deliver_EMapIconType = 6,
		[ProtoEnum(Name = "Doctor_EMapIconType", Value = 7)]
		Doctor_EMapIconType = 7,
		[ProtoEnum(Name = "Escape_EMapIconType", Value = 8)]
		Escape_EMapIconType = 8,
		[ProtoEnum(Name = "Find_EMapIconType", Value = 9)]
		Find_EMapIconType = 9,
		[ProtoEnum(Name = "Gather_EMapIconType", Value = 11)]
		Gather_EMapIconType = 11,
		[ProtoEnum(Name = "Hide_EMapIconType", Value = 12)]
		Hide_EMapIconType = 12,
		[ProtoEnum(Name = "Loot_EMapIconType", Value = 15)]
		Loot_EMapIconType = 15,
		[ProtoEnum(Name = "Plant_EMapIconType", Value = 16)]
		Plant_EMapIconType = 16,
		[ProtoEnum(Name = "Player_EMapIconType", Value = 17)]
		Player_EMapIconType = 17,
		[ProtoEnum(Name = "Enemy_EMapIconType", Value = 18)]
		Enemy_EMapIconType = 18,
		[ProtoEnum(Name = "MovingCover_EMapIconType", Value = 19)]
		MovingCover_EMapIconType = 19,
		[ProtoEnum(Name = "FreeCaptured_EMapIconType", Value = 20)]
		FreeCaptured_EMapIconType = 20,
		[ProtoEnum(Name = "Follow_EMapIconType", Value = 21)]
		Follow_EMapIconType = 21,
		[ProtoEnum(Name = "Reach_EMapIconType", Value = 22)]
		Reach_EMapIconType = 22,
		[ProtoEnum(Name = "UnknownTarget_EMapIconType", Value = 23)]
		UnknownTarget_EMapIconType = 23,
		[ProtoEnum(Name = "Chest_EMapIconType", Value = 24)]
		Chest_EMapIconType = 24
	}
}
