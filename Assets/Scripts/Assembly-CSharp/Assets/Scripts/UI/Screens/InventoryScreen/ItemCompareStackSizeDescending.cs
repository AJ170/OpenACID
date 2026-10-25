namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public sealed class ItemCompareStackSizeDescending : ItemCompareBase
	{
		public override int Compare(InventoryItem _lhs, InventoryItem _rhs)
		{
			return -1 * _lhs.Element.stackSize.CompareTo(_rhs.Element.stackSize);
		}
	}
}
