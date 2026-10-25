using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class MissionAchievementEntryData : MissionBaseData
	{
		public string Achievement { get; set; }

		public MissionAchievementEntryData()
		{
		}

		public MissionAchievementEntryData(MissionAchievementEntryContent _missionAchievementEntryContent)
			: base(_missionAchievementEntryContent.MissionBaseContent)
		{
			Achievement = _missionAchievementEntryContent.Achievement;
		}

		public override IExtensible SerializeProto()
		{
			MissionAchievementEntryContent missionAchievementEntryContent = new MissionAchievementEntryContent();
			missionAchievementEntryContent.Achievement = Achievement;
			missionAchievementEntryContent.MissionBaseContent = (MissionBaseContent)base.SerializeProto();
			return missionAchievementEntryContent;
		}
	}
}
