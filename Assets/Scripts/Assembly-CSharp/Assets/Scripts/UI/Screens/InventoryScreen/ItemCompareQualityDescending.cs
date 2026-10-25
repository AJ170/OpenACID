namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public sealed class ItemCompareQualityDescending : ItemCompareBase
	{
		public override int Compare(InventoryItem _lhs, InventoryItem _rhs)
		{
			return -1 * _lhs.Element.quality.CompareTo(_rhs.Element.quality);
		}
	}
}
