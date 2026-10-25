using System;
using AcierProtocol;
using Assets.Scripts.Mission.Mission;
using Assets.Scripts.UI.LoadoutScreen;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveLootData : ObjectiveTargetData
	{
		public bool IdentityActivatesMapIcon;

		public ObjectiveLootData()
		{
		}

		public ObjectiveLootData(ObjectiveLootContent _objectiveLootContent)
			: base(_objectiveLootContent.ObjectiveTargetContent)
		{
			IdentityActivatesMapIcon = _objectiveLootContent.IdentityActivatesMapIcon;
			UIMissionGoals.GoalData challengeDescription = MissionMetaDataManager.Instance.GetChallengeDescription(ID);
			if (challengeDescription != null)
			{
				bool forced = challengeDescription.Forced;
				bool hidden = challengeDescription.Hidden;
				if (forced && hidden)
				{
					ClampMapIconOnBorder = false;
				}
			}
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveLootContent objectiveLootContent = new ObjectiveLootContent();
			objectiveLootContent.ObjectiveTargetContent = (ObjectiveTargetContent)base.SerializeProto();
			objectiveLootContent.IdentityActivatesMapIcon = IdentityActivatesMapIcon;
			return objectiveLootContent;
		}
	}
}
