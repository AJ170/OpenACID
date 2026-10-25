using System;
using AcierProtocol;
using Assets.Scripts.UI.Utils.Extensions;

namespace Assets.Scripts.UI.State.EventManager.Data
{
	public class EventCategoryVisibility : EventBase
	{
		private const string c_categoryParamName = "shop.category";

		private const string c_visibilityParamName = "visibility";

		public EShopCategory ShopCategory { get; private set; }

		public bool IsVisible { get; private set; }

		public EventCategoryVisibility(Event _event)
			: base(_event)
		{
		}

		protected override void OnProcessEvent(Event _event)
		{
			for (int i = 0; i < _event.properties.Length; i++)
			{
				Event.Property property = _event.properties[i];
				if (string.Compare(property.name, "shop.category", StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					ShopCategory = property.ParseEnum<EShopCategory>();
				}
				else if (string.Compare(property.name, "visibility", StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					IsVisible = property.GetBoolValue();
				}
			}
		}
	}
}
