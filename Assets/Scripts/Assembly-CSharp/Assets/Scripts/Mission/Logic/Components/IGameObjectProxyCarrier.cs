namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IGameObjectProxyCarrier : IDefinitionCarrier, IEntityStatsCarrier, IEntity
	{
		GameObjectProxy GameObjectProxy { get; }
	}
}
