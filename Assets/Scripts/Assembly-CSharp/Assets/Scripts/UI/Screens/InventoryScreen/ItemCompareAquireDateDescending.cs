namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public sealed class ItemCompareAquireDateDescending : ItemCompareBase
	{
		public override int Compare(InventoryItem _lhs, InventoryItem _rhs)
		{
			return _lhs.Element.createdAt.CompareTo(_rhs.Element.createdAt);
		}
	}
}
