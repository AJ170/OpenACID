using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EObjectiveProgress")]
	public enum EObjectiveProgress
	{
		[ProtoEnum(Name = "NotStarted_EObjectiveProgress", Value = 0)]
		NotStarted_EObjectiveProgress = 0,
		[ProtoEnum(Name = "Succeeded_EObjectiveProgress", Value = 1)]
		Succeeded_EObjectiveProgress = 1,
		[ProtoEnum(Name = "Failed_EObjectiveProgress", Value = 2)]
		Failed_EObjectiveProgress = 2,
		[ProtoEnum(Name = "InRange_EObjectiveProgress", Value = 3)]
		InRange_EObjectiveProgress = 3,
		[ProtoEnum(Name = "InProgress_EObjectiveProgress", Value = 4)]
		InProgress_EObjectiveProgress = 4
	}
}
