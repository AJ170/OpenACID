namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public sealed class ItemCompareCategory : ItemCompareBase
	{
		public override int Compare(InventoryItem _lhs, InventoryItem _rhs)
		{
			return _lhs.Element.category.CompareTo(_rhs.Element.category);
		}
	}
}
