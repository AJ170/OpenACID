using AcierProtocol;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Network.Missions.Mission.Data;
using UnityEngine;

public class TemplateSpawnPoint : MissionSpawnEntity<TemplateSpawnPointData>
{
	[EnumFlags]
	public ESpawnPointKind SpawnPointKinds = ESpawnPointKind.EnemyGround;

	private MissionWaypoint[] m_waypoints;

	private int m_lastChildCount;

	public override void OnPreActivate()
	{
	}

	protected override GameEntity CreateEntity()
	{
		return null;
	}

	public override void OnPostRead(MissionBaseData _nodeData)
	{
		base.OnPostRead(_nodeData);
		SpawnPointKinds = base.Data.SpawnPointKind;
	}

	protected override void PostSetupEntity(GameEntity _entity)
	{
		base.PostSetupEntity(_entity);
	}

	public void OnDrawGizmos()
	{
		if (m_waypoints == null || m_waypoints.Length == 0)
		{
			return;
		}
		Gizmos.color = Color.white;
		for (int i = 0; i < m_waypoints.Length - 1; i++)
		{
			Gizmos.DrawLine(m_waypoints[i].transform.position, m_waypoints[i + 1].transform.position);
			Gizmos.DrawWireSphere(m_waypoints[i].transform.position, 0.2f);
		}
		if (m_waypoints.Length <= 1)
		{
			switch (SpawnPointKinds)
			{
			case ESpawnPointKind.Player:
				Gizmos.DrawIcon(base.transform.position, "player.png", false);
				break;
			case ESpawnPointKind.EnemyGround:
				Gizmos.DrawIcon(base.transform.position, "guard.png", false);
				break;
			case ESpawnPointKind.Captives:
				Gizmos.DrawIcon(base.transform.position, "captive.png", false);
				break;
			}
		}
	}
}
