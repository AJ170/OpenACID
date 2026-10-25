using System;
using AcierProtocol;
using Assets.Scripts.UI.Localization;

namespace Assets.Scripts.UI.Types
{
	public class UIAchievementGoal : IFormattable
	{
		private readonly long m_Value;

		private readonly ECounterType m_CounterType;

		public long DisplayValue
		{
			get
			{
				switch (m_CounterType)
				{
				case ECounterType.DisguiseTimeInSeconds:
				case ECounterType.EagleVisionUsageInSeconds:
				case ECounterType.BleedingSplinterBombSeconds:
				case ECounterType.RestOnBenchSeconds:
				case ECounterType.FrightenedTimeInSeconds:
				case ECounterType.SecondsTracedBySeekers:
				case ECounterType.MaxSecondsOnGround:
					return m_Value / 60;
				default:
					return m_Value;
				}
			}
		}

		public UIAchievementGoal(long _value, ECounterType _counterType)
		{
			m_Value = _value;
			m_CounterType = _counterType;
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return LocalizationManager.Instance.GetInteger(DisplayValue, format);
		}
	}
}
