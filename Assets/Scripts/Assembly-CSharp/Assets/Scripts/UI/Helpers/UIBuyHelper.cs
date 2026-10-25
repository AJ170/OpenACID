using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Tracking;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.ErrorPopup;
using Assets.Scripts.UI.Screens.Popup.InventorySpace;
using Assets.Scripts.UI.Screens.Popup.ItemPopup;
using Assets.Scripts.UI.Screens.ShopScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using SharedBaseLib.Core;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Helpers
{
	public class UIBuyHelper
	{
		public static bool CanEnterShopScreen
		{
			get
			{
				return Main.Instance.SceneLoader.InAnimus && Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.ShopTabEntitlementGUID);
			}
		}

		public static bool IsResourceTypeSoldInShop(EResourceType _resourceType)
		{
			return _resourceType == EResourceType.SocialCurrency || _resourceType == EResourceType.SoftCurrency;
		}

		public static PopUpButtonConfiguration CreateResourceCostButton(UIPopupResourcesContent.ResourceConfiguration _reRollCostConfiguration, StringId _buttonKey, Action _action, bool _forceHasResources = false)
		{
			bool flag = (_reRollCostConfiguration.HasEnoughResources = _forceHasResources || (bool)Singleton<StateManager>.Instance.ProfileManager.Profile.HasEnoughResources(_reRollCostConfiguration.Resource));
			PopUpButtonConfiguration popUpButtonConfiguration = new PopUpButtonConfiguration();
			popUpButtonConfiguration.ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick;
			popUpButtonConfiguration.Label = LocalizationManager.Instance.Get(_buttonKey);
			popUpButtonConfiguration.IsEnabled = flag;
			popUpButtonConfiguration.OnButtonPressed = delegate
			{
				_action();
			};
			PopUpButtonConfiguration popUpButtonConfiguration2 = popUpButtonConfiguration;
			if (!flag && IsResourceTypeSoldInShop(_reRollCostConfiguration.Resource.type))
			{
				popUpButtonConfiguration2.Label = LocalizationManager.Instance.Get(670);
				popUpButtonConfiguration2.IconSpriteName = "aUI_icon_shop";
				popUpButtonConfiguration2.IsEnabled = CanEnterShopScreen;
			}
			return popUpButtonConfiguration2;
		}

		public static void DetermineAllowPurches(Resource _productCost, Action _purchaseAllowedCallback, Action _goToShopCallback)
		{
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			if (_productCost != null && !profile.HasEnoughResources(_productCost) && !UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialCategoryActive(ETutorialCategory.TutCatGuideShop))
			{
				if (IsResourceTypeSoldInShop(_productCost.type))
				{
					OnGoToShopOnClose(_goToShopCallback, _productCost.type);
					return;
				}
				UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(2333, new StringMapper { 
				{
					"Resource_Type_Name",
					_productCost.type.DisplayName()
				} }));
			}
			else
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
				if (_purchaseAllowedCallback != null)
				{
					_purchaseAllowedCallback();
				}
			}
		}

		public static void DetermineAllowPurchaseShopBundle(Resource _productCost, ShopBundle _shopBundle, Action _purchaseAllowedCallback, Action _goToShopCallback, Action _cancelCallback = null)
		{
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			if (_productCost != null && !profile.HasEnoughResources(_productCost) && !UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialCategoryActive(ETutorialCategory.TutCatGuideShop))
			{
				if (IsResourceTypeSoldInShop(_productCost.type) && CanEnterShopScreen)
				{
					OpenMissingResourceConfiguration(_productCost, _shopBundle, _purchaseAllowedCallback, _goToShopCallback, _cancelCallback = null);
					return;
				}
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
				UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(2333, new StringMapper { 
				{
					"Resource_Type_Name",
					_productCost.type.DisplayName()
				} }));
			}
			else
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
				if (_purchaseAllowedCallback != null)
				{
					_purchaseAllowedCallback();
				}
			}
		}

		private static void OnGoToShopOnClose(Action _callback, EResourceType _type)
		{
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			if (_callback != null)
			{
				_callback();
			}
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_whoosh_Large);
			ShopTable shopTable = Singleton<StateManager>.Instance.TemplateManager.ShopTable;
			BBList<ShopBundle> shopBundleByResourcePayout = shopTable.GetShopBundleByResourcePayout(_type);
			if (shopBundleByResourcePayout.Count > 0)
			{
				for (int i = 0; i < shopBundleByResourcePayout.Count; i++)
				{
					UIShopDetails shopDetails = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails;
					ShopItemNode shopItemById = shopDetails.CategoryRootNode.GetShopItemById(shopBundleByResourcePayout[i].id);
					if (shopItemById != null)
					{
						shopDetails.OpenAndSetFocusOnItem(shopItemById);
						break;
					}
				}
			}
			else
			{
				UIShopDetails.OpenAndSetFocusOnItem(EShopCategory.Shopcat_HcPackage);
			}
		}

		private static void OpenMissingResourceConfiguration(Resource _resource, ShopBundle _shopBundle, Action _purchaseAllowedCallback, Action _goToShopCallback, Action _cancelCallback)
		{
			TrackingManager.AddNotEnoughHCPopUpTrackPoint();
			string heading = LocalizationManager.Instance.Get(2333, new StringMapper { 
			{
				"Resource_Type_Name",
				_resource.type.DisplayName()
			} });
			string description = LocalizationManager.Instance.Get(2332, new StringMapper
			{
				{ "amount", _resource.amount },
				{
					"Resource_Type_Name",
					_resource.type.DisplayName()
				}
			});
			List<UIPopupResourcesContent.ResourceConfiguration> list = new List<UIPopupResourcesContent.ResourceConfiguration>();
			list.Add(new UIPopupResourcesContent.ResourceConfiguration
			{
				HasEnoughResources = false,
				Resource = _resource
			});
			List<UIPopupResourcesContent.ResourceConfiguration> resourcesCost = list;
			PopUpButtonConfiguration[] buttons = new PopUpButtonConfiguration[1]
			{
				new PopUpButtonConfiguration
				{
					Label = LocalizationManager.Instance.Get(670),
					IconSpriteName = "aUI_icon_shop",
					ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
					OnButtonPressed = delegate
					{
						OnGoToShopOnClose(_goToShopCallback, _resource.type);
					},
					Data = true
				}
			};
			long amount = 1L;
			if (!_shopBundle.IsHeroicBundle() && _shopBundle.bundleValue == 0)
			{
				amount = _shopBundle.payout.lootTemplates.GetTotalResourceAmount();
			}
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
			popUpConfiguration.Heading = heading;
			popUpConfiguration.Buttons = buttons;
			popUpConfiguration.ShowCloseButton = true;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
				Action action = _cancelCallback;
				if (action != null)
				{
					_cancelCallback();
				}
			};
			popUpConfiguration.Data = new ItemPopupContent.ItemPopupData
			{
				ResourcesCost = resourcesCost,
				Description = description,
				ResourceSymbolAdditive = false,
				Items = new ItemPopupContent.PopupItemConfiguration[1]
				{
					new ItemPopupContent.PopupItemConfiguration(_shopBundle, amount)
				}
			};
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}

		public static void ShowInventoryAndMailIsFullPopup(Action _onActionCompleted, int _newItemCount, bool _checkCanSendToMail = true, int _overrideCanExpandKey = 0)
		{
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			int inventorySpaceNeeded = _newItemCount - profileManager.InventoryFreeSpace;
			if (inventorySpaceNeeded <= 0)
			{
				return;
			}
			bool flag = profileManager.Profile.CanExpandInventory(inventorySpaceNeeded);
			if (flag || _checkCanSendToMail)
			{
				List<PopUpButtonConfiguration> list = new List<PopUpButtonConfiguration>();
				list.Add(new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
					Label = LocalizationManager.Instance.Get((!flag) ? 1780 : 1781),
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
					},
					SoundIntent = ((!flag) ? EButtonIntent.Ok : EButtonIntent.Cancel)
				});
				List<PopUpButtonConfiguration> list2 = list;
				if (flag)
				{
					list2.Add(new PopUpButtonConfiguration
					{
						ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
						Label = LocalizationManager.Instance.Get(1782),
						SoundIntent = EButtonIntent.Accept,
						OnButtonPressed = delegate
						{
							Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
							OpenBuyInventoryPopup(_onActionCompleted, inventorySpaceNeeded);
						}
					});
				}
				int num = ((_overrideCanExpandKey != 0) ? _overrideCanExpandKey : 8167);
				PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
				popUpConfiguration.ShowCloseButton = true;
				popUpConfiguration.OnCloseButtonClicked = delegate
				{
					Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
				};
				popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
				popUpConfiguration.Heading = LocalizationManager.Instance.Get(7801);
				popUpConfiguration.Buttons = list2.ToArray();
				popUpConfiguration.Data = new ErrorContent.ErrorData
				{
					Description = LocalizationManager.Instance.Get((!flag) ? 7804 : num)
				};
				PopUpConfiguration config = popUpConfiguration;
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_prompt_general_info);
				Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
			}
			else
			{
				Action action = _onActionCompleted;
				if (action != null)
				{
					action();
				}
			}
		}

		public static void OpenBuyInventoryPopup(Action _onCompletedAction = null, int _inventorySpaceNeeded = 1)
		{
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			if (!profile.CanExpandInventory())
			{
				return;
			}
			long slotPurchases = profile.GetCounterAmountByType(ECounterType.InventorySlotPurchases);
			InventorySlotCostsMapping.Entry entry = Singleton<StateManager>.Instance.TemplateManager.InventorySlotsCostsMapping.entries.First((InventorySlotCostsMapping.Entry x) => x.purchaseIndex == slotPurchases + 1);
			Resource[] resources = entry.costs.resources;
			uint amount = entry.amount;
			string heading = LocalizationManager.Instance.Get(4660);
			int num = ((resources.Length != 1) ? 4664 : 4663);
			int num2 = ((resources.Length != 1) ? 692 : 1780);
			Array.Sort(resources, UIUtil.SortResourcesByType);
			List<UIPopupResourcesContent.ResourceConfiguration> list = new List<UIPopupResourcesContent.ResourceConfiguration>(resources.Length);
			List<PopUpButtonConfiguration> list2 = new List<PopUpButtonConfiguration>();
			foreach (Resource cost in resources)
			{
				UIPopupResourcesContent.ResourceConfiguration resConfig = new UIPopupResourcesContent.ResourceConfiguration
				{
					HasEnoughResources = profile.HasEnoughResources(cost),
					Resource = cost
				};
				PopUpButtonConfiguration item = CreateResourceCostButton(resConfig, num2, delegate
				{
					DetermineAllowPurches(resConfig.Resource, delegate
					{
						PurchaseInventorySlot((int)slotPurchases, cost, _onCompletedAction);
					}, null);
				});
				list2.Add(item);
				list.Add(resConfig);
			}
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.Top;
			popUpConfiguration.Heading = heading;
			popUpConfiguration.ShowCloseButton = true;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			};
			popUpConfiguration.Buttons = list2.ToArray();
			popUpConfiguration.Data = new InventorySpacePopUpContent.InventoryPopUpData
			{
				DisplayType = InventorySpacePopUpContent.InventoryPopUpData.EDisplayType.Inventory,
				Increase = (int)amount,
				ResourcesCost = list,
				ResourceSymbolAdditive = false,
				Description = LocalizationManager.Instance.Get(num)
			};
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}

		private static void PurchaseInventorySlot(int _numSlotPurchases, Resource _cost, Action _onCompletedAction)
		{
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_buy);
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncRequest)
			{
				Singleton<StateManager>.Instance.ProfileManager.PurchaseInventorySlot(_numSlotPurchases, _cost.type, _asyncRequest);
			}, null, delegate(IAsyncResult _ar)
			{
				if (!_ar.IsError())
				{
					Action action = _onCompletedAction;
					if (action != null)
					{
						action();
					}
				}
			});
		}

		public static void OpenBuyBluePrintPopup(bool _showBuyAnimation)
		{
			UIShopDetails shopDetails = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails;
			ShopCategoryNode shopCategoryByType = shopDetails.CategoryRootNode.GetShopCategoryByType(EShopCategory.Shopcat_Forge);
			List<ShopBundle> list = new List<ShopBundle>();
			for (int i = 0; i < shopCategoryByType.ItemCount; i++)
			{
				ShopBaseNode itemByIndex = shopCategoryByType.GetItemByIndex(i);
				if (itemByIndex != null && itemByIndex.DataType == EShopDataType.Item)
				{
					list.Add(((ShopItemNode)itemByIndex).ShopBundle);
				}
			}
			if (UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialCategoryActive(ETutorialCategory.TutCatMs25Forge))
			{
				list.Sort((ShopBundle _bundle, ShopBundle _shopBundle) => (_bundle != _shopBundle) ? ((!_bundle.id.Equals(Singleton<StateManager>.Instance.TemplateManager.Globals.forgeTutorialInputBlueprintId)) ? 1 : (-1)) : 0);
			}
			string title = LocalizationManager.Instance.Get(7771);
			string description = LocalizationManager.Instance.Get(7772);
			Singleton<UIScreenManager>.Instance.SceneData.ItemBundlePreview.OpenPreviewForShopItemSelection(list.ToArray(), title, description, _showBuyAnimation, true, false);
		}
	}
}
