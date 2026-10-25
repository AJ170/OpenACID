using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	public class MissionPlayerSpawnData : MissionBaseData
	{
		public MissionPlayerSpawnData()
		{
		}

		public MissionPlayerSpawnData(MissionPlayerSpawnContent _missionPlayerSpawnContent)
			: base(_missionPlayerSpawnContent.MissionBaseContent)
		{
		}

		public override IExtensible SerializeProto()
		{
			MissionPlayerSpawnContent missionPlayerSpawnContent = new MissionPlayerSpawnContent();
			missionPlayerSpawnContent.MissionBaseContent = (MissionBaseContent)base.SerializeProto();
			return missionPlayerSpawnContent;
		}
	}
}
