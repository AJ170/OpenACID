using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	public interface ISkillPayloadCarrier
	{
		ISkillPayload Payload { get; set; }
	}
}
