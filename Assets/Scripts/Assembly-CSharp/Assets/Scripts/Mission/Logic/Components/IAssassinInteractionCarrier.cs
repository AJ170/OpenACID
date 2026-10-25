namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IAssassinInteractionCarrier : IDefinitionCarrier, IEntityStatsCarrier, IGameObjectProxyCarrier, IHighlightCarrier, ISpacialEntityCarrier, IUnityComponentCacheCarrier, IEntity, IUpdateableEntity, IPositionProvider, IPositionEntity, IGameObjectEntity, IGameEntity
	{
		AssassinInteraction AssassinInteraction { get; }
	}
}
