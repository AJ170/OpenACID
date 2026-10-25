using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EAchievementConditionCombinationType")]
	public enum EAchievementConditionCombinationType
	{
		[ProtoEnum(Name = "CombinationTypeAll", Value = 0)]
		CombinationTypeAll = 0,
		[ProtoEnum(Name = "CombinationTypeOne", Value = 1)]
		CombinationTypeOne = 1
	}
}
