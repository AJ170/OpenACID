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
	public static class EntitlementInfoPopupHelper
	{
		public static void ShowInfoPopup(UIInfoPopup _infoPopup, InventoryElement _fakeItem, Transform _indicatorTarget = null, bool _showCloseButton = true, string _colorOverrride = "")
		{
			ColorDef colorDefForColorGuid = UIItemUtil.GetColorDefForColorGuid(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.HeroicShopFrameColor);
			ColorDef colorDefForColorGuid2 = UIItemUtil.GetColorDefForColorGuid((!string.IsNullOrEmpty(_colorOverrride)) ? Main.Instance.GameDB.FindGameIdFromGuid(_colorOverrride) : UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.HeroicShopGlowColor);
			ColorDef colorDefForColorGuid3 = UIItemUtil.GetColorDefForColorGuid(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.HeroicShopBackgroundColor);
			_infoPopup.IndicatorTarget = _indicatorTarget;
			_infoPopup.Configure(new InfoPopUpConfiguration
			{
				Buttons = null,
				Heading = LocalizationManager.Instance.Get(_fakeItem.langKey.neutral),
				FrameColor = colorDefForColorGuid.Color0,
				QualityColor = colorDefForColorGuid3.Color0,
				QualityTextColor = colorDefForColorGuid.Color0,
				OnCloseButtonClicked = null,
				ShowCloseButton = _showCloseButton,
				Data = new EntitlementInfoPopupContent.EntitlementInfoPopUpData
				{
					Description = LocalizationManager.Instance.Get(_fakeItem.descriptionlangKey.neutral),
					TextureGuid = _fakeItem.guid
				}
			});
		}
	}
}
