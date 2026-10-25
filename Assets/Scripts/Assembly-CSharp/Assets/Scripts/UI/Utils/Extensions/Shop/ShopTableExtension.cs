using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Screens.ShopScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager;
using Assets.Scripts.UI.State.EventManager.Data;
using SharedBaseLib.Core;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Utils.Extensions.Shop
{
	public static class ShopTableExtension
	{
		public static ShopCategoryNode GetShopCatalog(this ShopTable _shopTable)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			EventManager eventManager = Singleton<StateManager>.Instance.EventManager;
			EventShopBundleVisibilityManager eventShopBundleVisibilityManager = (EventShopBundleVisibilityManager)eventManager.GetEventManger(EEventType.Event_Shop_Item_Visibility);
			if (eventShopBundleVisibilityManager != null)
			{
				dictionary = eventShopBundleVisibilityManager.GetItemOverrideSortIds();
			}
			ShopCategoryNode shopCategoryNode = new ShopCategoryNode();
			bool isTutorial = UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialCategoryActive(ETutorialCategory.TutCatGuideShop);
			for (int i = 0; i < _shopTable.bundles.Length; i++)
			{
				ShopBundle shopBundle = _shopTable.bundles[i];
				if (!shopBundle.IsShownInShop(isTutorial))
				{
					continue;
				}
				ShopCategoryNode shopCategoryNode2 = shopCategoryNode.GetShopCategoryByType(shopBundle.category);
				if (shopCategoryNode2 == null)
				{
					ShopCategoryDescription value = null;
					if (!UbiCore.Utils.Singleton<UIDataMapper>.Instance.ShopCategoryDescriptions.TryGetValue((int)shopBundle.category, out value))
					{
						continue;
					}
					int sortID = value.SortId;
					if (AdColonyManager.IsAvailable || Singleton<StateManager>.Instance.TemplateManager.ShowAdColonyLastInShop)
					{
						sortID = value.SortNoColonyId;
					}
					ShopCategoryNode shopCategoryNode3 = new ShopCategoryNode();
					shopCategoryNode3.SortID = sortID;
					shopCategoryNode3.NameKey = value.CategoryNameId;
					shopCategoryNode3.ItemIconName = value.IconName;
					shopCategoryNode3.Category = shopBundle.category;
					shopCategoryNode3.ParentNode = shopCategoryNode;
					shopCategoryNode2 = shopCategoryNode3;
					shopCategoryNode.InsertNode(shopCategoryNode2);
				}
				int value2 = 0;
				if (!dictionary.TryGetValue(shopBundle.name, out value2))
				{
					value2 = (int)shopBundle.position;
				}
				if (shopBundle.IsDirectPurchasesBundle())
				{
					ShopDirectPurchasItemNode shopDirectPurchasItemNode = new ShopDirectPurchasItemNode();
					shopDirectPurchasItemNode.SortID = value2;
					shopDirectPurchasItemNode.ShopBundle = shopBundle;
					shopDirectPurchasItemNode.ParentNode = shopCategoryNode2;
					shopDirectPurchasItemNode.ItemCurrencyText = string.Empty;
					ShopDirectPurchasItemNode newNode = shopDirectPurchasItemNode;
					shopCategoryNode2.InsertNode(newNode);
				}
				else
				{
					ShopItemNode shopItemNode = new ShopItemNode();
					shopItemNode.SortID = value2;
					shopItemNode.ShopBundle = shopBundle;
					shopItemNode.ParentNode = shopCategoryNode2;
					ShopItemNode newNode2 = shopItemNode;
					shopCategoryNode2.InsertNode(newNode2);
				}
			}
			UpdateHeroicShop(shopCategoryNode);
			UpdateHappyHourBanners(shopCategoryNode);
			return shopCategoryNode;
		}

		public static string[] GetAllDirectPurchaseProductIds(this ShopTable _shopTable, bool _checkShownInShop = true)
		{
			BBList<string> bBList = new BBList<string>();
			for (int i = 0; i < _shopTable.bundles.Length; i++)
			{
				ShopBundle shopBundle = _shopTable.bundles[i];
				if (shopBundle.IsDirectPurchasesBundle() && (!_checkShownInShop || shopBundle.IsShownInShop()))
				{
					bBList.Add(shopBundle.houstonId);
				}
			}
			return bBList.ToArray();
		}

		public static ShopBundle GetShopBundleById(this ShopTable _shopTable, string _id)
		{
			for (int i = 0; i < _shopTable.bundles.Length; i++)
			{
				ShopBundle shopBundle = _shopTable.bundles[i];
				if (shopBundle.id == _id)
				{
					return shopBundle;
				}
			}
			return null;
		}

		public static BBList<ShopBundle> GetShopBundleByResourcePayout(this ShopTable _shopTable, EResourceType _resourceType)
		{
			BBList<ShopBundle> bBList = new BBList<ShopBundle>();
			for (int i = 0; i < _shopTable.bundles.Length; i++)
			{
				ShopBundle shopBundle = _shopTable.bundles[i];
				if (shopBundle.HasOnlyResourceTypePayout(_resourceType))
				{
					bBList.Add(shopBundle);
				}
			}
			return bBList;
		}

		public static BBList<ShopBundle> GetShopBundleByEntitlementsPayout(this ShopTable _shopTable, string[] _entitlements)
		{
			BBList<ShopBundle> bBList = new BBList<ShopBundle>();
			if (_entitlements.Length > 0)
			{
				for (int i = 0; i < _shopTable.bundles.Length; i++)
				{
					ShopBundle shopBundle = _shopTable.bundles[i];
					if (shopBundle.HasEntitlementsPayout(_entitlements))
					{
						bBList.Add(shopBundle);
					}
				}
			}
			return bBList;
		}

		public static ShopBaseNode GetHeroicRankShopBundleByEntitlement(this ShopCategoryNode _rootNode, string _entitlement)
		{
			ShopCategoryNode shopCategoryByType = _rootNode.GetShopCategoryByType(EShopCategory.Shopcat_Heroic);
			if (shopCategoryByType != null && !string.IsNullOrEmpty(_entitlement))
			{
				string[] entitlementPayout = new string[1] { _entitlement };
				for (int i = 0; i < shopCategoryByType.ItemCount; i++)
				{
					ShopBaseNode itemByIndex = shopCategoryByType.GetItemByIndex(i);
					ShopItemNode shopItemNode = itemByIndex as ShopItemNode;
					if (shopItemNode != null && shopItemNode.ShopBundle.HasEntitlementsPayout(entitlementPayout))
					{
						return itemByIndex;
					}
				}
			}
			return null;
		}

		public static void UpdateHappyHourBanners(ShopCategoryNode _root)
		{
			Array values = Enum.GetValues(typeof(EShopCategory));
			foreach (int item in values)
			{
				EventShopAdvertisementsManager eventShopAdvertisementsManager = (EventShopAdvertisementsManager)Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Shop_Ad_Banner);
				EventShopAdvertisement happyHourAdvertisementByCategory = eventShopAdvertisementsManager.GetHappyHourAdvertisementByCategory((EShopCategory)item);
				if (happyHourAdvertisementByCategory == null)
				{
					continue;
				}
				long _earliestEndTime = 0L;
				float _bonusValue = 0f;
				if (!((EShopCategory)item).GetShopCategoryHappyHourData(out _earliestEndTime, out _bonusValue))
				{
					continue;
				}
				bool flag = false;
				ShopCategoryNode shopCategoryByType = _root.GetShopCategoryByType((EShopCategory)item);
				if (shopCategoryByType == null)
				{
					continue;
				}
				for (int i = 0; i < shopCategoryByType.ItemCount; i++)
				{
					ShopBaseNode itemByIndex = shopCategoryByType.GetItemByIndex(i);
					if (itemByIndex != null && itemByIndex.DataType == EShopDataType.HappyHourBanner)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					shopCategoryByType.InsertNode(new ShopHappyHour
					{
						ParentNode = shopCategoryByType
					});
				}
			}
		}

		private static void UpdateHeroicShop(ShopCategoryNode _root)
		{
			ShopCategoryNode shopCategoryByType = _root.GetShopCategoryByType(EShopCategory.Shopcat_Heroic);
			if (shopCategoryByType == null)
			{
				return;
			}
			int num = 0;
			int itemCount = shopCategoryByType.ItemCount;
			for (int i = 0; i < itemCount; i++)
			{
				int num2 = shopCategoryByType.GetItemEntrieIndex(i) + num;
				ShopBaseNode shopBaseNode = shopCategoryByType.RawItemEntriesList[num2];
				ShopItemNode shopItemNode = shopBaseNode as ShopItemNode;
				if (shopItemNode != null && shopItemNode.ShopBundle.HasHeroicRankPayout())
				{
					shopCategoryByType.Insert(num2 + 1, null, false);
					num++;
					if (num2 % 2 != 0)
					{
						shopCategoryByType.Insert(num2, null, false);
						num++;
					}
				}
			}
			shopCategoryByType.ForceUpdateIndexList();
		}
	}
}
