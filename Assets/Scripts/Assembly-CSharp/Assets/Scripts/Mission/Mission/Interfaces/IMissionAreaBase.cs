using Assets.Scripts.Network.Missions.Mission.Data;
using UnityEngine;

namespace Assets.Scripts.Mission.Mission.Interfaces
{
	public interface IMissionAreaBase : IMissionBase
	{
		MissionAreaBaseData AreaData { get; }

		bool IsInside(Vector3 _checkPosition);
	}
}
