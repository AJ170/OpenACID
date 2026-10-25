namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IEnemyCharacterStateMachinesCarrier : IDefinitionCarrier, ICharacterStateMachinesCarrier, IEntityStatsCarrier, IStateMachinesCarrier, IEntity
	{
		EnemyCharacterStateMachines EnemyCharacterStateMachines { get; }
	}
}
