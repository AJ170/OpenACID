using System;
using Assets.Scripts.Mission.Logic.Entities;

namespace Assets.Scripts.Mission.Logic
{
	public class EntityId : IComparable<EntityId>
	{
		public static readonly EntityId InvalidId = new EntityId(0u, 0, null);

		public readonly uint Slot;

		public readonly byte UsageCounter;

		private readonly uint m_id;

		private IEntity m_Entity;

		public Entity Entity
		{
			get
			{
				return (Entity)m_Entity;
			}
		}

		public GameObjectEntity GameObjectEntity
		{
			get
			{
				return m_Entity as GameObjectEntity;
			}
		}

		public GameEntity GameEntity
		{
			get
			{
				return m_Entity as GameEntity;
			}
		}

		public EntityTypes.EType Type
		{
			get
			{
				Entity entity = Entity;
				return (entity != null) ? entity.Type : EntityTypes.EType.Invalid;
			}
		}

		public bool IsValid
		{
			get
			{
				return m_Entity != null;
			}
		}

		private EntityId(uint _slot, byte _usageCounter, IEntity _entity)
		{
			Slot = _slot;
			UsageCounter = _usageCounter;
			m_Entity = _entity;
			m_id = MakeId(_slot, _usageCounter);
		}

		public static uint MakeId(uint _slot, byte _usageCounter)
		{
			return (_slot & 0xFFFFu) | (uint)(_usageCounter << 16);
		}

		public static uint Dismantle(uint _id, out byte _usageCounter)
		{
			_usageCounter = (byte)(_id >> 16);
			return _id & 0xFFFFu;
		}

		public static EntityId GetOrCreateEntityId(uint _slot, byte _usageCounter, IEntity _entity)
		{
			EntityId entityId = LogicMain.EntityManager.GetEntityId(_slot, _usageCounter);
			if (entityId == InvalidId)
			{
				return new EntityId(_slot, _usageCounter, _entity);
			}
			if (entityId.Entity != _entity)
			{
				return InvalidId;
			}
			return entityId;
		}

		public void Invalidate()
		{
			m_Entity = null;
		}

		public T GetEntity<T>() where T : class, IEntity
		{
			return (T)m_Entity;
		}

		public T TryGetEntity<T>() where T : class, IEntity
		{
			return m_Entity as T;
		}

		public int CompareTo(EntityId other)
		{
			return m_id.CompareTo(other.m_id);
		}

		public override string ToString()
		{
			return m_id.ToString();
		}

		public static implicit operator uint(EntityId _entityId)
		{
			return _entityId.m_id;
		}
	}
}
