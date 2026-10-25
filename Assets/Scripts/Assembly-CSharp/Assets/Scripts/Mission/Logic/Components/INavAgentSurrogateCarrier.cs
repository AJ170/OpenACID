namespace Assets.Scripts.Mission.Logic.Components
{
	public interface INavAgentSurrogateCarrier : IDefinitionCarrier, IEntityStatsCarrier, IGameObjectProxyCarrier, IHighlightCarrier, ISpacialEntityCarrier, IUnityComponentCacheCarrier, IEntity, IUpdateableEntity, IPositionProvider, IPositionEntity, IGameObjectEntity, IGameEntity
	{
		NavAgentSurrogate NavAgentSurrogate { get; }
	}
}
