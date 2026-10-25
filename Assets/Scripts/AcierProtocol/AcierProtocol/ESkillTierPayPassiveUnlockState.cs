using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "ESkillTierPayPassiveUnlockState")]
	public enum ESkillTierPayPassiveUnlockState
	{
		[ProtoEnum(Name = "PayUnlockedNothing", Value = 1)]
		PayUnlockedNothing = 1,
		[ProtoEnum(Name = "PayUnlockedPassiveOne", Value = 2)]
		PayUnlockedPassiveOne = 2,
		[ProtoEnum(Name = "PayUnlockedPassiveTwo", Value = 3)]
		PayUnlockedPassiveTwo = 3
	}
}
