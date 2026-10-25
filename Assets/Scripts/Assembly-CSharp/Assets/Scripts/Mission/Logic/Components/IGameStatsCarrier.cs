namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IGameStatsCarrier : IDefinitionCarrier, IEntityStatsCarrier, IEntity
	{
		GameStats GameStats { get; }
	}
}
