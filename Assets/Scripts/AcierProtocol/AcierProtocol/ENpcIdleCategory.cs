using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "ENpcIdleCategory")]
	public enum ENpcIdleCategory
	{
		[ProtoEnum(Name = "NpcTypeDefault", Value = 0)]
		NpcTypeDefault = 0,
		[ProtoEnum(Name = "Neutral", Value = 1)]
		Neutral = 1,
		[ProtoEnum(Name = "HighProfile", Value = 2)]
		HighProfile = 2,
		[ProtoEnum(Name = "LowProfile", Value = 3)]
		LowProfile = 3,
		[ProtoEnum(Name = "Wait", Value = 4)]
		Wait = 4,
		[ProtoEnum(Name = "Talk", Value = 5)]
		Talk = 5,
		[ProtoEnum(Name = "Argument", Value = 6)]
		Argument = 6,
		[ProtoEnum(Name = "ActArrogant", Value = 7)]
		ActArrogant = 7,
		[ProtoEnum(Name = "Listen", Value = 8)]
		Listen = 8,
		[ProtoEnum(Name = "Sitting", Value = 9)]
		Sitting = 9,
		[ProtoEnum(Name = "SittingTalking", Value = 10)]
		SittingTalking = 10,
		[ProtoEnum(Name = "AdvertisingGoods", Value = 11)]
		AdvertisingGoods = 11,
		[ProtoEnum(Name = "Counting", Value = 12)]
		Counting = 12,
		[ProtoEnum(Name = "CourtesanUnhired", Value = 13)]
		CourtesanUnhired = 13,
		[ProtoEnum(Name = "CapturedNpc", Value = 14)]
		CapturedNpc = 14,
		[ProtoEnum(Name = "Shocked", Value = 15)]
		Shocked = 15,
		[ProtoEnum(Name = "CheerJoy", Value = 16)]
		CheerJoy = 16,
		[ProtoEnum(Name = "Searching", Value = 17)]
		Searching = 17,
		[ProtoEnum(Name = "Count_ENpcIdleCategory", Value = 18)]
		Count_ENpcIdleCategory = 18,
		[ProtoEnum(Name = "Invalid_ENpcIdleCategory", Value = 20)]
		Invalid_ENpcIdleCategory = 20
	}
}
