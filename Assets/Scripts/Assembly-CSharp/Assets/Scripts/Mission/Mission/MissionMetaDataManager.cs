using AcierProtocol;
using Assets.Scripts.UI.LoadoutScreen;

namespace Assets.Scripts.Mission.Mission
{
	public class MissionMetaDataManager
	{
		public static MissionMetaDataManager Instance { get; private set; }

		public UIMissionGoals MissionGoals { get; private set; }

		public static bool IsAvailable { get; private set; }

		static MissionMetaDataManager()
		{
			Instance = new MissionMetaDataManager
			{
				MissionGoals = new UIMissionGoals()
			};
			Instance.Clear(true);
		}

		public void SetMissionGoals(MissionGoals _missionGoals, bool _isElite)
		{
			MissionGoals = new UIMissionGoals(_missionGoals, _isElite);
			IsAvailable = true;
		}

		public UIMissionGoals.GoalData GetChallengeDescription(long _objectiveID)
		{
			if (MissionGoals == null || MissionGoals.Challenges == null)
			{
				return null;
			}
			foreach (UIMissionGoals.GoalData challenge in MissionGoals.Challenges)
			{
				if (challenge.ObjectiveId == _objectiveID)
				{
					return challenge;
				}
			}
			return null;
		}

		public void Clear(bool _isDebug)
		{
			if (MissionGoals != null)
			{
				MissionGoals.Clear(_isDebug);
			}
		}
	}
}
