using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Motion.Hotspots;
using UbiCore.Utils;
using UnityEngine;

namespace UbiCore.Acier
{
	public class RooftopGardenInterpreter : HotspotInterpreter
	{
		protected override bool OnKillFromHotspot(EntityId _id)
		{
			return UbiCore.Utils.Singleton<HotspotService>.Instance.RooftopGardenManager.OnKillFromHideout(_id);
		}

		protected override bool OnLeaveHotspot(Vector3 _destination)
		{
			return UbiCore.Utils.Singleton<HotspotService>.Instance.RooftopGardenManager.LeaveInteraction(PlayerHelper.Entity, _destination);
		}

		protected override void OnSetSelectedEnemy(EnemyCharacter _enemy)
		{
			LogicMain.PlayerEntity.KillFromRooftopGarden.SelectedEnemy = _enemy;
			LogicMain.PlayerEntity.KillFromRooftopGarden.AutoSelectionEnabled = false;
		}

		protected override EnemyCharacter GetSelectedEnemy()
		{
			return LogicMain.PlayerEntity.KillFromRooftopGarden.SelectedEnemy;
		}
	}
}
