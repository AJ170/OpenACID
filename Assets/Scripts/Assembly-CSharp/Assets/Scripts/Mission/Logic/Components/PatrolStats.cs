using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class PatrolStats : EntityStats
	{
		public new static readonly IStatsHelper[] StatsHelpers = new BBList<IStatsHelper>(EntityStats.StatsHelpers).Concat(new IStatsHelper[2]
		{
			StatsHelper<PatrolStatsHelper>.Instance,
			StatsHelper<InteractionStatsHelper>.Instance
		}).ToArray();

		public PatrolStats(IEntity _entity)
			: base(_entity, StatsHelpers)
		{
		}
	}
}
