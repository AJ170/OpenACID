using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class AreaNodeData : MissionBaseData
	{
		public AreaNodeData()
		{
		}

		public AreaNodeData(MissionAreaNodeContent _missionAreaNodeContent)
			: base(_missionAreaNodeContent.MissionBaseContent)
		{
		}

		public override IExtensible SerializeProto()
		{
			MissionAreaNodeContent missionAreaNodeContent = new MissionAreaNodeContent();
			missionAreaNodeContent.MissionBaseContent = (MissionBaseContent)base.SerializeProto();
			return missionAreaNodeContent;
		}
	}
}
