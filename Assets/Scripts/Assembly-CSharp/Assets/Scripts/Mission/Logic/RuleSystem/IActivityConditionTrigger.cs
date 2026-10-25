namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	public interface IActivityConditionTrigger : ITrigger
	{
		bool ActivityCondition { get; }
	}
}
