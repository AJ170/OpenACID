using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class MissionAreaMovingData : MissionAreaBaseData
	{
		public float Radius = 20f;

		public long Target;

		public MissionAreaMovingData()
		{
		}

		public MissionAreaMovingData(MissionAreaMovingContent _missionAreaMovingContent)
			: base(_missionAreaMovingContent.MissionAreaBaseContent)
		{
			Radius = _missionAreaMovingContent.Radius;
			Target = _missionAreaMovingContent.Target;
		}

		public override IExtensible SerializeProto()
		{
			MissionAreaMovingContent missionAreaMovingContent = new MissionAreaMovingContent();
			missionAreaMovingContent.MissionAreaBaseContent = (MissionAreaBaseContent)base.SerializeProto();
			missionAreaMovingContent.Radius = Radius;
			missionAreaMovingContent.Target = Target;
			return missionAreaMovingContent;
		}
	}
}
