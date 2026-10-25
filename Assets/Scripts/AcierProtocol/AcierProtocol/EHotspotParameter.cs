using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EHotspotParameter")]
	public enum EHotspotParameter
	{
		[ProtoEnum(Name = "WorldHotspotCountHaystack", Value = 9)]
		WorldHotspotCountHaystack = 9,
		[ProtoEnum(Name = "WorldHotspotCountBench", Value = 10)]
		WorldHotspotCountBench = 10,
		[ProtoEnum(Name = "WorldHotspotCountRooftopGarden", Value = 11)]
		WorldHotspotCountRooftopGarden = 11,
		[ProtoEnum(Name = "WorldHotspotCountThieves", Value = 12)]
		WorldHotspotCountThieves = 12,
		[ProtoEnum(Name = "WorldHotspotCountCourtesans", Value = 13)]
		WorldHotspotCountCourtesans = 13,
		[ProtoEnum(Name = "WorldHotspotCountCivilianCover", Value = 14)]
		WorldHotspotCountCivilianCover = 14
	}
}
