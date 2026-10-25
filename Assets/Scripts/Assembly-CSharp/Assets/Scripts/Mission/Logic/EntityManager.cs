using System;
using System.Collections.Generic;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using SharedBaseLib.Core;
using UbiCore.Debug;
using UbiCore.Helpers;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic
{
	public class EntityManager
	{
		private class FilterInfo
		{
			public IEntityFilter Filter { get; private set; }

			public BBList<IEntity> Bucket { get; private set; }

			public FilterInfo(IEntityFilter _filter)
			{
				Filter = _filter;
				Bucket = new BBList<IEntity>();
			}
		}

		public delegate void EntityRegistrationDelegate(IEntity sender);

		public const uint MaximumSize = 2000u;

		private readonly BBList<IEntity> m_entityBucket;

		private readonly BBList<IEntity> m_updateableEntityBucket;

		private readonly BBList<IEntity> m_gameEntityBucket;

		private readonly BBList<IEntity> m_enemyCharacterBucket;

		private readonly BBList<IEntity> m_npcCharacterBucket;

		private readonly BBList<IEntity> m_awareInteractionCharacterBucket;

		private readonly BBList<IEntity> m_crowdCharacterBucket;

		private readonly BBList<IEntity>[] m_buckets = new BBList<IEntity>[28];

		private readonly FilterInfo[] m_FilterInfos = new FilterInfo[1];

		private readonly BBList<EntityId> m_addList = new BBList<EntityId>();

		private readonly BBList<EntityId> m_removeList = new BBList<EntityId>();

		private readonly BBHashSet<EntityId> m_removeSet = new BBHashSet<EntityId>();

		private readonly IEntity[] m_entityAccess = new IEntity[2000];

		public byte[] m_idUsage = new byte[2000];

		private BBList<uint> m_freeIds = new BBList<uint>();

		private uint m_maximumId;

		private readonly BBList<IEntityRemovedFromListHandler> m_EntityRemovedFromListHandlers = new BBList<IEntityRemovedFromListHandler>();

		private OctreeDynamic m_Octree;

		private BBList<BBList<IOctreeDynamicEntity>> m_collectTemp = new BBList<BBList<IOctreeDynamicEntity>>();

		public int EntityCount
		{
			get
			{
				return m_entityBucket.Count;
			}
		}

		public int UpdateableEntityCount
		{
			get
			{
				return m_updateableEntityBucket.Count;
			}
		}

		public int NpcCharacterCount
		{
			get
			{
				return m_npcCharacterBucket.Count;
			}
		}

		public int EnemyCharacterCount
		{
			get
			{
				return m_enemyCharacterBucket.Count;
			}
		}

		public int AwareInteractionCharacterCount
		{
			get
			{
				return m_awareInteractionCharacterBucket.Count;
			}
		}

		public int CrowdCharacterCount
		{
			get
			{
				return m_crowdCharacterBucket.Count;
			}
		}

		public int GameEntityCount
		{
			get
			{
				return m_gameEntityBucket.Count;
			}
		}

		public event EntityRegistrationDelegate OnEntityRegistered;

		public event EntityRegistrationDelegate OnEntityInitialized;

		public event EntityRegistrationDelegate OnEntityUnregistered;

		public EntityManager()
		{
			for (int i = 0; i < 28; i++)
			{
				m_buckets[i] = new BBList<IEntity>();
			}
			m_FilterInfos[0] = new FilterInfo(new UpdateableEntityFilter());
			m_entityBucket = m_buckets[1];
			m_updateableEntityBucket = m_FilterInfos[0].Bucket;
			m_gameEntityBucket = m_buckets[4];
			m_npcCharacterBucket = m_buckets[11];
			m_enemyCharacterBucket = m_buckets[12];
			m_awareInteractionCharacterBucket = m_buckets[15];
			m_crowdCharacterBucket = m_buckets[13];
		}

		private void InvokeOnEntityRegistered(IEntity entity)
		{
			EntityRegistrationDelegate onEntityRegistered = this.OnEntityRegistered;
			if (onEntityRegistered != null)
			{
				onEntityRegistered(entity);
			}
		}

		internal void InvokeOnEntityInitialized(IEntity entity)
		{
			EntityRegistrationDelegate onEntityInitialized = this.OnEntityInitialized;
			if (onEntityInitialized != null)
			{
				onEntityInitialized(entity);
			}
		}

		private void InvokeOnEntityUnregistered(IEntity entity)
		{
			EntityRegistrationDelegate onEntityUnregistered = this.OnEntityUnregistered;
			if (onEntityUnregistered != null)
			{
				onEntityUnregistered(entity);
			}
		}

		public T GetEntity<T>(EntityTypes.EType _type, int _index) where T : IEntity
		{
			return (T)m_buckets[(int)_type][_index];
		}

		public int GetEntityCount(EntityTypes.EType _type)
		{
			return m_buckets[(int)_type].Count;
		}

		public IEnumerable<T> CollectEntitiesInRadius<T>(Vector3 _center, float _radius) where T : GameEntity
		{
			return CollectEntitiesInRadius<T>(_center, _radius, null);
		}

		public IEnumerable<T> CollectEntitiesInRadius<T>(Vector3 _center, float _radius, Predicate<T> _filter) where T : GameEntity
		{
			float sqrRadius = _radius * _radius;
			BBList<IOctreeDynamicEntity> collectList2 = null;
			if (m_collectTemp.Count > 0)
			{
				collectList2 = m_collectTemp[m_collectTemp.Count - 1];
				m_collectTemp.RemoveAt(m_collectTemp.Count - 1);
				collectList2.ClearIfNotEmpty();
			}
			else
			{
				collectList2 = new BBList<IOctreeDynamicEntity>();
			}
			m_Octree.GetObjects(collectList2, _center, _radius);
			for (int i = 0; i < collectList2.Count; i++)
			{
				T entity = ((SpacialEntity)collectList2[i]).GameEntity as T;
				if (entity != null && entity.Id != EntityId.InvalidId)
				{
					float distanceSqr = entity.Position.DistanceSquared(_center);
					if (distanceSqr <= sqrRadius && (_filter == null || _filter(entity)))
					{
						yield return entity;
					}
				}
			}
			m_collectTemp.Add(collectList2);
		}

		public IEnumerable<T> CollectEntitiesInRadius<T>(EntityTypes.EType _type, Vector3 _center, float _radius) where T : IPositionEntity
		{
			float sqrRadius = _radius * _radius;
			BBList<IEntity> bucket = m_buckets[(int)_type];
			for (int i = 0; i < bucket.Count; i++)
			{
				T entity = (T)bucket[i];
				if (entity.Id != EntityId.InvalidId)
				{
					float distanceSqr = ((IPositionProvider)entity).Position.DistanceSquared(_center);
					if (distanceSqr <= sqrRadius)
					{
						yield return entity;
					}
				}
			}
		}

		public EntityId GetEntityId(uint _slot, byte _usageCounter)
		{
			if (_slot == 0)
			{
				return EntityId.InvalidId;
			}
			IEntity entity = m_entityAccess[_slot - 1];
			if (entity == null)
			{
				return EntityId.InvalidId;
			}
			if (entity.Id.UsageCounter == _usageCounter)
			{
				return entity.Id;
			}
			return EntityId.InvalidId;
		}

		public EntityId GetEntityId(uint _id)
		{
			byte _usageCounter;
			uint slot = EntityId.Dismantle(_id, out _usageCounter);
			return GetEntityId(slot, _usageCounter);
		}

		public IEntity GetEntity(int _index)
		{
			return m_entityBucket[_index];
		}

		public IEntity GetUpdateableEntity(int _index)
		{
			return m_updateableEntityBucket[_index];
		}

		public NpcCharacter GetNpcCharacter(int _index)
		{
			return (NpcCharacter)m_npcCharacterBucket[_index];
		}

		public EnemyCharacter GetEnemyCharacter(int _index)
		{
			return (EnemyCharacter)m_enemyCharacterBucket[_index];
		}

		public AwareInteractionCharacter GetAwareInteractionCharacter(int _index)
		{
			return (AwareInteractionCharacter)m_awareInteractionCharacterBucket[_index];
		}

		public CrowdCharacter GetCrowdCharacter(int _index)
		{
			return (CrowdCharacter)m_crowdCharacterBucket[_index];
		}

		public GameEntity GetGameEntity(int _index)
		{
			return (GameEntity)m_gameEntityBucket[_index];
		}

		public T GetEntity<T>(EEntityFilterType _filterType, int _index) where T : IEntity
		{
			return (T)m_FilterInfos[(int)_filterType].Bucket[_index];
		}

		public int GetEntityCount(EEntityFilterType _filterType)
		{
			return m_FilterInfos[(int)_filterType].Bucket.Count;
		}

		public void RegisterHandler(IEntityRemovedFromListHandler _handler)
		{
			if (!m_EntityRemovedFromListHandlers.Contains(_handler))
			{
				m_EntityRemovedFromListHandlers.Add(_handler);
			}
		}

		public void RegisterEntity(Entity _entity)
		{
			Profiling.Start("EntityManager::RegisterEntity()", float.MaxValue);
			_entity.Id = CreateEntityId(_entity);
			m_entityAccess[_entity.Id.Slot - 1] = _entity;
			m_addList.Add(_entity.Id);
			_entity.OnRegistered();
			GameEntity gameEntity = _entity as GameEntity;
			if (gameEntity != null)
			{
				m_Octree.Add(gameEntity.SpacialEntity);
			}
			Profiling.Stop();
		}

		public void UnregisterEntity(Entity _entity)
		{
			if (!m_removeSet.Contains(_entity.Id))
			{
				m_removeSet.Add(_entity.Id);
				_entity.OnUnregistered();
				m_removeList.Add(_entity.Id);
				_entity.InvokeOnEntityUnregistered();
				GameEntity gameEntity = _entity as GameEntity;
				if (gameEntity != null)
				{
					m_Octree.Remove(gameEntity.SpacialEntity);
				}
				InvokeOnEntityUnregistered(_entity);
				_entity.Id.Invalidate();
				_entity.Id = EntityId.InvalidId;
			}
		}

		public void UnregisterEntities()
		{
			for (int i = 0; i < EntityCount; i++)
			{
				Entity entity = (Entity)GetEntity(i);
				if (entity.Id.IsValid)
				{
					UnregisterEntity(entity);
				}
			}
		}

		public void DoHouseKeeping()
		{
			Profiling.Start("EntityManager.DoHouseKeeping()", float.MaxValue);
			for (int i = 0; i < m_removeList.Count; i++)
			{
				EntityId entityId = m_removeList[i];
				uint slot = entityId.Slot;
				IEntity entity = m_entityAccess[slot - 1];
				if (!m_addList.Remove(entityId))
				{
					DoIfIsOnBucket<Entity>(entity, EntityTypes.EType.Entity, delegate(BBList<IEntity> list)
					{
						OnEntityRemovedFromList(entity, list.IndexOf(entity));
					});
					RemoveFromBuckets(entity);
				}
				m_entityAccess[slot - 1] = null;
				m_freeIds.Add(slot);
			}
			m_removeList.ClearIfNotEmpty();
			m_removeSet.Clear();
			for (int j = 0; j < m_addList.Count; j++)
			{
				AddToBuckets(m_entityAccess[m_addList[j].Slot - 1]);
			}
			m_addList.ClearIfNotEmpty();
			Profiling.Stop();
		}

		public void UpdateTurn()
		{
			Profiling.Start("Update spacial index", float.MaxValue);
			for (int i = 0; i < GameEntityCount; i++)
			{
				GameEntity gameEntity = GetGameEntity(i);
				if (gameEntity.IsActive && gameEntity.Id != EntityId.InvalidId)
				{
					SpacialEntity spacialEntity = gameEntity.SpacialEntity;
					if (spacialEntity.IsDirty)
					{
						m_Octree.Update(spacialEntity);
						spacialEntity.IsDirty = false;
					}
				}
			}
			Profiling.Stop();
		}

		public void OnMissionLoad()
		{
			Bounds sceneCollidersBounds = SceneTools.GetSceneCollidersBounds();
			m_Octree = new OctreeDynamic(sceneCollidersBounds.size.magnitude + 5f, 1f, 3, sceneCollidersBounds.center);
		}

		private void OnEntityRemovedFromList(IEntity _entity, int _idx)
		{
			for (int i = 0; i < m_EntityRemovedFromListHandlers.Count; i++)
			{
				m_EntityRemovedFromListHandlers[i].OnEntityRemovedFromList(_entity, _idx);
			}
		}

		private bool DoIfIsOnBucket<T>(IEntity _entity, EntityTypes.EType _type, Action<BBList<IEntity>> _action) where T : Entity
		{
			return _entity.DoIfIs<T>(delegate
			{
				_action(m_buckets[(int)_type]);
			});
		}

		private void DoOnBuckets(IEntity _entity, Action<BBList<IEntity>> _action)
		{
			DoIfIsOnBucket<Entity>(_entity, EntityTypes.EType.Entity, _action);
			DoIfIsOnBucket<GameLogic>(_entity, EntityTypes.EType.GameLogic, _action);
			DoIfIsOnBucket<GameEntity>(_entity, EntityTypes.EType.GameEntity, _action);
			DoIfIsOnBucket<Assets.Scripts.Mission.Logic.Entities.Patrol>(_entity, EntityTypes.EType.Patrol, _action);
			DoIfIsOnBucket<Character>(_entity, EntityTypes.EType.Character, _action);
			DoIfIsOnBucket<NpcCharacter>(_entity, EntityTypes.EType.NpcCharacter, _action);
			DoIfIsOnBucket<AssassinCharacter>(_entity, EntityTypes.EType.AssassinCharacter, _action);
			DoIfIsOnBucket<PlayerCharacter>(_entity, EntityTypes.EType.PlayerCharacter, _action);
			DoIfIsOnBucket<EnemyCharacter>(_entity, EntityTypes.EType.EnemyCharacter, _action);
			DoIfIsOnBucket<CrowdCharacter>(_entity, EntityTypes.EType.CrowdCharacter, _action);
			DoIfIsOnBucket<InteractionCharacter>(_entity, EntityTypes.EType.InteractionCharacter, _action);
			DoIfIsOnBucket<AwareInteractionCharacter>(_entity, EntityTypes.EType.AwareInteractionCharacter, _action);
			DoIfIsOnBucket<Assets.Scripts.Mission.Logic.Entities.Object>(_entity, EntityTypes.EType.Object, _action);
			DoIfIsOnBucket<Projectile>(_entity, EntityTypes.EType.Projectile, _action);
			DoIfIsOnBucket<InteractableObject>(_entity, EntityTypes.EType.InteractibleObject, _action);
			DoIfIsOnBucket<Haystack>(_entity, EntityTypes.EType.Haystack, _action);
			DoIfIsOnBucket<Codex>(_entity, EntityTypes.EType.Codex, _action);
			DoIfIsOnBucket<SpawnPoint>(_entity, EntityTypes.EType.SpawnPoint, _action);
			DoIfIsOnBucket<InteractableMissionObject>(_entity, EntityTypes.EType.InteractibleMissionObject, _action);
			DoIfIsOnBucket<Bench>(_entity, EntityTypes.EType.Bench, _action);
			DoIfIsOnBucket<RooftopGarden>(_entity, EntityTypes.EType.RooftopGarden, _action);
			DoIfIsOnBucket<StalkingZone>(_entity, EntityTypes.EType.StalkingZone, _action);
			for (int i = 0; i < 1; i++)
			{
				if (m_FilterInfos[i].Filter.Filter(_entity))
				{
					_action(m_FilterInfos[i].Bucket);
				}
			}
		}

		private void RemoveFromBuckets(IEntity _entity)
		{
			DoOnBuckets(_entity, delegate(BBList<IEntity> l)
			{
				l.Remove(_entity);
			});
		}

		private void AddToBuckets(IEntity _entity)
		{
			DoOnBuckets(_entity, delegate(BBList<IEntity> l)
			{
				l.Add(_entity);
			});
			((Entity)_entity).InvokeOnEntityRegistered();
			InvokeOnEntityRegistered(_entity);
		}

		private EntityId CreateEntityId(Entity _entity)
		{
			uint num = 0u;
			if (m_freeIds.Count > 0)
			{
				num = m_freeIds[0];
				m_freeIds.RemoveAt(0);
			}
			else
			{
				if (m_maximumId >= 2000)
				{
					return EntityId.InvalidId;
				}
				num = m_maximumId + 1;
				m_maximumId++;
			}
			return EntityId.GetOrCreateEntityId(num, ++m_idUsage[num - 1], _entity);
		}
	}
}
