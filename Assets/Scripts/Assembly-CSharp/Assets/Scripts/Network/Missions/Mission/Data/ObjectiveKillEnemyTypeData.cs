using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveKillEnemyTypeData : ObjectiveBaseData
	{
		public bool ForbidToKill;

		public string TypeGuid;

		public int KillAmount;

		public ObjectiveKillEnemyTypeData()
		{
		}

		public ObjectiveKillEnemyTypeData(ObjectiveKillEnemyTypeContent _objectiveKillEnemyTypeContent)
			: base(_objectiveKillEnemyTypeContent.ObjectiveBaseContent)
		{
			ForbidToKill = _objectiveKillEnemyTypeContent.ForbidToKill;
			TypeGuid = _objectiveKillEnemyTypeContent.TypeGuid;
			KillAmount = _objectiveKillEnemyTypeContent.KillAmount;
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveKillEnemyTypeContent objectiveKillEnemyTypeContent = new ObjectiveKillEnemyTypeContent();
			objectiveKillEnemyTypeContent.ForbidToKill = ForbidToKill;
			objectiveKillEnemyTypeContent.KillAmount = KillAmount;
			objectiveKillEnemyTypeContent.ObjectiveBaseContent = (ObjectiveBaseContent)base.SerializeProto();
			objectiveKillEnemyTypeContent.TypeGuid = TypeGuid;
			return objectiveKillEnemyTypeContent;
		}
	}
}
