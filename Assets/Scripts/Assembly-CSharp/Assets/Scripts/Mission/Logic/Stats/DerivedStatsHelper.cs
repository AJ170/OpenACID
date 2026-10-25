using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public static class DerivedStatsHelper
	{
		public static float StrengthDamage(float _sourceStrength, float _overrideValue)
		{
			return (!(_overrideValue >= 0f)) ? (_sourceStrength * 0.1f) : _overrideValue;
		}

		public static float StrengthDamageReduction(float _targetStrength, float _overrideValue)
		{
			return (!(_overrideValue >= 0f)) ? (_targetStrength * 0.1f) : _overrideValue;
		}

		public static float ArmourDamageReduction(float _targetArmour, float _overrideValue)
		{
			if (_overrideValue >= 0f)
			{
				return _overrideValue;
			}
			return DiminishingReturns(_targetArmour, 500f, 1000f, 2000f, 0.1f, 0.05f, 0.025f, 0.01f);
		}

		public static float DodgeChance(float _evasion, float _overrideValue)
		{
			if (_overrideValue >= 0f)
			{
				return _overrideValue;
			}
			float num = DiminishingReturns(_evasion, 250f, 500f, 1000f, 0.06f, 0.03f, 0.015f, 0.0075f);
			return num * 0.01f;
		}

		public static float CriticalChance(float _precision, float _overrideValue)
		{
			if (_overrideValue >= 0f)
			{
				return _overrideValue;
			}
			float num = DiminishingReturns(_precision, 250f, 500f, 1000f, 0.06f, 0.03f, 0.015f, 0.0075f);
			return num * 0.01f;
		}

		public static float ArmourPiercingFlat(float _precision, float _overrideValue)
		{
			if (_overrideValue >= 0f)
			{
				return _overrideValue;
			}
			return DiminishingReturns(_precision, 250f, 500f, 1000f, 0.2f, 0.1f, 0.05f, 0.02f);
		}

		public static float ArmourPiercingFactor(float _piercingSurprise)
		{
			return Mathf.Clamp01(1f - _piercingSurprise);
		}

		public static float Feint(float _evasion, float _overrideValue)
		{
			return (!(_overrideValue >= 0f)) ? (_evasion * (1f / 15f)) : _overrideValue;
		}

		public static float FeintDamage(float _feint)
		{
			return _feint;
		}

		public static float FeintDodgeChance(float _feint)
		{
			return _feint * 0.01f;
		}

		public static float StealthModifier(float _incognito)
		{
			return Mathf.Max(1f, _incognito * 0.1f);
		}

		private static float DiminishingReturns(float value, float t1, float t2, float t3, float f0, float f1, float f2, float f3)
		{
			if (value <= t1)
			{
				return value * f0;
			}
			if (value <= t2)
			{
				return t1 * f0 + (value - t1) * f1;
			}
			if (value <= t3)
			{
				return t1 * f0 + (t2 - t1) * f1 + (value - t2) * f2;
			}
			return t1 * f0 + (t2 - t1) * f1 + (t3 - t2) * f2 + (value - t3) * f3;
		}
	}
}
