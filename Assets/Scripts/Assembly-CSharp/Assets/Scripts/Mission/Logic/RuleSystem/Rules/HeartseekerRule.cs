using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class HeartseekerRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		private static readonly Enum<SkillTypes> ThisSkillType = "Heartseeker_01".ToEnum<SkillTypes>();

		public HeartseekerRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
		}

		public static IEffect MakeEffect(IRule _owner, ETargetSource _targetSource)
		{
			return new GiveHpByChanceEffect<AssassinCharacter, ISkillPayload>(_owner, _targetSource, ChanceGetter, HpBonusGetter);
		}

		private static float ChanceGetter(AssassinCharacter _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (_entity.Skills.Has(ThisSkillType))
			{
				return _entity.Skills.GetBuffedStat_float(ThisSkillType, StatTypes.EStatType.Rule_RetrieveHealthChance);
			}
			return 0f;
		}

		private static float HpBonusGetter(AssassinCharacter _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (_entity.Skills.Has(ThisSkillType))
			{
				float num = _entity.PlayerStats.Get_float(StatTypes.EStatType.Character_Conflict_ActualMaxHealth);
				return num * _entity.Skills.GetBuffedStat_float(ThisSkillType, StatTypes.EStatType.Rule_RetrieveHealthRelative);
			}
			return 0f;
		}
	}
}
