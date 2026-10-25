using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.UI.Localization;
using Database;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class EraExtensions
	{
		public enum EEarthPin
		{
			Italy = 0,
			America = 1,
			Caribbean = 2,
			Montreal = 3
		}

		public static EEarthPin GetEarthPinByEra(this EEra _era)
		{
			EEarthPin result = EEarthPin.Italy;
			switch (_era)
			{
			case EEra.Italy:
				result = EEarthPin.Italy;
				break;
			case EEra.Abstergo:
				result = EEarthPin.Montreal;
				break;
			case EEra.Caribbean:
				result = EEarthPin.Caribbean;
				break;
			}
			return result;
		}

		public static string DisplayName(this EEra _era)
		{
			List<KeyValuePair<GameID, DBEntry>> items = Main.Instance.GameDB.GetItems<RegionDefinition>();
			for (int i = 0; i < items.Count; i++)
			{
				RegionDefinition regionDefinition = items[i].Value as RegionDefinition;
				if (regionDefinition.Region == ERegion.Animus && regionDefinition.Age == _era)
				{
					return LocalizationManager.Instance.Get(regionDefinition.NameLocaKey.Key);
				}
			}
			EraDescription value;
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.EraDescriptions.TryGetValue((int)_era, out value))
			{
				return LocalizationManager.Instance.Get(value.NameId);
			}
			return string.Empty;
		}

		public static RegionDefinition GetFirstRegionDefinition(this EEra _era, bool _isGlobe = false)
		{
			List<KeyValuePair<GameID, DBEntry>> items = Main.Instance.GameDB.GetItems<RegionDefinition>();
			for (int i = 0; i < items.Count; i++)
			{
				RegionDefinition regionDefinition = (RegionDefinition)items[i].Value;
				if ((!_isGlobe || regionDefinition.Globe) && regionDefinition.Age == _era)
				{
					return regionDefinition;
				}
			}
			return null;
		}
	}
}
