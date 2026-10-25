using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class MissionSpawnObjectData : MissionSpawnEntityData
	{
		public string TypeGuid;

		public MissionSpawnObjectData()
		{
		}

		public MissionSpawnObjectData(MissionSpawnObjectContent _missionSpawnObjectContent)
			: base(_missionSpawnObjectContent.MissionSpawnEntityContent)
		{
			TypeGuid = _missionSpawnObjectContent.TypeGuid;
		}

		public override IExtensible SerializeProto()
		{
			MissionSpawnObjectContent missionSpawnObjectContent = new MissionSpawnObjectContent();
			missionSpawnObjectContent.MissionSpawnEntityContent = (MissionSpawnEntityContent)base.SerializeProto();
			missionSpawnObjectContent.TypeGuid = TypeGuid;
			return missionSpawnObjectContent;
		}
	}
}
