using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class ProjectileStats : EntityStats
	{
		public new static readonly IStatsHelper[] StatsHelpers = new BBList<IStatsHelper>(EntityStats.StatsHelpers).Concat(new IStatsHelper[1] { StatsHelper<ProjectileStatsHelper>.Instance }).ToArray();

		public ProjectileStats(IEntity _entity)
			: base(_entity, StatsHelpers)
		{
		}
	}
}
