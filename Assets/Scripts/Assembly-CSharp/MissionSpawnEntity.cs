using System;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Network.Missions.Mission.Data;
using UbiCore.Debug;
using UnityEngine;

public static class MissionSpawnEntity
{
	public delegate void EntitySpawnedHandler(IMissionSpawnEntity _missionSpawnEntity);

	private static event EntitySpawnedHandler s_EntitySpawned;

	public static event EntitySpawnedHandler EntitySpawned
	{
		add
		{
			MissionSpawnEntity.s_EntitySpawned = (EntitySpawnedHandler)Delegate.Combine(MissionSpawnEntity.s_EntitySpawned, value);
		}
		remove
		{
			MissionSpawnEntity.s_EntitySpawned = (EntitySpawnedHandler)Delegate.Remove(MissionSpawnEntity.s_EntitySpawned, value);
		}
	}

	public static void OnEntitySpawned(IMissionSpawnEntity _missionSpawnEntity)
	{
		EntitySpawnedHandler entitySpawnedHandler = MissionSpawnEntity.s_EntitySpawned;
		if (entitySpawnedHandler != null)
		{
			entitySpawnedHandler(_missionSpawnEntity);
		}
	}
}
public abstract class MissionSpawnEntity<TData> : MissionBase<TData>, IMissionBase, IMissionEntity, IMissionSpawnEntity where TData : MissionSpawnEntityData, new()
{
	private bool m_ListenerRegistered;

	public EntityId EntityID { get; private set; }

	public EntityId FormerEntityId { get; private set; }

	public GameEntity GameEntity
	{
		get
		{
			if (EntityID.IsValid)
			{
				return EntityID.GameEntity;
			}
			return null;
		}
	}

	public float ReleaseTime { get; private set; }

	public Vector3? LastKnownPosition { get; private set; }

	protected MissionSpawnEntity()
	{
		EntityID = EntityId.InvalidId;
	}

	public override void OnPreActivate()
	{
		Profiling.Start("MissionSpawnEntity::OnPreActivate()", float.MaxValue);
		if (Assets.Scripts.Offline.OfflineMode.Enabled)
		{
			// Offline missions are hand-authored with approximate positions: land on the walkable surface.
			UbiCore.Helpers.SceneTools.SnapToNavMesh(base.transform, -1, 100f);
		}
		base.OnPreActivate();
		PreCreateEntity();
	}

	private void PreCreateEntity()
	{
		Profiling.Start("Create Entity", float.MaxValue);
		GameEntity gameEntity = CreateEntity();
		Profiling.Stop();
		gameEntity.GameObjectParent = base.transform;
		LogicMain.EntityManager.RegisterEntity(gameEntity);
		EntityID = gameEntity.Id;
		gameEntity.Position = base.transform.position;
		gameEntity.Rotation = base.transform.rotation;
		PostSetupEntity(gameEntity);
		LogicMain.EntityManager.OnEntityUnregistered += OnEntityUnregistered;
		ICharacterVitalityCarrierEntity characterVitalityCarrierEntity = gameEntity as ICharacterVitalityCarrierEntity;
		if (characterVitalityCarrierEntity != null)
		{
			characterVitalityCarrierEntity.CharacterDied += OnKilled;
		}
		m_ListenerRegistered = true;
		Profiling.Stop();
	}

	public override void OnActivate()
	{
		base.OnActivate();
		if (EntityID.GameEntity != null)
		{
			EntityID.GameEntity.OnActivate();
			MissionSpawnEntity.OnEntitySpawned(this);
		}
	}

	private void OnEntityUnregistered(IEntity sender)
	{
		if (sender.Id == EntityID)
		{
			SetLastKnownPosition(sender.Cast<GameEntity>());
			FormerEntityId = EntityID;
			EntityID = EntityId.InvalidId;
			m_ListenerRegistered = false;
		}
	}

	public void SetLastKnownPosition()
	{
		SetLastKnownPosition(GameEntity);
	}

	public virtual void PlayInteractionSound()
	{
	}

	public virtual void PlaySuccessfullInteractionSound()
	{
	}

	private void SetLastKnownPosition(GameEntity sender)
	{
		if (!LastKnownPosition.HasValue)
		{
			LastKnownPosition = sender.Position;
			ReleaseTime = LogicMain.CurrentTime;
		}
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		if (EntityID.IsValid)
		{
			Entity entity = EntityID.Entity;
			if (m_ListenerRegistered)
			{
				ICharacterVitalityCarrierEntity characterVitalityCarrierEntity = entity as ICharacterVitalityCarrierEntity;
				if (characterVitalityCarrierEntity != null)
				{
					characterVitalityCarrierEntity.CharacterDied -= OnKilled;
				}
			}
			LogicMain.EntityManager.UnregisterEntity(entity);
		}
		if (m_ListenerRegistered)
		{
			LogicMain.EntityManager.OnEntityUnregistered -= OnEntityUnregistered;
			m_ListenerRegistered = false;
		}
	}

	private void OnKilled(ICharacterVitalityCarrierEntity _entity)
	{
		SetLastKnownPosition();
	}

	protected abstract GameEntity CreateEntity();

	protected virtual void PostSetupEntity(GameEntity _entity)
	{
		_entity.EntityStats.Set_int(StatTypes.EStatType.Entity_FadeMode, (int)base.Data.FadeMode);
		_entity.EntityStats.Set_float(StatTypes.EStatType.Entity_FadeDuration, base.Data.FadeDuration);
	}
}
