using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EMissionAreaType")]
	public enum EMissionAreaType
	{
		[ProtoEnum(Name = "Restricted", Value = 0)]
		Restricted = 0,
		[ProtoEnum(Name = "Guiding", Value = 1)]
		Guiding = 1,
		[ProtoEnum(Name = "Other", Value = 2)]
		Other = 2,
		[ProtoEnum(Name = "Template_EMissionAreaType", Value = 3)]
		Template_EMissionAreaType = 3,
		[ProtoEnum(Name = "Escape_EMissionAreaType", Value = 4)]
		Escape_EMissionAreaType = 4,
		[ProtoEnum(Name = "StalkingZone", Value = 5)]
		StalkingZone = 5,
		[ProtoEnum(Name = "Escort_EMissionAreaType", Value = 6)]
		Escort_EMissionAreaType = 6,
		[ProtoEnum(Name = "BlockCrowd", Value = 7)]
		BlockCrowd = 7
	}
}
