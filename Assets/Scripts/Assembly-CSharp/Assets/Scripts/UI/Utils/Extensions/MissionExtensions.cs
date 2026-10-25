using System.Collections.Generic;
using System.Text.RegularExpressions;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.UI.Localization;
using Database;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class MissionExtensions
	{
		private static readonly StringId s_GeneratedMissionName = 2225;

		public static readonly Regex s_placeholderRemoverRegex = new Regex("(?:(?:(?: | - )?\\{[0-9]+\\} ?[\\/] ?\\{[0-9]+\\})|(?:\\{[0-9]+\\}))(?:\\(s\\))?", RegexOptions.CultureInvariant);

		public static string LocaMissionName(this AcierProtocol.Mission _mission, RegionDefinition _def)
		{
			StringId key = ((_mission.type != ERuntimeMissionType.RandomMission) ? ((StringId)_mission.objectiveLangKey.neutral) : s_GeneratedMissionName);
			return LocalizationManager.Instance.Get(key, _mission.GetStringMapperForMissions(_def));
		}

		public static string LocaObjectiveLang(this AcierProtocol.Mission _mission, RegionDefinition _def)
		{
			if (_mission.type == ERuntimeMissionType.RandomMission)
			{
				return LocalizationManager.Instance.Get(_mission.objectiveLangKey.neutral, _mission.GetStringMapperForMissions(_def));
			}
			return LocalizationHelper.ParseStringToLocaKey(_mission.objectiveType, _mission.GetStringMapperForMissions(_def));
		}

		public static string LocaMissionDescription(this AcierProtocol.Mission _mission, RegionDefinition _def, bool _removePlaceHolders = false)
		{
			string text = LocalizationManager.Instance.Get(_mission.missionDescription.neutral, _mission.GetStringMapperForMissions(_def));
			if (text == null || !_removePlaceHolders)
			{
				return text;
			}
			return s_placeholderRemoverRegex.Replace(text, string.Empty);
		}

		public static string LocaMissionFlavorText(this AcierProtocol.Mission _mission, RegionDefinition _def)
		{
			return LocalizationManager.Instance.Get(_mission.missionFlavorTextLoca.neutral, _mission.GetStringMapperForMissions(_def));
		}

		public static StringMapper GetStringMapperForMissions(this AcierProtocol.Mission _mission, RegionDefinition _def)
		{
			if (_mission.type == ERuntimeMissionType.RandomMission)
			{
				return LocalizationHelper.GetStringMapperForGeneratedMissions(_def);
			}
			return null;
		}

		public static RegionDefinition GetRegionDefinition(this AcierProtocol.Mission _mission)
		{
			List<KeyValuePair<GameID, DBEntry>> items = Main.Instance.GameDB.GetItems<RegionDefinition>();
			for (int i = 0; i < items.Count; i++)
			{
				RegionDefinition regionDefinition = items[i].Value as RegionDefinition;
				if (regionDefinition.Region == _mission.locationSet.region && regionDefinition.Age == _mission.locationSet.era && regionDefinition.Mood == _mission.locationSet.mood)
				{
					return regionDefinition;
				}
			}
			return null;
		}

		public static LocationSet Find(this LocationSet[] _locations, EEra _era, ERegion _region, EMood _mood)
		{
			foreach (LocationSet locationSet in _locations)
			{
				if (locationSet.era == _era && locationSet.region == _region && locationSet.mood == _mood)
				{
					return locationSet;
				}
			}
			return null;
		}

		public static LocationSet Find(this LocationSet[] _locations, LocationSet _locationSet)
		{
			return _locations.Find(_locationSet.era, _locationSet.region, _locationSet.mood);
		}
	}
}
