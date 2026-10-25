namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IAwareInteractionCharacterStatsCarrier : ICharacterStatsCarrier, IDefinitionCarrier, IEntityStatsCarrier, IInteractionCharacterStatsCarrier, IEntity
	{
		AwareInteractionCharacterStats AwareInteractionCharacterStats { get; }
	}
}
