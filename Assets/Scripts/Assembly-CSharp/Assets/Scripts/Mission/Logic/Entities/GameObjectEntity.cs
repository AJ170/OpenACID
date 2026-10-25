using Assets.Scripts.Mission.Logic.Components;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public abstract class GameObjectEntity : Entity, IDefinitionCarrier, IEntityStatsCarrier, IUnityComponentCacheCarrier, IEntity, IPositionProvider, IPositionEntity, IGameObjectEntity
	{
		protected bool m_isDetaching;

		private GameObject m_gameObject;

		private EntityWrapperComponent m_wrapper;

		private Transform m_gameObjectParent;

		private readonly UnityComponentCache m_unityComponentCache;

		public bool IsAttached
		{
			get
			{
				return m_gameObject != null && UnityComponentCache.EntityWrapper.IsInitialized;
			}
		}

		public GameObject GameObject
		{
			get
			{
				return m_gameObject;
			}
		}

		public EntityWrapperComponent Wrapper
		{
			get
			{
				return m_wrapper;
			}
		}

		public virtual Vector3 Position
		{
			get
			{
				return UnityComponentCache.Transform.position;
			}
			set
			{
				UnityComponentCache.Transform.position = value;
			}
		}

		public virtual Vector3 Scale
		{
			get
			{
				return UnityComponentCache.Transform.localScale;
			}
			set
			{
				UnityComponentCache.Transform.localScale = value;
			}
		}

		public virtual Quaternion Rotation
		{
			get
			{
				return UnityComponentCache.Transform.rotation;
			}
			set
			{
				UnityComponentCache.Transform.rotation = value;
			}
		}

		public Transform GameObjectParent
		{
			get
			{
				return m_gameObjectParent;
			}
			set
			{
				m_gameObjectParent = value;
			}
		}

		public UnityComponentCache UnityComponentCache
		{
			get
			{
				return m_unityComponentCache;
			}
		}

		protected GameObjectEntity()
		{
			m_unityComponentCache = AddComponent<UnityComponentCache>();
		}

		public sealed override void OnUnregistered()
		{
			if (IsAttached)
			{
				m_wrapper.Entity = null;
			}
			base.OnUnregistered();
		}

		public virtual void AttachGameObject(EntityWrapperComponent _wrapperComponent)
		{
			m_wrapper = _wrapperComponent;
			m_gameObject = _wrapperComponent.gameObject;
			if (m_gameObjectParent != null)
			{
				m_gameObject.transform.parent = m_gameObjectParent;
			}
			for (int i = 0; i < LogicComponents.Count; i++)
			{
				LogicComponents[i].Attach(_wrapperComponent);
			}
		}

		public virtual bool DetachGameObject()
		{
			if (m_isDetaching)
			{
				return false;
			}
			m_isDetaching = true;
			for (int num = LogicComponents.Count - 1; num >= 0; num--)
			{
				LogicComponents[num].Detach();
			}
			m_wrapper = null;
			m_gameObject = null;
			m_isDetaching = false;
			return true;
		}
	}
}
