namespace Assets.Scripts.Mission.Logic.Components
{
	public interface ICrowdCharacterStateMachinesCarrier : IDefinitionCarrier, ICharacterStateMachinesCarrier, IEntityStatsCarrier, IStateMachinesCarrier, IEntity
	{
		CrowdCharacterStateMachines CrowdCharacterStateMachines { get; }
	}
}
