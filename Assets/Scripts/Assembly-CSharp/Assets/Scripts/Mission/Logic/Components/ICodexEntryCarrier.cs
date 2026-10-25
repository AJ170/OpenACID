namespace Assets.Scripts.Mission.Logic.Components
{
	public interface ICodexEntryCarrier : IDefinitionCarrier, IEntityStatsCarrier, IGameObjectProxyCarrier, IHighlightCarrier, ISpacialEntityCarrier, IUnityComponentCacheCarrier, IEntity, IUpdateableEntity, IPositionProvider, IPositionEntity, IGameObjectEntity, IGameEntity
	{
		CodexEntry CodexEntry { get; }
	}
}
