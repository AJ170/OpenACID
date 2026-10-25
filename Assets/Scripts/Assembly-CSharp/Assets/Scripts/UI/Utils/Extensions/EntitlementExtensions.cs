using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.UI.State;
using Database;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class EntitlementExtensions
	{
		public static EntitlementDefinition GetEntitlementByGUID(string _guid)
		{
			GameID gameID = Main.Instance.GameDB.FindGameIdFromGuid(_guid);
			return GetEntitlementByGUID(gameID);
		}

		public static EntitlementDefinition GetEntitlementByGUID(GameID _gameID)
		{
			return Main.Instance.GameDB.GetItem<EntitlementDefinition>(_gameID);
		}

		public static bool IsProfileAvailable()
		{
			return Main.IsInitialized && Main.Instance.IsNetworkEnabled && Singleton<StateManager>.Instance.ProfileManager.Profile != null;
		}

		public static bool HasCompletedIntroMission()
		{
			if (!IsProfileAvailable())
			{
				return true;
			}
			return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.CompletedTutorialIntroMission);
		}

		public static bool IsSkillTrainingUnlocked()
		{
			if (!IsProfileAvailable())
			{
				return true;
			}
			return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.SkillTreeEntitlementGUID);
		}

		public static bool IsHirelingUnlocked()
		{
			if (!IsProfileAvailable())
			{
				return true;
			}
			return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.HirelingEntitlementGUID);
		}

		public static bool IsEagleVisionUnlocked()
		{
			if (!IsProfileAvailable())
			{
				return true;
			}
			return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.EagleVisionEntitlementGUID);
		}

		public static bool AreHidingSpotKillsUnlocked()
		{
			if (!IsProfileAvailable())
			{
				return true;
			}
			return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.HidingSpotKillsEntitlementGUID);
		}

		public static bool AreCourtesansUnlocked()
		{
			if (!IsProfileAvailable())
			{
				return true;
			}
			return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.CourtesansEntitlementGUID);
		}

		public static bool AreMonksUnlocked()
		{
			if (!IsProfileAvailable())
			{
				return true;
			}
			return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.MonkEntitlementGUID);
		}

		public static bool IsFusionUnlocked()
		{
			if (!IsProfileAvailable())
			{
				return true;
			}
			return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.FusionEntitlementGUID);
		}

		public static bool IsDoctorUnlocked()
		{
			if (!IsProfileAvailable())
			{
				return true;
			}
			return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.DoctorEntitlementGUID);
		}

		public static HeroicRankMapping.Entry GetNextHeroicRankToUnlock(this HeroicRankMapping.Entry[] _rankMapping)
		{
			HeroicRankMapping.Entry entry = null;
			HeroicRankMapping.Entry entry2 = null;
			if (_rankMapping != null && _rankMapping.Length > 0)
			{
				foreach (HeroicRankMapping.Entry entry3 in _rankMapping)
				{
					if (Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(entry3.entitlement))
					{
						if (entry2 == null || entry2.rankIdx < entry3.rankIdx)
						{
							entry2 = entry3;
						}
					}
					else if (entry == null || entry.rankIdx > entry3.rankIdx)
					{
						entry = entry3;
					}
				}
			}
			return entry ?? entry2;
		}

		public static int GetHighestGrantedRankIndex(this HeroicRankMapping.Entry[] _rankMapping)
		{
			if (_rankMapping != null && _rankMapping.Length > 0)
			{
				for (int num = _rankMapping.Length - 1; num >= 0; num--)
				{
					if (Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(_rankMapping[num].entitlement))
					{
						return num;
					}
				}
			}
			return 0;
		}

		public static HeroicRankMapping.Entry GetHeroicRankMapping(int _rankIndex)
		{
			HeroicRankMapping.Entry[] heroicRankMapping = Singleton<StateManager>.Instance.TemplateManager.HeroicRankMapping;
			if (_rankIndex < heroicRankMapping.Length)
			{
				return heroicRankMapping[_rankIndex];
			}
			return null;
		}

		public static bool IsHeroicRankGranted(int _randIndex)
		{
			HeroicRankMapping.Entry heroicRankMapping = GetHeroicRankMapping(_randIndex);
			if (heroicRankMapping == null)
			{
				return true;
			}
			return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(heroicRankMapping.entitlement);
		}

		public static bool HasAllRequiredEntitlementInList(this EntitlementDefinition _entitlementDefinition, string[] _entitlements)
		{
			for (int i = 0; i < _entitlementDefinition.RequiredEntitlements.Count; i++)
			{
				GameID gameID = _entitlementDefinition.RequiredEntitlements[i];
				bool flag = false;
				foreach (string guid in _entitlements)
				{
					GameID gameID2 = Main.Instance.GameDB.FindGameIdFromGuid(guid);
					if (gameID == gameID2)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}
	}
}
