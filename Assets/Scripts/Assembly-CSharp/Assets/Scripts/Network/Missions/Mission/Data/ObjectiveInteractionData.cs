using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveInteractionData : ObjectiveTargetData
	{
		public ObjectiveInteractionData()
		{
		}

		public ObjectiveInteractionData(ObjectiveInteractionContent _objectiveInteractionContent)
			: base(_objectiveInteractionContent.ObjectiveTargetContent)
		{
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveInteractionContent objectiveInteractionContent = new ObjectiveInteractionContent();
			objectiveInteractionContent.ObjectiveTargetContent = (ObjectiveTargetContent)base.SerializeProto();
			return objectiveInteractionContent;
		}
	}
}
