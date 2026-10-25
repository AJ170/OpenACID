using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class StaticObjectStats : EntityStats
	{
		public new static readonly IStatsHelper[] StatsHelpers = new BBList<IStatsHelper>(EntityStats.StatsHelpers).Concat(new IStatsHelper[1] { StatsHelper<StaticObjectStatsHelper>.Instance }).ToArray();

		public StaticObjectStats(IEntity _entity)
			: base(_entity, StatsHelpers)
		{
		}
	}
}
