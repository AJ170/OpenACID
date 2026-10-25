using Assets.Scripts.Mission.Logic.Stats;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class StatTypeExtensions
	{
		public static StatTypes.EStatType RulesStart()
		{
			return StatTypes.EStatType.Rule_SkillDuration;
		}

		public static StatTypes.EStatType RulesEnd()
		{
			return StatTypes.EStatType.__End_Immutable_Rule;
		}

		public static bool IsRule(this StatTypes.EStatType _value)
		{
			return _value >= RulesStart() && _value < RulesEnd();
		}

		public static bool IsUiRelevantRuleStat(this StatTypes.EStatType _statType)
		{
			RuleDescription value;
			return UbiCore.Utils.Singleton<UIDataMapper>.Instance.RuleDescriptions.TryGetValue((int)_statType, out value) && value != null;
		}
	}
}
