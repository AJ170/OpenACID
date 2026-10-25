using System.Collections.Generic;
using Assets.Scripts.Animation;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.RuleSystem.Triggers;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public abstract class BlackjackRuleBase : ApproachSkillRuleBase
	{
		protected BlackjackRuleBase(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			AddMandatoryTrigger(new LivingEnemySkillTrigger<AssassinCharacter, IEntityIdSkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.Payload));
			AddMandatoryTrigger(new AssassinAnonymousTrigger(this, SkillFrequency, SkillCategories)
			{
				TriggerCondition = true
			});
			AddEffect(new ActionSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, OnBlackjack));
			BBList<IEffect> effects = new BBList<IEffect>
			{
				new PickpocketSkillEffect<AssassinCharacter>(this, ETargetSource.Payload, StatTypes.EStatType.Rule_HirelingLevelFactor_2, StatTypes.EStatType.Rule_HirelingLevelFactor_3),
				new ActionSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, OnHit),
				new AddCrowdHotspotSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, 0f, 20f, 15f, true, 0f, 0f, ReactionType.Invalid)
			};
			AddEffect(new AnimatorListeningSkillEffectsWrapper<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, effects, false, AnimatorEventType.Hit));
		}

		private static IEffectInstance OnBlackjack(IEffect _effect, AssassinCharacter _assassin, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, IEntityIdSkillPayload _payload)
		{
			Character entity = _targetEntityId.GetEntity<Character>();
			AnimationController.Direction targetDir = AnimationHelper.DetermineDirectionFrontOrBack(_assassin.GameObject, entity.GameObject);
			_assassin.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskBlackjackInstigator(_assassin.Id, _targetEntityId, targetDir);
			if (entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskBlackjackTarget(_assassin.Id, _targetEntityId, targetDir))
			{
				int stunDuration = RulesHelper.LinearHirelingLevelTurnCountProcessor1(_assassin, _stats, _stats.Get_int(StatTypes.EStatType.Rule_StunDuration));
				EffectsHelper.CharacterBlackjacked(entity, stunDuration);
			}
			return null;
		}

		private static IEffectInstance OnHit(IEffect _effect, AssassinCharacter _assassin, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, IEntityIdSkillPayload _payload)
		{
			Character entity = _targetEntityId.GetEntity<Character>();
			IEnumerable<EnemyCharacter> enumerable = LogicMain.EntityManager.CollectEntitiesInRadius<EnemyCharacter>(_assassin.Position, 5f, CharacterHelper.FilterEnemyAlivePredicate);
			foreach (EnemyCharacter item in enumerable)
			{
				if (item != entity)
				{
					EffectsHelper.CharacterAlarmed(_assassin, item);
				}
			}
			if (_assassin is HirelingCharacter)
			{
				LogicMain.MissionResult.IncreaseSuccessfullHirelingAttack();
			}
			return null;
		}
	}
}
