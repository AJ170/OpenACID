using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.RuleSystem.Triggers;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class EagleVisionRule : SkillRuleBase<PlayerCharacter>
	{
		public EagleVisionRule(RuleData.IRuleDefinition _definition, IList<float> _parameters)
			: base(_definition, _parameters)
		{
			ActivateCategoryCheckDuringActivity();
			AddMandatoryTrigger(new CharacterMainStateTrigger<PlayerCharacter, ISkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.RuleOwner, new int[8] { 53, 63, 64, 54, 65, 66, 67, 68 })
			{
				Sign = ETriggerSign.Negative,
				ActivityCondition = true
			});
			AddMandatoryTrigger(new AssassinExposedTrigger(this, SkillFrequency, SkillCategories)
			{
				Sign = ETriggerSign.Negative,
				ActivityCondition = true
			});
			AddTrigger(new TargetPredicateSkillTrigger<PlayerCharacter, ISkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.RuleOwner, CheckAbortEagleVision)
			{
				ActivityCondition = true,
				TriggerCondition = false
			});
			AddEffect(new BuffSkillEffect<PlayerCharacter, ISkillPayload>(this, ETargetSource.RuleOwner, new BooleanOrBuffGenerator(EBuffType.EagleEye, StatValueCollection, StatTypes.EStatType.Rule_SkillDuration, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid)
			{
				Value = true
			}));
		}

		private static bool HasProperProfileSpeed(PlayerCharacter _playerCharacter)
		{
			if (_playerCharacter.GetSpeed() != EMovementSpeed.Walk)
			{
				return _playerCharacter.GetIsHiding();
			}
			return true;
		}

		private bool CheckAbortEagleVision(PlayerCharacter _playerCharacter, IBuffedStatsCollectionHandle stats, EntityId targetId)
		{
			return HasProperProfileSpeed(_playerCharacter);
		}
	}
}
