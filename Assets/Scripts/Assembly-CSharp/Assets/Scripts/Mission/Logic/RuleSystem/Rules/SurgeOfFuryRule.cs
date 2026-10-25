using System.Collections.Generic;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.RuleSystem.Triggers;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class SurgeOfFuryRule : SkillRuleBase<PlayerCharacter>
	{
		public SurgeOfFuryRule(RuleData.IRuleDefinition _definition, IList<float> _parameters)
			: base(_definition, _parameters)
		{
			AddMandatoryTrigger(new CombatActiveSkillTrigger<PlayerCharacter, ISkillPayload>(this, ETargetSource.RuleOwner)
			{
				ActivityCondition = true
			});
			AddMandatoryTrigger(new CombatIdleDelayedSkillTrigger<PlayerCharacter, ISkillPayload>(this, ETargetSource.RuleOwner, true));
			AddMandatoryTrigger(new CombatActiveSkillTrigger<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload));
			BBList<IEffect> effects = new BBList<IEffect>
			{
				new GeneralDamageSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, EApplyDamageFlags.SkillNormal, false)
			};
			AddEffect(new ActionSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, OnExecuteSkill));
			BBList<IEffect> effects2 = new BBList<IEffect>
			{
				new SurgeOfFurySkillEffect(this, effects)
			};
			AddEffect(new AnimatorListeningSkillEffectsWrapper<PlayerCharacter, EntityIdSkillPayload>(this, ETargetSource.RuleOwner, effects2, true, AnimatorEventType.Hit));
		}

		private static IEffectInstance OnExecuteSkill(IEffect _effect, PlayerCharacter _player, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, IEntityIdSkillPayload _payload)
		{
			_player.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskSurgeOfFury(_payload.Id);
			return null;
		}
	}
}
