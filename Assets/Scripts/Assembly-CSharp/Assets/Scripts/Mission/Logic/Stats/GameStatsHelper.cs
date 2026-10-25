namespace Assets.Scripts.Mission.Logic.Stats
{
	public class GameStatsHelper : StatsHelper<GameStatsHelper>
	{
		public GameStatsHelper()
			: base(new StatTypes.StatsRange(StatTypes.EStatType.__Start_Immutable_Game, StatTypes.EStatType.__End_Immutable_Game), new StatTypes.StatsRange(StatTypes.EStatType.__Start_Mutable_Game, StatTypes.EStatType.__End_Mutable_Game), new StatTypes.StatsRange(StatTypes.EStatType.__Start_Computed_Game, StatTypes.EStatType.__End_Computed_Game))
		{
		}
	}
}
