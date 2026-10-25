using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.UI.State;
using Database;

namespace Assets.Scripts.UI.Utils
{
	public static class EarlyAccessHelpers
	{
		public enum EEarlyAccesFlowState
		{
			Invalid = 0,
			Inactive = 1,
			EarlyAccess = 2,
			Locked = 3,
			Available = 4,
			ComingSoon = 5
		}

		private static List<KeyValuePair<GameID, DBEntry>> m_regionDef;

		private static List<KeyValuePair<GameID, DBEntry>> RegionDefs
		{
			get
			{
				return m_regionDef ?? (m_regionDef = Main.Instance.GameDB.GetItems<RegionDefinition>());
			}
		}

		public static bool IsLockedByDefinition(EEra _era, ERegion _region, EMood _mood)
		{
			foreach (KeyValuePair<GameID, DBEntry> regionDef in RegionDefs)
			{
				RegionDefinition regionDefinition = regionDef.Value as RegionDefinition;
				if (regionDefinition != null && regionDefinition.Age == _era && regionDefinition.Region == _region && regionDefinition.Mood == _mood && regionDefinition.Locked)
				{
					return true;
				}
			}
			return false;
		}

		public static EResourceType GetFirstUnlockCost(EEra _era, ERegion _region, EMood _mood)
		{
			Resources unlockCosts = GetUnlockCosts(_era, _region, _mood);
			if (unlockCosts != null && unlockCosts.resources.Length > 0)
			{
				Array.Sort(unlockCosts.resources, UIUtil.SortResourcesByType);
				return unlockCosts.resources[0].type;
			}
			return EResourceType.HardCurrency;
		}

		public static LocationUnlockMappings.Entry GetMapping(EEra _era, ERegion _region, EMood _mood)
		{
			return Singleton<StateManager>.Instance.TemplateManager.LocationUnlockMappings.entries.Find((LocationUnlockMappings.Entry x) => x.location.era == _era && x.location.region == _region && x.location.mood == _mood);
		}

		public static bool VisibleToUser(EEra _era, ERegion _region, EMood _mood)
		{
			if (IsLockedByDefinition(_era, _region, _mood))
			{
				return true;
			}
			return true;
		}

		public static bool EarlyAccessActive(EEra _era, ERegion _region, EMood _mood)
		{
			if (IsLockedByDefinition(_era, _region, _mood))
			{
				return false;
			}
			return false;
		}

		public static Resources GetUnlockCosts(EEra _era, ERegion _region, EMood _mood)
		{
			if (IsLockedByDefinition(_era, _region, _mood))
			{
				return new Resources();
			}
			return null;
		}

		public static bool UnlockedByUser(EEra _era, ERegion _region, EMood _mood)
		{
			if (IsLockedByDefinition(_era, _region, _mood))
			{
				return false;
			}
			return true;
		}
	}
}
