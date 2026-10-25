using System;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Entities;
using UbiCore.Debug;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic
{
	public class EntityWrapperComponent : MonoBehaviour
	{
		public delegate void IsInitializedHandler(EntityWrapperComponent _wraper);

		private GameObjectEntity m_entity;

		public bool IsInitialized { get; private set; }

		public GameObjectEntity Entity
		{
			get
			{
				return m_entity;
			}
			set
			{
				Profiling.Start("Entity::set_Entity()", float.MaxValue);
				if (m_entity != null)
				{
					m_entity.DetachGameObject();
				}
				m_entity = value;
				if (m_entity != null)
				{
					m_entity.IsActive = base.enabled;
					m_entity.AttachGameObject(this);
				}
				Profiling.Stop();
			}
		}

		private event IsInitializedHandler m_OnInitialized;

		public event IsInitializedHandler OnInitialized
		{
			add
			{
				this.m_OnInitialized = (IsInitializedHandler)Delegate.Combine(this.m_OnInitialized, value);
			}
			remove
			{
				this.m_OnInitialized = (IsInitializedHandler)Delegate.Remove(this.m_OnInitialized, value);
			}
		}

		public void Start()
		{
			IsInitialized = true;
			IsInitializedHandler onInitialized = this.m_OnInitialized;
			if (onInitialized != null)
			{
				onInitialized(this);
			}
		}

		public void OnEnable()
		{
			if (Entity != null)
			{
				Entity.IsActive = true;
			}
		}

		public void OnDisable()
		{
			if (Entity != null)
			{
				Entity.IsActive = false;
			}
		}

		public T GetEntity<T>() where T : Entity
		{
			return m_entity.Cast<T>();
		}
	}
}
