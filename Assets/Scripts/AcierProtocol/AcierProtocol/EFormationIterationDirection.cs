using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EFormationIterationDirection")]
	public enum EFormationIterationDirection
	{
		[ProtoEnum(Name = "Down", Value = 0)]
		Down = 0,
		[ProtoEnum(Name = "Up", Value = 1)]
		Up = 1
	}
}
