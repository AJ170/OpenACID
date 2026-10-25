using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class GameStats : EntityStats
	{
		public new static readonly IStatsHelper[] StatsHelpers = new BBList<IStatsHelper>(EntityStats.StatsHelpers).Concat(new IStatsHelper[1] { StatsHelper<GameStatsHelper>.Instance }).ToArray();

		public GameStats(IEntity _entity)
			: base(_entity, StatsHelpers)
		{
		}
	}
}
