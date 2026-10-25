namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	public interface ISkillEffectInstanceResult : IEffectInstaceResult
	{
		bool AllowsSkipCooldown { get; }
	}
}
