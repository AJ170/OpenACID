using Assets.Scripts.UI.Localization;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.SkillTreeScreen
{
	public class ValueHandler : MonoBehaviour
	{
		[SerializeField]
		private UIExtendedLabel m_valueLabel;

		[SerializeField]
		private UISprite m_backgroundSprite;

		private SkillTreeData m_data;

		private Color m_color;

		private int m_value;

		private bool m_locked;

		private bool m_visible;

		public int Value
		{
			get
			{
				return m_value;
			}
			set
			{
				m_value = value;
				UpdateLabel();
			}
		}

		public bool Locked
		{
			get
			{
				return m_locked;
			}
			set
			{
				m_locked = value;
				UpdateSprite();
			}
		}

		public bool Visible
		{
			get
			{
				return m_visible;
			}
			set
			{
				m_visible = value;
				base.gameObject.SetActive(m_visible);
				m_backgroundSprite.gameObject.SetActive(m_visible);
			}
		}

		public void Init(SkillTreeData _data)
		{
			m_data = _data;
		}

		private void Awake()
		{
			m_valueLabel.supportEncoding = true;
			Visible = m_visible;
			m_color = m_valueLabel.color;
		}

		private void UpdateLabel()
		{
			m_valueLabel.supportEncoding = true;
			m_valueLabel.text = string.Format("{0}{1}[-]", (Value != 5) ? m_color.ToNGUIColorString() : m_data.MaxLevelColor.ToNGUIColorString(), LocalizationManager.Instance.GetInteger(Value));
			UpdateSprite();
			Visible = true;
		}

		private void UpdateSprite()
		{
			if (m_locked)
			{
				m_backgroundSprite.spriteName = "aUI_fra_skillRank_zero";
			}
			else
			{
				m_backgroundSprite.spriteName = ((Value != 5) ? "aUI_fra_skillRank" : "aUI_fra_skillRank_max");
			}
		}
	}
}
