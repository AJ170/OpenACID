using System;
using AcierProtocol;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils.Extensions;

namespace Assets.Scripts.UI.State.ProfileManager
{
	public class LastProfileData
	{
		public bool MissionUpdateShownToUse;

		public Assassins Assassins;

		public EntitlementInventory EntitlementInventory;

		public MissionCollection MissionCollection;

		public ProgressionQuests ProgressionQuests;

		public EventsProgress EventsProgress;

		private Counters m_counters;

		public Counters Counters
		{
			get
			{
				return m_counters;
			}
			set
			{
				Counter[] counters = value.counters;
				m_counters = new Counters
				{
					counters = new Counter[counters.Length]
				};
				for (int i = 0; i < counters.Length; i++)
				{
					Counter counter = counters[i];
					m_counters.counters[i] = new Counter
					{
						ECounterTypeOrdinal = counter.ECounterTypeOrdinal,
						num = counter.num
					};
				}
			}
		}

		public MissionCollection.RankMissions CurrrentRankMissions
		{
			get
			{
				UIAssasinId currentAssassinId = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassinId;
				MissionCollection.AssassinMissions missionsByAssassin = MissionCollection.GetMissionsByAssassin(currentAssassinId);
				if (missionsByAssassin == null)
				{
					return null;
				}
				int contractHeroicMissionRank = Singleton<StateManager>.Instance.MissionSelectionManager.ContractHeroicMissionRank;
				return missionsByAssassin.GetMissionsByAssassinAndRank(currentAssassinId, contractHeroicMissionRank);
			}
		}

		public bool HasEntitlement(string _entitlementGuidStr)
		{
			if (EntitlementInventory == null)
			{
				return false;
			}
			for (int i = 0; i < EntitlementInventory.entries.Length; i++)
			{
				EntitlementInventory.Entry entry = EntitlementInventory.entries[i];
				if (entry.gameIdGuid.Equals(_entitlementGuidStr, StringComparison.InvariantCultureIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		public long GetCounterAmountByType(ECounterType _type)
		{
			if (Counters != null)
			{
				for (int i = 0; i < Counters.counters.Length; i++)
				{
					Counter counter = Counters.counters[i];
					if (counter.ECounterTypeOrdinal == (long)_type)
					{
						return counter.num;
					}
				}
			}
			return 0L;
		}
	}
}
