namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IEntityTrackingCarrier : IDefinitionCarrier, IEntityStatsCarrier, IEntity
	{
		EntityTracking EntityTracking { get; }
	}
}
