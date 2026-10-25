using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.RuleSystem.Triggers;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class PiercingStrikeRule : SkillRuleBase<PlayerCharacter>
	{
		public PiercingStrikeRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
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
			int num = Definition.Get_int(StatTypes.EStatType.Rule_PiercingStrikeCount);
			AdditiveNumberBuffGenerator_int buffGenerator = new AdditiveNumberBuffGenerator_int(EBuffType.PiercingStrike, StatValueCollection, StatTypes.EStatType.Rule_SkillDuration, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid)
			{
				Value = 1
			};
			for (int i = 0; i < num; i++)
			{
				AddEffect(new BuffSkillEffect<PlayerCharacter, ISkillPayload>(this, ETargetSource.RuleOwner, buffGenerator));
			}
			AddEffect(new FearsomeSkillEffect<AssassinCharacter, ISkillPayload>(this, ETargetSource.RuleOwner));
			AddEffect(new ExplicitStatsUpdateSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner));
			AddEffect(new CombatInitSkillEffect(this));
		}

		private bool ActivityPredicate(PlayerCharacter _entity, IBuffedStatsCollectionHandle _stats, EntityId _entityId)
		{
			return _entityId.GetEntity<PlayerCharacter>().PlayerStats.Get_int(StatTypes.EStatType.Character_Conflict_PiercingStrikeCount) > 0;
		}
	}
}
