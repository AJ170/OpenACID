using UnityEngine;

namespace Assets.Scripts.UI.Screens.CreditsScreen
{
	internal class CreditsTextTab : CreditsTab
	{
		[SerializeField]
		private UIExtendedLabel m_text;

		private int m_baseFontSize = 20;

		public override void Init()
		{
			m_baseFontSize = m_text.fontSize;
		}

		private void Awake()
		{
		}

		public override void UpdateItem(ICreditsItem _item)
		{
			base.UpdateItem(_item);
			CreditsTextItem creditsTextItem = _item as CreditsTextItem;
			m_text.text = creditsTextItem.Text;
		}

		public void SetFontScale(float _scale)
		{
			m_text.fontSize = Mathf.RoundToInt((float)m_baseFontSize * _scale);
		}
	}
}
