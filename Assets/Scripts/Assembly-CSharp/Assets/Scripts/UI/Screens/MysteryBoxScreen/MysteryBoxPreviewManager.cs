using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Screens.MysteryBox;
using Assets.Scripts.UI.Utils;
using Database;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.MysteryBoxScreen
{
	public class MysteryBoxPreviewManager : MonoBehaviour
	{
		[SerializeField]
		private UIExtendedLabel m_header;

		[SerializeField]
		private UIExtendedLabel m_description;

		[SerializeField]
		private UIPortraitTexture m_previewTexture;

		private Assets.Scripts.UI.Screens.MysteryBox.MysteryBoxData m_data;

		private void Awake()
		{
		}

		public void Setup(Assets.Scripts.UI.Screens.MysteryBox.MysteryBoxData _data)
		{
			m_data = _data;
			ColorDef colorDefForColorGuid = UIItemUtil.GetColorDefForColorGuid(UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemQualityDescriptions[(int)m_data.RewardQuality].ColorGUID);
			m_previewTexture.RarityColor = colorDefForColorGuid.Color0;
			if (_data.Chain.mysteryBoxType == EMysteryBoxType.MysteryBoxRessourceType)
			{
				m_description.text = LocalizationManager.Instance.Get(7250);
				m_header.SetLocalizationKey(7257);
				return;
			}
			UINGUIFormattedText formattedColoredText = UIItemUtil.GetFormattedColoredText(m_data.RewardQuality);
			m_description.text = LocalizationManager.Instance.Get(6691, new StringMapper { { "color_Code", formattedColoredText } });
			m_header.SetLocalizationKey(6690, new StringMapper { { "item_Quality", formattedColoredText } });
		}

		private void OnDestroy()
		{
		}

		public void OnGeneralUpdate()
		{
		}
	}
}
