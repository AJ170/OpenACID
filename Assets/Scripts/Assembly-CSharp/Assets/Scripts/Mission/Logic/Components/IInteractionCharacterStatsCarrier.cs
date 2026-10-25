namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IInteractionCharacterStatsCarrier : ICharacterStatsCarrier, IDefinitionCarrier, IEntityStatsCarrier, IEntity
	{
		InteractionCharacterStats InteractionCharacterStats { get; }
	}
}
