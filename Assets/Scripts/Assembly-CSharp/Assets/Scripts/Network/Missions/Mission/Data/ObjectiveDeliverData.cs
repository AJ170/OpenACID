using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveDeliverData : ObjectiveTargetData
	{
		public ObjectiveDeliverData()
		{
		}

		public ObjectiveDeliverData(ObjectiveDeliverContent _objectiveDeliverContent)
			: base(_objectiveDeliverContent.ObjectiveTargetContent)
		{
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveDeliverContent objectiveDeliverContent = new ObjectiveDeliverContent();
			objectiveDeliverContent.ObjectiveTargetContent = (ObjectiveTargetContent)base.SerializeProto();
			return objectiveDeliverContent;
		}
	}
}
