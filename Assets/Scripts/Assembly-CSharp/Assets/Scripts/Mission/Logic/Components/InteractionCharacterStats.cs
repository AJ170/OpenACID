using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class InteractionCharacterStats : NpcStats, IInteractionStats, IReadonlyStatValueCollection, IStatValueCollection
	{
		public new static readonly IStatsHelper[] StatsHelpers = new BBList<IStatsHelper>(NpcStats.StatsHelpers).Concat(new IStatsHelper[2]
		{
			StatsHelper<InteractionStatsHelper>.Instance,
			StatsHelper<MissionCharacterStatsHelper>.Instance
		}).ToArray();

		public InteractionCharacterStats(IEntity _entity)
			: this(_entity, StatsHelpers)
		{
		}

		protected InteractionCharacterStats(IEntity _entity, IStatsHelper[] _statHelpers)
			: base(_entity, _statHelpers)
		{
		}
	}
}
