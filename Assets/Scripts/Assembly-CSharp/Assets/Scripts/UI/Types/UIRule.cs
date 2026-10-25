using System;
using System.Globalization;
using Assets.Scripts.Mission.Logic.RuleSystem;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Types
{
	public class UIRule : IFormattable
	{
		private readonly float m_value;

		private readonly StatTypes.EStatType m_statType;

		private readonly StatTypes.EStatUnit m_statUnit;

		private readonly IReadonlyStatValueCollection m_stats;

		private readonly int m_hirelingLevel;

		private readonly bool m_isStatDelta;

		public float Value
		{
			get
			{
				return m_value;
			}
		}

		public float DisplayValue
		{
			get
			{
				return GetDisplayValue();
			}
		}

		protected UIRule(float _value, StatTypes.EStatType _statType, StatTypes.EStatUnit _statUnit, IReadonlyStatValueCollection _stats, int _hirelingLevel, bool _isStatDelta)
		{
			m_stats = _stats;
			m_value = _value;
			m_statType = _statType;
			m_statUnit = _statUnit;
			m_hirelingLevel = _hirelingLevel;
			m_isStatDelta = _isStatDelta;
		}

		public UIRule(IReadonlyStatValue _statValue, IReadonlyStatValueCollection _stats)
			: this(_statValue, _stats, 0)
		{
		}

		public UIRule(float _value, StatTypes.EStatType _statType, StatTypes.EStatUnit _statUnit)
			: this(_value, _statType, _statUnit, null, 0, false)
		{
		}

		public UIRule(float _value, StatTypes.EStatType _statType, StatTypes.EStatUnit _statUnit, bool _isStatDelta)
			: this(_value, _statType, _statUnit, null, 0, _isStatDelta)
		{
		}

		public UIRule(IReadonlyStatValue _statValue, IReadonlyStatValueCollection _stats, int _hirelingLevel)
			: this(0f, _statValue.StatType, _statValue.Unit, _stats, _hirelingLevel, false)
		{
			if (_statValue.ValueType == typeof(int) || _statValue.ValueType == typeof(float))
			{
				m_value = RulesHelper.GetScaledRuleStatValue(_stats, _statValue.StatType, _hirelingLevel);
			}
		}

		private float GetDisplayValue()
		{
			float num = m_value;
			if (m_statType == StatTypes.EStatType.Rule_BleedAmountRelative || m_statType == StatTypes.EStatType.Rule_BleedAmount)
			{
				float scaledRuleStatValue = RulesHelper.GetScaledRuleStatValue(m_stats, StatTypes.EStatType.Rule_BleedDuration, m_hirelingLevel);
				float scaledRuleStatValue2 = RulesHelper.GetScaledRuleStatValue(m_stats, StatTypes.EStatType.Rule_BleedInterval, m_hirelingLevel);
				num *= scaledRuleStatValue / scaledRuleStatValue2;
			}
			if ((m_statUnit == StatTypes.EStatUnit.Factor || m_statUnit == StatTypes.EStatUnit.Chance) && m_statType != StatTypes.EStatType.Rule_StealthModifierBonus)
			{
				int num2 = (int)Mathf.Round(num * 100f);
				return num2;
			}
			if (m_statUnit == StatTypes.EStatUnit.FactorBonus)
			{
				int num3 = 0;
				num3 = (m_isStatDelta ? ((int)Mathf.Round(num * 100f)) : ((Mathf.Abs(num) > 0f) ? ((int)Mathf.Round((num - (float)((num > 1f) ? 1 : (-1))) * 100f)) : 0));
				return num3;
			}
			if (m_statType == StatTypes.EStatType.Rule_SkillCooldownDuration)
			{
				int num4 = (int)num;
				int num5 = ((m_stats != null && !m_isStatDelta) ? m_stats.Get_int(StatTypes.EStatType.Rule_SkillDuration) : 0);
				int num6 = Math.Max(num4 - num5, 0);
				return (float)num6 * 0.1f;
			}
			if (m_statUnit == StatTypes.EStatUnit.Turns)
			{
				return (float)(int)num * 0.1f;
			}
			if (m_stats != null && (m_statType == StatTypes.EStatType.Rule_SweetMusicHidingSpots || m_statType == StatTypes.EStatType.Rule_GlitchHidingSpots))
			{
				int activeBitCount = NumberExtensions.GetActiveBitCount(m_stats.Get_int(m_statType));
				return activeBitCount;
			}
			return num;
		}

		public bool DisplaysEqual(UIRule _rhs)
		{
			return ToString().Equals(_rhs.ToString());
		}

		public override string ToString()
		{
			return ToString(string.Empty, CultureInfo.InvariantCulture);
		}

		public string ToString(string _format)
		{
			return ToString(_format, CultureInfo.InvariantCulture);
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			switch (m_statType)
			{
			case StatTypes.EStatType.Rule_NotorietyIncreaseDelay:
				return LocalizationManager.Instance.GetInteger((int)Value);
			case StatTypes.EStatType.Rule_SweetMusicHidingSpots:
			case StatTypes.EStatType.Rule_GlitchHidingSpots:
				return LocalizationManager.Instance.GetInteger((int)DisplayValue);
			case StatTypes.EStatType.Rule_SkillCooldownDuration:
				return LocalizationManager.Instance.GetDecimal(DisplayValue);
			default:
				switch (m_statUnit)
				{
				case StatTypes.EStatUnit.Chance:
				case StatTypes.EStatUnit.Factor:
				case StatTypes.EStatUnit.FactorBonus:
					return LocalizationManager.Instance.GetInteger((int)DisplayValue);
				case StatTypes.EStatUnit.Turns:
					return LocalizationManager.Instance.GetDecimal(DisplayValue);
				default:
					return LocalizationManager.Instance.GetDecimal(DisplayValue, format);
				}
			}
		}
	}
}
