using Assets.Scripts.Database;
using Assets.Scripts.Network.Missions.Mission.Data;
using UbiCore.Utils;

public class MissionCodexEntry : MissionBase<MissionCodexEntryData>
{
	public override void OnActivate()
	{
		UbiCore.Utils.Singleton<LocalStorage>.Instance.CodexUnlockEntry(base.Data.TypeGuid);
	}
}
