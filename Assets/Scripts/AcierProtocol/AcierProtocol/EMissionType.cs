using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EMissionType")]
	public enum EMissionType
	{
		[ProtoEnum(Name = "Animus_EMissionType", Value = 0)]
		Animus_EMissionType = 0,
		[ProtoEnum(Name = "Mission_EMissionType", Value = 1)]
		Mission_EMissionType = 1,
		[ProtoEnum(Name = "Template_EMissionType", Value = 2)]
		Template_EMissionType = 2,
		[ProtoEnum(Name = "None_EMissionType", Value = 3)]
		None_EMissionType = 3
	}
}
