using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EFormationIterationType")]
	public enum EFormationIterationType
	{
		[ProtoEnum(Name = "BackAndForth", Value = 0)]
		BackAndForth = 0,
		[ProtoEnum(Name = "Circeling", Value = 1)]
		Circeling = 1
	}
}
