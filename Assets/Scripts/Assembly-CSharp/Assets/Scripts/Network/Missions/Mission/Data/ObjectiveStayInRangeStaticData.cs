using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveStayInRangeStaticData : ObjectiveStayInRangeData
	{
		public ObjectiveStayInRangeStaticData()
		{
		}

		public ObjectiveStayInRangeStaticData(ObjectiveStayInRangeStaticContent _objectiveStayInRangeStaticContent)
			: base(_objectiveStayInRangeStaticContent.ObjectiveStayInRangeContent)
		{
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveStayInRangeStaticContent objectiveStayInRangeStaticContent = new ObjectiveStayInRangeStaticContent();
			objectiveStayInRangeStaticContent.ObjectiveStayInRangeContent = (ObjectiveStayInRangeContent)base.SerializeProto();
			return objectiveStayInRangeStaticContent;
		}
	}
}
