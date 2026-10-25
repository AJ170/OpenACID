namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IPlayerEventHandlerCarrier : IDefinitionCarrier, IEntityStatsCarrier, IGameObjectProxyCarrier, IHighlightCarrier, ISpacialEntityCarrier, IUnityComponentCacheCarrier, IEntity, IUpdateableEntity, IPositionProvider, IPositionEntity, IGameObjectEntity, IGameEntity
	{
		PlayerEventHandler PlayerEventHandler { get; }
	}
}
