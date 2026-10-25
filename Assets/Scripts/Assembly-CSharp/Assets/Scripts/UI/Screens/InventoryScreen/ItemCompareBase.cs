using System.Collections.Generic;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public abstract class ItemCompareBase : IComparer<InventoryItem>
	{
		public virtual int Compare(InventoryItem _lhs, InventoryItem _rhs)
		{
			return 0;
		}
	}
}
