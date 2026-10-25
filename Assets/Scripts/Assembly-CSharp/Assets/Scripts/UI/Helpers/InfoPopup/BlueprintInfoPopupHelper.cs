using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.InfoPopup;
using Assets.Scripts.UI.InfoPopup.Content;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils;
using Database;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Helpers.InfoPopup
{
	public static class BlueprintInfoPopupHelper
	{
		private const int k_addToForge = 6738;

		private const int k_removeFromForge = 6739;

		public static SimpleDelegate OnAddToForgeCallback;

		public static SimpleDelegate OnRemoveFromForgeCallback;

		static BlueprintInfoPopupHelper()
		{
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
		}

		public static void ShowBlueprintInfoPopup(UIInfoPopup _infoPopup, InventoryElement _item, Button[] _buttons, bool _showCloseButton = true, Transform _indicatorTarget = null, string _itemColorOverrride = "")
		{
			ColorDef colorDefForColorGuid = UIItemUtil.GetColorDefForColorGuid(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.BlueprintFrameColor);
			ColorDef colorDefForColorGuid2 = UIItemUtil.GetColorDefForColorGuid((!string.IsNullOrEmpty(_itemColorOverrride)) ? Main.Instance.GameDB.FindGameIdFromGuid(_itemColorOverrride) : UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.BlueprintGlowColor);
			_infoPopup.IndicatorTarget = _indicatorTarget;
			_infoPopup.Configure(new InfoPopUpConfiguration
			{
				Buttons = CreateButtons(_buttons, _item, _infoPopup).ToArray(),
				QualityColor = colorDefForColorGuid2.Color0,
				FrameColor = colorDefForColorGuid.Color0,
				QualityTextColor = colorDefForColorGuid.Color0,
				Heading = LocalizationManager.Instance.Get(_item.langKey.neutral),
				OnCloseButtonClicked = null,
				ShowCloseButton = _showCloseButton,
				Data = new BlueprintInfoPopupContent.BlueprintInfoPopUpData
				{
					Element = _item,
					ItemColordef = colorDefForColorGuid2
				}
			});
		}

		private static List<PopUpButtonConfiguration> CreateButtons(Button[] _buttons, InventoryElement _element, UIInfoPopup _infoPopup)
		{
			List<PopUpButtonConfiguration> list = new List<PopUpButtonConfiguration>(_buttons.Length);
			foreach (Button button in _buttons)
			{
				list.Add(CreateButton(button, _element, _infoPopup));
			}
			return list;
		}

		private static PopUpButtonConfiguration CreateButton(Button _button, InventoryElement _element, UIInfoPopup _infoPopup)
		{
			switch (_button.Mode)
			{
			case Button.ButtonMode.AddToForge:
			{
				PopUpButtonConfiguration popUpButtonConfiguration = new PopUpButtonConfiguration();
				popUpButtonConfiguration.ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick;
				popUpButtonConfiguration.Label = LocalizationManager.Instance.Get(6738);
				popUpButtonConfiguration.IsEnabled = _button.Active;
				popUpButtonConfiguration.IconSpriteName = string.Empty;
				popUpButtonConfiguration.OnButtonPressed = delegate
				{
					OnButtonClicked(_button, _element, _infoPopup);
				};
				popUpButtonConfiguration.SoundIntent = EButtonIntent.Ok;
				popUpButtonConfiguration.IsDisabledClickAllowed = true;
				return popUpButtonConfiguration;
			}
			case Button.ButtonMode.RemoveFromForge:
			{
				PopUpButtonConfiguration popUpButtonConfiguration = new PopUpButtonConfiguration();
				popUpButtonConfiguration.ButtonType = PopUpButtonConfiguration.EButtonType.eNormal;
				popUpButtonConfiguration.Label = LocalizationManager.Instance.Get(6739);
				popUpButtonConfiguration.IsEnabled = _button.Active;
				popUpButtonConfiguration.IconSpriteName = string.Empty;
				popUpButtonConfiguration.OnButtonPressed = delegate
				{
					OnButtonClicked(_button, _element, _infoPopup);
				};
				popUpButtonConfiguration.SoundIntent = EButtonIntent.Ok;
				return popUpButtonConfiguration;
			}
			case Button.ButtonMode.StartForge:
			{
				PopUpButtonConfiguration popUpButtonConfiguration = new PopUpButtonConfiguration();
				popUpButtonConfiguration.ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick;
				popUpButtonConfiguration.Label = string.Empty;
				popUpButtonConfiguration.IconSpriteAtlas = UIScreenManager.EAtlasType.Animus;
				popUpButtonConfiguration.IconSpriteName = "aUI_icon_forge";
				popUpButtonConfiguration.IsEnabled = _button.Active;
				popUpButtonConfiguration.OnButtonPressed = delegate
				{
					OnButtonClicked(_button, _element, _infoPopup);
				};
				popUpButtonConfiguration.SoundIntent = EButtonIntent.Ok;
				return popUpButtonConfiguration;
			}
			default:
				return null;
			}
		}

		private static void OnButtonClicked(Button _button, InventoryElement _element, UIInfoPopup _infoPopup)
		{
			if (_button.OnClick != null)
			{
				_button.OnClick(_button.Active);
			}
			switch (_button.Mode)
			{
			case Button.ButtonMode.AddToForge:
				OnAddToForgeCallback.Trigger(_infoPopup, _element);
				break;
			case Button.ButtonMode.RemoveFromForge:
				OnRemoveFromForgeCallback.Trigger(_infoPopup, _element);
				break;
			case Button.ButtonMode.StartForge:
				break;
			}
		}
	}
}
