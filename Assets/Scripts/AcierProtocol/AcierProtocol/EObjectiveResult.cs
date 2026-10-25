using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EObjectiveResult")]
	public enum EObjectiveResult
	{
		[ProtoEnum(Name = "Undefined_EObjectiveResult", Value = 0)]
		Undefined_EObjectiveResult = 0,
		[ProtoEnum(Name = "Succeeded_EObjectiveResult", Value = 1)]
		Succeeded_EObjectiveResult = 1,
		[ProtoEnum(Name = "Failed_EObjectiveResult", Value = 2)]
		Failed_EObjectiveResult = 2
	}
}
