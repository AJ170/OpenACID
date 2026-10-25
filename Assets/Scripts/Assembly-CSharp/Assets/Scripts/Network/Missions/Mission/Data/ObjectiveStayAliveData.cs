using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	public class ObjectiveStayAliveData : ObjectiveBaseData
	{
		public ObjectiveStayAliveData()
		{
		}

		public ObjectiveStayAliveData(ObjectiveStayAliveContent _objectiveStayAliveContent)
			: base(_objectiveStayAliveContent.ObjectiveBaseContent)
		{
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveStayAliveContent objectiveStayAliveContent = new ObjectiveStayAliveContent();
			objectiveStayAliveContent.ObjectiveBaseContent = (ObjectiveBaseContent)base.SerializeProto();
			return objectiveStayAliveContent;
		}
	}
}
