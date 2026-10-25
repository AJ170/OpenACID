using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EMissionGoal")]
	public enum EMissionGoal
	{
		[ProtoEnum(Name = "Objective_EMissionGoal", Value = 1)]
		Objective_EMissionGoal = 1,
		[ProtoEnum(Name = "Modifier_EMissionGoal", Value = 2)]
		Modifier_EMissionGoal = 2,
		[ProtoEnum(Name = "Challenge_EMissionGoal", Value = 3)]
		Challenge_EMissionGoal = 3
	}
}
