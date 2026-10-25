using Assets.Scripts.Mission.Logic.Components;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic
{
	public interface IGameObjectEntity : IDefinitionCarrier, IEntityStatsCarrier, IUnityComponentCacheCarrier, IEntity, IPositionProvider, IPositionEntity
	{
		GameObject GameObject { get; }

		Transform GameObjectParent { get; set; }

		EntityWrapperComponent Wrapper { get; }

		bool IsAttached { get; }

		void AttachGameObject(EntityWrapperComponent _wrapperComponent);

		bool DetachGameObject();
	}
}
