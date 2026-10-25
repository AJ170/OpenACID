namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IProjectileStatsCarrier : IEntityStatsCarrier
	{
		ProjectileStats ProjectileStats { get; }
	}
}
