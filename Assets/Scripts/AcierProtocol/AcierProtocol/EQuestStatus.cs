using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EQuestStatus")]
	public enum EQuestStatus
	{
		[ProtoEnum(Name = "Created", Value = 0)]
		Created = 0,
		[ProtoEnum(Name = "InProgress", Value = 1)]
		InProgress = 1,
		[ProtoEnum(Name = "Finished", Value = 2)]
		Finished = 2,
		[ProtoEnum(Name = "Claimed", Value = 3)]
		Claimed = 3
	}
}
