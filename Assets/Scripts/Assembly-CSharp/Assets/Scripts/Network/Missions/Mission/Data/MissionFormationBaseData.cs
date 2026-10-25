using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	public abstract class MissionFormationBaseData : MissionBaseData
	{
		public EFormationIterationDirection IterationDirection = EFormationIterationDirection.Up;

		public EFormationIterationType IterationType;

		public EPatrolType Type;

		public bool FixedLeaderSlot;

		public float Radius;

		protected MissionFormationBaseData()
		{
		}

		protected MissionFormationBaseData(MissionFormationBaseContent _missionFormationBaseContent)
			: base(_missionFormationBaseContent.MissionBaseContent)
		{
			IterationDirection = _missionFormationBaseContent.IterationDirection;
			IterationType = _missionFormationBaseContent.IterationType;
			Type = _missionFormationBaseContent.Type;
			FixedLeaderSlot = _missionFormationBaseContent.FixedLeaderSlot;
			Radius = _missionFormationBaseContent.Radius;
		}

		public override IExtensible SerializeProto()
		{
			MissionFormationBaseContent missionFormationBaseContent = new MissionFormationBaseContent();
			missionFormationBaseContent.FixedLeaderSlot = FixedLeaderSlot;
			missionFormationBaseContent.IterationDirection = IterationDirection;
			missionFormationBaseContent.IterationType = IterationType;
			missionFormationBaseContent.MissionBaseContent = (MissionBaseContent)base.SerializeProto();
			missionFormationBaseContent.Type = Type;
			missionFormationBaseContent.Radius = Radius;
			return missionFormationBaseContent;
		}
	}
}
