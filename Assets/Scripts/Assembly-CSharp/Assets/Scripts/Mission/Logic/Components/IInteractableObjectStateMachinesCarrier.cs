namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IInteractableObjectStateMachinesCarrier : IDefinitionCarrier, IEntityStatsCarrier, IStateMachinesCarrier, IEntity
	{
		InteractableObjectStateMachines InteractableObjectStateMachines { get; }
	}
}
