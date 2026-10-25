using System.Collections.Generic;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	public abstract class ObjectiveTargetData : ObjectiveBaseData
	{
		public List<long> Targets { get; private set; }

		protected ObjectiveTargetData()
		{
			Targets = new List<long>();
		}

		protected ObjectiveTargetData(ObjectiveTargetContent _objectiveTargetContent)
			: base(_objectiveTargetContent.ObjectiveBaseContent)
		{
			Targets = new List<long>(_objectiveTargetContent.Targets);
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveTargetContent objectiveTargetContent = new ObjectiveTargetContent();
			objectiveTargetContent.ObjectiveBaseContent = (ObjectiveBaseContent)base.SerializeProto();
			ObjectiveTargetContent objectiveTargetContent2 = objectiveTargetContent;
			objectiveTargetContent2.Targets = objectiveTargetContent2.Targets.AddRange(Targets);
			return objectiveTargetContent2;
		}
	}
}
