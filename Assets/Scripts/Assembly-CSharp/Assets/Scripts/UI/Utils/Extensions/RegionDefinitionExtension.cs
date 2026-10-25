using System;
using Assets.Scripts.Database;
using Assets.Scripts.UI.Localization;
using Database;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class RegionDefinitionExtension
	{
		public static RegionDefinition.AppearanceOption GetRegionAppearanceOption(this RegionDefinition _regiondef, string _name)
		{
			for (int i = 0; i < _regiondef.AppearanceOptions.Count; i++)
			{
				RegionDefinition.AppearanceOption appearanceOption = _regiondef.AppearanceOptions[i];
				if (string.Equals(appearanceOption.Name, _name, StringComparison.InvariantCultureIgnoreCase))
				{
					return appearanceOption;
				}
			}
			return null;
		}

		public static LocaKey GetRegionNameLoca(this RegionDefinition _regiondef, string _name)
		{
			return _regiondef.GetRegionNameLoca(_regiondef.GetRegionAppearanceOption(_name));
		}

		public static LocaKey GetRegionNameLoca(this RegionDefinition _regiondef, RegionDefinition.AppearanceOption _appearance)
		{
			LocaKey nameLocaKey = _regiondef.NameLocaKey;
			if (_appearance != null && _appearance.NameLocaKey != null && _appearance.NameLocaKey.Key != StringId.Invalid)
			{
				nameLocaKey = _appearance.NameLocaKey;
			}
			return nameLocaKey;
		}

		public static LocaKey GetRegionNameShortLoca(this RegionDefinition _regiondef, string _name)
		{
			return _regiondef.GetRegionNameShortLoca(_regiondef.GetRegionAppearanceOption(_name));
		}

		public static LocaKey GetRegionNameShortLoca(this RegionDefinition _regiondef, RegionDefinition.AppearanceOption _appearance)
		{
			LocaKey locaKey = _regiondef.NameShortLocaKey;
			if (_appearance != null && _appearance.NameShortLocaKey != null && _appearance.NameShortLocaKey.Key != StringId.Invalid)
			{
				locaKey = _appearance.NameShortLocaKey;
			}
			if (locaKey == null || locaKey.Key <= 0)
			{
				locaKey = _regiondef.GetRegionNameLoca(_appearance);
			}
			return locaKey;
		}

		public static LocaKey GetRegionDateLoca(this RegionDefinition _regiondef, string _name)
		{
			return _regiondef.GetRegionDateLoca(_regiondef.GetRegionAppearanceOption(_name));
		}

		public static LocaKey GetRegionDateLoca(this RegionDefinition _regiondef, RegionDefinition.AppearanceOption _appearance)
		{
			LocaKey dateLocaKey = _regiondef.DateLocaKey;
			if (_appearance != null && _appearance.DateLocaKey != null && _appearance.DateLocaKey.Key != StringId.Invalid)
			{
				dateLocaKey = _appearance.DateLocaKey;
			}
			return dateLocaKey;
		}

		public static LocaKey GetRegionEarthPinLoca(this RegionDefinition _regiondef, string _name)
		{
			return _regiondef.GetRegionEarthPinLoca(_regiondef.GetRegionAppearanceOption(_name));
		}

		public static LocaKey GetRegionEarthPinLoca(this RegionDefinition _regiondef, RegionDefinition.AppearanceOption _appearance)
		{
			LocaKey locaKey = _regiondef.EarthPinLocaKey;
			if (_appearance != null && _appearance.EarthPinLocaKey != null && _appearance.EarthPinLocaKey.Key != StringId.Invalid)
			{
				locaKey = _appearance.EarthPinLocaKey;
			}
			if (locaKey.Key == 0)
			{
				locaKey = _regiondef.GetRegionNameLoca(_appearance);
			}
			return locaKey;
		}

		public static UnityResource GetRegionBanner(this RegionDefinition _regiondef, string _name)
		{
			return _regiondef.GetRegionBanner(_regiondef.GetRegionAppearanceOption(_name));
		}

		public static UnityResource GetRegionBanner(this RegionDefinition _regiondef, RegionDefinition.AppearanceOption _appearance)
		{
			UnityResource banner = _regiondef.Banner;
			if (_appearance != null && _appearance.Banner != null && !string.IsNullOrEmpty(_appearance.Banner.GetResourcePath()))
			{
				banner = _appearance.Banner;
			}
			return banner;
		}

		public static UnityResource GetRegionMap(this RegionDefinition _regiondef, string _name)
		{
			return _regiondef.GetRegionMap(_regiondef.GetRegionAppearanceOption(_name));
		}

		public static UnityResource GetRegionMap(this RegionDefinition _regiondef, RegionDefinition.AppearanceOption _appearance)
		{
			UnityResource map = _regiondef.Map;
			if (_appearance != null && _appearance.Map != null && !string.IsNullOrEmpty(_appearance.Map.GetResourcePath()))
			{
				map = _appearance.Map;
			}
			return map;
		}

		public static UnityResource GetRegionEarthPinImg(this RegionDefinition _regiondef, string _name)
		{
			return _regiondef.GetRegionBanner(_regiondef.GetRegionAppearanceOption(_name));
		}

		public static UnityResource GetRegionEarthPinImg(this RegionDefinition _regiondef, RegionDefinition.AppearanceOption _appearance)
		{
			UnityResource earthPinImg = _regiondef.EarthPinImg;
			if (_appearance != null && _appearance.EarthPinImg != null && !string.IsNullOrEmpty(_appearance.EarthPinImg.GetResourcePath()))
			{
				earthPinImg = _appearance.EarthPinImg;
			}
			return earthPinImg;
		}
	}
}
