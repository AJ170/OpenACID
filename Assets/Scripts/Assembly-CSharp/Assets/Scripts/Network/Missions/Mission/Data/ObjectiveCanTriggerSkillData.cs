using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveCanTriggerSkillData : ObjectiveBaseData
	{
		public string SkillName { get; set; }

		public ObjectiveCanTriggerSkillData()
		{
		}

		public ObjectiveCanTriggerSkillData(ObjectiveCanTriggerSkillContent _objectiveCanTriggerSkillContent)
			: base(_objectiveCanTriggerSkillContent.ObjectiveBaseContent)
		{
			SkillName = _objectiveCanTriggerSkillContent.SkillName;
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveCanTriggerSkillContent objectiveCanTriggerSkillContent = new ObjectiveCanTriggerSkillContent();
			objectiveCanTriggerSkillContent.ObjectiveBaseContent = (ObjectiveBaseContent)base.SerializeProto();
			objectiveCanTriggerSkillContent.SkillName = SkillName;
			return objectiveCanTriggerSkillContent;
		}
	}
}
