namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	public interface IApproachSkillRule : IRule, ISkillRule
	{
		float ApproachMaxDistance { get; }

		float ApproachMinDistance { get; }

		float ApproachMinHeight { get; }

		float ApproachStartDistance { get; }
	}
}
