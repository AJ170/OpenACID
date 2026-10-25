using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveTimeLimitData : ObjectiveBaseData
	{
		public bool Invert;

		public bool WaitForTimerEnd;

		public int TimeInSeconds = 300;

		public int TimeForRetry = 60;

		public ObjectiveTimeLimitData()
		{
		}

		public ObjectiveTimeLimitData(ObjectiveTimeLimitContent _objectiveTimeLimitContent)
			: base(_objectiveTimeLimitContent.ObjectiveBaseContent)
		{
			Invert = _objectiveTimeLimitContent.Invert;
			WaitForTimerEnd = _objectiveTimeLimitContent.WaitForTimerEnd;
			TimeInSeconds = _objectiveTimeLimitContent.TimeInSeconds;
			TimeForRetry = _objectiveTimeLimitContent.TimeForRetry;
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveTimeLimitContent objectiveTimeLimitContent = new ObjectiveTimeLimitContent();
			objectiveTimeLimitContent.Invert = Invert;
			objectiveTimeLimitContent.ObjectiveBaseContent = (ObjectiveBaseContent)base.SerializeProto();
			objectiveTimeLimitContent.TimeForRetry = TimeForRetry;
			objectiveTimeLimitContent.TimeInSeconds = TimeInSeconds;
			objectiveTimeLimitContent.WaitForTimerEnd = WaitForTimerEnd;
			return objectiveTimeLimitContent;
		}
	}
}
