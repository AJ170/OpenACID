namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public sealed class ItemCompareDefault : ItemCompareBase
	{
		private static readonly ItemCompareBase[] s_sortOrder = new ItemCompareBase[5]
		{
			new ItemCompareCategory(),
			new ItemCompareQualityDescending(),
			new ItemCompareLevelDescending(),
			new ItemCompareAquireDateDescending(),
			new ItemSortId()
		};

		public override int Compare(InventoryItem _lhs, InventoryItem _rhs)
		{
			ItemCompareBase[] array = s_sortOrder;
			foreach (ItemCompareBase itemCompareBase in array)
			{
				int num = itemCompareBase.Compare(_lhs, _rhs);
				if (num != 0)
				{
					return num;
				}
			}
			return 0;
		}
	}
}
