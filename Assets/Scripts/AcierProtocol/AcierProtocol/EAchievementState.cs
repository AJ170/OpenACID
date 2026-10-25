using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EAchievementState")]
	public enum EAchievementState
	{
		[ProtoEnum(Name = "AchievementNotReached", Value = 0)]
		AchievementNotReached = 0,
		[ProtoEnum(Name = "AchievementClaimable", Value = 1)]
		AchievementClaimable = 1,
		[ProtoEnum(Name = "AchievementClaimed", Value = 2)]
		AchievementClaimed = 2
	}
}
