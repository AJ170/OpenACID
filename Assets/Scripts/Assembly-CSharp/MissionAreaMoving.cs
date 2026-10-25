using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Mission.Utilities;
using Assets.Scripts.Network.Missions.Mission.Data;
using UbiCore.Utils;
using UnityEngine;

public class MissionAreaMoving : MissionAreaBase<MissionAreaMovingData>
{
	private IMissionSpawnEntity m_targetEntity;

	public IMissionSpawnEntity TargetEntity
	{
		get
		{
			return m_targetEntity;
		}
		internal set
		{
			if (m_targetEntity != null)
			{
				LogicMain.EntityManager.OnEntityUnregistered -= OnEntityUnregistered;
			}
			m_targetEntity = value;
			if (m_targetEntity != null)
			{
				LogicMain.EntityManager.OnEntityUnregistered += OnEntityUnregistered;
			}
		}
	}

	public override void OnDrawGizmosSelected()
	{
		if (TargetEntity == null)
		{
			return;
		}
		EntityId entityID = TargetEntity.EntityID;
		if (entityID.IsValid)
		{
			GameEntity gameEntity = entityID.GameEntity;
			if (gameEntity != null)
			{
				Gizmos.DrawWireSphere(gameEntity.Position, base.Data.Radius);
			}
		}
	}

	public override bool IsInside(Vector3 _checkPosition)
	{
		if (TargetEntity == null)
		{
			return false;
		}
		EntityId entityID = TargetEntity.EntityID;
		if (!entityID.IsValid)
		{
			return false;
		}
		GameEntity gameEntity = entityID.GameEntity;
		if (gameEntity == null)
		{
			return false;
		}
		Character character = gameEntity as Character;
		if (character != null && character.GetIsDead())
		{
			return false;
		}
		Vector2 vector = new Vector2(gameEntity.Position.x - _checkPosition.x, gameEntity.Position.z - _checkPosition.z);
		return vector.magnitude <= base.Data.Radius;
	}

	public override bool OnDeserialized()
	{
		if (!base.OnDeserialized())
		{
			return false;
		}
		IMissionBase missionObject = GetMissionObject(base.Data.Target);
		TargetEntity = missionObject as IMissionSpawnEntity;
		return true;
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		TargetEntity = null;
	}

	private void OnEntityUnregistered(IEntity sender)
	{
		if (sender.Id == TargetEntity.EntityID)
		{
			TargetEntity = null;
			UbiCore.Utils.Singleton<MissionManager>.Instance.Phase.RemoveMissionEntry(this);
		}
	}
}
