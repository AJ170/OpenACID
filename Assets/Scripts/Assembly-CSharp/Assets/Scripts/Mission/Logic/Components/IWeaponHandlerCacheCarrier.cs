namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IWeaponHandlerCacheCarrier : IDefinitionCarrier, IEntityStatsCarrier, IGameObjectProxyCarrier, IHighlightCarrier, ISpacialEntityCarrier, IUnityComponentCacheCarrier, IEntity, IUpdateableEntity, IPositionProvider, IPositionEntity, IGameObjectEntity, IGameEntity
	{
		WeaponHandlerCache WeaponHandlerCache { get; }
	}
}
