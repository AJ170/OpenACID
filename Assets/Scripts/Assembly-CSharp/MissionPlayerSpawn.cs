using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Network.Missions.Mission.Data;
using Humanoids;
using UbiCore.Utils;
using UnityEngine;

public class MissionPlayerSpawn : MissionBase<MissionPlayerSpawnData>
{
	private GameObject m_playerGO;

	public override void OnPreActivate()
	{
		AvatarData.IAvatarDefinition playerDefinition = UbiCore.Utils.Singleton<AvatarData>.Instance.GetPlayerDefinition();
		m_playerGO = SpawnHelper.SpawnPlayer(playerDefinition, base.transform);
	}

	public override void OnActivate()
	{
		base.OnActivate();
		if (LogicMain.PlayerEntity != null)
		{
			LogicMain.PlayerEntity.OnActivate();
		}
	}

	public override void OnDeactivate()
	{
		if (LogicMain.PlayerEntity != null)
		{
			Singleton<EffectManager>.Instance.DestroyEffects();
			Singleton<HumanoidFactory>.Instance.DestroyHumanoid(m_playerGO);
			m_playerGO = null;
		}
	}
}
