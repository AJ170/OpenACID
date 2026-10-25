namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IProjectileStateMachinesCarrier : IDefinitionCarrier, IEntityStatsCarrier, IStateMachinesCarrier, IEntity
	{
		ProjectileStateMachines ProjectileStateMachines { get; }
	}
}
