using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public class MissionGameLogicModData : MissionBaseData
	{
		[Serializable]
		public class MissionCombatSettings
		{
			public MissionGameLogicModContent.EBool KillTokenKill = MissionGameLogicModContent.EBool.Null;

			public MissionCombatSettings()
			{
			}

			public MissionCombatSettings(MissionGameLogicModContent.MissionCombatSettings _missionCombatSettings)
			{
				KillTokenKill = _missionCombatSettings.KillTokenKill;
			}

			public MissionGameLogicModContent.MissionCombatSettings Serialize()
			{
				MissionGameLogicModContent.MissionCombatSettings missionCombatSettings = new MissionGameLogicModContent.MissionCombatSettings();
				missionCombatSettings.KillTokenKill = KillTokenKill;
				return missionCombatSettings;
			}
		}

		[Serializable]
		public class MissionHealthSettings
		{
			public MissionGameLogicModContent.EBool SetHealthPercentage = MissionGameLogicModContent.EBool.Null;

			public int Percentage = 50;

			public MissionHealthSettings()
			{
			}

			public MissionHealthSettings(MissionGameLogicModContent.MissionHealthSettings _missionHealthSettings)
			{
				SetHealthPercentage = _missionHealthSettings.SetHealthPercentage;
				Percentage = _missionHealthSettings.Percentage;
			}

			public MissionGameLogicModContent.MissionHealthSettings Serialize()
			{
				MissionGameLogicModContent.MissionHealthSettings missionHealthSettings = new MissionGameLogicModContent.MissionHealthSettings();
				missionHealthSettings.SetHealthPercentage = SetHealthPercentage;
				missionHealthSettings.Percentage = Percentage;
				return missionHealthSettings;
			}
		}

		public MissionCombatSettings Combat = new MissionCombatSettings();

		public MissionHealthSettings Health = new MissionHealthSettings();

		public MissionGameLogicModData()
		{
		}

		public MissionGameLogicModData(MissionGameLogicModContent _content)
			: base(_content.MissionBaseContent)
		{
			if (_content.Combat != null)
			{
				Combat = new MissionCombatSettings(_content.Combat);
			}
			if (_content.Health != null)
			{
				Health = new MissionHealthSettings(_content.Health);
			}
		}

		public override IExtensible SerializeProto()
		{
			MissionGameLogicModContent missionGameLogicModContent = new MissionGameLogicModContent();
			missionGameLogicModContent.MissionBaseContent = (MissionBaseContent)base.SerializeProto();
			missionGameLogicModContent.Combat = Combat.Serialize();
			missionGameLogicModContent.Health = Health.Serialize();
			return missionGameLogicModContent;
		}
	}
}
