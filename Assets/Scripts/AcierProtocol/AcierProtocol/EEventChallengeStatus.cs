using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EEventChallengeStatus")]
	public enum EEventChallengeStatus
	{
		[ProtoEnum(Name = "Challenge_InProgress", Value = 1)]
		Challenge_InProgress = 1,
		[ProtoEnum(Name = "Challenge_Finished", Value = 2)]
		Challenge_Finished = 2,
		[ProtoEnum(Name = "Challenge_Claimed", Value = 3)]
		Challenge_Claimed = 3
	}
}
