using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveSkillLimitData : ObjectiveBaseData
	{
		public bool Invert;

		public int Limit;

		public string SkillName { get; set; }

		public ObjectiveSkillLimitData()
		{
		}

		public ObjectiveSkillLimitData(ObjectiveSkillLimitContent _objectiveSkillLimitContent)
			: base(_objectiveSkillLimitContent.ObjectiveBaseContent)
		{
			Invert = _objectiveSkillLimitContent.Invert;
			Limit = _objectiveSkillLimitContent.Limit;
			SkillName = _objectiveSkillLimitContent.SkillName;
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveSkillLimitContent objectiveSkillLimitContent = new ObjectiveSkillLimitContent();
			objectiveSkillLimitContent.Invert = Invert;
			objectiveSkillLimitContent.Limit = Limit;
			objectiveSkillLimitContent.ObjectiveBaseContent = (ObjectiveBaseContent)base.SerializeProto();
			objectiveSkillLimitContent.SkillName = SkillName;
			return objectiveSkillLimitContent;
		}
	}
}
