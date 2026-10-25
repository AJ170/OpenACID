using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EDurationType")]
	public enum EDurationType
	{
		[ProtoEnum(Name = "Time", Value = 1)]
		Time = 1,
		[ProtoEnum(Name = "MissionCount", Value = 2)]
		MissionCount = 2
	}
}
