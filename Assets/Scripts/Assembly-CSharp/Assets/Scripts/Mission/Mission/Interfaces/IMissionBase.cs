using AcierProtocol;
using Assets.Scripts.Network.Missions.Mission.Data;

namespace Assets.Scripts.Mission.Mission.Interfaces
{
	public interface IMissionBase
	{
		bool IsActive { get; }

		MissionBaseData MissionData { get; }

		EMapIconType MapIcon { get; }

		bool IsDynamic { get; set; }

		bool OnDeserialized();

		void OnPreActivate();

		void OnActivate();

		void OnDeactivate();

		void OnPostRead(MissionBaseData _nodeData);

		void OnReadCompleted();
	}
}
