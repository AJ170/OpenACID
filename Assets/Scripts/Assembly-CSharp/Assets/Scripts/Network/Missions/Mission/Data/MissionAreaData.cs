using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class MissionAreaData : MissionAreaBaseData
	{
		public EAreaSize AreaSize = EAreaSize.Medium;

		public MissionAreaData()
		{
		}

		public MissionAreaData(MissionAreaContent _missionAreaContent)
			: base(_missionAreaContent.MissionAreaBaseContent)
		{
			AreaSize = _missionAreaContent.AreaSize;
		}

		public override IExtensible SerializeProto()
		{
			MissionAreaContent missionAreaContent = new MissionAreaContent();
			missionAreaContent.AreaSize = AreaSize;
			missionAreaContent.MissionAreaBaseContent = (MissionAreaBaseContent)base.SerializeProto();
			return missionAreaContent;
		}
	}
}
