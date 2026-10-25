using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EWindowImageType")]
	public enum EWindowImageType
	{
		[ProtoEnum(Name = "TutorialWelcome", Value = 0)]
		TutorialWelcome = 0,
		[ProtoEnum(Name = "TutorialInteraction", Value = 1)]
		TutorialInteraction = 1,
		[ProtoEnum(Name = "TutorialMinimap", Value = 2)]
		TutorialMinimap = 2,
		[ProtoEnum(Name = "TutorialCombat", Value = 3)]
		TutorialCombat = 3,
		[ProtoEnum(Name = "TutorialHaystacks", Value = 4)]
		TutorialHaystacks = 4,
		[ProtoEnum(Name = "TutorialDetection", Value = 5)]
		TutorialDetection = 5,
		[ProtoEnum(Name = "TutorialWelcome2", Value = 6)]
		TutorialWelcome2 = 6,
		[ProtoEnum(Name = "TutorialCombatNew", Value = 7)]
		TutorialCombatNew = 7,
		[ProtoEnum(Name = "TutorialCombatNew2", Value = 8)]
		TutorialCombatNew2 = 8,
		[ProtoEnum(Name = "TutorialHaystacks1", Value = 9)]
		TutorialHaystacks1 = 9,
		[ProtoEnum(Name = "Invalid_EWindowImageType", Value = 20)]
		Invalid_EWindowImageType = 20
	}
}
