using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class CrowdCharacterStats : NpcStats, ICharacterConditionStats, IReadonlyStatValueCollection, IStatValueCollection
	{
		public new static readonly IStatsHelper[] StatsHelpers = new BBList<IStatsHelper>(NpcStats.StatsHelpers).Concat(new IStatsHelper[1] { StatsHelper<CharacterConditionStatsHelper>.Instance }).ToArray();

		public CrowdCharacterStats(IEntity _entity)
			: base(_entity, StatsHelpers)
		{
		}

		protected CrowdCharacterStats(IEntity _entity, IStatsHelper[] _statHelpers)
			: base(_entity, _statHelpers)
		{
		}
	}
}
