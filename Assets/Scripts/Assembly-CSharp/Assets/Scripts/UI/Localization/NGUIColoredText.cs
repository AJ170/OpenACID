using UnityEngine;

namespace Assets.Scripts.UI.Localization
{
	public class NGUIColoredText : INGUIFormatProvider
	{
		private readonly Color m_color;

		public NGUIColoredText(Color _color)
		{
			m_color = _color;
		}

		public string Format(string _text)
		{
			return string.Format("{0}{1}[-]", m_color.ToNGUIColorString(), _text);
		}
	}
}
