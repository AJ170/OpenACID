using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveKillData : ObjectiveTargetData
	{
		public bool DoNotKill;

		public int Minimum;

		public ObjectiveKillData()
		{
		}

		public ObjectiveKillData(ObjectiveKillContent _objectiveKillContent)
			: base(_objectiveKillContent.ObjectiveTargetContent)
		{
			DoNotKill = _objectiveKillContent.DoNotKill;
			Minimum = _objectiveKillContent.Minimum;
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveKillContent objectiveKillContent = new ObjectiveKillContent();
			objectiveKillContent.DoNotKill = DoNotKill;
			objectiveKillContent.Minimum = Minimum;
			objectiveKillContent.ObjectiveTargetContent = (ObjectiveTargetContent)base.SerializeProto();
			return objectiveKillContent;
		}
	}
}
