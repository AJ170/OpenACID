using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveStayInRangeMobileData : ObjectiveStayInRangeData
	{
		public long ToTargetID;

		public float Range;

		public ObjectiveStayInRangeMobileData()
		{
			ToTargetID = -2L;
		}

		public ObjectiveStayInRangeMobileData(ObjectiveStayInRangeMobileContent _objectiveStayInRangeMobileContent)
			: base(_objectiveStayInRangeMobileContent.ObjectiveStayInRangeContent)
		{
			Range = _objectiveStayInRangeMobileContent.Range;
			ToTargetID = _objectiveStayInRangeMobileContent.ToTargetID;
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveStayInRangeMobileContent objectiveStayInRangeMobileContent = new ObjectiveStayInRangeMobileContent();
			objectiveStayInRangeMobileContent.ObjectiveStayInRangeContent = (ObjectiveStayInRangeContent)base.SerializeProto();
			objectiveStayInRangeMobileContent.Range = Range;
			objectiveStayInRangeMobileContent.ToTargetID = ToTargetID;
			return objectiveStayInRangeMobileContent;
		}
	}
}
