using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "ERouteIterationStart")]
	public enum ERouteIterationStart
	{
		[ProtoEnum(Name = "Reset_ERouteIterationStart", Value = 1)]
		Reset_ERouteIterationStart = 1,
		[ProtoEnum(Name = "Up_ERouteIterationStart", Value = 2)]
		Up_ERouteIterationStart = 2,
		[ProtoEnum(Name = "Nearest_ERouteIterationStart", Value = 3)]
		Nearest_ERouteIterationStart = 3,
		[ProtoEnum(Name = "NearestUp_ERouteIterationStart", Value = 4)]
		NearestUp_ERouteIterationStart = 4
	}
}
