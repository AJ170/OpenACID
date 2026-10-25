using System.Collections.Generic;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class ThrowSmokeBombRule : ThrowSmokeBombRuleBase
	{
		public ThrowSmokeBombRule(RuleData.IRuleDefinition _definition, IList<float> _parameters)
			: base(_definition, _parameters)
		{
			AddEffect(new StopApproachingEffect<AssassinCharacter>(this)
			{
				Categories = ETriggerCategoryFlags.Roaming
			});
			AddEffect(new AbortSkillEffect(this, "Disguise_01".ToEnum<SkillTypes>())
			{
				Categories = ETriggerCategoryFlags.Roaming
			});
			AddEffect(new AbortSkillEffect(this, "Eagle_Vision".ToEnum<SkillTypes>())
			{
				Categories = ETriggerCategoryFlags.Roaming
			});
			BBList<IEffect> effects = new BBList<IEffect>
			{
				new ExitCombatSkillEffect<AssassinCharacter>(this, ETargetSource.RuleOwner, ExitCombatMode.SmokeBomb),
				new TargetCollectorBuffSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, base.CollectTargetsAndSetLastKnownPos, BuffHelpers.MakeSmokeBombedBuffGenerator(StatValueCollection, RulesHelper.LinearHirelingLevelTurnCountProcessor1), null),
				new NotorietyChangeSkillEffect<AssassinCharacter>(this, 0.5f, ETargetSource.Player)
				{
					Categories = SkillCategories
				},
				new SpawnFxSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, SpawnPosGetter),
				new AddCrowdHotspotEffect(this, ETargetSource.RuleOwner)
			};
			BBList<IEffect> effects2 = new BBList<IEffect>
			{
				new ActionSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, OnThrowBomb),
				new AnimatorListeningSkillEffectsWrapper<AssassinCharacter, EntityIdSkillPayload>(this, ETargetSource.RuleOwner, effects, false, AnimatorEventType.ReactionStart),
				new ExposingEffect(this)
			};
			AddEffect(new ActionSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, base.OnHaltCombat));
			AddEffect(new DelaySkillEffect<AssassinCharacter, ISkillPayload>(this, ETargetSource.RuleOwner, base.AllReadyForExitCondition, false, effects2));
		}

		private IEffectInstance OnThrowBomb(IEffect _effect, AssassinCharacter _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, IEntityIdSkillPayload _payload)
		{
			_entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskThrowSmokeBomb();
			if (!_entity.GetIsInCombat())
			{
				_entity.AssassinCharacterStateMachines.PlayerMain.EnterCallbackDelayState(EStateType.Generic_WaitingForAnim, EStateType.CombatCharacterMain_Roaming, EStateType.CombatCharacterMain_Roaming, AnimationTaskType.ThrowSmokeBomb, AnimationTaskManager.ELayer.UpperBody);
			}
			return null;
		}

		protected override Vector3 SpawnPosGetter(ISkillsCarrier _entity, EntityId _targetEntityId, IBuffedStatsCollectionHandle _stats)
		{
			Vector3 position = _targetEntityId.GameEntity.UnityComponentCache.AnimationController.Animator.GetBoneTransform(HumanBodyBones.RightHand).position;
			position.y = _targetEntityId.GameEntity.Position.y;
			return position;
		}
	}
}
