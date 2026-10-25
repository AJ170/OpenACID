using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveBecomeAnonymousData : ObjectiveBaseData
	{
		public ObjectiveBecomeAnonymousData()
		{
		}

		public ObjectiveBecomeAnonymousData(ObjectiveBecomeAnonymousContent _objectiveBecomeAnonymousContent)
			: base(_objectiveBecomeAnonymousContent.ObjectiveBaseContent)
		{
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveBecomeAnonymousContent objectiveBecomeAnonymousContent = new ObjectiveBecomeAnonymousContent();
			objectiveBecomeAnonymousContent.ObjectiveBaseContent = (ObjectiveBaseContent)base.SerializeProto();
			return objectiveBecomeAnonymousContent;
		}
	}
}
