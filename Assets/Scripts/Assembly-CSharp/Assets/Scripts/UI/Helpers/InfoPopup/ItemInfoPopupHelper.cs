using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.InfoPopup;
using Assets.Scripts.UI.InfoPopup.Content;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.MysteryBox;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.ItemPopup;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Helpers.InfoPopup
{
	public static class ItemInfoPopupHelper
	{
		[Flags]
		public enum EMode
		{
			None = 0,
			Equip = 1,
			Unequip = 2,
			Claim = 4,
			Sell = 8,
			OpenMysteryBox = 0x10,
			AddToForge = 0x20,
			RemoveForge = 0x40,
			ReRoll = 0x80,
			Disabled = 0x100,
			ForgeClaim = 0x200
		}

		private const int k_equipId = 1576;

		private const int k_unequipId = 1579;

		private const int k_claimId = 1808;

		private const int k_sellId = 1828;

		private const int k_addToForge = 6738;

		private const int k_removeFromForge = 6739;

		private const int k_openMysteryBox = 6684;

		private const int k_reRollId = 6803;

		public static SimpleDelegate OnEquipedItemCallback;

		public static SimpleDelegate OnClameItemCallback;

		public static SimpleDelegate OnAddToForgeCallback;

		public static SimpleDelegate OnRemoveFromForgeCallback;

		public static SimpleDelegate OnReRollCallback;

		static ItemInfoPopupHelper()
		{
			OnEquipedItemCallback = new SimpleDelegate(null, new Type[4]
			{
				typeof(UIInfoPopup),
				typeof(InventoryElement),
				typeof(InventoryElement),
				typeof(bool)
			});
			OnClameItemCallback = new SimpleDelegate(null, new Type[2]
			{
				typeof(UIInfoPopup),
				typeof(InventoryElement)
			});
			OnAddToForgeCallback = new SimpleDelegate(null, new Type[2]
			{
				typeof(UIInfoPopup),
				typeof(InventoryElement)
			});
			OnRemoveFromForgeCallback = new SimpleDelegate(null, new Type[2]
			{
				typeof(UIInfoPopup),
				typeof(InventoryElement)
			});
			OnReRollCallback = new SimpleDelegate(null, new Type[2]
			{
				typeof(UIInfoPopup),
				typeof(InventoryElement)
			});
		}

		public static void ShowInfoPopup(UIInfoPopup _infoPopup, Item.EItemCategory _category, Transform _indicatorTarget = null, bool _showCloseButton = true, int _itemPowerOverride = 0)
		{
			StringId categoryNameId = UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemCategoryDescriptions[(int)_category].CategoryNameId;
			ItemQualityDescription itemQualityDescription = UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemQualityDescriptions[1];
			ColorDef colorDefForColorGuid = UIItemUtil.GetColorDefForColorGuid(itemQualityDescription.ColorGUID);
			ColorDef colorDefForColorGuid2 = UIItemUtil.GetColorDefForColorGuid(itemQualityDescription.ColorTextGUID);
			_infoPopup.IndicatorTarget = _indicatorTarget;
			_infoPopup.Configure(new InfoPopUpConfiguration
			{
				Buttons = null,
				Heading = LocalizationManager.Instance.Get(categoryNameId),
				QualityColor = colorDefForColorGuid.Color0,
				FrameColor = colorDefForColorGuid.Color0,
				QualityTextColor = colorDefForColorGuid2.Color0,
				OnCloseButtonClicked = null,
				ShowCloseButton = _showCloseButton,
				Data = new ItemInfoPopupContent.ItemInfoPopUpData
				{
					Category = _category,
					ItemPowerOverride = _itemPowerOverride,
					ItemQualityDescription = itemQualityDescription
				}
			});
		}

		public static void ShowInfoPopup(UIInfoPopup _infoPopup, Assassin _assassin, EMode _modes, InventoryElement _item, InventoryElement _compareToItem = null, Transform _indicatorTarget = null, ItemQualityDescription _qualityDescription = null, bool _showCloseButton = true, int _itemPowerOverride = 0, string _itemColorOverrride = "", bool _hideCurrencyIcons = false)
		{
			if (_qualityDescription == null)
			{
				_qualityDescription = UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemQualityDescriptions[(int)_item.quality];
			}
			ColorDef colorDefForColorGuid = UIItemUtil.GetColorDefForColorGuid(_qualityDescription.ColorGUID);
			ColorDef colorDefForColorGuid2 = UIItemUtil.GetColorDefForColorGuid(_qualityDescription.ColorTextGUID);
			List<PopUpButtonConfiguration> list = CreateButtons(_infoPopup, _modes, _item, _hideCurrencyIcons);
			_infoPopup.IndicatorTarget = _indicatorTarget;
			_infoPopup.Configure(new InfoPopUpConfiguration
			{
				Buttons = list.ToArray(),
				Heading = LocalizationManager.Instance.Get(_item.langKey.neutral),
				QualityColor = colorDefForColorGuid.Color0,
				FrameColor = colorDefForColorGuid.Color0,
				QualityTextColor = colorDefForColorGuid2.Color0,
				OnCloseButtonClicked = null,
				ShowCloseButton = _showCloseButton,
				Data = new ItemInfoPopupContent.ItemInfoPopUpData
				{
					Assassin = _assassin,
					IsOutFit = (_item.IsItem() && _item.itemData.category == Item.EItemCategory.Outfit),
					IsMysteryBox = (_item.mysteryBoxData != null),
					Element = _item,
					ItemColorOverride = _itemColorOverrride,
					ItemPowerOverride = _itemPowerOverride,
					CompareToElement = _compareToItem,
					ItemQualityDescription = _qualityDescription,
					ShowItemQuality = true
				}
			});
		}

		public static void ShowInfoPopup(UIInfoPopup _infoPopup, EMode _modes, Resource _resource, Transform _indicatorTarget = null, ItemQualityDescription _qualityDescription = null, bool _showCloseButton = true, int _itemPowerOverride = 0, string _itemColorOverrride = "")
		{
			ResourceDescription value = null;
			UbiCore.Utils.Singleton<UIDataMapper>.Instance.ResourceDescriptions.TryGetValue((int)_resource.type, out value);
			Color color = UIItemUtil.s_defaultShopItemColor;
			Color32 color2 = UIDataMapper.UITextColors.ResourceInfoPopupHeaderColor;
			if (_qualityDescription != null)
			{
				ColorDef item = Main.Instance.GameDB.GetItem<ColorDef>(_qualityDescription.ColorGUID);
				ColorDef colorDefForColorGuid = UIItemUtil.GetColorDefForColorGuid(_qualityDescription.ColorTextGUID);
				color = item.Color0;
				color2 = colorDefForColorGuid.Color0;
			}
			List<PopUpButtonConfiguration> list = CreateButtons(_infoPopup, _modes);
			_infoPopup.IndicatorTarget = _indicatorTarget;
			_infoPopup.Configure(new InfoPopUpConfiguration
			{
				Buttons = list.ToArray(),
				Heading = LocalizationManager.Instance.Get(value.InfoHeadingId),
				QualityColor = color,
				FrameColor = color,
				QualityTextColor = color2,
				OnCloseButtonClicked = null,
				ShowCloseButton = _showCloseButton,
				Data = new ItemInfoPopupContent.ItemInfoPopUpData
				{
					Resource = _resource,
					ItemColorOverride = _itemColorOverrride,
					ItemPowerOverride = _itemPowerOverride,
					ItemQualityDescription = _qualityDescription,
					ShowItemQuality = (_qualityDescription != null)
				}
			});
		}

		public static void ShowInfoPopup(UIInfoPopup _infoPopup, EMode _modes, ItemDef _itemDef, LootFilter _lootFilter, Item.EItemCategory _category = Item.EItemCategory.None, Transform _indicatorTarget = null, ItemQualityDescription _qualityDescription = null, bool _showCloseButton = true, int _itemPowerOverride = 0, string _itemColorOverrride = "")
		{
			Color color = UIItemUtil.s_defaultShopItemColor;
			Color qualityTextColor = Color.white;
			if (_qualityDescription != null)
			{
				ColorDef item = Main.Instance.GameDB.GetItem<ColorDef>(_qualityDescription.ColorGUID);
				ColorDef colorDefForColorGuid = UIItemUtil.GetColorDefForColorGuid(_qualityDescription.ColorTextGUID);
				color = item.Color0;
				qualityTextColor = colorDefForColorGuid.Color0;
			}
			List<PopUpButtonConfiguration> list = CreateButtons(_infoPopup, _modes);
			string headingString = GetHeadingString(_lootFilter, _category, _qualityDescription.Quality);
			_infoPopup.IndicatorTarget = _indicatorTarget;
			_infoPopup.Configure(new InfoPopUpConfiguration
			{
				Heading = headingString,
				Buttons = list.ToArray(),
				QualityColor = color,
				FrameColor = color,
				QualityTextColor = qualityTextColor,
				OnCloseButtonClicked = null,
				ShowCloseButton = _showCloseButton,
				Data = new UnknownItemPopupContent.UnknownItemInfoPopUpData
				{
					LootFilter = _lootFilter,
					ItemCategory = _category,
					ItemDefinition = _itemDef,
					ItemQualityDescription = _qualityDescription,
					ShowItemQuality = (_qualityDescription != null)
				}
			});
		}

		private static string GetHeadingString(LootFilter _lootFilter, Item.EItemCategory _category, Quality.EQuality _Quality)
		{
			string result = string.Empty;
			if (_lootFilter.lootTypes.mysteryBoxTypes.Length == 0)
			{
				StringMapper stringMapper = new StringMapper();
				stringMapper.Add("quality", LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemQualityDescriptions[(int)_Quality].NameId));
				if (_category == Item.EItemCategory.None)
				{
					stringMapper.Add("type", LocalizationManager.Instance.Get(7718));
				}
				else
				{
					stringMapper.Add("type", LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemCategoryDescriptions[(int)_category].CategoryNameId));
				}
				result = LocalizationManager.Instance.Get(7703, stringMapper);
			}
			else
			{
				EMysteryBoxType eMysteryBoxType = _lootFilter.lootTypes.mysteryBoxTypes[0];
				MysteryBoxDescription value = null;
				if (eMysteryBoxType == EMysteryBoxType.MysteryBoxItemType)
				{
					UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemMysteryBoxDescription.TryGetValue((int)_Quality, out value);
				}
				else
				{
					UbiCore.Utils.Singleton<UIDataMapper>.Instance.ResourceMysteryBoxDescription.TryGetValue((int)_Quality, out value);
				}
				if (value != null)
				{
					result = LocalizationManager.Instance.Get(value.nameId);
				}
			}
			return result;
		}

		public static List<PopUpButtonConfiguration> CreateButtons(UIInfoPopup _infoPopup, EMode _modesFlags, InventoryElement _item = null, bool _hideCurrencyIcons = false)
		{
			List<PopUpButtonConfiguration> list = new List<PopUpButtonConfiguration>();
			if ((_modesFlags & EMode.Claim) == EMode.Claim)
			{
				list.Add(new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
					Label = LocalizationManager.Instance.Get(1808),
					IsEnabled = true,
					OnButtonPressed = delegate
					{
						OnModeButtonClicked(_infoPopup, EMode.Claim, _item);
					},
					SoundIntent = EButtonIntent.Claim
				});
			}
			else if ((_modesFlags & EMode.ForgeClaim) == EMode.ForgeClaim)
			{
				list.Add(new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
					Label = LocalizationManager.Instance.Get(1808),
					IsEnabled = true,
					OnButtonPressed = delegate
					{
						OnModeButtonClicked(_infoPopup, EMode.ForgeClaim, _item);
					}
				});
			}
			if ((_modesFlags & EMode.Equip) == EMode.Equip)
			{
				list.Add(new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
					Label = LocalizationManager.Instance.Get(1576),
					IsEnabled = _item.CanEquipItem(Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin),
					OnButtonPressed = delegate
					{
						OnModeButtonClicked(_infoPopup, EMode.Equip, _item);
					},
					IsDisabledClickAllowed = true
				});
			}
			if ((_modesFlags & EMode.Unequip) == EMode.Unequip)
			{
				list.Add(new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
					Label = LocalizationManager.Instance.Get(1579),
					IsEnabled = _item.HasInventorySpaceForItem(),
					OnButtonPressed = delegate
					{
						OnModeButtonClicked(_infoPopup, EMode.Unequip, _item);
					},
					SoundIntent = EButtonIntent.Unequip
				});
			}
			if ((_modesFlags & EMode.Sell) == EMode.Sell)
			{
				list.Add(new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
					Label = LocalizationManager.Instance.Get(1828),
					IsEnabled = _item.CanSell(),
					Resource = ((!_item.CanSell()) ? null : _item.SellValue()),
					OnButtonPressed = delegate
					{
						OnModeButtonClicked(_infoPopup, EMode.Sell, _item);
					},
					SoundIntent = EButtonIntent.Default
				});
			}
			if ((_modesFlags & EMode.ReRoll) == EMode.ReRoll)
			{
				list.Add(new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
					Label = LocalizationManager.Instance.Get(6803),
					IsEnabled = true,
					Resource = ((!_hideCurrencyIcons) ? new Resource
					{
						amount = 0L,
						type = EResourceType.SoftCurrency
					} : null),
					OnButtonPressed = delegate
					{
						OnModeButtonClicked(_infoPopup, EMode.ReRoll, _item);
					},
					SoundIntent = EButtonIntent.Default
				});
			}
			if ((_modesFlags & EMode.OpenMysteryBox) == EMode.OpenMysteryBox)
			{
				list.Add(new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
					Label = LocalizationManager.Instance.Get(6684),
					IsEnabled = true,
					OnButtonPressed = delegate
					{
						OnModeButtonClicked(_infoPopup, EMode.OpenMysteryBox, _item);
					},
					SoundIntent = EButtonIntent.Ok
				});
			}
			if ((_modesFlags & EMode.AddToForge) == EMode.AddToForge)
			{
				List<PopUpButtonConfiguration> list2 = new List<PopUpButtonConfiguration>();
				list2.Add(new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
					Label = LocalizationManager.Instance.Get(6738),
					IsEnabled = ((_modesFlags & EMode.Disabled) != EMode.Disabled),
					OnButtonPressed = delegate
					{
						OnModeButtonClicked(_infoPopup, EMode.AddToForge, _item);
					},
					SoundIntent = EButtonIntent.Ok
				});
				list = list2;
			}
			if ((_modesFlags & EMode.RemoveForge) == EMode.RemoveForge)
			{
				List<PopUpButtonConfiguration> list2 = new List<PopUpButtonConfiguration>();
				list2.Add(new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
					Label = LocalizationManager.Instance.Get(6739),
					IsEnabled = ((_modesFlags & EMode.Disabled) != EMode.Disabled),
					OnButtonPressed = delegate
					{
						OnModeButtonClicked(_infoPopup, EMode.RemoveForge, _item);
					},
					SoundIntent = EButtonIntent.Default
				});
				list = list2;
			}
			return list;
		}

		private static void OnModeButtonClicked(UIInfoPopup _infoPopup, EMode _mode, InventoryElement _item = null)
		{
			if (_item == null || _infoPopup == null)
			{
				return;
			}
			switch (_mode)
			{
			case EMode.Equip:
			{
				if (UIButton.current.isEnabled)
				{
					OnEquipItem(_infoPopup, _item);
					break;
				}
				StringId stringId = StringId.Invalid;
				if (!_item.EntitledToEquip())
				{
					stringId = _item.MissingEntitlementPopupMessage();
				}
				else if (!_item.IsAssassinLevelReachedForItem(Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin))
				{
					stringId = new StringId(4575);
				}
				if (stringId.Valid)
				{
					UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(stringId));
					AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_deny);
				}
				break;
			}
			case EMode.Unequip:
				OnUnEquipItem(_infoPopup, _item);
				break;
			case EMode.Claim:
			case EMode.ForgeClaim:
				if (UIButton.current.isEnabled)
				{
					OnClaimItem(_infoPopup, _mode, _item);
				}
				break;
			case EMode.Sell:
				OnSellItem(_infoPopup, _item);
				break;
			case EMode.ReRoll:
				OnReRollCallback.Trigger(_infoPopup, _item);
				break;
			case EMode.OpenMysteryBox:
				if (!Singleton<StateManager>.Instance.ProfileManager.Profile.CanClaimMysteryBoxRewards(_item.mysteryBoxData.mysteryBoxType, _item.quality))
				{
					UIMysteryBoxScreen.ShowInventoryAndMailIsFullPopup(delegate
					{
						OnOpenMysteryBox(_item);
					}, _item.mysteryBoxData.mysteryBoxType, _item.quality);
				}
				else
				{
					OnOpenMysteryBox(_item);
				}
				break;
			case EMode.AddToForge:
				OnAddToForgeCallback.Trigger(_infoPopup, _item);
				break;
			case EMode.RemoveForge:
				OnRemoveFromForgeCallback.Trigger(_infoPopup, _item);
				break;
			}
		}

		private static void OnClaimItem(UIInfoPopup _infoPopup, EMode _mode, InventoryElement _item = null)
		{
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_animmovement);
			UbiCore.Utils.Singleton<LocalStorage>.Instance.ItemAdd(_item.id);
			EventDelegate.Execute(Singleton<StateManager>.Instance.ItemNotificationManager.onInventoryNotificationChange);
			UIPortraitTexture portraitTexture = _infoPopup.GetPortraitTexture();
			UIRecivedItemManager recivedItemMgr = Singleton<UIScreenManager>.Instance.SceneData.RecivedItemMgr;
			recivedItemMgr.StartReciveItemAnimation(portraitTexture, portraitTexture.gameObject, null, delegate
			{
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_claim);
			});
			if (OnClameItemCallback != null)
			{
				OnClameItemCallback.Trigger(_infoPopup, _item);
			}
			_infoPopup.CloseWithDelayed(1.5f);
		}

		private static void OnEquipItem(UIInfoPopup _infoPopup, InventoryElement _item)
		{
			GameID equipSoundGameIDByItemType = _item.GetEquipSoundGameIDByItemType();
			AudioManager.Instance.Play(equipSoundGameIDByItemType);
			UIAssasinId assassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassinId;
			InventoryElement currentEquipedItem = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.GetEquipedEquivalent(_item);
			AsyncCallback callback = delegate(IAsyncResult _ar)
			{
				if (_ar.IsError())
				{
					_infoPopup.ReActivateButtons();
				}
				else
				{
					_infoPopup.Close();
					if (OnEquipedItemCallback != null)
					{
						OnEquipedItemCallback.Trigger(_infoPopup, _item, currentEquipedItem, true);
					}
				}
			};
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncResult)
			{
				Singleton<StateManager>.Instance.ProfileManager.EquipElement(assassin, _item.id, _asyncResult);
			}, null, callback);
		}

		private static void OnUnEquipItem(UIInfoPopup _infoPopup, InventoryElement _item)
		{
			if (!_item.HasInventorySpaceForItem())
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(4578));
				return;
			}
			UIAssasinId assassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassinId;
			AsyncCallback callback = delegate(IAsyncResult _ar)
			{
				if (_ar.IsError())
				{
					_infoPopup.ReActivateButtons();
				}
				else
				{
					_infoPopup.Close();
					if (OnEquipedItemCallback != null)
					{
						OnEquipedItemCallback.Trigger(_infoPopup, null, _item, false);
					}
				}
			};
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncResult)
			{
				Singleton<StateManager>.Instance.ProfileManager.UnequipElement(assassin, _item.id, _asyncResult);
			}, null, callback);
		}

		private static void OnSellItem(UIInfoPopup _infoPopup, InventoryElement _item)
		{
			PopUpButtonConfiguration[] buttons = new PopUpButtonConfiguration[1]
			{
				new PopUpButtonConfiguration
				{
					Label = LocalizationManager.Instance.Get(1780),
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
						UbiCore.Utils.Singleton<LocalStorage>.Instance.ItemRemove(_item.id);
						EventDelegate.Execute(Singleton<StateManager>.Instance.ItemNotificationManager.onInventoryNotificationChange);
						OnSellItemAsync(_infoPopup, _item);
					},
					SoundIntent = EButtonIntent.Buy
				}
			};
			ItemPopupContent.PopupItemConfiguration[] items = new ItemPopupContent.PopupItemConfiguration[1]
			{
				new ItemPopupContent.PopupItemConfiguration(_item, _item.stackSize)
			};
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.ShowCloseButton = true;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			};
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(_item.langKey.neutral);
			popUpConfiguration.Buttons = buttons;
			popUpConfiguration.Data = new ItemPopupContent.ItemPopupData
			{
				Description = LocalizationManager.Instance.Get(2244),
				Items = items,
				ResourcesCost = new List<UIPopupResourcesContent.ResourceConfiguration>
				{
					new UIPopupResourcesContent.ResourceConfiguration
					{
						HasEnoughResources = true,
						Resource = _item.SellValue()
					}
				}
			};
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}

		private static void OnOpenMysteryBox(InventoryElement _element)
		{
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_mysterybox_unlock);
			if (_element.mysteryBoxData.mysteryBoxType == EMysteryBoxType.MysteryBoxRessourceType)
			{
				Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.WalletHandler.ProfileSyncEnabled = false;
			}
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncRequest)
			{
				Singleton<StateManager>.Instance.ProfileManager.OpenMysteryBox(_element.quality, _element.mysteryBoxData.mysteryBoxType, Singleton<StateManager>.Instance.ProfileManager.CurrentAssassinId, _element.id, true, false, _asyncRequest);
			}, null, delegate(IAsyncResult _result)
			{
				if (!_result.IsError())
				{
					Singleton<StateManager>.Instance.CurrentChain = ((AsyncDataResult<OpenMysteryBoxResponse>)_result).Result().mysteryBoxChain;
					UbiCore.Utils.Singleton<LocalStorage>.Instance.MysteryBoxSlotCount = -1;
					Singleton<UIScreenManager>.Instance.ChangeScreen(EScreenID.MysteryBox, new UITweenHandle(null, null));
				}
			});
		}

		private static void OnSellItemAsync(UIInfoPopup _infoPopup, InventoryElement _item)
		{
			AsyncCallback callback = delegate(IAsyncResult _ar)
			{
				if (_ar.IsError())
				{
					_infoPopup.ReActivateButtons();
				}
				else
				{
					_infoPopup.Close();
					UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(4576));
				}
			};
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncResult)
			{
				Singleton<StateManager>.Instance.ProfileManager.SellItem(_item.id, _asyncResult);
			}, null, callback);
		}
	}
}
