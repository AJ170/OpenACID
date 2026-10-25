using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class MissionNpcFormationData : MissionFormationBaseData
	{
		public MissionNpcFormationData()
		{
		}

		public MissionNpcFormationData(MissionNpcFormationContent _missionNpcFormationContent)
			: base(_missionNpcFormationContent.MissionFormationBaseContent)
		{
		}

		public override IExtensible SerializeProto()
		{
			MissionNpcFormationContent missionNpcFormationContent = new MissionNpcFormationContent();
			missionNpcFormationContent.MissionFormationBaseContent = (MissionFormationBaseContent)base.SerializeProto();
			return missionNpcFormationContent;
		}
	}
}
