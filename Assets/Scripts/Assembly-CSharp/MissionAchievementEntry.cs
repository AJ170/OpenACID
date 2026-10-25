using Assets.Scripts.Network.Missions.Mission.Data;

public class MissionAchievementEntry : MissionBase<MissionAchievementEntryData>
{
	public override void OnActivate()
	{
		LogicMain.MissionResult.OnMissionNodeActivated(this);
	}
}
