using Assets.Scripts.UI.InfoPopup;
using Assets.Scripts.UI.InfoPopup.Content;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Helpers.InfoPopup
{
	public static class CustomItemTypeInfoPopupHelper
	{
		public static void ShowInfoPopup(UIInfoPopup _infoPopup, ECustomItemType _customItemType, int _customItemCount, bool _showCloseButton = true, Transform _indicatorTarget = null)
		{
			CustomItemDescription value = null;
			UbiCore.Utils.Singleton<UIDataMapper>.Instance.CustomItemDescriptions.TryGetValue((int)_customItemType, out value);
			Color s_defaultShopItemColor = UIItemUtil.s_defaultShopItemColor;
			Color32 resourceInfoPopupHeaderColor = UIDataMapper.UITextColors.ResourceInfoPopupHeaderColor;
			_infoPopup.IndicatorTarget = _indicatorTarget;
			_infoPopup.Configure(new InfoPopUpConfiguration
			{
				Buttons = null,
				Heading = LocalizationManager.Instance.Get(value.InfoHeadingId),
				QualityColor = s_defaultShopItemColor,
				FrameColor = s_defaultShopItemColor,
				QualityTextColor = resourceInfoPopupHeaderColor,
				OnCloseButtonClicked = null,
				ShowCloseButton = _showCloseButton,
				Data = new BasicWithItemCountInfoPopupContent.BasicWithItemCountInfoPopupData
				{
					CustomItemType = _customItemType,
					Value = _customItemCount,
					DescriptionlangKey = value.InfoDescriptionId
				}
			});
		}
	}
}
