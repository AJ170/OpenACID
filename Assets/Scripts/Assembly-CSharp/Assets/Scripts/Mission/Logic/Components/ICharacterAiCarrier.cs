namespace Assets.Scripts.Mission.Logic.Components
{
	public interface ICharacterAiCarrier : IDefinitionCarrier, IEntityStatsCarrier, IGameObjectProxyCarrier, IHighlightCarrier, ISpacialEntityCarrier, IUnityComponentCacheCarrier, IEntity, IUpdateableEntity, IPositionProvider, IPositionEntity, IGameObjectEntity, IGameEntity
	{
		CharacterAi CharacterAi { get; }
	}
}
