using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class InteractableObjectStats : EntityStats, IInteractionStats, IReadonlyStatValueCollection, IStatValueCollection
	{
		public new static readonly IStatsHelper[] StatsHelpers = new BBList<IStatsHelper>(EntityStats.StatsHelpers).Concat(new IStatsHelper[2]
		{
			StatsHelper<InteractionStatsHelper>.Instance,
			StatsHelper<MapGeneratorStatsHelper>.Instance
		}).ToArray();

		public InteractableObjectStats(IEntity _entity)
			: base(_entity, StatsHelpers)
		{
		}
	}
}
