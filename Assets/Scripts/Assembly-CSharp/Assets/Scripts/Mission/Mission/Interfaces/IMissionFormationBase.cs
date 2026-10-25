using Assets.Scripts.Network.Missions.Mission.Data;

namespace Assets.Scripts.Mission.Mission.Interfaces
{
	public interface IMissionFormationBase : IMissionBase, IMissionEntity
	{
		void OnWaypointLoaded(MissionWaypointData _waypoint);

		void OnWaypointUnload(MissionWaypointData _waypoint);
	}
}
