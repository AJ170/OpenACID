namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IAwareInteractionCharacterStateMachinesCarrier : IDefinitionCarrier, ICharacterStateMachinesCarrier, IInteractionCharacterStateMachinesCarrier, IEntityStatsCarrier, IStateMachinesCarrier, IEntity
	{
		AwareInteractionCharacterStateMachines AwareInteractionCharacterStateMachines { get; }
	}
}
