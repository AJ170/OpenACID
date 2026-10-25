using System;
using AcierProtocol;
using Assets.Scripts.UI.Utils.Extensions;

namespace Assets.Scripts.UI.State.EventManager.Data
{
	public class EventShopBundleVisibility : EventBase
	{
		private const string c_shopBundleIdParamName = "shop.item.id";

		private const string c_visibilityParamName = "visibility";

		private const string c_positionParamName = "position";

		public string ShopBundleId { get; private set; }

		public bool IsVisible { get; private set; }

		public int Position { get; private set; }

		public EventShopBundleVisibility(Event _event)
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
				else if (string.Compare(property.name, "visibility", StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					IsVisible = property.GetBoolValue();
				}
				else if (string.Compare(property.name, "position", StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					Position = property.GetIntValue();
				}
			}
		}
	}
}
