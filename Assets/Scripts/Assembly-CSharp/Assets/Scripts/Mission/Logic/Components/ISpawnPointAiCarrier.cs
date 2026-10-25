namespace Assets.Scripts.Mission.Logic.Components
{
	public interface ISpawnPointAiCarrier : IDefinitionCarrier, IEntityStatsCarrier, IGameObjectProxyCarrier, IHighlightCarrier, ISpacialEntityCarrier, IUnityComponentCacheCarrier, IEntity, IUpdateableEntity, IPositionProvider, IPositionEntity, IGameObjectEntity, IGameEntity
	{
		SpawnPointAi SpawnPointAi { get; }
	}
}
