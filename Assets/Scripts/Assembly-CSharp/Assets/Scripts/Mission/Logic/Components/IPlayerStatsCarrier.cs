using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IPlayerStatsCarrier : ICharacterStatsCarrier, IDefinitionCarrier, IEntityStatsCarrier, IEntity, ISuperBuffsStatsCarrier
	{
		PlayerStats PlayerStats { get; }
	}
}
