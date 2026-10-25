using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup.SkillPopup
{
	public class UISkillPopupSkillWidget : MonoBehaviour
	{
		[SerializeField]
		private UIExtendedLabel m_label;

		[SerializeField]
		private UITexture m_icon;

		private string m_colorCode;

		private void Awake()
		{
			m_colorCode = ColorExtensions.ToNGUIColorString(UIDataMapper.UITextColors.SkillColor);
			m_label.supportEncoding = true;
		}

		public void Init(Assets.Scripts.UI.Utils.Extensions.SkillData _data, bool _showName)
		{
			Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition definition = UbiCore.Utils.Singleton<Assets.Scripts.Mission.Definitions.SkillData>.Instance.GetDefinition(_data.SkillId, (_data.Level <= 0) ? 1 : _data.Level);
			if (definition != null)
			{
				m_icon.mainTexture = definition.IconUp.GetObject<Texture2D>();
				m_label.text = m_colorCode + LocalizationManager.Instance.Get(definition.NameLocaKey.Key);
			}
			m_label.gameObject.SetActive(_showName);
		}
	}
}
