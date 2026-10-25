namespace Assets.Scripts.Mission.Logic.Stats
{
	public class MapGeneratorStatsHelper : StatsHelper<MapGeneratorStatsHelper>
	{
		public MapGeneratorStatsHelper()
			: base(new StatTypes.StatsRange(StatTypes.EStatType.__Start_Immutable_MapGenerator, StatTypes.EStatType.__End_Immutable_MapGenerator), new StatTypes.StatsRange(), new StatTypes.StatsRange())
		{
		}
	}
}
