using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EEra")]
	public enum EEra
	{
		[ProtoEnum(Name = "EEra_None", Value = 0)]
		EEra_None = 0,
		[ProtoEnum(Name = "Italy", Value = 1)]
		Italy = 1,
		[ProtoEnum(Name = "Abstergo", Value = 2)]
		Abstergo = 2,
		[ProtoEnum(Name = "Caribbean", Value = 4)]
		Caribbean = 4,
		[ProtoEnum(Name = "ACM", Value = 8)]
		ACM = 8,
		[ProtoEnum(Name = "Common", Value = 16)]
		Common = 0x10,
		[ProtoEnum(Name = "America", Value = 32)]
		America = 0x20
	}
}
