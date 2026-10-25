using AcierProtocol;
using Assets.Scripts.UI.InfoPopup;
using Assets.Scripts.UI.InfoPopup.Content;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils;
using Database;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Helpers.InfoPopup
{
	public static class CustomLootInfoPopupHelper
	{
		public static void ShowInfoPopup(UIInfoPopup _infoPopup, InventoryElement _item, bool _showCloseButton = true, Transform _indicatorTarget = null, string _itemColorOverrride = "")
		{
			Color32 lockPickFrameQualityColor = UIDataMapper.UITextColors.LockPickFrameQualityColor;
			Color32 color = UIDataMapper.UITextColors.LockPickQualityColor;
			if (!string.IsNullOrEmpty(_itemColorOverrride))
			{
				ColorDef colorDefForColorGuid = UIItemUtil.GetColorDefForColorGuid(Main.Instance.GameDB.FindGameIdFromGuid(_itemColorOverrride));
				color = colorDefForColorGuid.Color0;
			}
			_infoPopup.IndicatorTarget = _indicatorTarget;
			_infoPopup.Configure(new InfoPopUpConfiguration
			{
				Buttons = null,
				Heading = LocalizationManager.Instance.Get(_item.langKey.neutral),
				QualityColor = color,
				FrameColor = lockPickFrameQualityColor,
				QualityTextColor = lockPickFrameQualityColor,
				ShowCloseButton = _showCloseButton,
				OnCloseButtonClicked = null,
				Data = new BasicInfoPopupContent.BasicInfoPopupData
				{
					Element = _item,
					ItemColordef = UIItemUtil.GetColorDefForColorGuid(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.LockpicksGlowColor)
				}
			});
		}
	}
}
