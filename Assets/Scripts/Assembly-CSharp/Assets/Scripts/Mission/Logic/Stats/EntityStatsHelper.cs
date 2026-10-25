namespace Assets.Scripts.Mission.Logic.Stats
{
	public class EntityStatsHelper : StatsHelper<EntityStatsHelper>
	{
		public EntityStatsHelper()
			: base(new StatTypes.StatsRange(), new StatTypes.StatsRange(StatTypes.EStatType.__Start_Mutable_Entity, StatTypes.EStatType.__End_Mutable_Entity), new StatTypes.StatsRange())
		{
		}
	}
}
