using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Motion.Hotspots;
using UbiCore.Utils;
using UnityEngine;

namespace UbiCore.Acier
{
	public class BenchInterpreter : HotspotInterpreter
	{
		protected override bool OnKillFromHotspot(EntityId _id)
		{
			return UbiCore.Utils.Singleton<HotspotService>.Instance.BenchManager.OnKillFromHideout(_id);
		}

		protected override bool OnLeaveHotspot(Vector3 _destination)
		{
			return UbiCore.Utils.Singleton<HotspotService>.Instance.BenchManager.LeaveInteraction(PlayerHelper.Entity, _destination);
		}

		protected override void OnSetSelectedEnemy(EnemyCharacter _enemy)
		{
			LogicMain.PlayerEntity.KillFromBench.SelectedEnemy = _enemy;
			LogicMain.PlayerEntity.KillFromBench.AutoSelectionEnabled = false;
		}

		protected override EnemyCharacter GetSelectedEnemy()
		{
			return LogicMain.PlayerEntity.KillFromBench.SelectedEnemy;
		}
	}
}
