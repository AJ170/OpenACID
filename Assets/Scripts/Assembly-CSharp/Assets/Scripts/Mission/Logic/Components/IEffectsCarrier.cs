namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IEffectsCarrier : IDefinitionCarrier, IEntityStatsCarrier, IUnityComponentCacheCarrier, IEntity, IPositionProvider, IPositionEntity, IGameObjectEntity
	{
		Effects Effects { get; }
	}
}
