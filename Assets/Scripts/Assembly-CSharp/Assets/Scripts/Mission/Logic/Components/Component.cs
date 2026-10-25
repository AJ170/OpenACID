using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public abstract class Component : IComponent
	{
		private readonly IEntity m_Entity;

		public IEntity Entity
		{
			get
			{
				return m_Entity;
			}
		}

		public IGameObjectEntity GameObjectEntity
		{
			get
			{
				return m_Entity as IGameObjectEntity;
			}
		}

		public IGameEntity GameEntity
		{
			get
			{
				return m_Entity as IGameEntity;
			}
		}

		public GameObject GameObject
		{
			get
			{
				return (GameObjectEntity != null) ? GameObjectEntity.GameObject : null;
			}
		}

		public abstract int Priority { get; }

		protected Component(IEntity _entity)
		{
			m_Entity = _entity;
		}

		public virtual void Attach(EntityWrapperComponent _wrapperComponent)
		{
		}

		public virtual void Detach()
		{
		}

		public virtual void OnRegistered()
		{
		}

		public virtual void OnUnregistered()
		{
		}

		public virtual void OnActivate()
		{
		}
	}
}
