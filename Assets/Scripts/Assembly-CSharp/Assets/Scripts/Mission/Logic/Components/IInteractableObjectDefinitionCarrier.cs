namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IInteractableObjectDefinitionCarrier : IDefinitionCarrier
	{
		InteractableObjectDefinition InteractableObjectDefinition { get; }
	}
}
