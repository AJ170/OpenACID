using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveVoidData : ObjectiveBaseData
	{
		public ObjectiveVoidData()
		{
		}

		public ObjectiveVoidData(ObjectiveVoidContent _objectiveVoidContent)
			: base(_objectiveVoidContent.ObjectiveBaseContent)
		{
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveVoidContent objectiveVoidContent = new ObjectiveVoidContent();
			objectiveVoidContent.ObjectiveBaseContent = (ObjectiveBaseContent)base.SerializeProto();
			return objectiveVoidContent;
		}
	}
}
