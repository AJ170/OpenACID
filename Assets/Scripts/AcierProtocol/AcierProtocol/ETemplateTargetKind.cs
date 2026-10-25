using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "ETemplateTargetKind")]
	public enum ETemplateTargetKind
	{
		[ProtoEnum(Name = "Escape_ETemplateTargetKind", Value = 1)]
		Escape_ETemplateTargetKind = 1,
		[ProtoEnum(Name = "Mission_ETemplateTargetKind", Value = 2)]
		Mission_ETemplateTargetKind = 2,
		[ProtoEnum(Name = "Parkour_ETemplateTargetKind", Value = 4)]
		Parkour_ETemplateTargetKind = 4,
		[ProtoEnum(Name = "Stay_ETemplateTargetKind", Value = 8)]
		Stay_ETemplateTargetKind = 8,
		[ProtoEnum(Name = "LastPosition_ETemplateTargetKind", Value = 16)]
		LastPosition_ETemplateTargetKind = 0x10
	}
}
