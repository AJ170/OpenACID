using System;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Mail;
using Assets.Scripts.UI.Screens.MysteryBox;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;
using Assets.Scripts.UI.Types;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class UserProfileExtension
	{
		public static bool IsPlayer(this UserProfile _profile)
		{
			return Singleton<StateManager>.Instance.ProfileManager.Profile.id == _profile.id;
		}

		public static int AssassinCount(this UserProfile _profile)
		{
			return (_profile.assassins != null) ? _profile.assassins.assassins.Length : 0;
		}

		public static Assassin GetAssassin(this UserProfile _profile, UIAssasinId _id)
		{
			for (int i = 0; i < _profile.assassins.assassins.Length; i++)
			{
				if (_profile.assassins.assassins[i].id == _id)
				{
					return _profile.assassins.assassins[i];
				}
			}
			return null;
		}

		public static SecondSkillTreeCostMapping.SecondSkillTreeCostEntry GetSecondClassUnlockCost(this UserProfile _profile)
		{
			int num = 0;
			for (int i = 0; i < _profile.assassins.assassins.Length; i++)
			{
				Assassin assassin = _profile.assassins.assassins[i];
				if (assassin.extendedClassTypeId != Assassin.EClassTypeId.None)
				{
					num++;
				}
			}
			return Singleton<StateManager>.Instance.TemplateManager.SecondSkillTreeCostMapping.GetUnlockSecondClassUnlockResourceByIndex(num);
		}

		public static SecondSkillTreeCostMapping.SecondSkillTreeCostEntry GetUnlockSecondClassUnlockResourceByIndex(this SecondSkillTreeCostMapping _mapping, int _unlockNumber)
		{
			Debug.Log(_mapping.secondSkillTreeUnlockCosts.Length);
			for (int i = 0; i < _mapping.secondSkillTreeUnlockCosts.Length; i++)
			{
				SecondSkillTreeCostMapping.SecondSkillTreeCostEntry secondSkillTreeCostEntry = _mapping.secondSkillTreeUnlockCosts[i];
				if (secondSkillTreeCostEntry.unlockNumber == _unlockNumber + 1)
				{
					return secondSkillTreeCostEntry;
				}
			}
			return null;
		}

		public static long GetResourceAmountByType(this UserProfile _profile, EResourceType _type)
		{
			if (_profile.resources != null)
			{
				return _profile.resources.GetResourceAmountByType(_type);
			}
			return 0L;
		}

		public static bool CanBuyMoreAssassinSlots(this UserProfile _profile)
		{
			long counterAmountByType = _profile.GetCounterAmountByType(ECounterType.BoughtAssassinSlots);
			int num = Singleton<StateManager>.Instance.TemplateManager.AssassinSlotsCostsMapping.entries.Length;
			return counterAmountByType < num;
		}

		public static long GetCounterAmountByType(this UserProfile _profile, ECounterType _type)
		{
			if (_profile.counters != null)
			{
				for (int i = 0; i < _profile.counters.counters.Length; i++)
				{
					Counter counter = _profile.counters.counters[i];
					if (counter.ECounterTypeOrdinal == (long)_type)
					{
						return counter.num;
					}
				}
			}
			return 0L;
		}

		public static ProgressionQuests.RankQuests GetQuestByRank(this UserProfile _profile, int _rankId)
		{
			if (_profile.progressionQuests != null && _profile.progressionQuests.rankQuests != null)
			{
				for (int i = 0; i < _profile.progressionQuests.rankQuests.Length; i++)
				{
					ProgressionQuests.RankQuests rankQuests = _profile.progressionQuests.rankQuests[i];
					if (rankQuests.rankIdx == _rankId)
					{
						return rankQuests;
					}
				}
			}
			return null;
		}

		public static long GetLowestRemainingQuestTimeInMilliSeconds(this UserProfile _profile)
		{
			if (_profile.progressionQuests != null && _profile.progressionQuests.rankQuests != null && _profile.progressionQuests.rankQuests.Length > 0)
			{
				long num = long.MaxValue;
				for (int i = 0; i < _profile.progressionQuests.rankQuests.Length; i++)
				{
					ProgressionQuests.RankQuests rankQuests = _profile.progressionQuests.rankQuests[i];
					num = Math.Min((long)rankQuests.expiresAtTimeStamp, num);
				}
				if (num != long.MaxValue)
				{
					DateTime utcNow = Singleton<StateManager>.Instance.ServerTimeManager.UtcNow;
					DateTime dateTime = ServerTimeManager.ToUtcTime(num);
					long val = (long)(dateTime - utcNow).TotalMilliseconds;
					return Math.Max(val, 0L);
				}
			}
			return -1L;
		}

		public static bool IsMysteryBoxOpen(this UserProfile _profile)
		{
			return Singleton<StateManager>.Instance.ServerTimeManager.Time < _profile.mysteryBoxExpireTime;
		}

		public static Assassin GetHighestAssassinByItemPower(this UserProfile _profile)
		{
			bool isPlayer = _profile.IsPlayer();
			Assassin[] assassins = _profile.assassins.assassins;
			Array.Sort(assassins, (Assassin _ass1, Assassin _ass2) => isPlayer ? _ass2.TotalItemPower().CompareTo(_ass1.TotalItemPower()) : _ass2.itemPower.CompareTo(_ass1.itemPower));
			return (assassins.Length <= 0) ? null : assassins[0];
		}

		public static long GetHighestAssassinItemPower(this UserProfile _profile)
		{
			bool flag = _profile.IsPlayer();
			Assassin highestAssassinByItemPower = _profile.GetHighestAssassinByItemPower();
			long result = 0L;
			if (highestAssassinByItemPower != null)
			{
				result = highestAssassinByItemPower.itemPower;
				if (flag)
				{
					result = highestAssassinByItemPower.TotalItemPower();
				}
			}
			return result;
		}

		public static EnoughResources HasEnoughResources(this UserProfile _profile, Resource _resource)
		{
			return _profile.HasEnoughResources(new Resource[1] { _resource });
		}

		public static EnoughResources HasEnoughResources(this UserProfile _profile, AcierProtocol.Resources _resources)
		{
			return _profile.HasEnoughResources((_resources != null) ? _resources.resources : null);
		}

		public static EnoughResources HasEnoughResources(this UserProfile _profile, Resource[] _resource)
		{
			EnoughResources enoughResources = new EnoughResources();
			if (_resource != null)
			{
				for (int i = 0; i < _resource.Length; i++)
				{
					long resourceAmountByType = _profile.resources.GetResourceAmountByType(_resource[i].type);
					enoughResources.AddResourceAndSetCurrent(_resource[i], resourceAmountByType);
				}
			}
			return enoughResources;
		}

		public static void ShowMessageForMissingCurrency(this UserProfile _profile, Resource[] _resource)
		{
			for (int i = 0; i < _resource.Length; i++)
			{
				long resourceAmountByType = _profile.resources.GetResourceAmountByType(_resource[i].type);
				if (resourceAmountByType < _resource[i].amount)
				{
					if (_resource[i].type == EResourceType.HardCurrency)
					{
						UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(4581));
					}
					else if (_resource[i].type == EResourceType.SoftCurrency)
					{
						UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(4579));
					}
					else if (_resource[i].type == EResourceType.SocialCurrency)
					{
						UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(4580));
					}
				}
			}
		}

		public static uint GetRemainingInventoryExpansion(this UserProfile _profile)
		{
			uint num = 0u;
			InventorySlotCostsMapping.Entry[] entries = Singleton<StateManager>.Instance.TemplateManager.InventorySlotsCostsMapping.entries;
			long counterAmountByType = _profile.GetCounterAmountByType(ECounterType.InventorySlotPurchases);
			for (int i = 0; i < entries.Length; i++)
			{
				if (entries[i].purchaseIndex > counterAmountByType)
				{
					num += entries[i].amount;
				}
			}
			return num;
		}

		public static bool CanExpandInventory(this UserProfile _profile, int _inventorySpaceNeeded = 1)
		{
			return _profile.GetRemainingInventoryExpansion() >= _inventorySpaceNeeded;
		}

		public static int GetInventoryFreeSpace(this UserProfile _profile)
		{
			return _profile.GetInventorySize() - _profile.GetInventoryUsage();
		}

		public static int GetInventoryUsage(this UserProfile _profile)
		{
			return (_profile.Inventory.elements != null) ? _profile.Inventory.elements.Length : 0;
		}

		public static int GetInventorySize(this UserProfile _profile)
		{
			return _profile.Inventory.maxSize;
		}

		public static bool CanReciveMissionRewards(this UserProfile _profile)
		{
			int num = MailExtensions.MailLimit(EMailPriority.MailPriorityLow);
			return MailCache.NumberOfHeaders < num || _profile.GetInventoryFreeSpace() >= 3;
		}

		public static bool CanClaimMysteryBoxRewards(this UserProfile _profile, MysteryBoxChain _chain)
		{
			if (_chain == null)
			{
				return false;
			}
			int num = MailExtensions.MailLimit(EMailPriority.MailPriorityLow);
			if (MailCache.NumberOfHeaders < num)
			{
				return true;
			}
			int inventorySpaceRequiredForClaim = _chain.GetInventorySpaceRequiredForClaim();
			return _profile.GetInventoryFreeSpace() >= inventorySpaceRequiredForClaim;
		}

		public static bool CanClaimMysteryBoxRewards(this UserProfile _profile, EMysteryBoxType _mysteryBoxType, Quality.EQuality _quality)
		{
			int num = MailExtensions.MailLimit(EMailPriority.MailPriorityLow);
			if (MailCache.NumberOfHeaders < num)
			{
				return true;
			}
			int inventorySpaceRequiredForClaim = _mysteryBoxType.GetInventorySpaceRequiredForClaim(_quality);
			return _profile.GetInventoryFreeSpace() >= inventorySpaceRequiredForClaim;
		}

		public static InventoryElement GatInventoryElementByCustomType(this UserProfile _profile, ECustomLootTemplateType _type)
		{
			InventoryElement[] elements = _profile.Inventory.elements;
			foreach (InventoryElement inventoryElement in elements)
			{
				CustomLootData customLootData = inventoryElement.customLootData;
				if (customLootData != null && customLootData.customLootType == _type)
				{
					return inventoryElement;
				}
			}
			return null;
		}

		public static bool HasInventoryStackSpaceForBlueprint(this UserProfile _profile, EBlueprintType _type)
		{
			int maxItemStackSize = Singleton<StateManager>.Instance.TemplateManager.Globals.maxItemStackSize;
			InventoryElement[] elements = _profile.Inventory.elements;
			for (int i = 0; i < elements.Length; i++)
			{
				BlueprintData blueprintData = elements[i].blueprintData;
				if (blueprintData != null && blueprintData.type == _type && elements[i].stackSize < maxItemStackSize)
				{
					return true;
				}
			}
			return false;
		}

		public static bool HasInventoryStackSpaceForModule(this UserProfile _profile, EModuleType _moduleType, Quality.EQuality _quality)
		{
			int maxItemStackSize = Singleton<StateManager>.Instance.TemplateManager.Globals.maxItemStackSize;
			InventoryElement[] elements = _profile.Inventory.elements;
			for (int i = 0; i < elements.Length; i++)
			{
				ModuleData moduleData = elements[i].moduleData;
				if (moduleData != null && moduleData.moduleType == _moduleType && elements[i].quality == _quality && elements[i].stackSize < maxItemStackSize)
				{
					return true;
				}
			}
			return false;
		}

		public static bool HasInventoryStackSpaceForMysteryBox(this UserProfile _profile, EMysteryBoxType _type, Quality.EQuality _quality)
		{
			int maxItemStackSize = Singleton<StateManager>.Instance.TemplateManager.Globals.maxItemStackSize;
			InventoryElement[] elements = _profile.Inventory.elements;
			for (int i = 0; i < elements.Length; i++)
			{
				AcierProtocol.MysteryBoxData mysteryBoxData = elements[i].mysteryBoxData;
				if (mysteryBoxData != null && mysteryBoxData.mysteryBoxType == _type && elements[i].quality == _quality && elements[i].stackSize < maxItemStackSize)
				{
					return true;
				}
			}
			return false;
		}

		public static bool HasInventoryStackSpaceForCustomLoot(this UserProfile _profile, ECustomLootTemplateType _type, Quality.EQuality _quality)
		{
			int maxItemStackSize = Singleton<StateManager>.Instance.TemplateManager.Globals.maxItemStackSize;
			InventoryElement[] elements = _profile.Inventory.elements;
			for (int i = 0; i < elements.Length; i++)
			{
				CustomLootData customLootData = elements[i].customLootData;
				if (customLootData != null && customLootData.customLootType == _type && elements[i].quality == _quality && elements[i].stackSize < maxItemStackSize)
				{
					return true;
				}
			}
			return false;
		}

		public static int GetInventorySpaceNeededForLootTemplates(this LootTemplate[] _lootTemplates)
		{
			int num = 0;
			if (_lootTemplates != null)
			{
				ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
				bool[] _checkedIndexes = new bool[_lootTemplates.Length];
				for (int i = 0; i < _lootTemplates.Length; i++)
				{
					_checkedIndexes[i] = false;
				}
				UserProfile profile = profileManager.Profile;
				for (int j = 0; j < _lootTemplates.Length; j++)
				{
					if (_checkedIndexes[j])
					{
						continue;
					}
					LootTemplate lootTemplate = _lootTemplates[j];
					if (lootTemplate.resourceTemplate != null || lootTemplate.entitlementTemplate != null)
					{
						continue;
					}
					if (lootTemplate.itemTemplate != null)
					{
						num++;
					}
					else if (lootTemplate.moduleTemplate != null)
					{
						if (!profile.HasInventoryStackSpaceForModule(lootTemplate.moduleTemplate.type, lootTemplate.quality))
						{
							CheckForDuplicatedEntires(lootTemplate, j, _lootTemplates, ref _checkedIndexes);
							num++;
						}
					}
					else if (lootTemplate.blueprintTemplate != null)
					{
						if (!profile.HasInventoryStackSpaceForBlueprint(lootTemplate.blueprintTemplate.type))
						{
							CheckForDuplicatedEntires(lootTemplate, j, _lootTemplates, ref _checkedIndexes);
							num++;
						}
					}
					else if (lootTemplate.mysteryBoxTemplate != null)
					{
						if (!profile.HasInventoryStackSpaceForMysteryBox(lootTemplate.mysteryBoxTemplate.type, lootTemplate.quality))
						{
							CheckForDuplicatedEntires(lootTemplate, j, _lootTemplates, ref _checkedIndexes);
							num++;
						}
					}
					else if (lootTemplate.customTemplate != null && !profile.HasInventoryStackSpaceForCustomLoot(lootTemplate.customTemplate.type, lootTemplate.quality))
					{
						CheckForDuplicatedEntires(lootTemplate, j, _lootTemplates, ref _checkedIndexes);
						num++;
					}
				}
			}
			return num;
		}

		private static void CheckForDuplicatedEntires(LootTemplate _entry, int _startIndex, LootTemplate[] _lootTemplates, ref bool[] _checkedIndexes)
		{
			for (int i = _startIndex + 1; i < _lootTemplates.Length; i++)
			{
				LootTemplate lootTemplate = _lootTemplates[i];
				if (_entry.moduleTemplate != null && lootTemplate.moduleTemplate != null)
				{
					if (_entry.moduleTemplate.type == lootTemplate.moduleTemplate.type && _entry.quality == lootTemplate.quality)
					{
						_checkedIndexes[i] = true;
					}
				}
				else if (_entry.blueprintTemplate != null && lootTemplate.blueprintTemplate != null)
				{
					if (_entry.blueprintTemplate.type == lootTemplate.blueprintTemplate.type)
					{
						_checkedIndexes[i] = true;
					}
				}
				else if (_entry.mysteryBoxTemplate != null && lootTemplate.mysteryBoxTemplate != null)
				{
					if (_entry.mysteryBoxTemplate.type == lootTemplate.mysteryBoxTemplate.type && _entry.quality == lootTemplate.quality)
					{
						_checkedIndexes[i] = true;
					}
				}
				else if (_entry.customTemplate != null && lootTemplate.customTemplate != null && _entry.customTemplate.type == lootTemplate.customTemplate.type && _entry.quality == lootTemplate.quality)
				{
					_checkedIndexes[i] = true;
				}
			}
		}

		public static bool HasInventorySpaceForLootTemplates(this LootTemplate[] _lootTemplates)
		{
			if (_lootTemplates != null)
			{
				ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
				int inventoryFreeSpace = profileManager.InventoryFreeSpace;
				inventoryFreeSpace -= _lootTemplates.GetInventorySpaceNeededForLootTemplates();
				return inventoryFreeSpace >= 0 || inventoryFreeSpace == profileManager.InventoryFreeSpace;
			}
			return true;
		}

		public static long GetEventScore(this UserProfile _profile)
		{
			if (_profile.eventsProgress == null)
			{
				return 0L;
			}
			return _profile.eventsProgress.score;
		}

		public static bool HasClaimableEvent(this UserProfile _profile)
		{
			if (_profile.eventsProgress == null || _profile.eventsProgress.challengeEvents.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < _profile.eventsProgress.challengeEvents.Length; i++)
			{
				if (_profile.eventsProgress.challengeEvents[i].status == EEventChallengeStatus.Challenge_Finished)
				{
					return true;
				}
			}
			return false;
		}

		public static EventsProgress.ChallengeEvent GetEventsProgressChallenge(this UserProfile _profile, string _id)
		{
			if (_profile.eventsProgress == null)
			{
				return null;
			}
			for (int i = 0; i < _profile.eventsProgress.challengeEvents.Length; i++)
			{
				EventsProgress.ChallengeEvent challengeEvent = _profile.eventsProgress.challengeEvents[i];
				if (challengeEvent.challengeId == _id)
				{
					return challengeEvent;
				}
			}
			return null;
		}

		public static void UpdateLocalStorageForNewProfile(this UserProfile _profile)
		{
			LocalStorage instance = UbiCore.Utils.Singleton<LocalStorage>.Instance;
			if (instance == null || _profile == null)
			{
				return;
			}
			if (_profile.entitlementInventory != null)
			{
				for (int i = 0; i < _profile.entitlementInventory.entries.Length; i++)
				{
					EntitlementInventory.Entry entry = _profile.entitlementInventory.entries[i];
					instance.SetEntitlementShown(entry.gameIdGuid);
				}
			}
			CampaignCatalog.Campaign[] campaigns = Singleton<StateManager>.Instance.TemplateManager.CampaignCatalog.campaigns;
			foreach (CampaignCatalog.Campaign campaign in campaigns)
			{
				for (int k = 0; k < campaign.missions.Length; k++)
				{
					MissionsAtLocation missionsAtLocation = campaign.missions[k];
					for (int l = 0; l < missionsAtLocation.missions.Length; l++)
					{
						MissionInfo missionInfo = missionsAtLocation.missions[l];
						if (missionInfo.HasCompletedMission())
						{
							UbiCore.Utils.Singleton<LocalStorage>.Instance.SetMissionProgressCompleted(missionInfo.baseName, true);
							UbiCore.Utils.Singleton<LocalStorage>.Instance.SetMissionProgressUnlocked(missionInfo.baseName, true);
							TutorialEntitlementDescription tutorialDescription = missionInfo.GetTutorialDescription();
							if (tutorialDescription != null)
							{
								UbiCore.Utils.Singleton<LocalStorage>.Instance.SetEntitlementShown(tutorialDescription.EntitlementGUID.Guid.ToString());
							}
						}
					}
				}
			}
			for (int m = 0; m < _profile.achievements.entries.Length; m++)
			{
				Achievement achievement = _profile.achievements.entries[m];
				AchievementMapping.Definition achievementMappingDefinition = Singleton<StateManager>.Instance.AchievementsManager.GetAchievementMappingDefinition(achievement.definitionId);
				if (achievementMappingDefinition == null)
				{
					continue;
				}
				for (int n = 0; n < achievement.ranks.Length; n++)
				{
					AchievementRank achievementRank = achievement.ranks[n];
					if (achievementRank.state == EAchievementState.AchievementClaimed)
					{
						UbiCore.Utils.Singleton<LocalStorage>.Instance.SetAchievementRead(achievementMappingDefinition.id, achievementRank.idx);
					}
				}
			}
		}
	}
}
