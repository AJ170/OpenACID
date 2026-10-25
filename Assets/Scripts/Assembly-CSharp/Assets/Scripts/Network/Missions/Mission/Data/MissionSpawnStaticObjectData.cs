using System;
using AcierProtocol;
using Assets.Scripts.Network.Missions.Math;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class MissionSpawnStaticObjectData : MissionSpawnEntityData
	{
		public string TypeGuid;

		public Point3D Scale { get; set; }

		public MissionSpawnStaticObjectData()
		{
		}

		public MissionSpawnStaticObjectData(MissionSpawnStaticObjectContent _missionSpawnStaticObjectData)
			: base(_missionSpawnStaticObjectData.MissionSpawnEntityContent)
		{
			Scale = new Point3D(_missionSpawnStaticObjectData.ScaleX, _missionSpawnStaticObjectData.ScaleY, _missionSpawnStaticObjectData.ScaleZ);
			TypeGuid = _missionSpawnStaticObjectData.TypeGuid;
		}

		public override IExtensible SerializeProto()
		{
			MissionSpawnStaticObjectContent missionSpawnStaticObjectContent = new MissionSpawnStaticObjectContent();
			missionSpawnStaticObjectContent.MissionSpawnEntityContent = (MissionSpawnEntityContent)base.SerializeProto();
			missionSpawnStaticObjectContent.ScaleX = Scale.x;
			missionSpawnStaticObjectContent.ScaleY = Scale.y;
			missionSpawnStaticObjectContent.ScaleZ = Scale.z;
			missionSpawnStaticObjectContent.TypeGuid = TypeGuid;
			return missionSpawnStaticObjectContent;
		}
	}
}
