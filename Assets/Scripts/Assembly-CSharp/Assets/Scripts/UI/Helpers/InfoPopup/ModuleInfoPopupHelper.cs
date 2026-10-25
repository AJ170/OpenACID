using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.InfoPopup;
using Assets.Scripts.UI.InfoPopup.Content;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.ModulePopup;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Helpers.InfoPopup
{
	public static class ModuleInfoPopupHelper
	{
		[Flags]
		public enum EMode
		{
			None = 0,
			Equip = 1
		}

		private const int s_equipId = 1576;

		public static SimpleDelegate OnModuleChangedCallback;

		static ModuleInfoPopupHelper()
		{
			OnModuleChangedCallback = new SimpleDelegate(null, new Type[1] { typeof(InventoryElement) });
		}

		public static void ShowInfoPopup(UIInfoPopup _infoPopup, EMode _modes, InventoryElement _element, Transform _indicatorTarget = null, bool _showCloseButton = true, string _overrideModuleColorGuid = "")
		{
			Quality.EQuality itemDisplayQuality = _element.GetItemDisplayQuality();
			Color itemQualityColor = UIItemUtil.GetItemQualityColor(itemDisplayQuality);
			Color itemTextQualityColor = UIItemUtil.GetItemTextQualityColor(itemDisplayQuality);
			List<PopUpButtonConfiguration> list = CreateButtons(_infoPopup, _modes, _element);
			_infoPopup.IndicatorTarget = _indicatorTarget;
			_infoPopup.Configure(new InfoPopUpConfiguration
			{
				Buttons = list.ToArray(),
				Heading = _element.GetTitle(),
				QualityColor = itemQualityColor,
				FrameColor = itemQualityColor,
				QualityTextColor = itemTextQualityColor,
				OnCloseButtonClicked = null,
				ShowCloseButton = _showCloseButton,
				Data = new ModuleInfoPopupContent.ModuleInfoPopUpData
				{
					Element = _element,
					OverrideModuleColorGuid = _overrideModuleColorGuid
				}
			});
		}

		public static bool ModuleEquippable(Assassin _assassin, InventoryElement _element)
		{
			return _element.IsItemEquippable(_assassin);
		}

		private static List<PopUpButtonConfiguration> CreateButtons(UIInfoPopup _infoPopup, EMode _modesFlags, InventoryElement _element = null)
		{
			List<PopUpButtonConfiguration> list = new List<PopUpButtonConfiguration>();
			if ((_modesFlags & EMode.Equip) == EMode.Equip)
			{
				list.Add(new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
					Label = LocalizationManager.Instance.Get(1576),
					IsEnabled = (ModuleEquippable(Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin, _element) && _element != null && _element.EntitledToEquip()),
					OnButtonPressed = delegate
					{
						OnModeButtonClicked(_infoPopup, EMode.Equip, _element);
					},
					SoundIntent = EButtonIntent.Equip
				});
			}
			return list;
		}

		private static void OnModeButtonClicked(UIInfoPopup _infoPopup, EMode _mode, InventoryElement _module = null)
		{
			if (_module == null || _infoPopup == null)
			{
				return;
			}
			if (_mode == EMode.Equip)
			{
				if (UIButton.current.isEnabled)
				{
					OnEquipItem(_infoPopup, _module);
				}
				else
				{
					UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(4575));
				}
			}
		}

		private static void OnEquipItem(UIInfoPopup _infoPopup, InventoryElement _module)
		{
			if (UIButton.current.isEnabled)
			{
				InventoryElement elementForSlot = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.GetElementForSlot(EInventorySlot.SlotModule);
				if (elementForSlot != null && !elementForSlot.moduleData.Expired())
				{
					string label = LocalizationManager.Instance.Get(5412);
					string heading = LocalizationManager.Instance.Get(5411);
					string title = elementForSlot.GetTitle();
					Quality.EQuality itemDisplayQuality = elementForSlot.GetItemDisplayQuality();
					string value = LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemQualityDescriptions[(int)itemDisplayQuality].NameId);
					string description = LocalizationManager.Instance.Get(5413, new StringMapper
					{
						{ "Module_Active", title },
						{ "Module_Rarity", value }
					});
					PopUpButtonConfiguration[] buttons = new PopUpButtonConfiguration[1]
					{
						new PopUpButtonConfiguration
						{
							Label = label,
							ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
							OnButtonPressed = delegate
							{
								Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
								DoEquipItem(_infoPopup, _module);
							},
							SoundIntent = EButtonIntent.Equip
						}
					};
					PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
					popUpConfiguration.ShowCloseButton = true;
					popUpConfiguration.OnCloseButtonClicked = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
					};
					popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
					popUpConfiguration.Heading = heading;
					popUpConfiguration.Buttons = buttons;
					popUpConfiguration.Data = new ModulePopupContent.ModulePopupData
					{
						Description = description,
						Module = new ModulePopupContent.PopupModuleConfiguration(elementForSlot)
					};
					PopUpConfiguration config = popUpConfiguration;
					Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
				}
				else
				{
					DoEquipItem(_infoPopup, _module);
				}
			}
			else
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(4575));
			}
		}

		private static void DoEquipItem(UIInfoPopup _infoPopup, InventoryElement _module)
		{
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_inventory_equip_booster);
			Assassin assassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
			AsyncCallback callback = delegate(IAsyncResult _ar)
			{
				if (_ar.IsError())
				{
					_infoPopup.ReActivateButtons();
				}
				else
				{
					_infoPopup.Close();
					if (OnModuleChangedCallback != null)
					{
						OnModuleChangedCallback.Trigger(_module);
					}
				}
			};
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncResult)
			{
				Singleton<StateManager>.Instance.ProfileManager.EquipElement(assassin.id, _module.id, _asyncResult);
			}, null, callback);
		}
	}
}
