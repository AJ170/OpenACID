namespace Assets.Scripts.Mission.Logic.Stats
{
	public class ProjectileStatsHelper : StatsHelper<ProjectileStatsHelper>
	{
		public ProjectileStatsHelper()
			: base(new StatTypes.StatsRange(), new StatTypes.StatsRange(StatTypes.EStatType.__Start_Mutable_Projectile, StatTypes.EStatType.__End_Mutable_Projectile), new StatTypes.StatsRange())
		{
		}
	}
}
