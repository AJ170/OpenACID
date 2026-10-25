using Assets.Scripts.Mission.Logic.Components;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public abstract class GameEntity : GameObjectEntity, IDefinitionCarrier, IEntityStatsCarrier, IGameObjectProxyCarrier, IHighlightCarrier, ISpacialEntityCarrier, IUnityComponentCacheCarrier, IEntity, IUpdateableEntity, IPositionProvider, IPositionEntity, IGameObjectEntity, IGameEntity
	{
		private string m_name;

		private int m_CannotBePooledCounter;

		private readonly SpacialEntity m_spacialEntity;

		private readonly GameObjectProxy m_gameObjectProxy;

		public override Vector3 Position
		{
			get
			{
				return GameObjectProxy.Position;
			}
			set
			{
				if (SpacialEntity.IsDirty || GameObjectProxy.Position != value)
				{
					SpacialEntity.IsDirty = true;
					GameObjectProxy.Position = value;
				}
			}
		}

		public override Quaternion Rotation
		{
			get
			{
				return GameObjectProxy.Rotation;
			}
			set
			{
				if (SpacialEntity.IsDirty || GameObjectProxy.Rotation != value)
				{
					SpacialEntity.IsDirty = true;
					GameObjectProxy.Rotation = value;
				}
			}
		}

		public SpacialEntity SpacialEntity
		{
			get
			{
				return m_spacialEntity;
			}
		}

		public GameObjectProxy GameObjectProxy
		{
			get
			{
				return m_gameObjectProxy;
			}
		}

		public Highlight Highlight { get; private set; }

		public virtual bool CanBePooled
		{
			get
			{
				return m_CannotBePooledCounter <= 0;
			}
			set
			{
				if (value)
				{
					m_CannotBePooledCounter--;
				}
				else
				{
					m_CannotBePooledCounter++;
				}
				if (m_CannotBePooledCounter < 0)
				{
					m_CannotBePooledCounter = 0;
				}
			}
		}

		public float DistanceBucket { get; set; }

		public float DistanceBucketExpireTime { get; set; }

		public string Name
		{
			get
			{
				return m_name;
			}
		}

		public bool AutoUnregisterOnDetach { get; set; }

		public virtual bool SkipLogicUpdateAllowed
		{
			get
			{
				return true;
			}
		}

		public virtual bool HighlightedSelection
		{
			get
			{
				return true;
			}
		}

		public virtual bool HighlightUsesRimColor
		{
			get
			{
				return false;
			}
		}

		protected GameEntity()
		{
			m_gameObjectProxy = AddComponent<GameObjectProxy>();
			Highlight = AddComponent<Highlight>();
			m_spacialEntity = AddComponent<SpacialEntity>();
		}

		public sealed override void OnRegistered()
		{
			base.OnRegistered();
			m_name = string.Format("{0}_{1}", Type.ToString(), Id.ToString());
			m_CannotBePooledCounter = 0;
		}

		public override void AttachGameObject(EntityWrapperComponent _wrapperComponent)
		{
			base.AttachGameObject(_wrapperComponent);
			GameObject.name = m_name;
		}

		public sealed override bool DetachGameObject()
		{
			if (!base.DetachGameObject())
			{
				return false;
			}
			m_isDetaching = true;
			if (AutoUnregisterOnDetach)
			{
				LogicMain.EntityManager.UnregisterEntity(this);
			}
			m_isDetaching = false;
			return true;
		}
	}
}
