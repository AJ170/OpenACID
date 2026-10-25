using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	public abstract class MissionAreaBaseData : MissionBaseData
	{
		public bool DoNotRenderOnMiniMap;

		public EMissionAreaType Type;

		protected MissionAreaBaseData()
		{
		}

		protected MissionAreaBaseData(MissionAreaBaseContent _missionAreaBaseContent)
			: base(_missionAreaBaseContent.MissionBaseContent)
		{
			DoNotRenderOnMiniMap = _missionAreaBaseContent.DoNotRenderOnMiniMap;
			Type = _missionAreaBaseContent.Type;
		}

		public override IExtensible SerializeProto()
		{
			MissionAreaBaseContent missionAreaBaseContent = new MissionAreaBaseContent();
			missionAreaBaseContent.DoNotRenderOnMiniMap = DoNotRenderOnMiniMap;
			missionAreaBaseContent.MissionBaseContent = (MissionBaseContent)base.SerializeProto();
			missionAreaBaseContent.Type = Type;
			return missionAreaBaseContent;
		}
	}
}
