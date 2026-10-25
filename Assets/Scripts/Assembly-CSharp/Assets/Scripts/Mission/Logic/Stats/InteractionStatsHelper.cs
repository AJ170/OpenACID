namespace Assets.Scripts.Mission.Logic.Stats
{
	public class InteractionStatsHelper : StatsHelper<InteractionStatsHelper>
	{
		public InteractionStatsHelper()
			: base(new StatTypes.StatsRange(), new StatTypes.StatsRange(StatTypes.EStatType.__Start_Mutable_Interaction, StatTypes.EStatType.__End_Mutable_Interaction), new StatTypes.StatsRange())
		{
		}
	}
}
