using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Network.Missions.Mission.Data;
using UnityEngine;

public class MissionSpawnPoint : MissionBase<MissionSpawnPointData>
{
	private EntityId m_entityID = EntityId.InvalidId;

	public EntityId EntityID
	{
		get
		{
			return m_entityID;
		}
	}

	public GameEntity GameEntity
	{
		get
		{
			return m_entityID.GameEntity;
		}
	}

	public void OnDrawGizmosSelected()
	{
		Color color = Gizmos.color;
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(base.transform.position, 2f);
		Gizmos.color = color;
	}

	public override void OnPreActivate()
	{
		SpawnPoint spawnPoint = Helpers.CreateEntity<SpawnPoint>(EntityTypes.EType.SpawnPoint);
		LogicMain.EntityManager.RegisterEntity(spawnPoint);
		m_entityID = spawnPoint.Id;
	}
}
