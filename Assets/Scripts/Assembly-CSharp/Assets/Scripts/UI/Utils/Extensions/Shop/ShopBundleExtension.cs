using System;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Network.Missions.Helpers;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Screens.MysteryBox;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager;
using Assets.Scripts.UI.State.EventManager.Data;
using Assets.Scripts.UI.State.EventManager.HppyCourCategoryManager;
using Assets.Scripts.UI.Tutorial;
using Assets.Scripts.UI.Types;
using Database;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Utils.Extensions.Shop
{
	public static class ShopBundleExtension
	{
		public const string _locationAvailableString = "location available";

		public const string _missionTypeAvailableString = "mission type available";

		public static bool IsLocationEntitlementBundle(this ShopBundle _shopBundle)
		{
			return _shopBundle.payout.lootTemplates.Any(delegate(LootTemplate x)
			{
				if (x.entitlementTemplate == null)
				{
					return false;
				}
				GameID gameID = Main.Instance.GameDB.FindGameIdFromGuid(x.entitlementTemplate.gameIdGuid);
				EntitlementDefinition item = Main.Instance.GameDB.GetItem<EntitlementDefinition>(gameID);
				return item.Name.ToLowerInvariant().StartsWith("location available");
			});
		}

		public static bool IsMissionTypeEntitlementBundle(this ShopBundle _shopBundle)
		{
			return _shopBundle.payout.lootTemplates.Any(delegate(LootTemplate x)
			{
				if (x.entitlementTemplate == null)
				{
					return false;
				}
				GameID gameID = Main.Instance.GameDB.FindGameIdFromGuid(x.entitlementTemplate.gameIdGuid);
				EntitlementDefinition item = Main.Instance.GameDB.GetItem<EntitlementDefinition>(gameID);
				return item.Name.ToLowerInvariant().StartsWith("mission type available");
			});
		}

		public static bool IsCategoryShownInShop(this EShopCategory _shopCategory)
		{
			bool result = true;
			EventManager eventManager = Singleton<StateManager>.Instance.EventManager;
			EventCategoryVisibilityManager eventCategoryVisibilityManager = (EventCategoryVisibilityManager)eventManager.GetEventManger(EEventType.Event_Shop_Category_Visibility);
			if (eventCategoryVisibilityManager != null && !eventCategoryVisibilityManager.IsCategoryVisible(_shopCategory))
			{
				result = false;
			}
			return result;
		}

		public static bool IsAnUnlockCampaignBundle(this ShopBundle _shopBundle, ref CampaignCatalog.Campaign _campaign)
		{
			if (_shopBundle.HasSoloEntitlemetnPayout())
			{
				CampaignCatalog campaignCatalog = Singleton<StateManager>.Instance.TemplateManager.CampaignCatalog;
				for (int i = 0; i < campaignCatalog.campaigns.Length; i++)
				{
					CampaignCatalog.Campaign campaign = campaignCatalog.campaigns[i];
					if (campaign.reqEntitlements.Length > 0 && _shopBundle.HasEntitlementsPayout(campaign.reqEntitlements))
					{
						_campaign = campaign;
						return true;
					}
				}
			}
			return false;
		}

		public static bool IsHeroicBundle(this ShopBundle _shopBundle)
		{
			return _shopBundle.category == EShopCategory.Shopcat_Heroic;
		}

		public static bool IsUnlocked(this ShopBundle _shopBundle)
		{
			string[] reqEntitlementGuids = _shopBundle.reqEntitlementGuids;
			if (reqEntitlementGuids.Length > 0)
			{
				return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlements(reqEntitlementGuids);
			}
			return true;
		}

		public static bool IsAlreadyOwned(this ShopBundle _shopBundle)
		{
			string[] entitlementsFromLootTemplate = _shopBundle.payout.lootTemplates.GetEntitlementsFromLootTemplate();
			if (entitlementsFromLootTemplate.Length > 0)
			{
				return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlements(entitlementsFromLootTemplate);
			}
			return false;
		}

		public static bool HasHeroicRankPayout(this ShopBundle _shopBundle)
		{
			string[] entitlementsFromLootTemplate = _shopBundle.payout.lootTemplates.GetEntitlementsFromLootTemplate();
			if (entitlementsFromLootTemplate.Length > 0)
			{
				HeroicRankMapping.Entry[] heroicRankMapping = Singleton<StateManager>.Instance.TemplateManager.HeroicRankMapping;
				foreach (HeroicRankMapping.Entry entry in heroicRankMapping)
				{
					if (entry == null)
					{
						continue;
					}
					foreach (string text in entitlementsFromLootTemplate)
					{
						if (entry.entitlement == text)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public static bool HasEntitlementsPayout(this ShopBundle _shopBundle, string[] _entitlementPayout)
		{
			string[] entitlementsFromLootTemplate = _shopBundle.payout.lootTemplates.GetEntitlementsFromLootTemplate();
			if (entitlementsFromLootTemplate.Length > 0)
			{
				for (int i = 0; i < _entitlementPayout.Length; i++)
				{
					bool flag = false;
					string strA = _entitlementPayout[i];
					for (int j = 0; j < entitlementsFromLootTemplate.Length; j++)
					{
						string strB = entitlementsFromLootTemplate[i];
						if (string.Compare(strA, strB, StringComparison.InvariantCultureIgnoreCase) == 0)
						{
							flag = true;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public static bool TryGetHeroicRank(this ShopBundle _shopBundle, out int _rank)
		{
			string[] entitlementsFromLootTemplate = _shopBundle.payout.lootTemplates.GetEntitlementsFromLootTemplate();
			if (entitlementsFromLootTemplate.Length > 0)
			{
				HeroicRankMapping.Entry[] heroicRankMapping = Singleton<StateManager>.Instance.TemplateManager.HeroicRankMapping;
				foreach (HeroicRankMapping.Entry entry in heroicRankMapping)
				{
					if (entry == null)
					{
						continue;
					}
					foreach (string text in entitlementsFromLootTemplate)
					{
						if (entry.entitlement == text)
						{
							_rank = entry.rankIdx;
							return true;
						}
					}
				}
			}
			_rank = -1;
			return false;
		}

		public static bool HasCampaignUnlock(this ShopBundle _shopBundle)
		{
			return _shopBundle.HasEntitlementsPayout(new string[1] { StaticEntitlementIds.CampaignAvailableForli });
		}

		public static bool IsEnabled(this ShopBundle _shopBundle)
		{
			bool result = _shopBundle.enabled;
			EventManager eventManager = Singleton<StateManager>.Instance.EventManager;
			EventShopBundleVisibilityManager eventShopBundleVisibilityManager = (EventShopBundleVisibilityManager)eventManager.GetEventManger(EEventType.Event_Shop_Item_Visibility);
			if (eventShopBundleVisibilityManager != null)
			{
				EventShopBundleVisibility shopBundleVisibilityEvent = eventShopBundleVisibilityManager.GetShopBundleVisibilityEvent(_shopBundle.name);
				if (shopBundleVisibilityEvent != null && shopBundleVisibilityEvent.LastValidState)
				{
					result = shopBundleVisibilityEvent.IsVisible;
				}
			}
			return result;
		}

		public static bool IsShownInShop(this ShopBundle _shopBundle, bool _isTutorial = false)
		{
			if (_isTutorial)
			{
				if (_shopBundle.id == TutorialHelper.TutorialShopSpecialItem)
				{
					return true;
				}
				if (_shopBundle.id == TutorialHelper.TutorialShopItemToHide)
				{
					return false;
				}
			}
			bool flag = _shopBundle.enabled;
			EventManager eventManager = Singleton<StateManager>.Instance.EventManager;
			if (!_shopBundle.category.IsCategoryShownInShop())
			{
				flag = false;
			}
			else
			{
				EventShopBundleVisibilityManager eventShopBundleVisibilityManager = (EventShopBundleVisibilityManager)eventManager.GetEventManger(EEventType.Event_Shop_Item_Visibility);
				if (eventShopBundleVisibilityManager != null)
				{
					EventShopBundleVisibility shopBundleVisibilityEvent = eventShopBundleVisibilityManager.GetShopBundleVisibilityEvent(_shopBundle.id);
					if (shopBundleVisibilityEvent != null && shopBundleVisibilityEvent.LastValidState)
					{
						flag = shopBundleVisibilityEvent.IsVisible;
					}
				}
			}
			if (flag && _shopBundle.HasCampaignUnlock())
			{
				flag = false;
			}
			return flag;
		}

		public static bool IsDirectPurchasesBundle(this ShopBundle _shopBundle)
		{
			return !string.IsNullOrEmpty(_shopBundle.houstonId);
		}

		public static bool IsBundleWithSoloItemType(this ShopBundle _shopBundle, Item.EItemCategory _type)
		{
			if (_shopBundle.payout.lootTemplates.Length == 1)
			{
				LootTemplate lootTemplate = _shopBundle.payout.lootTemplates[0];
				return lootTemplate.itemTemplate != null && lootTemplate.itemTemplate.category == _type;
			}
			return false;
		}

		public static bool IsSoloOutFit(this ShopBundle _shopBundle)
		{
			return _shopBundle.IsBundleWithSoloItemType(Item.EItemCategory.Outfit);
		}

		public static bool IsMysteryBox(this ShopBundle _shopBundle, bool _isSingleItem = true)
		{
			return _shopBundle.payout.lootTemplates.IsMysteryBox(_isSingleItem);
		}

		public static bool IsBluePrint(this ShopBundle _shopBundle, bool _isSingleItem = true)
		{
			return _shopBundle.payout.lootTemplates.IsBluePrint(_isSingleItem);
		}

		public static bool IsCustomLootItemType(this ShopBundle _shopBundle, ECustomLootTemplateType _type, bool _isSingleItem = true)
		{
			return _shopBundle.payout.lootTemplates.IsCustomItemType(_type, _isSingleItem);
		}

		public static bool HasSoloEntitlemetnPayout(this ShopBundle _shopBundle)
		{
			if (_shopBundle.payout.lootTemplates.Length == 1)
			{
				LootTemplate lootTemplate = _shopBundle.payout.lootTemplates[0];
				return lootTemplate.entitlementTemplate != null;
			}
			return false;
		}

		public static string LocaName(this ShopBundle _shopBundle, StringMapper _mapper = null)
		{
			if (_mapper == null)
			{
				_mapper = _shopBundle.GetShopBundleTitleStringMapper();
			}
			return LocalizationManager.Instance.Get(_shopBundle.nameLangKey.neutral, _mapper);
		}

		public static string LocaDescription(this ShopBundle _shopBundle, StringMapper _mapper = null)
		{
			if (_mapper == null)
			{
				_mapper = _shopBundle.GetShopBundleStringMapper();
			}
			return LocalizationManager.Instance.Get(_shopBundle.descLangKey.neutral, _mapper);
		}

		public static string LocaShortDescription(this ShopBundle _shopBundle, StringMapper _mapper = null)
		{
			if (_shopBundle.shortDescLangKey == null)
			{
				return string.Empty;
			}
			if (_mapper == null)
			{
				_mapper = _shopBundle.GetShopBundleStringMapper();
			}
			return LocalizationManager.Instance.Get(_shopBundle.shortDescLangKey.neutral, _mapper);
		}

		public static bool GetShopCategoryDiscountData(this EShopCategory _category, out long _earliestEndTime, out int _discountValue)
		{
			bool result = false;
			int _currentDiscountValue = 0;
			long _earliestDiscountEndTime = long.MaxValue;
			EventManager eventManager = Singleton<StateManager>.Instance.EventManager;
			EventCategoryPercentageManager eventCategoryPercentageManager = (EventCategoryPercentageManager)eventManager.GetEventManger(EEventType.Event_Shop_Category_Percentage);
			BBList<EventCategoryPercentage> categoryPercentageEventsByCategory = eventCategoryPercentageManager.GetCategoryPercentageEventsByCategory(_category);
			for (int i = 0; i < categoryPercentageEventsByCategory.Count; i++)
			{
				EventCategoryPercentage eventCategoryPercentage = categoryPercentageEventsByCategory[i];
				if (eventCategoryPercentage != null && eventCategoryPercentage.LastValidState)
				{
					MultiplyEventDiscountValue(eventCategoryPercentage.Multiplier, eventCategoryPercentage.Event.endTimeUTC, ref _currentDiscountValue, ref _earliestDiscountEndTime);
					result = true;
				}
			}
			_discountValue = _currentDiscountValue;
			_earliestEndTime = _earliestDiscountEndTime;
			return result;
		}

		public static bool GetDiscountData(this ShopBundle _shopBundle, out long _earliestEndTime, out int _discountValue)
		{
			bool result = false;
			int _discountValue2 = 0;
			long _earliestEndTime2 = long.MaxValue;
			if (!_shopBundle.IsAlreadyOwned())
			{
				EventManager eventManager = Singleton<StateManager>.Instance.EventManager;
				result = _shopBundle.category.GetShopCategoryDiscountData(out _earliestEndTime2, out _discountValue2);
				EventShopBundlePercentageManager eventShopBundlePercentageManager = (EventShopBundlePercentageManager)eventManager.GetEventManger(EEventType.Event_Shop_Item_Percentage);
				BBList<EventShopBundlePercentage> shopBundlePercentageEvents = eventShopBundlePercentageManager.GetShopBundlePercentageEvents(_shopBundle.id);
				for (int i = 0; i < shopBundlePercentageEvents.Count; i++)
				{
					EventShopBundlePercentage eventShopBundlePercentage = shopBundlePercentageEvents[i];
					if (eventShopBundlePercentage != null && eventShopBundlePercentage.LastValidState)
					{
						MultiplyEventDiscountValue(eventShopBundlePercentage.Multiplier, eventShopBundlePercentage.Event.endTimeUTC, ref _discountValue2, ref _earliestEndTime2);
						result = true;
					}
				}
			}
			_earliestEndTime = _earliestEndTime2;
			_discountValue = _discountValue2;
			return result;
		}

		public static long GetAvailableEndTimer(this ShopBundle _shopBundle)
		{
			long num = long.MaxValue;
			EventManager eventManager = Singleton<StateManager>.Instance.EventManager;
			EventShopBundlePercentageManager eventShopBundlePercentageManager = (EventShopBundlePercentageManager)eventManager.GetEventManger(EEventType.Event_Shop_Item_Percentage);
			BBList<EventShopBundlePercentage> shopBundlePercentageEvents = eventShopBundlePercentageManager.GetShopBundlePercentageEvents(_shopBundle.name);
			for (int i = 0; i < shopBundlePercentageEvents.Count; i++)
			{
				EventShopBundlePercentage eventShopBundlePercentage = shopBundlePercentageEvents[i];
				if (eventShopBundlePercentage != null && eventShopBundlePercentage.LastValidState)
				{
					num = Math.Min(eventShopBundlePercentage.Event.endTimeUTC, num);
				}
			}
			EventCategoryPercentageManager eventCategoryPercentageManager = (EventCategoryPercentageManager)eventManager.GetEventManger(EEventType.Event_Shop_Category_Percentage);
			BBList<EventCategoryPercentage> categoryPercentageEventsByCategory = eventCategoryPercentageManager.GetCategoryPercentageEventsByCategory(_shopBundle.category);
			for (int j = 0; j < categoryPercentageEventsByCategory.Count; j++)
			{
				EventCategoryPercentage eventCategoryPercentage = categoryPercentageEventsByCategory[j];
				if (eventCategoryPercentage != null && eventCategoryPercentage.LastValidState)
				{
					num = Math.Min(eventCategoryPercentage.Event.endTimeUTC, num);
				}
			}
			EventShopBundleVisibilityManager eventShopBundleVisibilityManager = (EventShopBundleVisibilityManager)eventManager.GetEventManger(EEventType.Event_Shop_Item_Visibility);
			EventShopBundleVisibility shopBundleVisibilityEvent = eventShopBundleVisibilityManager.GetShopBundleVisibilityEvent(_shopBundle.name);
			if (shopBundleVisibilityEvent != null && shopBundleVisibilityEvent.LastValidState)
			{
				num = Math.Min(shopBundleVisibilityEvent.Event.endTimeUTC, num);
			}
			EventCategoryVisibilityManager eventCategoryVisibilityManager = (EventCategoryVisibilityManager)eventManager.GetEventManger(EEventType.Event_Shop_Category_Visibility);
			EventCategoryVisibility categoryVisibilityEvent = eventCategoryVisibilityManager.GetCategoryVisibilityEvent(_shopBundle.category);
			if (categoryVisibilityEvent != null && categoryVisibilityEvent.LastValidState)
			{
				num = Math.Min(categoryVisibilityEvent.Event.endTimeUTC, num);
			}
			if (num != long.MaxValue)
			{
				return num;
			}
			return 0L;
		}

		public static bool GetHappyHourData(this ShopBundle _shopBundle, out long _earliestEndTime, out float _bonusValue)
		{
			bool result = false;
			float _bonusValue2 = 0f;
			long _earliestEndTime2 = long.MaxValue;
			EventManager eventManager = Singleton<StateManager>.Instance.EventManager;
			if (!_shopBundle.IsAlreadyOwned())
			{
				result = _shopBundle.category.GetShopCategoryHappyHourData(out _earliestEndTime2, out _bonusValue2);
				EventShopCategoryHappyHourManager eventShopCategoryHappyHourManager = (EventShopCategoryHappyHourManager)eventManager.GetEventManger(EEventType.Event_Shop_Category_Payout_Increase);
				BBList<EventShopCategoryHappyHour> happyHourEvents = eventShopCategoryHappyHourManager.GetHappyHourEvents(_shopBundle.name);
				for (int i = 0; i < happyHourEvents.Count; i++)
				{
					EventShopCategoryHappyHour eventShopCategoryHappyHour = happyHourEvents[i];
					if (eventShopCategoryHappyHour != null && eventShopCategoryHappyHour.LastValidState)
					{
						_bonusValue2 += eventShopCategoryHappyHour.Multiplier;
						result = true;
					}
				}
			}
			_earliestEndTime = _earliestEndTime2;
			_bonusValue = _bonusValue2;
			return result;
		}

		public static bool IsPayoutIncreaseActForCategory(EShopCategory _category)
		{
			EventManager eventManager = Singleton<StateManager>.Instance.EventManager;
			EventShopCategoryHappyHourManager eventShopCategoryHappyHourManager = (EventShopCategoryHappyHourManager)eventManager.GetEventManger(EEventType.Event_Shop_Category_Payout_Increase);
			BBList<EventShopCategoryHappyHour> happyHourEventsbyByCategory = eventShopCategoryHappyHourManager.GetHappyHourEventsbyByCategory(_category);
			for (int i = 0; i < happyHourEventsbyByCategory.Count; i++)
			{
				EventShopCategoryHappyHour eventShopCategoryHappyHour = happyHourEventsbyByCategory[i];
				if (eventShopCategoryHappyHour != null && eventShopCategoryHappyHour.LastValidState)
				{
					return true;
				}
			}
			return false;
		}

		public static bool GetShopCategoryHappyHourData(this EShopCategory _category, out long _earliestEndTime, out float _bonusValue)
		{
			bool result = false;
			float _currentHappyHourValue = 0f;
			long _earliestHappyHourEndTime = long.MaxValue;
			EventManager eventManager = Singleton<StateManager>.Instance.EventManager;
			EventShopCategoryHappyHourManager eventShopCategoryHappyHourManager = (EventShopCategoryHappyHourManager)eventManager.GetEventManger(EEventType.Event_Shop_Category_Payout_Increase);
			BBList<EventShopCategoryHappyHour> happyHourEventsbyByCategory = eventShopCategoryHappyHourManager.GetHappyHourEventsbyByCategory(_category);
			for (int i = 0; i < happyHourEventsbyByCategory.Count; i++)
			{
				EventShopCategoryHappyHour eventShopCategoryHappyHour = happyHourEventsbyByCategory[i];
				if (eventShopCategoryHappyHour != null && eventShopCategoryHappyHour.LastValidState)
				{
					MultiplyEventHappyHourValue(eventShopCategoryHappyHour.Multiplier, eventShopCategoryHappyHour.Event.endTimeUTC, ref _currentHappyHourValue, ref _earliestHappyHourEndTime);
					result = true;
				}
			}
			_bonusValue = _currentHappyHourValue;
			_earliestEndTime = _earliestHappyHourEndTime;
			return result;
		}

		private static void MultiplyEventDiscountValue(float _multiplyer, long _discountEndTimeUTC, ref int _currentDiscountValue, ref long _earliestDiscountEndTime)
		{
			float num = 1f - (float)_currentDiscountValue / 100f;
			float num2 = 1f - num * _multiplyer;
			int value = MissionMathHelper.FloatToCeilFixed(num2 * 100f);
			_currentDiscountValue = MissionMathHelper.FixedToIntCeil(value);
			_earliestDiscountEndTime = Math.Min(_discountEndTimeUTC, _earliestDiscountEndTime);
		}

		private static void MultiplyEventHappyHourValue(float _multiplyer, long _happyHourEndTimeUTC, ref float _currentHappyHourValue, ref long _earliestHappyHourEndTime)
		{
			float num = 1f + _currentHappyHourValue / 100f;
			float num2 = num * _multiplyer;
			int value = MissionMathHelper.FloatToCeilFixed(Mathf.Abs((1f - num2) * 100f));
			_currentHappyHourValue = MissionMathHelper.FixedToIntCeil(value);
			_earliestHappyHourEndTime = Math.Min(_happyHourEndTimeUTC, _earliestHappyHourEndTime);
		}

		public static int GetDisplayDiscountvalue(this ShopBundle _shopBundle)
		{
			int _discountValue = 0;
			long _earliestEndTime = long.MaxValue;
			_shopBundle.GetDiscountData(out _earliestEndTime, out _discountValue);
			return _discountValue;
		}

		public static Resource GetFinalDiscountedResourceCost(this ShopBundle _shopBundle)
		{
			Resource resource = _shopBundle.GetFirstResourceCost();
			if (resource != null)
			{
				int _discountValue = 0;
				long _earliestEndTime = long.MaxValue;
				if (_shopBundle.GetDiscountData(out _earliestEndTime, out _discountValue))
				{
					Resource resource2 = new Resource();
					resource2.amount = Math.Max(1, Mathf.FloorToInt((float)resource.amount * (100f - (float)_discountValue) / 100f));
					resource2.type = resource.type;
					return resource2;
				}
			}
			else
			{
				Resource resource2 = new Resource();
				resource2.amount = 0L;
				resource2.type = EResourceType.SoftCurrency;
				resource = resource2;
			}
			return resource;
		}

		public static long GetFinalHappyHourPayout(this ShopBundle _shopBundle)
		{
			long num = 0L;
			Resource firstResourcePayout = _shopBundle.GetFirstResourcePayout();
			if (firstResourcePayout != null)
			{
				num = firstResourcePayout.amount;
				float _bonusValue = 0f;
				long _earliestEndTime = long.MaxValue;
				if (_shopBundle.GetHappyHourData(out _earliestEndTime, out _bonusValue))
				{
					return num + Mathf.FloorToInt((float)num / 100f * _bonusValue);
				}
			}
			return num;
		}

		public static Resource GetFirstResourceCost(this ShopBundle _shopBundle)
		{
			if (_shopBundle.costs != null && _shopBundle.costs.resources.Length > 0)
			{
				if (_shopBundle.costs.resources.Length > 1)
				{
				}
				return _shopBundle.costs.resources[0];
			}
			return null;
		}

		public static Resource GetFirstResourcePayout(this ShopBundle _shopBundle)
		{
			LootTemplate.ResourceTemplate firstResourceTemplate = _shopBundle.payout.lootTemplates.GetFirstResourceTemplate();
			return (firstResourceTemplate != null) ? firstResourceTemplate.minResource : null;
		}

		public static int GetInventorySpaceNeededForLootTemplates(this ShopBundle _shopBundle)
		{
			if (_shopBundle.payout != null && _shopBundle.payout.lootTemplates != null)
			{
				return _shopBundle.payout.lootTemplates.GetInventorySpaceNeededForLootTemplates();
			}
			return 0;
		}

		public static bool HasInventorySpaceForBundle(this ShopBundle _shopBundle)
		{
			if (_shopBundle.payout != null && _shopBundle.payout.lootTemplates != null)
			{
				return _shopBundle.payout.lootTemplates.HasInventorySpaceForLootTemplates();
			}
			return true;
		}

		public static bool HasOnlyResourcePayout(this ShopBundle _shopBundle)
		{
			if (_shopBundle.payout.lootTemplates.Length > 0)
			{
				for (int i = 0; i < _shopBundle.payout.lootTemplates.Length; i++)
				{
					LootTemplate lootTemplate = _shopBundle.payout.lootTemplates[i];
					if (lootTemplate.resourceTemplate == null)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public static bool HasOnlyResourceTypePayout(this ShopBundle _shopBundle, EResourceType _resourceType)
		{
			if (_shopBundle.payout.lootTemplates.Length > 0)
			{
				for (int i = 0; i < _shopBundle.payout.lootTemplates.Length; i++)
				{
					LootTemplate lootTemplate = _shopBundle.payout.lootTemplates[i];
					if (lootTemplate.resourceTemplate == null || lootTemplate.resourceTemplate.minResource.type != _resourceType)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public static bool HasWalletPayoutResource(this ShopBundle _shopBundle)
		{
			bool result = false;
			_shopBundle.payout.lootTemplates.DoForResourceTemplates(delegate(LootTemplate.ResourceTemplate _template)
			{
				if (_template.minResource.type == EResourceType.HardCurrency || _template.minResource.type == EResourceType.SocialCurrency || _template.minResource.type == EResourceType.SoftCurrency)
				{
					result = true;
				}
			});
			return result;
		}

		public static LootTemplate GetShopPreviewLootTemplate(this ShopBundle _shopBundle)
		{
			LootTemplate firstItemTemplateWrapper = _shopBundle.payout.lootTemplates.GetFirstItemTemplateWrapper();
			if (firstItemTemplateWrapper != null)
			{
				return firstItemTemplateWrapper;
			}
			for (int i = 0; i < _shopBundle.payout.lootTemplates.Length; i++)
			{
				firstItemTemplateWrapper = _shopBundle.payout.lootTemplates[i];
				if (firstItemTemplateWrapper != null && !string.IsNullOrEmpty(firstItemTemplateWrapper.guid))
				{
					return firstItemTemplateWrapper;
				}
			}
			return null;
		}

		public static GameID GetPreviewImageGameID(this ShopBundle _shopBundle)
		{
			GameID result = GameID.Invalid;
			if (string.IsNullOrEmpty(_shopBundle.itemDefGuid))
			{
				LootTemplate shopPreviewLootTemplate = _shopBundle.GetShopPreviewLootTemplate();
				if (shopPreviewLootTemplate != null)
				{
					result = AvatarData.TryGetItemModification(shopPreviewLootTemplate);
				}
			}
			else
			{
				result = Main.SafeGameDb.FindGameIdFromGuid(_shopBundle.itemDefGuid);
			}
			return result;
		}

		public static GameID GetPreviewColorGameID(this ShopBundle _shopBundle)
		{
			GameID result = GameID.Invalid;
			if (string.IsNullOrEmpty(_shopBundle.colorDefGuid))
			{
				LootTemplate firstItemTemplateWrapper = _shopBundle.payout.lootTemplates.GetFirstItemTemplateWrapper();
				if (firstItemTemplateWrapper != null)
				{
					result = AvatarData.TryGetItemColorModification(firstItemTemplateWrapper);
				}
				else if (_shopBundle.IsBluePrint())
				{
					result = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.BlueprintGlowColor;
				}
				else if (_shopBundle.IsCustomLootItemType(ECustomLootTemplateType.LockPick))
				{
					result = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.LockpicksGlowColor;
				}
			}
			else
			{
				result = Main.SafeGameDb.FindGameIdFromGuid(_shopBundle.colorDefGuid);
			}
			return result;
		}

		public static EntitlementDefinition GetFirstRequiredEntitlement(this ShopBundle _shopBundle)
		{
			string[] reqEntitlementGuids = _shopBundle.reqEntitlementGuids;
			if (reqEntitlementGuids.Length > 0)
			{
				return EntitlementExtensions.GetEntitlementByGUID(reqEntitlementGuids[0]);
			}
			return null;
		}

		public static bool HasResourceCost(this ShopBundle _shopBundle)
		{
			if (_shopBundle.costs != null && _shopBundle.costs.resources.Length > 0)
			{
				return true;
			}
			return false;
		}

		public static bool ShowShopBundleQuility(this ShopBundle _shopBundle)
		{
			if (_shopBundle.payout.lootTemplates.Length == 1)
			{
				LootTemplate lootTemplate = _shopBundle.payout.lootTemplates[0];
				return lootTemplate.blueprintTemplate == null && lootTemplate.resourceTemplate == null && lootTemplate.mysteryBoxTemplate == null && lootTemplate.customTemplate == null && _shopBundle.category != EShopCategory.Shopcat_StarterPacks && (lootTemplate.itemTemplate == null || lootTemplate.itemTemplate.category != Item.EItemCategory.Outfit);
			}
			return false;
		}

		public static Color32 GetShopBundleRewardItemColor(this ShopBundle _shopBundle)
		{
			LootTemplate shopPreviewLootTemplate = _shopBundle.GetShopPreviewLootTemplate();
			bool flag = shopPreviewLootTemplate != null && shopPreviewLootTemplate.itemTemplate != null;
			Color color = UIItemUtil.GetItemQualityColor(_shopBundle.quality);
			if (_shopBundle.payout.lootTemplates.GetFirstModuleTemplate() != null && !flag)
			{
				color = UIItemUtil.GetItemQualityColor(_shopBundle.GetModuleDisplayItemQuality());
			}
			else if (_shopBundle.IsBluePrint())
			{
				color = UIItemUtil.GetColorDefForColorGuid(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.BlueprintGlowColor).Color0;
			}
			else if (_shopBundle.IsCustomLootItemType(ECustomLootTemplateType.LockPick))
			{
				color = UIItemUtil.GetColorDefForColorGuid(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.LockpicksGlowColor).Color0;
			}
			return color;
		}

		public static Color32 GetShopBundleItemColor(this ShopBundle _shopBundle)
		{
			LootTemplate shopPreviewLootTemplate = _shopBundle.GetShopPreviewLootTemplate();
			bool flag = shopPreviewLootTemplate != null && shopPreviewLootTemplate.itemTemplate != null;
			Color color = UIDataMapper.UITextColors.ShopNoQualityColor;
			if (_shopBundle.payout.lootTemplates.GetFirstModuleTemplate() != null && !flag)
			{
				color = UIItemUtil.GetItemQualityColor(_shopBundle.GetModuleDisplayItemQuality());
			}
			else if (_shopBundle.ShowShopBundleQuility())
			{
				color = UIItemUtil.GetItemQualityColor(_shopBundle.quality);
			}
			return color;
		}

		public static Quality.EQuality GetModuleDisplayItemQuality(this ShopBundle _shopBundle)
		{
			LootTemplate firstModuleTemplateWrapper = _shopBundle.payout.lootTemplates.GetFirstModuleTemplateWrapper();
			if (firstModuleTemplateWrapper != null)
			{
				ModuleQualityMapping moduleQualityMapping = Singleton<StateManager>.Instance.TemplateManager.Globals.moduleQualityMapping;
				return moduleQualityMapping.GetModuleDisplayItemQuality(firstModuleTemplateWrapper.quality);
			}
			return Quality.EQuality.Common;
		}

		public static StringMapper GetShopBundleStringMapper(this ShopBundle _shopBundle)
		{
			StateManager instance = Singleton<StateManager>.Instance;
			StringMapper _stringMapper = new StringMapper();
			UserProfile profile = instance.ProfileManager.Profile;
			profile.resources.FillResourceStringBuilder(ref _stringMapper, "current_amount");
			_shopBundle.payout.lootTemplates.FillResourceStringBuilder(ref _stringMapper, "amount");
			LootTemplate.ResourceTemplate firstResourceTemplate = _shopBundle.payout.lootTemplates.GetFirstResourceTemplate();
			if (firstResourceTemplate != null)
			{
				_stringMapper.Add("amount", firstResourceTemplate.minResource.amount);
				_stringMapper.Add("current_amount", profile.resources.GetResourceAmountByType(firstResourceTemplate.minResource.type));
			}
			_stringMapper.Add("premium_time", UIDuration.FromSeconds(_shopBundle.payout.premiumTime));
			_shopBundle.FillModuleStringBuilder(_stringMapper);
			return _stringMapper;
		}

		public static StringMapper GetShopBundleTitleStringMapper(this ShopBundle _shopBundle)
		{
			StringMapper _stringMapper = new StringMapper();
			_shopBundle.payout.lootTemplates.FillLootTemplateStringBuilder(ref _stringMapper);
			return _stringMapper;
		}

		public static void FillModuleStringBuilder(this ShopBundle _shopBundle, StringMapper _mapper)
		{
			LootTemplate firstMysteryBoxTemplate = _shopBundle.payout.lootTemplates.GetFirstMysteryBoxTemplate();
			if (firstMysteryBoxTemplate != null)
			{
				firstMysteryBoxTemplate.mysteryBoxTemplate.type.FillMysteryBoxStringMapper(firstMysteryBoxTemplate.quality, ref _mapper);
			}
			int i = 0;
			_shopBundle.payout.lootTemplates.DoForModuleTemplates(delegate(LootTemplate.ModuleTemplate _template)
			{
				_mapper.Add(string.Format("modules_{0}", i + 1), new UIShopModuleQuality(_template));
				for (int j = 0; j < _template.skills.Length; j++)
				{
					string key = string.Format("modules_{0}_skill_{1}_factor", i + 1, j + 1);
					_mapper.Add(key, _template.skills[j].effectValue);
				}
				i++;
			});
		}
	}
}
