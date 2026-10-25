using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Network.Missions.Mission.Data;

public class MissionWaypoint : MissionBase<MissionWaypointData>
{
	public IMissionFormationBase MissionNpcFormation { get; set; }

	public override void OnActivate()
	{
		if (MissionNpcFormation != null)
		{
			MissionNpcFormation.OnWaypointLoaded(base.Data);
		}
		base.OnActivate();
	}

	public override void OnDeactivate()
	{
		if (MissionNpcFormation != null)
		{
			MissionNpcFormation.OnWaypointUnload(base.Data);
		}
		base.OnDeactivate();
	}
}
