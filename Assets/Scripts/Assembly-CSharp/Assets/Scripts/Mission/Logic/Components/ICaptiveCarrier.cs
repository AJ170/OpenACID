namespace Assets.Scripts.Mission.Logic.Components
{
	public interface ICaptiveCarrier : IDefinitionCarrier, IEntityStatsCarrier, IEntity
	{
		Captive CaptiveComponent { get; }
	}
}
