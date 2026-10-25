using System;
using AcierProtocol;
using Assets.Scripts.UI.Localization;

namespace Assets.Scripts.UI.Types
{
	public class UIShopModuleQuality : IFormattable
	{
		private static readonly StringId s_longMisisonCount = 2011;

		private static readonly StringId s_longMissionTime = 2012;

		private static readonly StringId s_shortMissionCount = 5625;

		private static readonly StringId s_shortMissionTime = 5626;

		private readonly LootTemplate.ModuleTemplate m_quality;

		private readonly bool m_useShortString;

		public UIShopModuleQuality(LootTemplate.ModuleTemplate _quality, bool _useShortString = false)
		{
			m_quality = _quality;
			m_useShortString = _useShortString;
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
			LocalizationManager instance = LocalizationManager.Instance;
			if (m_quality.durationType == EDurationType.MissionCount)
			{
				StringId key = ((!m_useShortString) ? s_longMisisonCount : s_shortMissionCount);
				return instance.Get(key, new StringMapper { 
				{
					"value",
					instance.GetInteger(m_quality.duration)
				} });
			}
			if (m_quality.durationType == EDurationType.Time)
			{
				StringId key2 = ((!m_useShortString) ? s_longMissionTime : s_shortMissionTime);
				return instance.Get(key2, new StringMapper { 
				{
					"time",
					UIDuration.FromSeconds(m_quality.duration, false, true)
				} });
			}
			return m_quality.duration.ToString();
		}
	}
}
