namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IPatrolStatsCarrier : IDefinitionCarrier, IEntityStatsCarrier, IEntity
	{
		PatrolStats PatrolStats { get; }
	}
}
