using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveDeliverNotorietyData : ObjectiveTargetData
	{
		public bool FleeOnTransgression = true;

		public long FleeTargetID;

		public EAlertLevel MaxNotoriety = EAlertLevel.Alerted;

		public ObjectiveDeliverNotorietyData()
		{
		}

		public ObjectiveDeliverNotorietyData(ObjectiveDeliverNotorietyContent _objectiveDeliverNotorietyContent)
			: base(_objectiveDeliverNotorietyContent.ObjectiveTargetContent)
		{
			FleeOnTransgression = _objectiveDeliverNotorietyContent.FleeOnTransgression;
			FleeTargetID = _objectiveDeliverNotorietyContent.FleeTargetID;
			MaxNotoriety = _objectiveDeliverNotorietyContent.MaxNotoriety;
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveDeliverNotorietyContent objectiveDeliverNotorietyContent = new ObjectiveDeliverNotorietyContent();
			objectiveDeliverNotorietyContent.FleeOnTransgression = FleeOnTransgression;
			objectiveDeliverNotorietyContent.FleeTargetID = FleeTargetID;
			objectiveDeliverNotorietyContent.MaxNotoriety = MaxNotoriety;
			objectiveDeliverNotorietyContent.ObjectiveTargetContent = (ObjectiveTargetContent)base.SerializeProto();
			return objectiveDeliverNotorietyContent;
		}
	}
}
