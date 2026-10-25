using AcierProtocol;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class InventorySlotCostsMappingExtensions
	{
		public static Resources GetCost(this InventorySlotCostsMapping _mapping, int _index, int _numberOfPurchases)
		{
			Resources resources = new Resources();
			InventorySlotCostsMapping.Entry[] entries = _mapping.entries;
			for (int i = 0; i < _numberOfPurchases; i++)
			{
				int num = _index + i;
				for (int j = 0; j < entries.Length; j++)
				{
					if (entries[j].purchaseIndex == num)
					{
						resources = resources.Join(entries[j].costs);
						break;
					}
				}
			}
			return resources;
		}
	}
}
