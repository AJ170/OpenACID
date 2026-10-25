using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class MissionSpawnPointData : MissionSpawnEntityData
	{
		public int SpawnCount;

		public int SpawnDelaySec;

		public MissionSpawnPointData()
		{
		}

		public MissionSpawnPointData(MissionSpawnPointContent _missionSpawnPointContent)
			: base(_missionSpawnPointContent.MissionSpawnEntityContent)
		{
			SpawnCount = _missionSpawnPointContent.SpawnCount;
			SpawnDelaySec = _missionSpawnPointContent.SpawnDelaySec;
		}

		public override IExtensible SerializeProto()
		{
			MissionSpawnPointContent missionSpawnPointContent = new MissionSpawnPointContent();
			missionSpawnPointContent.MissionSpawnEntityContent = (MissionSpawnEntityContent)base.SerializeProto();
			missionSpawnPointContent.SpawnCount = SpawnCount;
			missionSpawnPointContent.SpawnDelaySec = SpawnDelaySec;
			return missionSpawnPointContent;
		}
	}
}
