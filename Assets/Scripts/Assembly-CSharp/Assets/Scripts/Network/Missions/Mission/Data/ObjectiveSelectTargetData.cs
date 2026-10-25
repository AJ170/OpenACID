using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveSelectTargetData : ObjectiveTargetData
	{
		public bool ForceSelection;

		public bool FailOnUnselect;

		public ObjectiveSelectTargetData()
		{
		}

		public ObjectiveSelectTargetData(ObjectiveSelectTargetContent _objectiveSelectTargetContent)
			: base(_objectiveSelectTargetContent.ObjectiveTargetContent)
		{
			ForceSelection = _objectiveSelectTargetContent.ForceSelection;
			FailOnUnselect = _objectiveSelectTargetContent.FailOnUnselect;
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveSelectTargetContent objectiveSelectTargetContent = new ObjectiveSelectTargetContent();
			objectiveSelectTargetContent.ObjectiveTargetContent = (ObjectiveTargetContent)base.SerializeProto();
			objectiveSelectTargetContent.FailOnUnselect = FailOnUnselect;
			objectiveSelectTargetContent.ForceSelection = ForceSelection;
			return objectiveSelectTargetContent;
		}
	}
}
