namespace Assets.Scripts.Mission.Logic.Stats
{
	public class StaticObjectStatsHelper : StatsHelper<StaticObjectStatsHelper>
	{
		public StaticObjectStatsHelper()
			: base(new StatTypes.StatsRange(), new StatTypes.StatsRange(StatTypes.EStatType.__Start_Mutable_StaticObject, StatTypes.EStatType.__End_Mutable_StaticObject), new StatTypes.StatsRange())
		{
		}
	}
}
