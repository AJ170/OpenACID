using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public abstract class ObjectiveStayInRangeData : ObjectiveBaseData
	{
		public long TargetID = -2L;

		public int TimerSeconds = 10;

		public int StayTimerSeconds = -1;

		public bool MaintainLineOfSight;

		protected ObjectiveStayInRangeData()
		{
		}

		protected ObjectiveStayInRangeData(ObjectiveStayInRangeContent _data)
			: base(_data.ObjectiveBaseContent)
		{
			TargetID = _data.TargetID;
			TimerSeconds = _data.TimerSeconds;
			StayTimerSeconds = _data.StayTimerSeconds;
			MaintainLineOfSight = _data.MaintainLineOfSight;
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveStayInRangeContent objectiveStayInRangeContent = new ObjectiveStayInRangeContent();
			objectiveStayInRangeContent.MaintainLineOfSight = MaintainLineOfSight;
			objectiveStayInRangeContent.ObjectiveBaseContent = (ObjectiveBaseContent)base.SerializeProto();
			objectiveStayInRangeContent.StayTimerSeconds = StayTimerSeconds;
			objectiveStayInRangeContent.TargetID = TargetID;
			objectiveStayInRangeContent.TimerSeconds = TimerSeconds;
			return objectiveStayInRangeContent;
		}
	}
}
