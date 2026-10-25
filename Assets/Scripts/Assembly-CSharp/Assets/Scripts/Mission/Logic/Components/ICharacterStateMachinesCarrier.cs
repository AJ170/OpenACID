namespace Assets.Scripts.Mission.Logic.Components
{
	public interface ICharacterStateMachinesCarrier : IDefinitionCarrier, IEntityStatsCarrier, IStateMachinesCarrier, IEntity
	{
		CharacterStateMachines CharacterStateMachines { get; }
	}
}
