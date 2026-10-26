using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Mail;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.InventoryScreen;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.ShopScreen;
using Assets.Scripts.UI.Screens.SkillTreeScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;
using Assets.Scripts.UI.Tutorial.Flow;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial
{
	internal class TutorialHelper
	{
		public class TutorialItemCompare : ItemCompareBase
		{
			private readonly ItemCompareBase m_default = new ItemCompareDefault();

			private readonly string[] m_itemsInFrontTechnicalNames;

			public TutorialItemCompare(string[] _itemsInFrontTechnicalNames)
			{
				m_itemsInFrontTechnicalNames = _itemsInFrontTechnicalNames;
			}

			public override int Compare(InventoryItem _lhs, InventoryItem _rhs)
			{
				int num = m_itemsInFrontTechnicalNames.IndexOf(_lhs.Element.technicalName);
				int num2 = m_itemsInFrontTechnicalNames.IndexOf(_rhs.Element.technicalName);
				if (num >= 0 && num2 >= 0)
				{
					return num - num2;
				}
				if (num >= 0)
				{
					return -1;
				}
				if (num2 >= 0)
				{
					return 1;
				}
				return m_default.Compare(_lhs, _rhs);
			}
		}

		public class TutorialMysteryBoxItemCompare : ItemCompareBase
		{
			private readonly ItemCompareBase m_default = new ItemCompareDefault();

			public override int Compare(InventoryItem _lhs, InventoryItem _rhs)
			{
				if (_lhs.Element.mysteryBoxData != null && _lhs.Element.mysteryBoxData.mysteryBoxType == EMysteryBoxType.MysteryBoxItemType_Tutorial)
				{
					return -1;
				}
				if (_rhs.Element.mysteryBoxData != null && _rhs.Element.mysteryBoxData.mysteryBoxType == EMysteryBoxType.MysteryBoxItemType_Tutorial)
				{
					return 1;
				}
				return m_default.Compare(_lhs, _rhs);
			}
		}

		public const EEra kDefaultEra = EEra.Italy;

		public const string kDefaultRegionGameId = "0ab7ec1e-66cd-403a-bef7-622fa6f004b2";

		public const string kDefaultMissionName = "story_0001";

		public const string kDefaultButtonHighlightPrefab = "UI/Misc/ButtonHighlight";

		public const float kDefaultPopupFadeInDelay = 1f;

		public const float kDefaultHighlightFadeInDelay = 1.5f;

		public const int kNumberEncryptedItemsNeeded = 15;

		public const int kNumberWeaponHacksNeeded = 1;

		public const int kNumberDecryptAlgorithmsNeeded = 1;

		public const int kShopTutorialTriggerAssassinLevel = 3;

		public const int kShopTutorialTriggerMailHeaderId = 4668;

		public const int kShopTutorialTriggerMailBodyId = 4670;

		public const int kForgeLevel = 5;

		public const Quality.EQuality kForgeQuality = Quality.EQuality.Mundane;

		public const int kQuestInfoTutorialTriggerHeroicRankToSelect = 1;

		public const int FakeTrainingSkillPointsCost = 1;

		public const int HeroicRankToBeBoughtInTutorial = 1;

		public static string TutorialCampaignId
		{
			get
			{
				return Singleton<StateManager>.Instance.TemplateManager.Globals.tutorialCampaignGuidStr;
			}
		}

		public static string TutorialEquipItem
		{
			get
			{
				return Singleton<StateManager>.Instance.TemplateManager.Globals.tutorialEquipItemGuidStr;
			}
		}

		public static string TutorialEquipModule
		{
			get
			{
				return Singleton<StateManager>.Instance.TemplateManager.Globals.tutorialEquipModuleGuidStr;
			}
		}

		public static string TutorialShopSpecialItem
		{
			get
			{
				return Singleton<StateManager>.Instance.TemplateManager.Globals.tutorialShopSpecialItemGuidStr;
			}
		}

		public static string TutorialShopItemToHide
		{
			get
			{
				return Singleton<StateManager>.Instance.TemplateManager.Globals.tutorialShopItemToHideGuidStr;
			}
		}

		public static string TutorialShopHeroicRank1Item
		{
			get
			{
				return Singleton<StateManager>.Instance.TemplateManager.Globals.tutorialShopHeroicRank1GuidStr;
			}
		}

		public static string TutorialForgeItemOne
		{
			get
			{
				return Singleton<StateManager>.Instance.TemplateManager.Globals.tutorialForgeItemOneGuidStr;
			}
		}

		public static string TutorialForgeItemTwo
		{
			get
			{
				return Singleton<StateManager>.Instance.TemplateManager.Globals.tutorialForgeItemTwoGuidStr;
			}
		}

		public static AcierProtocol.Tutorial.Category GetProfileTutorialCategory(UserProfile _profile, ETutorialCategory _category)
		{
			int num = _profile.tutorial.categories.Length;
			for (int i = 0; i < num; i++)
			{
				AcierProtocol.Tutorial.Category category = _profile.tutorial.categories[i];
				if (category.category == _category)
				{
					return category;
				}
			}
			return null;
		}

		public static TutorialCategoryMapping.Entry GetCategoryMapping(ETutorialCategory _category)
		{
			TutorialCategoryMapping tutorialCategoryMapping = Singleton<StateManager>.Instance.TemplateManager.TutorialCategoryMapping;
			for (int i = 0; i < tutorialCategoryMapping.entries.Length; i++)
			{
				TutorialCategoryMapping.Entry entry = tutorialCategoryMapping.entries[i];
				if (entry.tutorialCategory == _category)
				{
					return entry;
				}
			}
			// A category the mapping does not list (offline mapping, newer categories): no entitlements either way.
			// Callers read the arrays without a null check.
			return new TutorialCategoryMapping.Entry
			{
				tutorialCategory = _category,
				grantedEntitlements = new string[0],
				reqEntitlements = new string[0]
			};
		}

		public static bool OwningRequiredEntitlements(ETutorialCategory _category)
		{
			string[] reqEntitlements = GetCategoryMapping(_category).reqEntitlements;
			return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlements(reqEntitlements);
		}

		public static bool OwningFinishedEntitlements(ETutorialCategory _category)
		{
			string[] grantedEntitlements = GetCategoryMapping(_category).grantedEntitlements;
			return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlements(grantedEntitlements);
		}

		public static int GetCurrentAssassinLevel()
		{
			int result = -1;
			Assassin currentAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
			if (currentAssassin != null)
			{
				result = currentAssassin.level;
			}
			return result;
		}

		public static int AssassinEquippedItemCount()
		{
			int result = 0;
			if (Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin != null)
			{
				result = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.equippedElements.Length;
			}
			return result;
		}

		public static bool CheckMailCacheForShopTutorialTriggerMail()
		{
			bool result = false;
			List<MailHeader> headers = MailCache.Headers;
			if (headers != null)
			{
				for (int i = 0; i < headers.Count; i++)
				{
					MailHeader mailHeader = headers[i];
					if (mailHeader.subjectLangKey != null && mailHeader.subjectLangKey.neutral == 4668)
					{
						MailBody mailBody = MailCache.GetMailBody(mailHeader);
						if (mailBody != null && mailBody.bodyLangKey != null && mailBody.bodyLangKey.neutral == 4670)
						{
							result = true;
							break;
						}
					}
				}
			}
			return result;
		}

		public static bool JustFinishedFirstStoryMissionInTutorial(bool _success, MissionRootData _missionData)
		{
			bool result = false;
			if (_success && _missionData.BaseName.ToLower().Contains("story_0001".ToLower()))
			{
				UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
				if (profile != null)
				{
					TutorialCategory category = UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.GetCategory(ETutorialCategory.TutCatGuideEraSelect);
					if (category != null)
					{
						int index = category.Index;
						if (index + 1 < UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.Categories.Length)
						{
							TutorialCategory tutorialCategory = UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.Categories[index + 1];
							result = !tutorialCategory.Finished && (int)tutorialCategory.CurrentFlowId <= 0;
						}
					}
				}
			}
			return result;
		}

		public static SkillTreeDetails GetSkillTreeDetails(EScreenID _screenId)
		{
			UIScreen screen = Singleton<UIScreenManager>.Instance.GetScreen(_screenId);
			return (!(screen != null)) ? null : screen.gameObject.GetComponentInChildren<SkillTreeDetails>();
		}

		public static bool InRange(long _value, EFlowId _min, EFlowId _max)
		{
			return (long)_min <= _value && _value <= (long)_max;
		}

		public static bool InRangeExcludeMax(long _value, EFlowId _min, EFlowId _max)
		{
			return (long)_min <= _value && _value < (long)_max;
		}

		public static void TrackIntroMissionStep()
		{
			TutorialCategory category = UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.GetCategory(ETutorialCategory.TutCatIntroMission);
			if (!category.Finished && Main.Instance.IsNetworkEnabled)
			{
				ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
				profileManager.SetTutorialStep(ETutorialCategory.TutCatIntroMission, 0);
			}
		}

		public static int GetFakeTrainSkillPoints()
		{
			return Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.skillPoints - 1;
		}

		public static int GetFakeTrainSkillPointsSpent()
		{
			return Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.PointSpent() + 1;
		}

		public static UIInventoryItemView GetInventoryItemView(List<Transform> _inventoryChildren, int _index)
		{
			for (int i = 0; i < _inventoryChildren.Count; i++)
			{
				Transform transform = _inventoryChildren[i];
				UIInventoryScrollItem component = transform.GetComponent<UIInventoryScrollItem>();
				if (component.Index == _index)
				{
					return transform.GetComponent<UIInventoryItemView>();
				}
			}
			return null;
		}

		public static bool HasClaimableQuest(out int _rank)
		{
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			for (int num = profile.maxRankIdx; num >= 0; num--)
			{
				ProgressionQuests.RankQuests questByRank = profile.GetQuestByRank(num);
				if (questByRank != null && questByRank.HasClaimableQuest())
				{
					_rank = num;
					return true;
				}
			}
			_rank = -1;
			return false;
		}

		public static void PreInitInventorySorting(string[] _itemsInFrontTechnicalNames)
		{
			GetInventoryHandler().PreInitSortingComparer(new TutorialItemCompare(_itemsInFrontTechnicalNames));
		}

		public static UIInventoryHandler GetInventoryHandler()
		{
			UIScreen screen = Singleton<UIScreenManager>.Instance.GetScreen(EScreenID.InventoryScreen);
			UIInventoryHandler[] componentsInChildren = screen.GetComponentsInChildren<UIInventoryHandler>(true);
			return componentsInChildren[0];
		}

		public static bool ConditionAllowShowGuideEquip(TutorialFlowBase _tutorialFlowBase)
		{
			return ConditionDefaultCheck(_tutorialFlowBase) && HasTheItemToEquip(TutorialEquipItem);
		}

		public static bool ConditionAllowShowGuideSkillPointsNotInAssassinSelection(TutorialFlowBase _tutorialFlowBase)
		{
			return ConditionAllowShowGuideSkillPoints(_tutorialFlowBase) && Singleton<UIScreenManager>.Instance.CurrentScreenId != EScreenID.AssassinSelection;
		}

		public static bool ConditionAllowShowGuideSkillPoints(TutorialFlowBase _tutorialFlowBase)
		{
			return ConditionDefaultCheck(_tutorialFlowBase) && Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.skillPoints > 0 && CanFirstActiveSkillBeTrained();
		}

		public static bool ConditionAllowShowGuideSkillPointsAfterFinishedTraining(TutorialFlowBase _tutorialFlowBase)
		{
			return ConditionDefaultCheck(_tutorialFlowBase, true);
		}

		public static bool ConditionAllowShowGuideShop(TutorialFlowBase _tutorialFlowBase)
		{
			if (!ConditionDefaultCheck(_tutorialFlowBase))
			{
				return false;
			}
			ShopTable shopTable = Singleton<StateManager>.Instance.TemplateManager.ShopTable;
			ShopBundle shopBundleById = shopTable.GetShopBundleById(TutorialShopSpecialItem);
			AcierProtocol.Resources costs = shopBundleById.costs;
			return Singleton<StateManager>.Instance.ProfileManager.Profile.HasEnoughResources(costs);
		}

		public static bool ConditionAllowShowGuideModules(TutorialFlowBase _tutorialFlowBase)
		{
			return ConditionDefaultCheck(_tutorialFlowBase) && HasTheItemToEquip(TutorialEquipModule);
		}

		public static bool ConditionAllowShowGuideForge(TutorialFlowBase _tutorialFlowBase)
		{
			return ConditionDefaultCheck(_tutorialFlowBase) && CanForge(TutorialForgeItemOne) && CanForge(TutorialForgeItemTwo);
		}

		public static bool ConditionAllowShowGuideForgeAfterForge(TutorialFlowBase _tutorialFlowBase)
		{
			return ConditionDefaultCheck(_tutorialFlowBase, true);
		}

		private static bool CanForge(string _itemTechnicalName)
		{
			InventoryElement inventoryElement = InventoryExtensions.FindItemWithTechnicalName(_itemTechnicalName);
			return inventoryElement != null && inventoryElement.CanBeForged();
		}

		public static bool ConditionAllowShowGuideMysteryBox(TutorialFlowBase _tutorialFlowBase)
		{
			return ConditionDefaultCheck(_tutorialFlowBase) && HasTutorialMysteryBox();
		}

		private static bool HasTutorialMysteryBox()
		{
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			InventoryElement[] elements = profileManager.Profile.Inventory.elements;
			int num = elements.Length;
			for (int i = 0; i < num; i++)
			{
				InventoryElement inventoryElement = elements[i];
				if (inventoryElement.mysteryBoxData != null && inventoryElement.mysteryBoxData.mysteryBoxType == EMysteryBoxType.MysteryBoxItemType_Tutorial)
				{
					return true;
				}
			}
			return false;
		}

		public static bool ConditionAllowShowGuideHirelings(TutorialFlowBase _tutorialFlowBase)
		{
			return ConditionDefaultCheck(_tutorialFlowBase) && Singleton<UIScreenManager>.Instance.CurrentScreenId == _tutorialFlowBase.GetScreenId();
		}

		public static bool ConditionAllowShowGuideCodexTab(TutorialFlowBase _tutorialFlowBase)
		{
			return ConditionDefaultCheck(_tutorialFlowBase);
		}

		public static long GetCostOfType(AcierProtocol.Resources _resources, EResourceType _type)
		{
			long result = 0L;
			if (_resources != null)
			{
				result = GetCostOfType(_resources.resources, _type);
			}
			return result;
		}

		public static long GetCostOfType(Resource[] _resources, EResourceType _type)
		{
			long num = 0L;
			foreach (Resource resource in _resources)
			{
				if (resource.type == _type)
				{
					num += resource.amount;
				}
			}
			return num;
		}

		public static long GetCostOfType(List<UIPopupResourcesContent.ResourceConfiguration> _resourceConfigurationList, EResourceType _type)
		{
			long num = 0L;
			if (_resourceConfigurationList != null)
			{
				for (int i = 0; i < _resourceConfigurationList.Count; i++)
				{
					UIPopupResourcesContent.ResourceConfiguration resourceConfiguration = _resourceConfigurationList[i];
					Resource resource = resourceConfiguration.Resource;
					if (resource.type == _type)
					{
						num += resource.amount;
					}
				}
			}
			return num;
		}

		public static bool ConditionAllowShowHeroicWithoutRank(TutorialFlowBase _tutorialFlowBase)
		{
			return ConditionDefaultCheck(_tutorialFlowBase) && !EntitlementExtensions.IsHeroicRankGranted(1) && IsHeroicRankAffordable();
		}

		private static bool IsHeroicRankAffordable()
		{
			UIShopDetails shopDetails = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails;
			ShopCategoryNode categoryRootNode = shopDetails.CategoryRootNode;
			ShopItemNode shopItemById = categoryRootNode.GetShopItemById(TutorialShopHeroicRank1Item);
			return Singleton<StateManager>.Instance.ProfileManager.Profile.HasEnoughResources(shopItemById.ShopBundle.costs);
		}

		public static bool ConditionAllowShowHeroicWithRank(TutorialFlowBase _tutorialFlowBase)
		{
			return ConditionDefaultCheck(_tutorialFlowBase) && EntitlementExtensions.IsHeroicRankGranted(1);
		}

		public static bool ConditionAllowShowQuestRewards(TutorialFlowBase _tutorialFlowBase, out int _rank)
		{
			_rank = -1;
			return ConditionDefaultCheck(_tutorialFlowBase, false, true) && Singleton<UIScreenManager>.Instance.CurrentScreenId == _tutorialFlowBase.GetScreenId() && HasClaimableQuest(out _rank);
		}

		public static string ExchangeCurrencyInString(int _translationId, EResourceType _resourceType)
		{
			string value = "?";
			CurrencyDescription value2;
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.CurrencyDescriptions.TryGetValue((int)_resourceType, out value2))
			{
				value = LocalizationManager.Instance.Get(value2.LocaKey);
			}
			string key = string.Empty;
			switch (_resourceType)
			{
			case EResourceType.HardCurrency:
				key = "HardCurrency_Name";
				break;
			case EResourceType.SoftCurrency:
				key = "SoftCurrency_Name";
				break;
			}
			return LocalizationManager.Instance.Get(_translationId, new StringMapper { { key, value } });
		}

		public static bool ConditionDefaultCheck(TutorialFlowBase _tutorialFlowBase, bool _checkForPreviousActive = false, bool _ignoreInvalid = false)
		{
			return Main.Instance.SceneLoader.InAnimus && CheckStep(_tutorialFlowBase, _ignoreInvalid) && (!_checkForPreviousActive || UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.GetCategory(_tutorialFlowBase.Category).AnyFlowWasActive);
		}

		public static ShopBundle GetForgeBlueprintShopBundle()
		{
			string forgeTutorialInputBlueprintId = Singleton<StateManager>.Instance.TemplateManager.Globals.forgeTutorialInputBlueprintId;
			return Singleton<StateManager>.Instance.TemplateManager.ShopTable.GetShopBundleById(forgeTutorialInputBlueprintId);
		}

		public static Resource CalculateRerollCost()
		{
			ForgeMapping forgeMapping = Singleton<StateManager>.Instance.TemplateManager.ForgeMapping;
			return forgeMapping.reRollCosts.Find((ForgeMapping.ReRollCost _x) => _x.quality == Quality.EQuality.Mundane).cost;
		}

		private static bool CheckStep(TutorialFlowBase _tutorialFlowBase, bool _ignoreInvalid)
		{
			STutorialFlowId currentTutorialStepOfCategory = GetCurrentTutorialStepOfCategory(_tutorialFlowBase.Category);
			if (currentTutorialStepOfCategory == _tutorialFlowBase.Index)
			{
				return true;
			}
			return _ignoreInvalid && currentTutorialStepOfCategory < 0L;
		}

		private static STutorialFlowId GetCurrentTutorialStepOfCategory(ETutorialCategory _category)
		{
			TutorialCategory category = UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.GetCategory(_category);
			return (!(category != null)) ? STutorialFlowId.Invalid : category.CurrentFlowId;
		}

		private static bool HasTheItemToEquip(string _technicalName)
		{
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			InventoryElement[] elements = profileManager.Profile.Inventory.elements;
			int num = elements.Length;
			for (int i = 0; i < num; i++)
			{
				InventoryElement inventoryElement = elements[i];
				if (inventoryElement.technicalName.Equals(_technicalName))
				{
					return true;
				}
			}
			return false;
		}

		private static bool CanFirstActiveSkillBeTrained()
		{
			Assassin currentAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
			AssassinSkillTier_ assassinSkillTier_ = currentAssassin.baseClassskillTiers.skillTiers[0];
			AssassinSkills skills = assassinSkillTier_.skills;
			return skills.activeSkill.unlocked && skills.activeSkill.level < 5;
		}
	}
}
