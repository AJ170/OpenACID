namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IStaticObjectStatsCarrier : IEntityStatsCarrier
	{
		StaticObjectStats StaticObjectStats { get; }
	}
}
