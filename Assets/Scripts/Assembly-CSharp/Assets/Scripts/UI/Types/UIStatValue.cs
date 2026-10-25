using System;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils;
using MathNet.Numerics;
using UnityEngine;

namespace Assets.Scripts.UI.Types
{
	public class UIStatValue : IFormattable
	{
		private static readonly StringId s_baseValue = new StringId(2059);

		private static readonly StringId s_buffedValue = new StringId(7094);

		private static readonly StringId s_percentId = new StringId(1902);

		private readonly bool m_factor;

		private readonly bool m_buffed;

		private readonly StringMapper m_context;

		public UIStatValue(bool _factor, float _baseValue, float _buffedValue)
		{
			m_factor = _factor;
			m_buffed = !_baseValue.AlmostEqualInDecimalPlaces(_buffedValue, 3);
			string @decimal = LocalizationManager.Instance.GetDecimal((!m_factor) ? _baseValue : ((float)UIUtil.FormatPercentValue(_baseValue)));
			m_context = new StringMapper
			{
				{ "value", @decimal },
				{ "base_value", @decimal }
			};
			string decimal2 = LocalizationManager.Instance.GetDecimal((!m_factor) ? _buffedValue : ((float)UIUtil.FormatPercentValue(_buffedValue)));
			if (m_buffed)
			{
				Color32 color = ((!(_buffedValue < _baseValue)) ? UIDataMapper.UITextColors.PositiveColor : UIDataMapper.UITextColors.NegativeColor);
				m_context.Add("buffed_value", new UINGUIFormattedText(decimal2, new NGUIColoredText(color)));
			}
			else
			{
				m_context.Add("buffed_value", decimal2);
			}
		}

		public override string ToString()
		{
			return ToString(string.Empty, null);
		}

		public string ToString(string _format)
		{
			return ToString(_format, null);
		}

		public string ToString(string _format, IFormatProvider _formatProvider)
		{
			string text = LocalizationManager.Instance.Get((!m_buffed) ? s_baseValue : s_buffedValue, m_context);
			return (!m_factor) ? text : LocalizationManager.Instance.Get(s_percentId, new StringMapper { 
			{
				"value",
				new UINGUIFormattedText(text)
			} });
		}
	}
}
