using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "ETutorialTriggerType")]
	public enum ETutorialTriggerType
	{
		[ProtoEnum(Name = "EnterTargetArea", Value = 0)]
		EnterTargetArea = 0,
		[ProtoEnum(Name = "LeaveTargetArea", Value = 1)]
		LeaveTargetArea = 1,
		[ProtoEnum(Name = "Timer", Value = 2)]
		Timer = 2,
		[ProtoEnum(Name = "InCombat", Value = 3)]
		InCombat = 3,
		[ProtoEnum(Name = "AlertlevelAnonymous", Value = 4)]
		AlertlevelAnonymous = 4,
		[ProtoEnum(Name = "WindowImageClosed", Value = 5)]
		WindowImageClosed = 5
	}
}
