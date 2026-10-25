namespace Assets.Scripts.Mission.Logic.Stats
{
	public class RulesStatsHelper : StatsHelper<RulesStatsHelper>
	{
		public RulesStatsHelper()
			: base(new StatTypes.StatsRange(StatTypes.EStatType.__Start_Immutable_Rule, StatTypes.EStatType.__End_Immutable_Rule), new StatTypes.StatsRange(), new StatTypes.StatsRange())
		{
		}
	}
}
