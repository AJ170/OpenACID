using System;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public sealed class ItemSortId : ItemCompareBase
	{
		public override int Compare(InventoryItem _lhs, InventoryItem _rhs)
		{
			return string.Compare(_lhs.Element.id, _rhs.Element.id, StringComparison.InvariantCultureIgnoreCase);
		}
	}
}
