using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Motion.Hotspots;
using UbiCore.Utils;
using UnityEngine;

namespace UbiCore.Acier
{
	public class HaystackInterpreter : HotspotInterpreter
	{
		protected override bool OnKillFromHotspot(EntityId _id)
		{
			return UbiCore.Utils.Singleton<HotspotService>.Instance.HaystackManager.OnKillFromHideout(_id);
		}

		protected override bool OnLeaveHotspot(Vector3 _destination)
		{
			return UbiCore.Utils.Singleton<HotspotService>.Instance.HaystackManager.LeaveInteraction(PlayerHelper.Entity, _destination);
		}

		protected override void OnSetSelectedEnemy(EnemyCharacter _enemy)
		{
			LogicMain.PlayerEntity.KillFromHaystack.SelectedEnemy = _enemy;
			LogicMain.PlayerEntity.KillFromHaystack.AutoSelectionEnabled = false;
		}

		protected override EnemyCharacter GetSelectedEnemy()
		{
			return LogicMain.PlayerEntity.KillFromHaystack.SelectedEnemy;
		}
	}
}
