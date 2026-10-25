using System;
using AcierProtocol;
using Assets.Scripts.UI.State.ThemeManager;
using Assets.Scripts.UI.Utils.Extensions;

namespace Assets.Scripts.UI.State.EventManager.Data
{
	public class EventFlavorTheme : EventBase
	{
		private const string c_flavorParamName = "flavor";

		public ThemesManager.EFlavorTheme FlavorThemeType { get; private set; }

		public EventFlavorTheme(Event _event)
			: base(_event)
		{
		}

		protected override void OnProcessEvent(Event _event)
		{
			for (int i = 0; i < _event.properties.Length; i++)
			{
				Event.Property property = _event.properties[i];
				if (string.Compare(property.name, "flavor", StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					FlavorThemeType = property.ParseEnum<ThemesManager.EFlavorTheme>();
				}
			}
		}
	}
}
