namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IMissionResultCarrier : IDefinitionCarrier, IEntityStatsCarrier, IEntity
	{
		MissionResult MissionResult { get; }
	}
}
