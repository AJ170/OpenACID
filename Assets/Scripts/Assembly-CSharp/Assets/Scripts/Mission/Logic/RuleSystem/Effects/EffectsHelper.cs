using System;
using System.Collections.Generic;
using Assets.Scripts.Animation;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Audio;
using Assets.Scripts.Crowd;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Database;
using SharedBaseLib.Core;
using UbiCore.Messenger;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public static class EffectsHelper
	{
		private class Target
		{
			public EnemyCharacter Enemy { get; set; }

			public float Dot { get; set; }
		}

		private static readonly BBList<Target> Targets = new BBList<Target>(10);

		public static void CharacterKnockdown(IPositionProvider _source, Character _target, int _knockdownTurns = 0)
		{
			if (_knockdownTurns > 0)
			{
				IBuff buff = new AdditiveNumberBuff(EBuffType.Knockdowned, _knockdownTurns, 1f);
				_target.Buffs.AddBuff(buff);
			}
			if (!_target.IsAttached || _target.UnityComponentCache.AnimationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) != AnimationTaskType.Knockdowned)
			{
				AnimationTaskKnockdowned.Parameters parameters = new AnimationTaskKnockdowned.Parameters();
				parameters.Source = _source;
				parameters.Target = _target;
				AnimationTaskKnockdowned.Parameters externalParameters = parameters;
				CharacterCondition characterCondition = _target.StateMachines.Cast<ICharacterConditionCarrier>().CharacterCondition;
				characterCondition.ExternalChange(EStateType.CharacterCondition_Knockdowned, externalParameters);
			}
		}

		public static void CharacterSanded(Character _target, int _sandTurns = 0)
		{
			if (_sandTurns > 0)
			{
				IBuff buff = new AdditiveNumberBuff(EBuffType.Sanded, _sandTurns, 1f);
				_target.Buffs.AddBuff(buff);
			}
			CharacterCondition characterCondition = _target.StateMachines.Cast<ICharacterConditionCarrier>().CharacterCondition;
			characterCondition.ExternalChange(EStateType.CharacterCondition_Sanded);
		}

		public static void CharacterHit(IPositionProvider _source, Character _target)
		{
			if (_target.IsAttached)
			{
				AnimationTaskManager taskManager = _target.UnityComponentCache.AnimationController.TaskManager;
				taskManager.CreateAnimationTaskGetHit(_source, AttackMeleeType.Invalid);
			}
		}

		public static void CharacterBombHit(IPositionProvider _source, Character _target, int _knockdownTurns = 0)
		{
			bool hasZeroHealth = _target.GetHasZeroHealth();
			if (_target.GetIsInParcour() || (_target.IsAttached && _target.UnityComponentCache.AnimationController.TaskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base).IsParkourTask()))
			{
				ParcourCancelEvent.ECancelType eCancelCancelType = (hasZeroHealth ? ParcourCancelEvent.ECancelType.Die : ParcourCancelEvent.ECancelType.Hit);
				Messenger.Instance.Broadcast(new ParcourCancelEvent(_target.Id, eCancelCancelType));
				if (hasZeroHealth)
				{
					AnimationTaskManager taskManager = _target.UnityComponentCache.AnimationController.TaskManager;
					taskManager.CreateAnimationTaskKnockdowned(null, _target, false, false);
				}
			}
			else if (hasZeroHealth || _knockdownTurns > 0)
			{
				CharacterKnockdown(_source, _target, Mathf.Max(_knockdownTurns, 10));
			}
			else
			{
				CharacterHit(_source, _target);
			}
		}

		public static bool CanAssassinBeHitByBomb(IPositionProvider _bomb, float _explosionRadiusSqr, AssassinCharacter _target)
		{
			if (_target.Position.DistanceSquared(_bomb.Position) >= _explosionRadiusSqr)
			{
				return false;
			}
			if (_target.GetDeadOrAboutToDie())
			{
				return false;
			}
			if (_target.GetIsInParcour())
			{
				return false;
			}
			if (_target.GetIsImmortal())
			{
				return false;
			}
			if (_target.GetIsInObjectInteraction())
			{
				return false;
			}
			return !_target.GetIsHidingInObject() && !_target.GetIsStalking();
		}

		public static void CharacterBombHitSpecialNpc(IPositionProvider _source, float _explosionRadiusSqr, Character _target)
		{
			CrowdCharacter crowdCharacter = _target as CrowdCharacter;
			if (crowdCharacter != null)
			{
				crowdCharacter.NpcStats.Set_bool(StatTypes.EStatType.Npc_DidSeeBombExplosion, true);
				crowdCharacter.NpcStats.Set_Vector3(StatTypes.EStatType.Npc_FleeSourcePosition, _source.Position);
			}
			if (_target.Position.DistanceSquared(_source.Position) >= _explosionRadiusSqr || !_target.IsAttached)
			{
				return;
			}
			if (_target is InteractionCharacter)
			{
				InteractionCharacterMain interactionCharacterMain = _target.Cast<InteractionCharacter>().InteractionCharacterStateMachines.InteractionCharacterMain;
				if (interactionCharacterMain.ScheduledStateType == EStateType.Invalid && interactionCharacterMain.CurrentStateType == EStateType.CharacterInteractionMain_Idle)
				{
					AnimationTaskManager taskManager = _target.UnityComponentCache.AnimationController.TaskManager;
					taskManager.CreateAnimationTaskKnockdowned(_source, _target, false);
				}
			}
			else if (_target is CrowdCharacter)
			{
				CharacterKnockdown(_source, _target, 2);
			}
		}

		public static void CharacterAlarmed(IPositionProvider _source, Character _target)
		{
			Vector3 position = _source.Position;
			CharacterAlarmed(position, _target);
		}

		public static void CharacterAlarmed(Vector3 _sourcePos, Character _target)
		{
			EnemyCharacter enemyCharacter = _target as EnemyCharacter;
			CharacterAwareness characterAwareness = enemyCharacter.EnemyCharacterStateMachines.CharacterAwareness;
			CharacterAwareness.AlarmedState.AlarmedPayload payload = characterAwareness.GetPayload<CharacterAwareness.AlarmedState.AlarmedPayload>(EStateType.CharacterAwareness_Alarmed);
			payload.UpdateLastKnownPosition(enemyCharacter, _sourcePos);
			if (AwarenessPredicateInternal(enemyCharacter, EStateType.CharacterAwareness_Alarmed))
			{
				characterAwareness.ScheduleStateChangeForced(EStateType.CharacterAwareness_Alarmed);
			}
		}

		public static void CharacterConfused(Character _target, int _confusedDuration)
		{
			_target.Buffs.AddBuff(new AdditiveNumberBuff(EBuffType.Confused, _confusedDuration, 1f));
		}

		public static void CharacterBlackjacked(Character _target, int _stunDuration)
		{
			_target.Buffs.AddBuff(new AdditiveNumberBuff(EBuffType.Stunned, _stunDuration, 1f));
			_target.CharacterStateMachines.Cast<ICharacterConditionCarrier>().CharacterCondition.ExternalChange(EStateType.CharacterCondition_Blackjacked);
		}

		public static void CharacterBleeding(Character _target, int _duration, int _interval, float _amount)
		{
			if (_duration > 0 && _amount > 0f)
			{
				_target.Buffs.AddBuff(new AdditiveNumberBuff(EBuffType.Bleeding, EBuffDurationType.Finite, _duration, _interval, _amount));
			}
		}

		public static void CharacterBleedingRelative(Character _target, int _duration, int _interval, float _amountRelative)
		{
			if (_duration > 0 && _amountRelative > 0f)
			{
				_target.Buffs.AddBuff(new AdditiveNumberBuff(EBuffType.BleedingRelative, EBuffDurationType.Finite, _duration, _interval, _amountRelative));
			}
		}

		public static bool TargetNotAlarmedPredicate(IEffect _effect, IRulesCarrier _entity)
		{
			ITargetEffect targetEffect = _effect as ITargetEffect;
			if (targetEffect == null)
			{
				return false;
			}
			EnemyCharacter enemyCharacter = targetEffect.TargetEntityId.TryGetEntity<EnemyCharacter>();
			if (enemyCharacter == null || enemyCharacter.GetIsDead())
			{
				return false;
			}
			return AwarenessPredicateInternal(enemyCharacter, EStateType.CharacterAwareness_Alarmed);
		}

		public static bool TargetNotInCombatPredicate(IEffect _effect, IRulesCarrier _entity)
		{
			ITargetEffect targetEffect = _effect as ITargetEffect;
			if (targetEffect == null)
			{
				return true;
			}
			IParticipantCarrier participantCarrier = targetEffect.TargetEntityId.TryGetEntity<IParticipantCarrier>();
			return participantCarrier == null || !participantCarrier.CombatParticipantProxy.IsInCombat;
		}

		public static void CharacterSilence(Character _target, int _silenceDuration)
		{
			if (_silenceDuration > 0)
			{
				_target.Buffs.AddBuff(new BooleanOrBuff(EBuffType.Silenced, _silenceDuration, true));
			}
		}

		public static void CharacterBlinded(Character _target, int _blindDuration, int _blindStartDelay = 0)
		{
			if (_blindDuration > 0)
			{
				_target.Buffs.AddBuff(new MinimumNumberBuff(EBuffType.Blinded, EBuffDurationType.Finite, _blindDuration, 0, 0f, _blindStartDelay));
				_target.Buffs.AddBuff(new MaximumNumberBuff(StatTypes.EStatType.Character_Conflict_DodgeChance, EBuffDurationType.Finite, _blindDuration, 0, 1f, _blindStartDelay));
			}
		}

		public static BBList<EnemyCharacter> CollectBestTargetCharacters(AssassinCharacter _assassin, EntityId _targetId, IBuffedStatsCollectionHandle _stats)
		{
			int num = _stats.Get_int(StatTypes.EStatType.Rule_ThrowTargets);
			BBList<EnemyCharacter> bBList = new BBList<EnemyCharacter>(num);
			EnemyCharacter entity = _targetId.GetEntity<EnemyCharacter>();
			bBList.Add(entity);
			num--;
			if (num <= 0)
			{
				return bBList;
			}
			Targets.ClearIfNotEmpty();
			float radius = _stats.Get_float(StatTypes.EStatType.Rule_ThrowRange);
			float num2 = _stats.Get_float(StatTypes.EStatType.Rule_ThrowAngle);
			float num3 = Mathf.Cos(num2 * ((float)Math.PI / 180f) * 0.5f);
			Vector3 position = _assassin.Position;
			Vector3 position2 = entity.Position;
			Vector3 lhs = position2 - position;
			lhs.y = 0f;
			float magnitude = lhs.magnitude;
			if (magnitude > 0.0001f)
			{
				lhs /= magnitude;
			}
			else
			{
				lhs = _assassin.UnityComponentCache.Transform.forward;
			}
			IEnumerable<EnemyCharacter> enumerable = LogicMain.EntityManager.CollectEntitiesInRadius<EnemyCharacter>(position, radius);
			foreach (EnemyCharacter item in enumerable)
			{
				if (item == entity || item.GetDeadOrAboutToDie() || item.GetIsInParcour() || item.GetIsDisguised())
				{
					continue;
				}
				Vector3 rhs = item.Position - position;
				rhs.y = 0f;
				magnitude = rhs.magnitude;
				if (!(magnitude <= 0.0001f))
				{
					rhs /= magnitude;
					float num4 = Vector3.Dot(lhs, rhs);
					if (num4 >= num3)
					{
						Targets.Add(new Target
						{
							Enemy = item,
							Dot = num4
						});
					}
				}
			}
			while (num > 0)
			{
				float num5 = -2f;
				int num6 = -1;
				for (int i = 0; i < Targets.Count; i++)
				{
					float dot = Targets[i].Dot;
					if (!(dot <= num5))
					{
						num5 = dot;
						num6 = i;
					}
				}
				if (num6 < 0)
				{
					break;
				}
				num--;
				bBList.Add(Targets[num6].Enemy);
				Targets[num6] = new Target
				{
					Dot = float.MinValue
				};
			}
			return bBList;
		}

		public static void LetCrowdFleeFromBomb(Vector3 _bombPosition, float _fleeRadius)
		{
			Assets.Scripts.Crowd.Crowd.Instance.AddHotspot(new CrowdHotspotData
			{
				Position = _bombPosition,
				DynamicWeight = 0f,
				EndTime = LogicMain.CurrentTime + 10f,
				Radius = _fleeRadius,
				Flee = true
			});
		}

		private static bool AwarenessPredicateInternal(EnemyCharacter _target, EStateType _characterAwarenessState)
		{
			EStateType currentStateType = _target.EnemyCharacterStateMachines.CharacterAwareness.CurrentStateType;
			return currentStateType < _characterAwarenessState;
		}

		public static void AudioOnKnifeHit(EntityId _targetEntityId)
		{
			Character character = _targetEntityId.TryGetEntity<Character>();
			if (character != null)
			{
				HumanoidDef item = Main.SafeGameDb.GetItem<HumanoidDef>(character.CharacterDefinition.Definition.HumanoidGameID);
				HumanoidDef.EArmorType armorType = ((item != null) ? item.ArmorType : HumanoidDef.EArmorType.LightLeather);
				IAudioHandle handle = CombatController.Instance.AudioController.GetHandle(EAudioWeaponType.Stab, armorType);
				EntityWrapperComponent wrapper = _targetEntityId.GameEntity.Wrapper;
				if (handle != null)
				{
					handle.Load();
					handle.Play(false, (!(wrapper != null)) ? null : wrapper.GetComponent<AudioSource>());
				}
				IAudioHandle audioHandle = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_vo_enemy_takehit, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
				audioHandle.Load();
				audioHandle.Play(false, (!(wrapper != null)) ? null : wrapper.GetComponent<AudioSource>());
			}
		}
	}
}
