using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Screens.Popup.ItemPopup;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.ScreenManager.Popup.Widgets
{
	public class UIPopupItemImageWidget : MonoBehaviour
	{
		[SerializeField]
		private UIPortraitTexture m_itemTexture;

		[SerializeField]
		private UIExtendedLabel m_itemCountLabel;

		public void Configure(ItemPopupContent.PopupItemConfiguration _config, bool _resize = true)
		{
			m_itemCountLabel.SetLocalizationKey(1784, new StringMapper { { "Items_Current", _config.Amount } });
			m_itemCountLabel.gameObject.SetActive(_config.Amount > 1);
			m_itemTexture.IsResizeWidgetToOrgSize = _resize;
			UIItemUtil.UpdateShopItem(_config, true, m_itemTexture, UIItemUtil.ItemUVCoordType.ENone, 0f);
		}
	}
}
