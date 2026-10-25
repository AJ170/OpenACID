namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IInteractableObjectStatsCarrier : IDefinitionCarrier, IEntityStatsCarrier, IEntity
	{
		InteractableObjectStats InteractableObjectStats { get; }
	}
}
