namespace Assets.Scripts.Mission.Logic.Components
{
	public interface ICharacterAppearanceCarrier : IDefinitionCarrier, IEntityStatsCarrier, IGameObjectProxyCarrier, IHighlightCarrier, ISpacialEntityCarrier, IUnityComponentCacheCarrier, IEntity, IUpdateableEntity, IPositionProvider, IPositionEntity, IGameObjectEntity, IGameEntity
	{
		CharacterAppearance CharacterAppearance { get; }
	}
}
