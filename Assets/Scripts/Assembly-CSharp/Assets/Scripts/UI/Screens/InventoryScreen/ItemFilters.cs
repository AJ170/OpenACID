using Assets.Scripts.UI.Utils.Extensions;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public static class ItemFilters
	{
		public static bool FilterAll(InventoryItemBase _item)
		{
			return true;
		}

		public static bool FilterModules(InventoryItemBase _item)
		{
			return _item != null && _item.Element != null && _item.Element.IsModule();
		}

		public static bool FilterEquipment(InventoryItemBase _item)
		{
			return _item != null && _item.Element != null && _item.Element.IsItem();
		}

		public static bool FilterForgeItems(InventoryItemBase _item)
		{
			return _item != null && ((_item.Element != null && _item.Element.VisibleForForge()) || _item is BuyBluePrintSlot);
		}

		public static bool FilterForgeItemsNoBluePrints(InventoryItemBase _item)
		{
			return _item != null && ((_item.Element != null && _item.Element.VisibleForForge() && !_item.Element.IsBlueprint()) || _item is BuyBluePrintSlot);
		}
	}
}
