using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveFreeCaptivesData : ObjectiveBaseData
	{
		public int MaximumCasualties;

		public ObjectiveFreeCaptivesData()
		{
		}

		public ObjectiveFreeCaptivesData(ObjectiveFreeCaptivesContent _objectiveFreeCaptivesContent)
			: base(_objectiveFreeCaptivesContent.ObjectiveBaseContent)
		{
			MaximumCasualties = _objectiveFreeCaptivesContent.MaximumCasualties;
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveFreeCaptivesContent objectiveFreeCaptivesContent = new ObjectiveFreeCaptivesContent();
			objectiveFreeCaptivesContent.ObjectiveBaseContent = (ObjectiveBaseContent)base.SerializeProto();
			objectiveFreeCaptivesContent.MaximumCasualties = MaximumCasualties;
			return objectiveFreeCaptivesContent;
		}
	}
}
