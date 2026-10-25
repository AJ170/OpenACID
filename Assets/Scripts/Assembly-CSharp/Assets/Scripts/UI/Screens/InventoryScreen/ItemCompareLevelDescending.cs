using Assets.Scripts.UI.Utils.Extensions;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public sealed class ItemCompareLevelDescending : ItemCompareBase
	{
		public override int Compare(InventoryItem _lhs, InventoryItem _rhs)
		{
			if (_lhs.Element.IsItem() && _rhs.Element.IsItem())
			{
				return -1 * _lhs.Element.itemData.level.CompareTo(_rhs.Element.itemData.level);
			}
			return 0;
		}
	}
}
