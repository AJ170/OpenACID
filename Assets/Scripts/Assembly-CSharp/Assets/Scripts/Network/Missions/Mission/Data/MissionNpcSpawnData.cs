using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class MissionNpcSpawnData : MissionSpawnEntityData
	{
		public string TypeGuid;

		public float ScalingMultiplier = 1f;

		public MissionNpcSpawnData()
		{
		}

		public MissionNpcSpawnData(MissionNpcSpawnContent _missionNpcSpawnContent)
			: base(_missionNpcSpawnContent.MissionSpawnEntityContent)
		{
			TypeGuid = _missionNpcSpawnContent.TypeGuid;
			ScalingMultiplier = _missionNpcSpawnContent.ScalingMultiplier;
		}

		public override IExtensible SerializeProto()
		{
			MissionNpcSpawnContent missionNpcSpawnContent = new MissionNpcSpawnContent();
			missionNpcSpawnContent.MissionSpawnEntityContent = (MissionSpawnEntityContent)base.SerializeProto();
			missionNpcSpawnContent.TypeGuid = TypeGuid;
			missionNpcSpawnContent.ScalingMultiplier = ScalingMultiplier;
			return missionNpcSpawnContent;
		}
	}
}
