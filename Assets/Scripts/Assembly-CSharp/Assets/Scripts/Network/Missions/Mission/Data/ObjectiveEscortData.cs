using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveEscortData : ObjectiveTargetData
	{
		public float EscortAreaRadius = 10f;

		public int MaxSurviveDurationSec = 15;

		public int EnemyCountCap = 6;

		public int ResumePatrolDelaySec = 3;

		public ObjectiveEscortData()
		{
		}

		public ObjectiveEscortData(ObjectiveEscortContent _objectiveEscortContent)
			: base(_objectiveEscortContent.ObjectiveTargetContent)
		{
			EscortAreaRadius = _objectiveEscortContent.EscortAreaRadius;
			MaxSurviveDurationSec = _objectiveEscortContent.MaxSurviveDurationSec;
			ResumePatrolDelaySec = _objectiveEscortContent.ResumePatrolDelaySec;
			EnemyCountCap = _objectiveEscortContent.EnemyCountCap;
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveEscortContent objectiveEscortContent = new ObjectiveEscortContent();
			objectiveEscortContent.EnemyCountCap = EnemyCountCap;
			objectiveEscortContent.EscortAreaRadius = EscortAreaRadius;
			objectiveEscortContent.MaxSurviveDurationSec = MaxSurviveDurationSec;
			objectiveEscortContent.ObjectiveTargetContent = (ObjectiveTargetContent)base.SerializeProto();
			objectiveEscortContent.ResumePatrolDelaySec = ResumePatrolDelaySec;
			return objectiveEscortContent;
		}
	}
}
