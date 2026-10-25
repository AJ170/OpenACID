using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveNotorietyData : ObjectiveBaseData
	{
		public bool FailOnTransgression = true;

		public EAlertLevel MaxNotoriety;

		public int StayBelowSec;

		public ECompare Comparer = ECompare.LessEqual;

		public bool ResetIfInvalidated;

		public ObjectiveNotorietyData()
		{
		}

		public ObjectiveNotorietyData(ObjectiveNotorietyContent _objectiveNotorietyContent)
			: base(_objectiveNotorietyContent.ObjectiveBaseContent)
		{
			ResetIfInvalidated = _objectiveNotorietyContent.ResetIfInvalidated;
			Comparer = _objectiveNotorietyContent.Comparer;
			StayBelowSec = _objectiveNotorietyContent.StayBelowSec;
			MaxNotoriety = _objectiveNotorietyContent.MaxNotoriety;
			FailOnTransgression = _objectiveNotorietyContent.FailOnTransgression;
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveNotorietyContent objectiveNotorietyContent = new ObjectiveNotorietyContent();
			objectiveNotorietyContent.Comparer = Comparer;
			objectiveNotorietyContent.FailOnTransgression = FailOnTransgression;
			objectiveNotorietyContent.MaxNotoriety = MaxNotoriety;
			objectiveNotorietyContent.ObjectiveBaseContent = (ObjectiveBaseContent)base.SerializeProto();
			objectiveNotorietyContent.ResetIfInvalidated = ResetIfInvalidated;
			objectiveNotorietyContent.StayBelowSec = StayBelowSec;
			return objectiveNotorietyContent;
		}
	}
}
