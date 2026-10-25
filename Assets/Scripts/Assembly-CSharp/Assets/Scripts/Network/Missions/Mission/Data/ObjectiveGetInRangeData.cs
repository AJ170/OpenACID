using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public abstract class ObjectiveGetInRangeData : ObjectiveTargetData
	{
		public long TargetID = -2L;

		public bool Inverse;

		public bool ResetIfInvalidated;

		public ObjectiveGetInRangeData()
		{
		}

		protected ObjectiveGetInRangeData(ObjectiveGetInRangeContent _objectiveGetInRangeContent)
			: base(_objectiveGetInRangeContent.ObjectiveTargetContent)
		{
			TargetID = _objectiveGetInRangeContent.TargetID;
			Inverse = _objectiveGetInRangeContent.Inverse;
			ResetIfInvalidated = _objectiveGetInRangeContent.ResetIfInvalidated;
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveGetInRangeContent objectiveGetInRangeContent = new ObjectiveGetInRangeContent();
			objectiveGetInRangeContent.Inverse = Inverse;
			objectiveGetInRangeContent.ObjectiveTargetContent = (ObjectiveTargetContent)base.SerializeProto();
			objectiveGetInRangeContent.TargetID = TargetID;
			objectiveGetInRangeContent.ResetIfInvalidated = ResetIfInvalidated;
			return objectiveGetInRangeContent;
		}
	}
}
