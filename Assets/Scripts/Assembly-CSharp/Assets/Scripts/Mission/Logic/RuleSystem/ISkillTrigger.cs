namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	public interface ISkillTrigger : ISkillPayloadCarrier, ITargetCarrier, ITrigger, IActivityConditionTrigger, ITargetTrigger
	{
		bool DelayedCheck { get; }
	}
}
