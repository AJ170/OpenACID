using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Triggers;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public abstract class ApproachSkillRuleBase : SkillRuleBase<AssassinCharacter>, IRule, ISkillRule, IApproachSkillRule
	{
		public SkillData.IApproachSkillDefinition ApproachSkillDefinition
		{
			get
			{
				return (SkillData.IApproachSkillDefinition)SkillDefinition;
			}
		}

		public float ApproachMaxDistance
		{
			get
			{
				return ApproachSkillDefinition.MaxDistance;
			}
		}

		public float ApproachMinDistance
		{
			get
			{
				return ApproachSkillDefinition.MinDistance;
			}
		}

		public float ApproachMinHeight
		{
			get
			{
				return ApproachSkillDefinition.MinHeight;
			}
		}

		public float ApproachStartDistance
		{
			get
			{
				return ApproachSkillDefinition.StartDistance;
			}
		}

		protected ApproachSkillRuleBase(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			if (ApproachMinDistance > 0f)
			{
				AddMandatoryTrigger(new EntityInMinRangeApproachingSkillTrigger<AssassinCharacter, IEntityIdSkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.Payload, 0, true, (AssassinCharacter e, EntityId id, IBuffedStatsCollectionHandle s) => ApproachMinDistance));
			}
			if (ApproachMaxDistance != float.MaxValue)
			{
				AddMandatoryTrigger(new EntityInMaxRangeSkillTrigger<AssassinCharacter, IEntityIdSkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.Payload, 0, true, (AssassinCharacter e, EntityId id, IBuffedStatsCollectionHandle s) => ApproachMaxDistance));
			}
			if (ApproachMinHeight > 0f)
			{
				AddMandatoryTrigger(new AboveTargetTrigger(this, SkillFrequency, SkillCategories, ETargetSource.Payload, ApproachMinHeight));
			}
			AddMandatoryTrigger(new CharacterMainStateTrigger<AssassinCharacter, IEntityIdSkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.Payload, EStateType.CombatCharacterMain_Parcour)
			{
				Sign = ETriggerSign.Negative
			});
			AddTrigger(new CharacterMainStateTrigger<AssassinCharacter, ISkillPayload>(this, SkillFrequency, SkillCategories & ~ETriggerCategoryFlags.Parcour, ETargetSource.RuleOwner));
		}

		protected override void ApplyInternal(AssassinCharacter _entity, BBList<IEffect> _cumulatedEffects)
		{
			ESkillAvailabilityFlags _availabilityFlags;
			if (IsFulfilledInternal(_entity, null, TriggerPredicate, out _availabilityFlags))
			{
				_cumulatedEffects.AddRange(Effects);
			}
		}

		private bool TriggerPredicate(ITrigger _trigger, IRulesCarrier _rulesCarrier)
		{
			if (_trigger.Type == ETriggerType.EntityInMinRangeApproaching)
			{
				return false;
			}
			return true;
		}
	}
}
