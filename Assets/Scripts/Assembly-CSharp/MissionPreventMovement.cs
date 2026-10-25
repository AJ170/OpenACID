using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Network.Missions.Mission.Data;

public class MissionPreventMovement : MissionBase<MissionPreventMovementData>
{
	private void Update()
	{
		if (IsActive && !LogicMain.PlayerEntity.EntityStats.Get_bool(StatTypes.EStatType.Player_MovementPrevented))
		{
			LogicMain.PlayerEntity.EntityStats.Set_bool(StatTypes.EStatType.Player_MovementPrevented, true);
			LogicMain.PlayerEntity.EntityStats.Set_int(StatTypes.EStatType.Player_MovementPreventedException, (int)base.Data.PreventMovementException);
			LogicMain.PlayerEntity.StopMoving();
		}
	}

	public override void OnDeactivate()
	{
		LogicMain.PlayerEntity.EntityStats.Set_bool(StatTypes.EStatType.Player_MovementPrevented, false);
		LogicMain.PlayerEntity.EntityStats.Set_int(StatTypes.EStatType.Player_MovementPreventedException, 0);
		base.OnDeactivate();
	}
}
