namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IInteractionCharacterStateMachinesCarrier : IDefinitionCarrier, ICharacterStateMachinesCarrier, IEntityStatsCarrier, IStateMachinesCarrier, IEntity
	{
		InteractionCharacterStateMachines InteractionCharacterStateMachines { get; }
	}
}
