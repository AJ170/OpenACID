namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IPatrolCarrier : IDefinitionCarrier, IEntityStatsCarrier, IEntity
	{
		Patrol PatrolComponent { get; }
	}
}
