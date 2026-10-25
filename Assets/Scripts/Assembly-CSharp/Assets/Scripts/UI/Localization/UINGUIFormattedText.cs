using System;

namespace Assets.Scripts.UI.Localization
{
	public class UINGUIFormattedText : IFormattable
	{
		private readonly object m_text;

		private readonly INGUIFormatProvider[] m_providers;

		public UINGUIFormattedText(object _text, params INGUIFormatProvider[] _providers)
		{
			m_text = _text;
			m_providers = _providers ?? new INGUIFormatProvider[0];
		}

		public override string ToString()
		{
			return ToString(string.Empty, null);
		}

		public string ToString(string _format)
		{
			return ToString(_format, null);
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			string text = string.Format(formatProvider, "{0}", m_text);
			for (int i = 0; i < m_providers.Length; i++)
			{
				text = m_providers[i].Format(text);
			}
			return text;
		}
	}
}
