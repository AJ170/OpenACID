using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;

namespace Assets.Scripts.Mission.Logic.Components
{
	public abstract class CharacterStats : EntityStats
	{
		public new static readonly IStatsHelper[] StatsHelpers = new BBList<IStatsHelper>(EntityStats.StatsHelpers).Concat(new IStatsHelper[1] { StatsHelper<CharacterStatsHelper>.Instance }).ToArray();

		protected CharacterStats(IEntity _entity, IStatsHelper[] _statHelpers)
			: base(_entity, _statHelpers)
		{
		}
	}
}
