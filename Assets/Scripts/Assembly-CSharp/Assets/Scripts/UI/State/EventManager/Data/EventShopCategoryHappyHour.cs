using System;
using AcierProtocol;
using Assets.Scripts.UI.Utils.Extensions;

namespace Assets.Scripts.UI.State.EventManager.Data
{
	public class EventShopCategoryHappyHour : EventBase
	{
		private const string c_categoryParamName = "shop.category";

		private const string c_percentageParamName = "multiplier";

		public string ShopBundleName { get; private set; }

		public EShopCategory ShopCategory { get; private set; }

		public float Multiplier { get; private set; }

		public EventShopCategoryHappyHour(Event _event)
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
				else if (string.Compare(property.name, "multiplier", StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					Multiplier = property.GetFloatValue();
				}
			}
		}
	}
}
