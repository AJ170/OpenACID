using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EAlertLevel")]
	public enum EAlertLevel
	{
		[ProtoEnum(Name = "Anonymous", Value = 0)]
		Anonymous = 0,
		[ProtoEnum(Name = "Alerted", Value = 1)]
		Alerted = 1,
		[ProtoEnum(Name = "Exposed", Value = 2)]
		Exposed = 2
	}
}
