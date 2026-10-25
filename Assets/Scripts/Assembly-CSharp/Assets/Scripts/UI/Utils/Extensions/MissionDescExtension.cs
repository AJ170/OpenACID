using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Localization;
using Database;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class MissionDescExtension
	{
		public static string LocaName(this MissionRootData _missionDesc, RegionDefinition _def = null)
		{
			if (_def != null && (_missionDesc.ObjectiveType != EObjectiveType.Story_EObjectiveType || _missionDesc.ObjectiveType != EObjectiveType.Tutorial_EObjectiveType))
			{
				return LocalizationHelper.ParseStringToLocaKey(_missionDesc.Name, LocalizationHelper.GetStringMapperForGeneratedMissions(_def));
			}
			return LocalizationHelper.ParseStringToLocaKey(_missionDesc.Name);
		}

		public static string LocaBriefing(this MissionRootData _missionDesc)
		{
			return LocalizationHelper.ParseStringToLocaKey(_missionDesc.BriefingText);
		}

		public static string LocaDebriefing(this MissionRootData _missionDesc)
		{
			return LocalizationHelper.ParseStringToLocaKey(_missionDesc.DebriefingText);
		}

		public static RegionDefinition GetRegionDefinition(this MissionRootData _missionDesc)
		{
			List<KeyValuePair<GameID, DBEntry>> items = Main.Instance.GameDB.GetItems<RegionDefinition>();
			for (int i = 0; i < items.Count; i++)
			{
				RegionDefinition regionDefinition = items[i].Value as RegionDefinition;
				if (regionDefinition.Region == _missionDesc.Region && regionDefinition.Age == _missionDesc.Age && regionDefinition.Mood == _missionDesc.Mood && !regionDefinition.Globe)
				{
					return regionDefinition;
				}
			}
			return null;
		}
	}
}
