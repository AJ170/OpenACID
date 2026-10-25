using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class MissionCodexEntryData : MissionBaseData
	{
		public string TypeGuid { get; set; }

		public MissionCodexEntryData()
		{
		}

		public MissionCodexEntryData(MissionCodexEntryContent _missionCodexEntryContent)
			: base(_missionCodexEntryContent.MissionBaseContent)
		{
			TypeGuid = _missionCodexEntryContent.TypeGuid;
		}

		public override IExtensible SerializeProto()
		{
			MissionCodexEntryContent missionCodexEntryContent = new MissionCodexEntryContent();
			missionCodexEntryContent.MissionBaseContent = (MissionBaseContent)base.SerializeProto();
			missionCodexEntryContent.TypeGuid = TypeGuid;
			return missionCodexEntryContent;
		}
	}
}
