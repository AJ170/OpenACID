using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveGetInRangeMobileData : ObjectiveGetInRangeData
	{
		public float Range;

		public ObjectiveGetInRangeMobileData()
		{
		}

		public ObjectiveGetInRangeMobileData(ObjectiveGetInRangeMobileContent _objectiveGetInRangeMobileContent)
			: base(_objectiveGetInRangeMobileContent.ObjectiveGetInRangeContent)
		{
			Range = _objectiveGetInRangeMobileContent.Range;
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveGetInRangeMobileContent objectiveGetInRangeMobileContent = new ObjectiveGetInRangeMobileContent();
			objectiveGetInRangeMobileContent.ObjectiveGetInRangeContent = (ObjectiveGetInRangeContent)base.SerializeProto();
			objectiveGetInRangeMobileContent.Range = Range;
			return objectiveGetInRangeMobileContent;
		}
	}
}
