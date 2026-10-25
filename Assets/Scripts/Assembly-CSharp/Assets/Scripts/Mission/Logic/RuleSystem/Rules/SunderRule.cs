using System.Collections.Generic;
using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.RuleSystem.Triggers;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class SunderRule : CombatSkillRuleBase
	{
		public override SkillActBehaviour.IParams SkillParams
		{
			get
			{
				SkillActBehaviour.Params @params = new SkillActBehaviour.Params();
				@params.ActAnimation = new ActAnimation
				{
					AttackMelee = AttackMeleeType.ShreddingArmor,
					ActionUpperBody = ActionTypeUpperBody.Invalid,
					WaitForHash = AnimationStateMachineHashes.StateMachine_CombatAbilities
				};
				@params.ReactAnimation = new ReactAnimation
				{
					Reaction = ReactionType.GetHit,
					ReactionEvent = AnimatorEventType.Hit,
					AttackMelee = AttackMeleeType.ShreddingArmor,
					ActionUpperBody = ActionTypeUpperBody.Invalid,
					WaitForHash = AnimationStateMachineHashes.StateMachine_GetHit,
					ReactionDie = ReactionType.Die,
					AttackMeleeDie = AttackMeleeType.ShreddingArmor
				};
				return @params;
			}
		}

		public SunderRule(RuleData.IRuleDefinition _definition, IList<float> _parameters)
			: base(_definition, _parameters)
		{
			AddMandatoryTrigger(new CombatActiveSkillTrigger<PlayerCharacter, ISkillPayload>(this, ETargetSource.RuleOwner)
			{
				ActivityCondition = true
			});
			AddMandatoryTrigger(new CombatIdleDelayedSkillTrigger<PlayerCharacter, ISkillPayload>(this, ETargetSource.RuleOwner, true));
			AddMandatoryTrigger(new CombatActiveSkillTrigger<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload));
			AddMandatoryTrigger(new CombatTargetMatchPayloadSkillTrigger<IEntityIdSkillPayload>(this));
			AddEffect(new CombatTargetedSkillEffect(this, this));
			BBList<IEffect> effects = new BBList<IEffect>
			{
				new GeneralDamageSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, EApplyDamageFlags.SkillNormal, true),
				new BuffSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, new InverseMultiplicativeNumberBuffGenerator_float(EBuffType.Shredded, StatValueCollection, StatTypes.EStatType.Rule_ShreddedDuration, StatTypes.EStatType.Invalid, StatTypes.EStatType.Rule_ShreddedFactor))
				{
					IsAbortable = false
				},
				new CombatReactSkillEffect(this, AnimatorEventType.Hit, true, CharacterCondition.EAnimAllowed.GetHit)
			};
			AddEffect(new AnimatorListeningSkillEffectsWrapper<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, effects, true, AnimatorEventType.Hit));
		}
	}
}
