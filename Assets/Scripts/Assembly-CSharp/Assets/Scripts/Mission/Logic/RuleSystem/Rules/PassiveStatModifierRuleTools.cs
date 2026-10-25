using System.Collections.Generic;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.System.Extensions;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public static class PassiveStatModifierRuleTools
	{
		public static object LinearSkillParameterValueProcessor_float(IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value, IList<float> _parameters)
		{
			float num = _stats.Get_float(StatTypes.EStatType.Rule_ParameterFactor);
			return (float)_value + num * _parameters.FirstOrDefault((float P_0) => true);
		}

		public static object LinearSkillParameterValueProcessor_int(IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value, IList<float> _parameters)
		{
			float num = _stats.Get_float(StatTypes.EStatType.Rule_ParameterFactor);
			return (int)((float)(int)_value + num * _parameters.FirstOrDefault((float P_0) => true));
		}
	}
}
