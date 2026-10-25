namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IPatrolStateMachinesCarrier : IDefinitionCarrier, IEntityStatsCarrier, IStateMachinesCarrier, IEntity
	{
		PatrolStateMachines PatrolStateMachines { get; }
	}
}
