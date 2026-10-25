using System.Collections.Generic;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.RuleSystem.Triggers;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class FastFingersRule : SkillRuleBase<PlayerCharacter>
	{
		private const int MaxStrikeCount = 6;

		public FastFingersRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			AddMandatoryTrigger(new LivingEnemySkillTrigger<PlayerCharacter, IEntityIdSkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.Payload));
			AddMandatoryTrigger(new CategorySkillTrigger<AssassinCharacter>(this, SkillFrequency, ETriggerCategoryFlags.Exclusions | ETriggerCategoryFlags.Roaming | ETriggerCategoryFlags.InCombat | ETriggerCategoryFlags.BeingChased | ETriggerCategoryFlags.Parcour)
			{
				TriggerCondition = false,
				ActivityCondition = true
			});
			AddMandatoryTrigger(new TargetPredicateSkillTrigger<PlayerCharacter, ISkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.RuleOwner, ActivityPredicate)
			{
				TriggerCondition = false,
				ActivityCondition = true
			});
			AddTrigger(new CombatIdleDelayedSkillTrigger<PlayerCharacter, ISkillPayload>(this, ETargetSource.RuleOwner));
			AddTrigger(new CombatActiveSkillTrigger<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload));
			AdditiveNumberBuffGenerator_int buffGenerator = new AdditiveNumberBuffGenerator_int(EBuffType.FastFingers, StatValueCollection, StatTypes.EStatType.Rule_SkillDuration, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid)
			{
				Value = 1
			};
			for (int i = 0; i < 6; i++)
			{
				int strike = i;
				AddEffect(new BuffSkillEffect<PlayerCharacter, ISkillPayload>(this, ETargetSource.RuleOwner, buffGenerator)
				{
					Predicate = (IEffect _effect, IRulesCarrier _rulesCarrier) => StrikeCountPredicate(_rulesCarrier, this, strike)
				});
			}
			AddEffect(new BuffSkillEffect<PlayerCharacter, ISkillPayload>(this, ETargetSource.RuleOwner, new MultiplicativeNumberBuffGenerator_float(StatTypes.EStatType.Character_Conflict_ActualCriticalMultiplier, StatValueCollection, StatTypes.EStatType.Rule_SkillDuration, StatTypes.EStatType.Invalid, StatTypes.EStatType.Rule_CriticalMultiplierFactor))
			{
				IsAbortable = true
			});
			AddEffect(new ExplicitStatsUpdateSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner));
			AddEffect(new CombatInitSkillEffect(this));
		}

		private static bool ActivityPredicate(PlayerCharacter _entity, IBuffedStatsCollectionHandle _stats, EntityId _entityId)
		{
			return _entityId.GetEntity<PlayerCharacter>().PlayerStats.Get_int(StatTypes.EStatType.Character_Conflict_FastFingersStrikeCount) > 0;
		}

		private bool StrikeCountPredicate(IRulesCarrier _rulesCarrier, IRule _rule, int _strike)
		{
			ISuperBuffsStatsCarrier superBuffsStatsCarrier = _rulesCarrier.Cast<ISuperBuffsStatsCarrier>();
			int num = _rule.StatValueCollection.Get_int(StatTypes.EStatType.Rule_FastFingersStrikeCount, superBuffsStatsCarrier);
			return _strike < num;
		}
	}
}
