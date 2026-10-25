using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	public abstract class MissionSpawnEntityData : MissionBaseData
	{
		public bool IsMimic;

		public EFadeMode FadeMode;

		public float FadeDuration = 1.5f;

		protected MissionSpawnEntityData()
		{
		}

		protected MissionSpawnEntityData(MissionSpawnEntityContent _missionSpawnEntityContent)
			: base(_missionSpawnEntityContent.MissionBaseContent)
		{
			IsMimic = _missionSpawnEntityContent.IsMimic;
			FadeMode = _missionSpawnEntityContent.FadeMode;
			FadeDuration = _missionSpawnEntityContent.FadeDuration;
		}

		public override IExtensible SerializeProto()
		{
			MissionSpawnEntityContent missionSpawnEntityContent = new MissionSpawnEntityContent();
			missionSpawnEntityContent.IsMimic = IsMimic;
			missionSpawnEntityContent.FadeMode = FadeMode;
			missionSpawnEntityContent.FadeDuration = FadeDuration;
			missionSpawnEntityContent.MissionBaseContent = (MissionBaseContent)base.SerializeProto();
			return missionSpawnEntityContent;
		}
	}
}
