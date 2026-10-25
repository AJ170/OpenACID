using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic
{
	public interface IGameEntity : IDefinitionCarrier, IEntityStatsCarrier, IGameObjectProxyCarrier, IHighlightCarrier, ISpacialEntityCarrier, IUnityComponentCacheCarrier, IEntity, IUpdateableEntity, IPositionProvider, IPositionEntity, IGameObjectEntity
	{
		string Name { get; }

		float DistanceBucket { get; set; }

		float DistanceBucketExpireTime { get; set; }

		bool CanBePooled { get; set; }

		bool AutoUnregisterOnDetach { get; set; }

		bool SkipLogicUpdateAllowed { get; }
	}
}
