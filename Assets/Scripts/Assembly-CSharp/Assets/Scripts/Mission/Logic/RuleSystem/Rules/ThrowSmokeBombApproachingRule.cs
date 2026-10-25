using System.Collections.Generic;
using Assets.Scripts.Animation.Tasks;
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
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class ThrowSmokeBombApproachingRule : ThrowSmokeBombRuleBase, IRule, ISkillRule, IApproachSkillRule
	{
		public float ApproachMaxDistance
		{
			get
			{
				return ((SkillData.ApproachSkillDefinition)SkillDefinition).MaxDistance;
			}
		}

		public float ApproachMinDistance
		{
			get
			{
				return ((SkillData.ApproachSkillDefinition)SkillDefinition).MinDistance;
			}
		}

		public float ApproachMinHeight
		{
			get
			{
				return ((SkillData.ApproachSkillDefinition)SkillDefinition).MinHeight;
			}
		}

		public float ApproachStartDistance
		{
			get
			{
				return ((SkillData.ApproachSkillDefinition)SkillDefinition).StartDistance;
			}
		}

		public ThrowSmokeBombApproachingRule(RuleData.IRuleDefinition _definition, IList<float> _parameters)
			: base(_definition, _parameters)
		{
			if (ApproachMinDistance > 0f)
			{
				AddMandatoryTrigger(new EntityInMinRangeSkillTrigger<AssassinCharacter, IEntityIdSkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.Payload, 0, true, (AssassinCharacter e, EntityId id, IBuffedStatsCollectionHandle s) => ApproachMinDistance));
			}
			if (ApproachMaxDistance != float.MaxValue)
			{
				AddMandatoryTrigger(new EntityInMaxRangeSkillTrigger<AssassinCharacter, IEntityIdSkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.Payload, 0, true, (AssassinCharacter e, EntityId id, IBuffedStatsCollectionHandle s) => ApproachMaxDistance));
			}
			if (ApproachMinHeight > 0f)
			{
				AddMandatoryTrigger(new AboveTargetTrigger(this, SkillFrequency, SkillCategories, ETargetSource.Payload, ApproachMinHeight));
			}
			AddMandatoryTrigger(new EntityInMaxRangeSkillTrigger<AssassinCharacter, IEntityIdSkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.Payload, 0, true, EntityInRange.GetRangeFromStats<AssassinCharacter>(StatTypes.EStatType.Rule_ThrowRange)));
			BBList<IEffect> effects = new BBList<IEffect>
			{
				new ActionSkillEffect<HirelingCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, OnHirelingSuccessful),
				new ExitCombatSkillEffect<AssassinCharacter>(this, ETargetSource.RuleOwner, ExitCombatMode.SmokeBomb),
				new TargetCollectorBuffSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, base.CollectTargetsAndSetLastKnownPos, BuffHelpers.MakeSmokeBombedBuffGenerator(StatValueCollection, RulesHelper.LinearHirelingLevelTurnCountProcessor1), null),
				new NotorietyChangeSkillEffect<AssassinCharacter>(this, 0.5f, ETargetSource.Player)
				{
					Categories = SkillCategories
				}
			};
			BBList<IEffect> effects2 = new BBList<IEffect>
			{
				new SpawnFxSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, SpawnPosGetter),
				new AddCrowdHotspotEffect(this, ETargetSource.Payload),
				new DelaySkillEffect<AssassinCharacter, ISkillPayload>(this, ETargetSource.RuleOwner, base.AllReadyForExitCondition, false, effects)
			};
			SkillEffectsWrapper hitEffectsWrapper = new SkillEffectsWrapper(effects2);
			BBList<IEffect> effects3 = new BBList<IEffect>
			{
				new SpawnProjectileSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, ProjectileData.EProjectileType.SmokeBomb, hitEffectsWrapper),
				new ExposingEffect(this)
			};
			AddEffect(new ActionSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, OnThrowBomb));
			AddEffect(new AnimatorListeningSkillEffectsWrapper<AssassinCharacter, EntityIdSkillPayload>(this, ETargetSource.RuleOwner, effects3, false, AnimatorEventType.ProjectileStart));
			AddEffect(new BuffEffect<AssassinCharacter>(this, new AdditiveNumberBuffGenerator_float(EBuffType.CriticalEye, StatValueCollection, StatTypes.EStatType.Rule_CriticalEyeDuration, StatTypes.EStatType.Invalid, StatTypes.EStatType.Rule_CriticalDamageBonus)
			{
				DurationProcessor = RulesHelper.LinearHirelingLevelTurnCountProcessor3,
				ValueProcessor = RulesHelper.LinearHirelingLevelValueProcessor4_float
			}, ETargetSource.Player));
		}

		private IEffectInstance OnThrowBomb(IEffect _effect, AssassinCharacter _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, IEntityIdSkillPayload _payload)
		{
			OnHaltCombat(_effect, _entity, _stats, _targetEntityId, _payload);
			_entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskSplinterBomb(_payload.Id.GameEntity);
			if (!_entity.GetIsInCombat())
			{
				_entity.AssassinCharacterStateMachines.PlayerMain.EnterCallbackDelayState(EStateType.Generic_WaitingForAnim, EStateType.CombatCharacterMain_Roaming, EStateType.CombatCharacterMain_Roaming, AnimationTaskType.SplinterBomb, AnimationTaskManager.ELayer.UpperBody);
			}
			return null;
		}

		private static IEffectInstance OnHirelingSuccessful(IEffect _effect, HirelingCharacter _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetentityid, IEntityIdSkillPayload _payload)
		{
			LogicMain.MissionResult.IncreaseSuccessfullHirelingAttack();
			return null;
		}

		protected override Vector3 SpawnPosGetter(ISkillsCarrier _entity, EntityId _targetEntityId, IBuffedStatsCollectionHandle _stats)
		{
			return _targetEntityId.GetEntity<IPositionEntity>().Position;
		}
	}
}
