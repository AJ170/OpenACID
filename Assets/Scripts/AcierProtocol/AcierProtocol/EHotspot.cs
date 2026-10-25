using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EHotspot")]
	public enum EHotspot
	{
		[ProtoEnum(Name = "EHotspot_Haystack", Value = 0)]
		EHotspot_Haystack = 0,
		[ProtoEnum(Name = "EHotspot_Bench", Value = 1)]
		EHotspot_Bench = 1,
		[ProtoEnum(Name = "EHotspot_RooftopGarden", Value = 2)]
		EHotspot_RooftopGarden = 2,
		[ProtoEnum(Name = "EHotspot_CivilianCover", Value = 3)]
		EHotspot_CivilianCover = 3,
		[ProtoEnum(Name = "EHotspot_Doctor", Value = 4)]
		EHotspot_Doctor = 4,
		[ProtoEnum(Name = "EHotspot_Captives", Value = 5)]
		EHotspot_Captives = 5,
		[ProtoEnum(Name = "EHotspot_Chests", Value = 6)]
		EHotspot_Chests = 6,
		[ProtoEnum(Name = "EHotspot_PigeonCoup", Value = 7)]
		EHotspot_PigeonCoup = 7,
		[ProtoEnum(Name = "EHotspot_Courtesans", Value = 8)]
		EHotspot_Courtesans = 8,
		[ProtoEnum(Name = "EHotspot_Thieves", Value = 9)]
		EHotspot_Thieves = 9,
		[ProtoEnum(Name = "EHotspot_StalkingZone", Value = 10)]
		EHotspot_StalkingZone = 10
	}
}
