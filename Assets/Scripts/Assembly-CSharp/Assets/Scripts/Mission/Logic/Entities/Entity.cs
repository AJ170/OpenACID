using System;
using System.Collections.ObjectModel;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;
using UbiCore.Debug;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public abstract class Entity : IDefinitionCarrier, IEntityStatsCarrier, IEntity
	{
		public delegate void EntityRegistrationDelegate(IEntity sender);

		private readonly BBList<IComponent> m_components = new BBList<IComponent>();

		private readonly ReadOnlyCollection<IComponent> m_componentsAccess;

		private readonly BBList<IUpdateableComponent> m_updateableComponents = new BBList<IUpdateableComponent>();

		private readonly ReadOnlyCollection<IUpdateableComponent> m_updateableComponentsAccess;

		private bool m_isActive = true;

		private bool m_dirtyComponentSorting;

		private EntityId m_id = EntityId.InvalidId;

		public ReadOnlyCollection<IComponent> LogicComponents
		{
			get
			{
				return m_componentsAccess;
			}
		}

		public ReadOnlyCollection<IUpdateableComponent> UpdateableLogicComponents
		{
			get
			{
				return m_updateableComponentsAccess;
			}
		}

		public bool IsValid
		{
			get
			{
				return m_id.IsValid;
			}
		}

		public EntityId Id
		{
			get
			{
				return m_id;
			}
			set
			{
				m_id = value;
			}
		}

		public abstract EntityTypes.EType Type { get; }

		public abstract Definition Definition { get; }

		public abstract EntityStats EntityStats { get; }

		public bool IsActive
		{
			get
			{
				return m_isActive;
			}
			set
			{
				m_isActive = value;
			}
		}

		public bool IsReadyForUpdate { get; private set; }

		public event EntityRegistrationDelegate OnEntityRegistered;

		public event EntityRegistrationDelegate OnEntityInitialized;

		public event EntityRegistrationDelegate OnEntityUnregistered;

		protected Entity()
		{
			m_componentsAccess = new ReadOnlyCollection<IComponent>(m_components);
			m_updateableComponentsAccess = new ReadOnlyCollection<IUpdateableComponent>(m_updateableComponents);
		}

		public virtual void OnRegistered()
		{
			IsActive = true;
			IsReadyForUpdate = false;
			SortComponents();
			for (int i = 0; i < LogicComponents.Count; i++)
			{
				if (Main.ProfilingDetailed)
				{
					Profiling.Start(LogicComponents[i].GetType().Name, float.MaxValue);
				}
				LogicComponents[i].OnRegistered();
				if (Main.ProfilingDetailed)
				{
					Profiling.Stop();
				}
			}
		}

		public virtual void OnUnregistered()
		{
			for (int num = LogicComponents.Count - 1; num >= 0; num--)
			{
				LogicComponents[num].OnUnregistered();
			}
		}

		public void OnActivate()
		{
			for (int num = LogicComponents.Count - 1; num >= 0; num--)
			{
				LogicComponents[num].OnActivate();
			}
			IsReadyForUpdate = true;
			InvokeOnEntityInitialized();
			LogicMain.EntityManager.InvokeOnEntityInitialized(this);
		}

		public T FindComponent<T>() where T : IComponent
		{
			return (T)m_components.FirstOrDefault((IComponent c) => c is T);
		}

		public void SortComponents()
		{
			if (m_dirtyComponentSorting)
			{
				m_dirtyComponentSorting = false;
				m_components.Sort((IComponent l, IComponent r) => l.Priority.CompareTo(r.Priority));
				m_updateableComponents.Sort((IUpdateableComponent l, IUpdateableComponent r) => l.Priority.CompareTo(r.Priority));
			}
		}

		protected T AddComponent<T>(T _component) where T : class, IComponent
		{
			T val = (T)m_components.FirstOrDefault((IComponent c) => c.GetType() == _component.GetType());
			if (Assets.Scripts.Engine.Helpers.Verify(val == null))
			{
				val = _component;
				m_components.Add(val);
				IUpdateableComponent updateableComponent = val as IUpdateableComponent;
				if (updateableComponent != null)
				{
					m_updateableComponents.Add(updateableComponent);
				}
				m_dirtyComponentSorting = true;
			}
			return val;
		}

		protected T AddComponent<T>() where T : class, IComponent
		{
			T val = (T)null;
			val = (T)Activator.CreateInstance(typeof(T), this);
			AddComponent(val);
			return val;
		}

		internal void InvokeOnEntityRegistered()
		{
			EntityRegistrationDelegate onEntityRegistered = this.OnEntityRegistered;
			if (onEntityRegistered != null)
			{
				onEntityRegistered(this);
			}
		}

		private void InvokeOnEntityInitialized()
		{
			EntityRegistrationDelegate onEntityInitialized = this.OnEntityInitialized;
			if (onEntityInitialized != null)
			{
				onEntityInitialized(this);
			}
		}

		internal void InvokeOnEntityUnregistered()
		{
			EntityRegistrationDelegate onEntityUnregistered = this.OnEntityUnregistered;
			if (onEntityUnregistered != null)
			{
				onEntityUnregistered(this);
			}
		}
	}
}
