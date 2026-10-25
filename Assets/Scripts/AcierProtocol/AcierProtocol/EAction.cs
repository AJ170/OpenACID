using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EAction")]
	public enum EAction
	{
		[ProtoEnum(Name = "Hide", Value = 0)]
		Hide = 0,
		[ProtoEnum(Name = "Block", Value = 1)]
		Block = 1,
		[ProtoEnum(Name = "Highlight", Value = 2)]
		Highlight = 2,
		[ProtoEnum(Name = "NoAction", Value = 3)]
		NoAction = 3
	}
}
