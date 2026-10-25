using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveEscapeData : ObjectiveTargetData
	{
		public float Distance = 10f;

		public bool HideOutside;

		public ObjectiveEscapeData()
		{
		}

		public ObjectiveEscapeData(ObjectiveEscapeContent _objectiveEscapeContent)
			: base(_objectiveEscapeContent.ObjectiveTargetContent)
		{
			Distance = _objectiveEscapeContent.Distance;
			HideOutside = _objectiveEscapeContent.HideOutside;
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveEscapeContent objectiveEscapeContent = new ObjectiveEscapeContent();
			objectiveEscapeContent.Distance = Distance;
			objectiveEscapeContent.HideOutside = HideOutside;
			objectiveEscapeContent.ObjectiveTargetContent = (ObjectiveTargetContent)base.SerializeProto();
			return objectiveEscapeContent;
		}
	}
}
