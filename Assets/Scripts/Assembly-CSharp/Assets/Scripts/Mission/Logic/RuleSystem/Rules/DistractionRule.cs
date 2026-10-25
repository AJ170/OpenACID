using System.Collections.Generic;
using Assets.Scripts.Animation;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.RuleSystem.Triggers;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class DistractionRule : SkillRuleBase<PlayerCharacter>
	{
		private const ETriggerCategoryFlags CatDistraction = ETriggerCategoryFlags.Roaming | ETriggerCategoryFlags.BeingChased;

		private const ETriggerCategoryFlags CatSweetMusic = ETriggerCategoryFlags.HidingInObject | ETriggerCategoryFlags.HidingInPatrol;

		private readonly ISkillEffect m_sweetMusicEffect;

		public DistractionRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			AddTrigger(new TargetPredicateSkillTrigger<PlayerCharacter, ISkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.RuleOwner, IsAllowed));
			AddTrigger(new ClosestEnemyInRangeTrigger(this, SkillFrequency, SkillCategories, RangeGetter)
			{
				Predicate = (ITrigger trigger, IRulesCarrier rulesCarrier, IBuffedStatsCollectionHandle stats) => SweetMusicPredicate((PlayerCharacter)rulesCarrier)
			});
			BBList<IEffect> effects = new BBList<IEffect>
			{
				new GreedyAreaSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner)
			};
			AddEffect(new ActionSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, OnThrowCoins)
			{
				Categories = (ETriggerCategoryFlags.Roaming | ETriggerCategoryFlags.BeingChased),
				Predicate = AssassinIsNotStalking
			});
			AddEffect(new AnimatorListeningSkillEffectsWrapper<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, effects, false, AnimatorEventType.ReactionStart, AnimationTaskManager.ELayer.UpperBody, AnimationStateMachineHashes.StateMachine_CoinThrow)
			{
				Categories = (ETriggerCategoryFlags.Roaming | ETriggerCategoryFlags.BeingChased),
				Predicate = AssassinIsNotStalking
			});
			AddEffect(new ActionSkillEffect<PlayerCharacter, ISkillPayload>(this, ETargetSource.RuleOwner, OnSweetMusicInPatrol)
			{
				Categories = ETriggerCategoryFlags.HidingInPatrol
			});
			m_sweetMusicEffect = AddEffect(new BuffSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, BuffHelpers.MakeSweetMusicBuffGenerator(StatValueCollection))
			{
				IsAbortable = true,
				Predicate = (IEffect _effect, IRulesCarrier _rulesCarrier) => SweetMusicPredicate((PlayerCharacter)_rulesCarrier)
			});
			AddEffect(new SpawnFxSkillEffect<PlayerCharacter, ISkillPayload>(this, ETargetSource.RuleOwner, new Vector3(0f, 0.5f, 0f))
			{
				Predicate = (IEffect _effect, IRulesCarrier _rulesCarrier) => SweetMusicPredicate((PlayerCharacter)_rulesCarrier)
			});
		}

		private IEffectInstance OnSweetMusicInPatrol(IEffect _effect, PlayerCharacter _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, ISkillPayload _payload)
		{
			PlayerMain playerMain = _entity.AssassinCharacterStateMachines.PlayerMain;
			PlayerMain.HidingInPatrolState.HidingInPatrolPayload payload = playerMain.GetPayload<PlayerMain.HidingInPatrolState.HidingInPatrolPayload>(EStateType.PlayerMain_HidingInPatrol);
			Assets.Scripts.Mission.Logic.Entities.Patrol entity = payload.PatrolId.GetEntity<Assets.Scripts.Mission.Logic.Entities.Patrol>();
			int timeout = _stats.Get_int(StatTypes.EStatType.Rule_SweetMusicDuration) + 10;
			entity.PatrolComponent.StartSidetrack(entity.Position, false, false, timeout);
			return null;
		}

		private bool AssassinIsStalking(IEffect _effect, IRulesCarrier _entity)
		{
			bool result = false;
			AssassinCharacter assassinCharacter = _entity as AssassinCharacter;
			if (assassinCharacter != null)
			{
				result = assassinCharacter.GetIsStalking();
			}
			return result;
		}

		private bool AssassinIsNotStalking(IEffect _effect, IRulesCarrier _entity)
		{
			return !AssassinIsStalking(_effect, _entity);
		}

		private bool IsAllowed(PlayerCharacter _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId)
		{
			EHidingSpotFlags value = (EHidingSpotFlags)_stats.Get_int(StatTypes.EStatType.Rule_SweetMusicHidingSpots);
			AssassinCharacter entity = _targetEntityId.GetEntity<AssassinCharacter>();
			PlayerStalkingZone playerStalkingZone = entity.AssassinCharacterStateMachines.PlayerStalkingZone;
			PlayerMain playerMain = entity.AssassinCharacterStateMachines.PlayerMain;
			EStateType currentStateType = playerMain.CurrentStateType;
			EStateType currentStateType2 = playerStalkingZone.CurrentStateType;
			switch (currentStateType)
			{
			case EStateType.PlayerMain_HidingInObject:
			{
				PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload payload = playerMain.GetPayload<PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload>(EStateType.PlayerMain_HidingInObject);
				switch (payload.EntityId.Type)
				{
				case EntityTypes.EType.Haystack:
					return IsSet(value, EHidingSpotFlags.Haystack);
				case EntityTypes.EType.Bench:
					return IsSet(value, EHidingSpotFlags.Bench);
				case EntityTypes.EType.RooftopGarden:
					return IsSet(value, EHidingSpotFlags.RooftopGarden);
				default:
					Assets.Scripts.Engine.Helpers.Verify(false, "Unknown hiding spot!");
					return false;
				}
			}
			case EStateType.PlayerMain_HidingInPatrol:
				return IsSet(value, EHidingSpotFlags.Crowd);
			default:
				if (currentStateType2 == EStateType.PlayerStalkingZone_Stalking)
				{
					return IsSet(value, EHidingSpotFlags.StalkingZone);
				}
				if (currentStateType == EStateType.CombatCharacterMain_Roaming || currentStateType == EStateType.PlayerMain_BeingChased)
				{
					return true;
				}
				return false;
			}
		}

		private bool SweetMusicPredicate(PlayerCharacter _entity)
		{
			PlayerStalkingZone playerStalkingZone = _entity.AssassinCharacterStateMachines.PlayerStalkingZone;
			PlayerMain playerMain = _entity.AssassinCharacterStateMachines.PlayerMain;
			EStateType currentStateType = playerMain.CurrentStateType;
			EStateType currentStateType2 = playerStalkingZone.CurrentStateType;
			switch (currentStateType)
			{
			case EStateType.PlayerMain_HidingInObject:
			{
				PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload payload = playerMain.GetPayload<PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload>(EStateType.PlayerMain_HidingInObject);
				switch (payload.EntityId.Type)
				{
				case EntityTypes.EType.Haystack:
				case EntityTypes.EType.Bench:
				case EntityTypes.EType.RooftopGarden:
					return true;
				default:
					Assets.Scripts.Engine.Helpers.Verify(false, "Unknown hiding spot!");
					return false;
				}
			}
			case EStateType.PlayerMain_HidingInPatrol:
				return true;
			default:
				if (currentStateType2 == EStateType.PlayerStalkingZone_Stalking)
				{
					return true;
				}
				return false;
			}
		}

		private static bool IsSet(EHidingSpotFlags _value, EHidingSpotFlags _checkFor)
		{
			return (_value & _checkFor) == _checkFor;
		}

		private float RangeGetter(PlayerCharacter _entity, IBuffedStatsCollectionHandle _stats)
		{
			return _stats.Get_float(StatTypes.EStatType.Rule_AttractionRange);
		}

		private static IEffectInstance OnThrowCoins(IEffect _effect, PlayerCharacter _playerCharacter, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, IEntityIdSkillPayload _entityIdSkillPayload)
		{
			_playerCharacter.PlayerEventHandler.OverrideResetPathOnCancel();
			Vector3 vector = GreedyAreaSkillEffect<PlayerCharacter, IEntityIdSkillPayload>.ComputeZoneDirection();
			SimplePositionProvider targetProvider = new SimplePositionProvider(_playerCharacter.Position + vector);
			_playerCharacter.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskThrowCoins(targetProvider);
			if (!_playerCharacter.GetIsInCombat())
			{
				_playerCharacter.AssassinCharacterStateMachines.PlayerMain.EnterCallbackDelayState(EStateType.Generic_WaitingForAnim, EStateType.CombatCharacterMain_Roaming, EStateType.CombatCharacterMain_Roaming, AnimationTaskType.ThrowCoins, AnimationTaskManager.ELayer.UpperBody);
			}
			return null;
		}

		protected override void PrepareEffect(PlayerCharacter _entity, IEffect _effect)
		{
			if (_effect == m_sweetMusicEffect)
			{
				ISkillEffect skillEffect = _effect.Cast<ISkillEffect>();
				using (IBuffedStatsCollectionHandle buffedStatsCollectionHandle = StatValueCollection.Apply(_entity))
				{
					EnemyCharacter closestEnemyThreateningOrNormal = _entity.GetClosestEnemyThreateningOrNormal(buffedStatsCollectionHandle.Get_float(StatTypes.EStatType.Rule_AttractionRange));
					skillEffect.Payload = new EntityIdSkillPayload((closestEnemyThreateningOrNormal == null) ? EntityId.InvalidId : closestEnemyThreateningOrNormal.Id);
				}
				skillEffect.Prepare(_entity);
			}
			else
			{
				base.PrepareEffect(_entity, _effect);
			}
		}
	}
}
