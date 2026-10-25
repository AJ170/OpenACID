using System;
using AcierProtocol;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class AassassinSlotCostsMappingExtensions
	{
		public static Resource[] GetCost(this AssassinSlotCostsMapping _mapping, int _index)
		{
			AssassinSlotCostsMapping.Entry[] entries = _mapping.entries;
			for (int i = 0; i < entries.Length; i++)
			{
				if (entries[i].purchaseIndex == _index)
				{
					return entries[i].costs.resources;
				}
			}
			return new Resource[0];
		}

		public static EResourceType GetFirstAvailableResourceCostType(this AssassinSlotCostsMapping _mapping, UserProfile _profile)
		{
			long counterAmountByType = _profile.GetCounterAmountByType(ECounterType.BoughtAssassinSlots);
			Resource[] cost = _mapping.GetCost((int)counterAmountByType);
			Array.Sort(cost, UIUtil.SortResourcesByType);
			if (cost.Length > 0)
			{
				return cost[0].type;
			}
			return EResourceType.SoftCurrency;
		}
	}
}
