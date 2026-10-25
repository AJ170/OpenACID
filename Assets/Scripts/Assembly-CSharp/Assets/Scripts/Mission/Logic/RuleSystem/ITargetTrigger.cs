namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	public interface ITargetTrigger : ITargetCarrier, ITrigger
	{
		ETargetSource TargetSource { get; }
	}
}
