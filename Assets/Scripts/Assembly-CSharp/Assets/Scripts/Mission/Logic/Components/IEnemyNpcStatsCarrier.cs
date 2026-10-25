namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IEnemyNpcStatsCarrier : ICharacterStatsCarrier, IDefinitionCarrier, IEntityStatsCarrier, IEntity
	{
		EnemyNpcStats EnemyNpcStats { get; }
	}
}
