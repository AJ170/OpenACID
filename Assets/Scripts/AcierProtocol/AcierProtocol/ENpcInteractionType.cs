using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "ENpcInteractionType")]
	public enum ENpcInteractionType
	{
		[ProtoEnum(Name = "CivilianCover_ENpcInteractionType", Value = 0)]
		CivilianCover_ENpcInteractionType = 0,
		[ProtoEnum(Name = "Doctor_ENpcInteractionType", Value = 1)]
		Doctor_ENpcInteractionType = 1,
		[ProtoEnum(Name = "Captives_ENpcInteractionType", Value = 2)]
		Captives_ENpcInteractionType = 2,
		[ProtoEnum(Name = "Courtesans_ENpcInteractionType", Value = 3)]
		Courtesans_ENpcInteractionType = 3,
		[ProtoEnum(Name = "Thieves_ENpcInteractionType", Value = 4)]
		Thieves_ENpcInteractionType = 4,
		[ProtoEnum(Name = "Deliver_ENpcInteractionType", Value = 50)]
		Deliver_ENpcInteractionType = 50,
		[ProtoEnum(Name = "Kill_ENpcInteractionType", Value = 51)]
		Kill_ENpcInteractionType = 51,
		[ProtoEnum(Name = "KillType_ENpcInteractionType", Value = 52)]
		KillType_ENpcInteractionType = 52,
		[ProtoEnum(Name = "Escort_ENpcInteractionType", Value = 53)]
		Escort_ENpcInteractionType = 53,
		[ProtoEnum(Name = "Tail_ENpcInteractionType", Value = 54)]
		Tail_ENpcInteractionType = 54,
		[ProtoEnum(Name = "SupportUnits_ENpcInteractionType", Value = 5)]
		SupportUnits_ENpcInteractionType = 5
	}
}
