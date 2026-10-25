using System;
using AcierProtocol;
using Assets.Scripts.UI.Utils.Extensions;

namespace Assets.Scripts.UI.State.EventManager.Data
{
	public class EventShopBundlePercentage : EventBase
	{
		private const string c_shopBundleIdParamName = "shop.item.id";

		private const string c_percentageParamName = "multiplier";

		public string ShopBundleId { get; private set; }

		public float Multiplier { get; private set; }

		public EventShopBundlePercentage(Event _event)
			: base(_event)
		{
		}

		protected override void OnProcessEvent(Event _event)
		{
			for (int i = 0; i < _event.properties.Length; i++)
			{
				Event.Property property = _event.properties[i];
				if (string.Compare(property.name, "shop.item.id", StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					ShopBundleId = property.GetStringValue();
				}
				else if (string.Compare(property.name, "multiplier", StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					Multiplier = property.GetFloatValue();
				}
			}
		}
	}
}
