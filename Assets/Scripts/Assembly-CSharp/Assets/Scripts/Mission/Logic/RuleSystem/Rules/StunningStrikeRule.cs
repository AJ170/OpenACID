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
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class StunningStrikeRule : CombatSkillRuleBase
	{
		public override SkillActBehaviour.IParams SkillParams
		{
			get
			{
				SkillActBehaviour.Params @params = new SkillActBehaviour.Params();
				@params.ActAnimation = new ActAnimation
				{
					AttackMelee = AttackMeleeType.StunningStrike,
					ActionUpperBody = ActionTypeUpperBody.Invalid,
					WaitForHash = AnimationStateMachineHashes.StateMachine_CombatAbilities
				};
				@params.ReactAnimation = new ReactAnimation
				{
					Reaction = ReactionType.StunningStrike,
					ReactionEvent = AnimatorEventType.ReactionStart,
					AttackMelee = AttackMeleeType.Invalid,
					ActionUpperBody = ActionTypeUpperBody.Invalid,
					WaitForHash = AnimationStateMachineHashes.StateMachine_StunStrikeReaction,
					ReactionDie = ReactionType.Die,
					AttackMeleeDie = AttackMeleeType.StunningStrike
				};
				return @params;
			}
		}

		public StunningStrikeRule(RuleData.IRuleDefinition _definition, IList<float> _parameters)
			: base(_definition, _parameters)
		{
			AddMandatoryTrigger(new CombatActiveSkillTrigger<PlayerCharacter, ISkillPayload>(this, ETargetSource.RuleOwner)
			{
				ActivityCondition = true
			});
			AddMandatoryTrigger(new CombatIdleDelayedSkillTrigger<PlayerCharacter, ISkillPayload>(this, ETargetSource.RuleOwner, true));
			AddMandatoryTrigger(new CombatCanReactSkillTrigger<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, CharacterCondition.EActionAllowed.StunningStrike));
			AddMandatoryTrigger(new CombatTargetMatchPayloadSkillTrigger<IEntityIdSkillPayload>(this));
			AddEffect(new CombatTargetedSkillEffect(this, this));
			BBList<IEffect> effects = new BBList<IEffect>
			{
				new GeneralDamageSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, EApplyDamageFlags.SkillNormal, true),
				new BuffSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, BuffHelpers.MakeStunningBuffGenerator(StatValueCollection))
				{
					IsAbortable = false
				},
				new CombatReactSkillEffect(this, AnimatorEventType.Hit, true, CharacterCondition.EAnimAllowed.StunReaction, EStateType.CharacterCondition_StunningStrike)
			};
			BBList<IEffect> effects2 = new BBList<IEffect>
			{
				new CombatReactSkillEffect(this, AnimatorEventType.ReactionStart, false, CharacterCondition.EAnimAllowed.StunReaction),
				new AnimatorListeningSkillEffectsWrapper<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, effects, false, AnimatorEventType.Hit)
			};
			AddEffect(new AnimatorListeningSkillEffectsWrapper<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, effects2, true, AnimatorEventType.ReactionStart));
		}
	}
}
