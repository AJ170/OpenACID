using System.Collections.Generic;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public sealed class CompareItemGroups : IComparer<InventoryItemBase>
	{
		public ItemCompareBase ItemCompare { get; set; }

		public int Compare(InventoryItemBase _lhs, InventoryItemBase _rhs)
		{
			if (_lhs.Order != _rhs.Order)
			{
				return _lhs.Order.CompareTo(_rhs.Order);
			}
			if (_lhs is InventoryItem && _rhs is InventoryItem)
			{
				return ItemCompare.Compare(_lhs as InventoryItem, _rhs as InventoryItem);
			}
			return 0;
		}
	}
}
