using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class AwareInteractionCharacterStats : InteractionCharacterStats, ICharacterDetectionStats, IReadonlyStatValueCollection, IStatValueCollection
	{
		public new static readonly IStatsHelper[] StatsHelpers = new BBList<IStatsHelper>(InteractionCharacterStats.StatsHelpers).Concat(new IStatsHelper[1] { StatsHelper<CharacterDetectionStatsHelper>.Instance }).ToArray();

		public override float FrameUpdateIntervalSeconds
		{
			get
			{
				return 0.4f;
			}
		}

		public override int TurnUpdateIntervalTurns
		{
			get
			{
				return 5;
			}
		}

		public AwareInteractionCharacterStats(IEntity _entity)
			: base(_entity, StatsHelpers)
		{
		}
	}
}
