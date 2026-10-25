using System.Collections.ObjectModel;
using AcierProtocol;
using Assets.Scripts.Animation;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using SharedBaseLib.System;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.Logic
{
	public static class CharacterHelper
	{
		public enum ECharacterNameType
		{
			Plain = 0,
			PortraitWithLevel = 1
		}

		public static string GetLocalizedName(this Character _character, ECharacterNameType _characterNameType)
		{
			return _character.EntityStats.GetLocalizedName(_characterNameType);
		}

		public static string GetLocalizedName(this IInstanceStatValueCollection _characterStats, ECharacterNameType _characterNameType)
		{
			string text = string.Empty;
			int localizedNameLocaId = _characterStats.GetLocalizedNameLocaId(_characterNameType);
			if (localizedNameLocaId != 0)
			{
				text = LocalizationManager.Instance.Get(localizedNameLocaId, new StringMapper { 
				{
					"enemy_lvl",
					UbiCore.Utils.Singleton<MissionManager>.Instance.MissionTargetLevel
				} });
			}
			if (string.IsNullOrEmpty(text) && _characterStats.HasStat(StatTypes.EStatType.Entity_DisplayNameFallback))
			{
				text = _characterStats.Get_String(StatTypes.EStatType.Entity_DisplayNameFallback);
			}
			return text;
		}

		public static int GetLocalizedLocaId(string _guid, ECharacterNameType _characterNameType)
		{
			GameID gameID = Main.Instance.GameDB.FindGameIdFromGuid(_guid);
			NpcData.NpcType npcType = new NpcData.NpcType(gameID);
			if (!npcType.IsValid)
			{
				return 0;
			}
			NpcData.INpcDefinition definition = npcType.Definition;
			if (definition == null)
			{
				return 0;
			}
			return definition.GetLocalizedNameLocaId(_characterNameType);
		}

		public static int GetLocalizedObjectLocaId(string _guid, ECharacterNameType _characterNameType)
		{
			StaticObjectData.ObjectType objectType = new StaticObjectData.ObjectType(Main.Instance.GameDB.FindGameIdFromGuid(_guid));
			StaticObjectData.IStaticObjectDefinition definition = objectType.Definition;
			return definition.GetLocalizedNameLocaId(_characterNameType);
		}

		public static int GetLocalizedNameLocaId(this IReadonlyStatValueCollection _characterStats, ECharacterNameType _characterNameType)
		{
			int num = 0;
			StatTypes.EStatType eStatType = StatTypes.EStatType.Character_PortraitDisplayNameLocaKey;
			switch (_characterNameType)
			{
			case ECharacterNameType.Plain:
				eStatType = StatTypes.EStatType.Entity_DisplayNameLocaKey;
				break;
			case ECharacterNameType.PortraitWithLevel:
				eStatType = StatTypes.EStatType.Character_PortraitDisplayNameLocaKey;
				break;
			}
			if (_characterStats.HasStat(eStatType))
			{
				num = _characterStats.Get_int(eStatType);
			}
			if (num == 0)
			{
				eStatType = StatTypes.EStatType.Character_DisplayNameLocaKeys;
				switch (_characterNameType)
				{
				case ECharacterNameType.Plain:
					eStatType = StatTypes.EStatType.Character_DisplayNameLocaKeys;
					break;
				case ECharacterNameType.PortraitWithLevel:
					eStatType = StatTypes.EStatType.Character_PortraitDisplayNameLocaKeys;
					break;
				}
				if (_characterStats.HasStat(eStatType))
				{
					IReadonlyStatCollectionValue readonlyStatCollectionValue = _characterStats.GetReadonlyStatCollectionValue(eStatType);
					if (readonlyStatCollectionValue.Count == 0 && eStatType == StatTypes.EStatType.Character_PortraitDisplayNameLocaKeys)
					{
						readonlyStatCollectionValue = _characterStats.GetReadonlyStatCollectionValue(StatTypes.EStatType.Character_DisplayNameLocaKeys);
					}
					if (readonlyStatCollectionValue.Count > 0)
					{
						return readonlyStatCollectionValue.Get_int(Random.Range(0, readonlyStatCollectionValue.Count - 1));
					}
				}
			}
			return num;
		}

		public static float ComputeCurrentSpeed(this Character _character)
		{
			if (_character.IsAttached)
			{
				NavMeshAgent navMeshAgent = _character.UnityComponentCache.NavMeshAgent;
				return navMeshAgent.velocity.magnitude;
			}
			NpcCharacter npcCharacter = _character as NpcCharacter;
			if (npcCharacter != null && npcCharacter.Patrol != null)
			{
				return npcCharacter.Patrol.PatrolComponent.Approacher.Speed;
			}
			return 0f;
		}

		public static bool CancelMovement(this Character _character, bool _startIdle, bool _removeFromPatrol = true)
		{
			if (_removeFromPatrol)
			{
				NpcCharacter npcCharacter = _character as NpcCharacter;
				if (npcCharacter != null && npcCharacter.Patrol != null)
				{
					npcCharacter.Patrol.Remove(_character.Id, false);
				}
			}
			if (!_character.IsAttached)
			{
				return true;
			}
			AnimationTaskType currentTaskType = _character.UnityComponentCache.AnimationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base);
			if (currentTaskType != AnimationTaskType.Unknown && !currentTaskType.IsMovementTask())
			{
				return false;
			}
			if (_character.UnityComponentCache.NavMeshAgent.enabled)
			{
				_character.UnityComponentCache.NavMeshAgent.Stop();
			}
			_character.UnityComponentCache.AnimationController.TaskManager.CancelCurrentTask(AnimationTaskManager.ELayer.Base, _startIdle);
			return true;
		}

		public static void ChooseWeaponState(this Character _entity, EWeaponCategory _weaponCategory, EWeaponState _weaponState)
		{
			CharacterDrawWeaponBase[] characterDrawWeapon = _entity.StateMachines.Cast<ICharacterDrawWeaponCarrier>().CharacterDrawWeapon;
			int num = characterDrawWeapon.Length;
			for (int i = 0; i < num; i++)
			{
				CharacterDrawWeaponBase characterDrawWeaponBase = characterDrawWeapon[i];
				if (characterDrawWeaponBase != null)
				{
					if (characterDrawWeaponBase.WeaponCatgory == _weaponCategory)
					{
						characterDrawWeaponBase.ChosenWeaponState = _weaponState;
					}
					else if (_weaponState == EWeaponState.Unsheathed && characterDrawWeaponBase.CurrentStateType != EStateType.CharacterDrawWeapon_Sheathed)
					{
						characterDrawWeaponBase.ChosenWeaponState = EWeaponState.Detached;
					}
				}
			}
		}

		public static void DropUnsheathedWeapons(this Character _entity)
		{
			CharacterDrawWeaponBase[] characterDrawWeapon = _entity.StateMachines.Cast<ICharacterDrawWeaponCarrier>().CharacterDrawWeapon;
			int num = characterDrawWeapon.Length;
			for (int i = 0; i < num; i++)
			{
				CharacterDrawWeaponBase characterDrawWeaponBase = characterDrawWeapon[i];
				if (characterDrawWeaponBase != null && characterDrawWeaponBase.IsCurrentOrScheduled(EStateType.CharacterDrawWeapon_Unsheathed))
				{
					characterDrawWeaponBase.ChosenWeaponState = EWeaponState.Detached;
				}
			}
		}

		public static void InstantSheathWeapons(this Character _entity)
		{
			CharacterDrawWeaponBase[] characterDrawWeapon = _entity.StateMachines.Cast<ICharacterDrawWeaponCarrier>().CharacterDrawWeapon;
			int num = characterDrawWeapon.Length;
			for (int i = 0; i < num; i++)
			{
				CharacterDrawWeaponBase characterDrawWeaponBase = characterDrawWeapon[i];
				if (characterDrawWeaponBase != null && characterDrawWeaponBase.IsCurrentOrScheduled(EStateType.CharacterDrawWeapon_Unsheathed))
				{
					characterDrawWeaponBase.ChosenWeaponState = EWeaponState.Sheathed;
				}
			}
		}

		public static void ExitCombat(this Character _entity, bool _dead)
		{
			if (_entity.IsAttached)
			{
				ReadOnlyCollection<AnimationTaskBase> currentTasks = _entity.UnityComponentCache.AnimationController.TaskManager.GetCurrentTasks(AnimationTaskManager.ELayer.Base);
				if (currentTasks.Count > 0 && currentTasks[0].TaskType == AnimationTaskType.Combat)
				{
					AnimationTaskCombat animationTaskCombat = (AnimationTaskCombat)currentTasks[0];
					animationTaskCombat.OnExitCombat(_dead);
				}
			}
		}

		public static bool RemoveBuffsOnEnterCombat(this IParticipantCarrier _participantCarrier)
		{
			return _participantCarrier.Cast<IBuffsCarrier>().Buffs.RemoveBuffs(BuffHelpers.CancelBuffOnEnterCombat, int.MaxValue);
		}

		public static float RefillHealth(this Character _entity)
		{
			if (_entity.EntityStats is ICharacterConflictStatsCarrier)
			{
				return CombatHelper.ApplyGenericHeal(null, _entity, EApplyDamageFlags.None, float.MaxValue);
			}
			return 0f;
		}

		public static void Despawn(this Character _entity)
		{
			if (!_entity.IsValid)
			{
				return;
			}
			if (_entity.IsAttached)
			{
				if (!_entity.AutoUnregisterOnDetach)
				{
					ObjectFade objectFade = _entity.GameObject.AddComponentIfNotExist<ObjectFade>();
					objectFade.FadeMode = EFadeMode.Fancy_EFadeMode;
					objectFade.FadeDuration = 1.5f;
					objectFade.OnFadeDoneCallback = ObjectFadeCallbacks.OnFadeFinishedDeactivateGameObject;
					objectFade.FadeOut();
					_entity.AutoUnregisterOnDetach = true;
				}
			}
			else
			{
				LogicMain.EntityManager.UnregisterEntity(_entity);
			}
		}

		public static void AddKillTypeFlag(this ICharacterVitalityCarrierEntity _entity, EKillType _killType)
		{
			if (_entity.EntityStats.HasStat(StatTypes.EStatType.Character_Conflict_KillTypeFlags))
			{
				EKillType eKillType = (EKillType)_entity.EntityStats.Get_int(StatTypes.EStatType.Character_Conflict_KillTypeFlags);
				_entity.EntityStats.Set_int(StatTypes.EStatType.Character_Conflict_KillTypeFlags, (int)(_killType | eKillType));
			}
		}

		public static void KillCharacter(this ICharacterVitalityCarrierEntity _entity, EKillType _killType)
		{
			_entity.EntityStats.Set_float(StatTypes.EStatType.Character_Conflict_Health, 0f);
			_entity.HeadIcon.IsVisible = false;
			_entity.AddKillTypeFlag(_killType);
			CharacterDrawWeaponBase[] characterDrawWeapon = _entity.StateMachines.Cast<ICharacterDrawWeaponCarrier>().CharacterDrawWeapon;
			int num = characterDrawWeapon.Length;
			for (int i = 0; i < num; i++)
			{
				CharacterDrawWeaponBase characterDrawWeaponBase = characterDrawWeapon[i];
				if (characterDrawWeaponBase != null && characterDrawWeaponBase.CurrentStateType != EStateType.CharacterDrawWeapon_Sheathed)
				{
					characterDrawWeaponBase.ChosenWeaponState = EWeaponState.Detached;
				}
			}
			Character character = _entity as Character;
			if (character != null && character.IsAttached)
			{
				character.UnityComponentCache.NavMeshAgent.enabled = false;
			}
		}

		public static void SetKnowinglyInCombat(this Character _entity)
		{
			IParticipantCarrier participantCarrier = _entity as IParticipantCarrier;
			if (participantCarrier != null)
			{
				CombatParticipant combatParticipant = participantCarrier.CombatParticipantProxy.CombatParticipant;
				if (combatParticipant != null)
				{
					combatParticipant.KnowinglyInCombat = true;
				}
			}
		}

		public static bool GetKnowinglyInCombat(this Character _entity)
		{
			IParticipantCarrier participantCarrier = _entity as IParticipantCarrier;
			if (participantCarrier == null)
			{
				return false;
			}
			CombatParticipant combatParticipant = participantCarrier.CombatParticipantProxy.CombatParticipant;
			return combatParticipant != null && combatParticipant.KnowinglyInCombat;
		}

		public static bool SampleNormal(this Character _entity, out Vector3 _normal)
		{
			Vector3 position = _entity.Position;
			NavMeshAgent navMeshAgent = _entity.UnityComponentCache.NavMeshAgent;
			_normal = Vector3.up;
			NavMeshHit _hit;
			if (NavigationHelper.CascadingSamplePosition(position, out _hit, navMeshAgent.areaMask))
			{
				float num = 0.15f;
				Vector3 vector = _entity.UnityComponentCache.Transform.forward * num;
				NavMeshHit _hit2;
				if (NavigationHelper.CascadingSamplePosition(position + vector, out _hit2, navMeshAgent.areaMask))
				{
					vector = _hit2.position - _hit.position;
					float magnitude = vector.magnitude;
					if (magnitude < 0.01f)
					{
						return false;
					}
					if (magnitude > num * 2f)
					{
						return false;
					}
					Vector3 lhs = Vector3.Cross(vector, Vector3.up);
					_normal = Vector3.Cross(lhs, vector);
					_normal.Normalize();
					return true;
				}
			}
			return false;
		}

		public static IdleType GetIdleTypeOnTransition(this Character _character)
		{
			ICharacterInteractionCarrier characterInteractionCarrier = _character.StateMachines as ICharacterInteractionCarrier;
			if (characterInteractionCarrier != null)
			{
				return characterInteractionCarrier.CharacterInteraction.IdleTypeOnTransition;
			}
			return _character.CharacterDefinition.Definition.TransitionIdleType;
		}

		public static bool GetHasZeroHealth(this Character _entity)
		{
			CharacterStats characterStats = _entity.CharacterStats;
			return characterStats.Get_float(StatTypes.EStatType.Character_Conflict_Health) <= 0f;
		}

		public static bool GetDeadOrAboutToDie(this Character _entity)
		{
			if (_entity is IParticipantCarrier)
			{
				return _entity.GetHasZeroHealth() || _entity.CharacterStats.Get_bool(StatTypes.EStatType.Character_Conflict_AboutToDie);
			}
			return _entity.GetIsDead();
		}

		public static bool GetIsCaptured(this Character _entity)
		{
			bool result = false;
			if (_entity is NpcCharacter)
			{
				NpcCharacter npcCharacter = _entity.Cast<NpcCharacter>();
				result = npcCharacter.Patrol != null && npcCharacter.Patrol.PatrolComponent.MissionData.Data.Type == EPatrolType.CapturedNpcs && !npcCharacter.Patrol.PatrolStats.Get_bool(StatTypes.EStatType.Interaction_Triggered);
			}
			return result;
		}

		public static bool GetIsDead(this Character _entity)
		{
			ICharacterVitalityCarrier characterVitalityCarrier = _entity.CharacterStateMachines as ICharacterVitalityCarrier;
			if (characterVitalityCarrier == null)
			{
				return false;
			}
			return IsStateDead(characterVitalityCarrier);
		}

		public static bool GetIsDisguised(this Character _entity)
		{
			AssassinCharacter assassinCharacter = _entity as AssassinCharacter;
			if (assassinCharacter != null)
			{
				return assassinCharacter.GetIsInDisguise();
			}
			EnemyCharacter enemyCharacter = _entity as EnemyCharacter;
			if (enemyCharacter != null)
			{
				return enemyCharacter.GetIsInDisguise();
			}
			return false;
		}

		private static bool IsStateDead(ICharacterVitalityCarrier s)
		{
			return s.CharacterVitality.CurrentStateType == EStateType.Character_Dead;
		}

		public static bool GetIsImmortal(this Character _entity)
		{
			return _entity.CharacterStateMachines.DoIfIs<ICharacterVitalityCarrier, bool>(IsStateImmortal);
		}

		private static bool IsStateImmortal(ICharacterVitalityCarrier s)
		{
			return s.CharacterVitality.CurrentStateType == EStateType.Character_Immortal;
		}

		public static EWeaponState GetMeleeWeaponState(this Character _entity)
		{
			return _entity.StateMachines.DoIfIs<ICharacterDrawMeleeWeaponCarrier, EWeaponState>(OnGetMeleeWeaponState, EWeaponState.Invalid);
		}

		private static EWeaponState OnGetMeleeWeaponState(ICharacterDrawMeleeWeaponCarrier s)
		{
			return s.CharacterDrawMeleeWeapon.CurrentStateType.ToWeaponState();
		}

		public static bool GetIsMainStateDelayed(this Character _entity, EStateType _delayedState)
		{
			IStateMachine mainStateMachine = _entity.GetMainStateMachine();
			object obj;
			if (mainStateMachine != null)
			{
				ICallbackPayload callbackPayload = mainStateMachine.CurrentStatePayload as ICallbackPayload;
				obj = callbackPayload;
			}
			else
			{
				obj = null;
			}
			ICallbackPayload callbackPayload2 = (ICallbackPayload)obj;
			return callbackPayload2 != null && callbackPayload2.NextState == _delayedState;
		}

		public static EStateMachineType GetMainStateMachineType(this Character _entity)
		{
			AssassinCharacter assassinCharacter = _entity as AssassinCharacter;
			if (assassinCharacter != null)
			{
				return EStateMachineType.PlayerMain;
			}
			EnemyCharacter enemyCharacter = _entity as EnemyCharacter;
			if (enemyCharacter != null)
			{
				return EStateMachineType.EnemyMain;
			}
			InteractionCharacter interactionCharacter = _entity as InteractionCharacter;
			if (interactionCharacter != null)
			{
				return EStateMachineType.InteractionCharacterMain;
			}
			return EStateMachineType.Invalid;
		}

		public static IStateMachine GetMainStateMachine(this Character _entity)
		{
			EStateMachineType mainStateMachineType = _entity.GetMainStateMachineType();
			if (mainStateMachineType == EStateMachineType.Invalid)
			{
				return null;
			}
			return _entity.StateMachines.GetStateMachine(mainStateMachineType);
		}

		public static bool GetIsAttackable(this Character _entity)
		{
			PlayerCharacter playerCharacter = _entity as PlayerCharacter;
			if (playerCharacter != null)
			{
				return playerCharacter.GetIsAttackable();
			}
			EnemyCharacter enemyCharacter = _entity as EnemyCharacter;
			if (enemyCharacter != null)
			{
				return enemyCharacter.EnemyCharacterStateMachines.CharacterVitality.CurrentStateType == EStateType.Character_Alive;
			}
			return false;
		}

		public static bool GetIsInCombat(this Character _entity)
		{
			IStateMachine mainStateMachine = _entity.GetMainStateMachine();
			return mainStateMachine != null && mainStateMachine.CurrentStateType == EStateType.CombatCharacterMain_Combat;
		}

		public static GameEntity GetTargetEntity(this NpcCharacter _entity)
		{
			return LogicMain.PlayerEntity;
		}

		public static PlayerCharacter GetTargetAsPlayer(this NpcCharacter _entity)
		{
			return LogicMain.PlayerEntity;
		}

		public static bool GetIsBeingAssassinated(this Character _character)
		{
			int entityCount = LogicMain.EntityManager.GetEntityCount(EntityTypes.EType.AssassinCharacter);
			for (int i = 0; i < entityCount; i++)
			{
				AssassinCharacter entity = LogicMain.EntityManager.GetEntity<AssassinCharacter>(EntityTypes.EType.AssassinCharacter, i);
				if (!entity.GetIsPerformingAssassination())
				{
					return false;
				}
				PlayerMain.TargetState.Payload payload = entity.AssassinCharacterStateMachines.PlayerMain.CurrentStatePayload as PlayerMain.TargetState.Payload;
				if (payload != null && payload.Target == _character)
				{
					return true;
				}
			}
			return false;
		}

		public static bool GetIsApproachedToBeAssasinated(this Character _character)
		{
			int entityCount = LogicMain.EntityManager.GetEntityCount(EntityTypes.EType.AssassinCharacter);
			for (int i = 0; i < entityCount; i++)
			{
				AssassinCharacter entity = LogicMain.EntityManager.GetEntity<AssassinCharacter>(EntityTypes.EType.AssassinCharacter, i);
				AssassinApproachingTarget assassinApproachingTarget = entity.AssassinCharacterStateMachines.AssassinApproachingTarget;
				if (assassinApproachingTarget.TargetId == _character.Id)
				{
					switch (assassinApproachingTarget.Reason)
					{
					case EInteractionReason.AssassinationLeap:
					case EInteractionReason.AssassinationStealth:
					case EInteractionReason.AssassinationStealthFallback:
					case EInteractionReason.AssassinationAir:
						return true;
					}
				}
			}
			return false;
		}

		public static bool GetIsApproachedByAssassin(this Character _character, EInteractionReason _interactionReason)
		{
			int entityCount = LogicMain.EntityManager.GetEntityCount(EntityTypes.EType.AssassinCharacter);
			for (int i = 0; i < entityCount; i++)
			{
				AssassinCharacter entity = LogicMain.EntityManager.GetEntity<AssassinCharacter>(EntityTypes.EType.AssassinCharacter, i);
				AssassinApproachingTarget assassinApproachingTarget = entity.AssassinCharacterStateMachines.AssassinApproachingTarget;
				if (assassinApproachingTarget.TargetId == _character.Id)
				{
					EInteractionReason reason = assassinApproachingTarget.Reason;
					if (reason == _interactionReason)
					{
						return true;
					}
					continue;
				}
				Assets.Scripts.Mission.Definitions.SkillData.IApproachSkillDefinition buffedApproachSkillDefinition = UbiCore.Utils.Singleton<Assets.Scripts.Mission.Definitions.SkillData>.Instance.GetBuffedApproachSkillDefinition(_interactionReason, entity);
				if (buffedApproachSkillDefinition != null && entity.Skills.GetState(buffedApproachSkillDefinition.Type) == ESkillState.Active)
				{
					IEntityIdSkillPayload payload = entity.Skills.GetPayload<IEntityIdSkillPayload>(buffedApproachSkillDefinition.Type);
					if (payload != null && payload.Id == _character.Id)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool GetIsInNormalCondition(this Character _character)
		{
			ICharacterConditionCarrierEntity characterConditionCarrierEntity = _character as ICharacterConditionCarrierEntity;
			if (characterConditionCarrierEntity != null)
			{
				return characterConditionCarrierEntity.GetIsInCondition(EStateType.CharacterCondition_Normal);
			}
			return true;
		}

		public static bool GetIsThreateningOrNormal(this Character _character)
		{
			ICharacterConditionCarrier characterConditionCarrier = _character.CharacterStateMachines as ICharacterConditionCarrier;
			if (characterConditionCarrier != null)
			{
				EStateType currentStateType = characterConditionCarrier.CharacterCondition.CurrentStateType;
				return currentStateType == EStateType.CharacterCondition_Normal || currentStateType == EStateType.CharacterCondition_ThreateningVictim;
			}
			return true;
		}

		public static bool GetIsConfused(this Character _character)
		{
			ICharacterConditionCarrierEntity characterConditionCarrierEntity = _character as ICharacterConditionCarrierEntity;
			if (characterConditionCarrierEntity != null)
			{
				return characterConditionCarrierEntity.GetIsInCondition(EStateType.CharacterCondition_Confused);
			}
			return false;
		}

		public static bool GetIsInCondition(this ICharacterConditionCarrierEntity _character, EStateType _conditionState)
		{
			EStateType currentStateType = _character.CharacterConditionCarrier.CharacterCondition.CurrentStateType;
			return currentStateType == _conditionState;
		}

		public static bool GetIsUnconscious(this Character _character)
		{
			ICharacterConditionCarrier characterConditionCarrier = _character.CharacterStateMachines as ICharacterConditionCarrier;
			if (characterConditionCarrier != null)
			{
				EStateType currentStateType = characterConditionCarrier.CharacterCondition.CurrentStateType;
				return currentStateType != EStateType.CharacterCondition_Normal && currentStateType != EStateType.CharacterCondition_Seduced && currentStateType != EStateType.CharacterCondition_Greedy && currentStateType != EStateType.CharacterCondition_JackBoxed && currentStateType != EStateType.CharacterCondition_StickyBombPanic && currentStateType != EStateType.CharacterCondition_Fleeing && currentStateType != EStateType.CharacterCondition_Captured && currentStateType != EStateType.CharacterCondition_ThreateningVictim && currentStateType != EStateType.CharacterCondition_Confused && currentStateType != EStateType.CharacterCondition_LuredBySweetMusic;
			}
			return false;
		}

		public static bool GetIsKnockdowned(this Character _character)
		{
			ICharacterConditionCarrier characterConditionCarrier = _character.CharacterStateMachines as ICharacterConditionCarrier;
			return characterConditionCarrier != null && characterConditionCarrier.CharacterCondition.CurrentStateType == EStateType.CharacterCondition_Knockdowned;
		}

		public static bool GetIsInParcour(this Character _character)
		{
			IStateMachine mainStateMachine = _character.GetMainStateMachine();
			return mainStateMachine != null && mainStateMachine.CurrentStateType == EStateType.CombatCharacterMain_Parcour;
		}

		public static bool GetIsSchedulingParcour(this Character _character)
		{
			IStateMachine mainStateMachine = _character.GetMainStateMachine();
			return mainStateMachine != null && mainStateMachine.ScheduledStateType == EStateType.CombatCharacterMain_Parcour;
		}

		public static bool GetIsInParcourOrScheduled(this Character _character)
		{
			IStateMachine mainStateMachine = _character.GetMainStateMachine();
			return mainStateMachine != null && (mainStateMachine.ScheduledStateType == EStateType.CombatCharacterMain_Parcour || mainStateMachine.CurrentStateType == EStateType.CombatCharacterMain_Parcour);
		}

		public static void StartFollowPath(this Character _character, EMovementSpeed _speed, NavMeshPath _path, bool _retainPathOnCancel = false, EStartIdleAnimAfterTaskPolicy _startIdleWhenFinishedPolicy = EStartIdleAnimAfterTaskPolicy.DontCare, float _stoppingDistance = float.MaxValue, int _layerMask = 0)
		{
			float speed;
			if (_character.IsAttached)
			{
				speed = _character.UnityComponentCache.AnimationController.CalculateSpeed(_speed);
				if (_speed != EMovementSpeed.Invalid_EMovementSpeed)
				{
					_character.UnityComponentCache.AnimationController.ReferenceMovementSpeed = _speed;
					_character.UnityComponentCache.AnimationController.Agent.speed = _character.UnityComponentCache.AnimationController.CalculateSpeed(_speed);
				}
			}
			else
			{
				speed = _speed.GetDefaultSpeedValue();
			}
			_character.StartFollowPath(speed, _path, _retainPathOnCancel, _startIdleWhenFinishedPolicy, _stoppingDistance, _layerMask);
		}

		public static void StartFollowPath(this Character _character, float _speed, NavMeshPath _path, bool _retainPathOnCancel, EStartIdleAnimAfterTaskPolicy _startIdleWhenFinishedPolicy, float _stoppingDistance, int _layerMask)
		{
			if (_character.IsAttached)
			{
				AnimationController animationController = _character.UnityComponentCache.AnimationController;
				if (_character is PlayerCharacter)
				{
					animationController.TaskManager.CreateAnimationTaskPlayerMoveTo(_path, !_retainPathOnCancel, _startIdleWhenFinishedPolicy, _stoppingDistance, false, Vector3.zero, _speed, _layerMask);
				}
				else
				{
					animationController.TaskManager.CreateAnimationTaskMoveTo(_path, _speed, !_retainPathOnCancel, _startIdleWhenFinishedPolicy, true, _stoppingDistance, _layerMask);
				}
			}
			else
			{
				NavAgentSurrogate navAgentSurrogate = _character.Cast<INavAgentSurrogateCarrier>().NavAgentSurrogate;
				navAgentSurrogate.Speed = _speed;
				navAgentSurrogate.Path = _path;
			}
		}

		public static Quaternion FacePosition(this IPositionProvider _positionProvider, Vector3 _facingTarget, float _damping = 0.1f)
		{
			Vector3 forward = _facingTarget - _positionProvider.Position;
			forward.y = 0f;
			Quaternion quaternion = _positionProvider.Rotation;
			if (forward.sqrMagnitude > 0.01f)
			{
				quaternion = Quaternion.LookRotation(forward);
			}
			_positionProvider.Rotation = Quaternion.Lerp(_positionProvider.Rotation, quaternion, LogicMain.FrameTime / (LogicMain.FrameTime + _damping));
			return quaternion;
		}

		public static void RotateInstantly(this Character _character, Vector3 _lookAtPos)
		{
			Vector3 direction = _lookAtPos - _character.Position;
			Quaternion rotation = Quaternion.Euler(0f, direction.AngleZX(), 0f);
			_character.Rotation = rotation;
		}

		public static bool FilterEnemyAlivePredicate(EnemyCharacter _enemyCharacter)
		{
			return _enemyCharacter != null && !_enemyCharacter.GetIsDead();
		}

		public static bool FilterBombHitCharacters(Character _character)
		{
			return _character is CrowdCharacter || _character is EnemyCharacter || _character is InteractionCharacter || _character is AssassinCharacter;
		}

		public static bool CanInteract(this InteractionCharacter _entity)
		{
			NpcData.INpcDefinition definition = _entity.NpcDefinition.Definition;
			if (definition.InteractionType == NpcData.NpcDefinition.ENpcInteractionType.Merchant && !EntitlementExtensions.IsDoctorUnlocked())
			{
				return false;
			}
			bool flag = _entity.InteractionCharacterStateMachines.InteractionCharacterMain.CurrentStateType == EStateType.CharacterInteractionMain_Idle && _entity.InteractionCharacterStateMachines.CharacterInteraction.CurrentStateType == EStateType.CharacterInteraction_Enabled;
			if (_entity.IsAttached)
			{
				int currentStateMachineHash = _entity.UnityComponentCache.AnimationController.StateMachine.GetCurrentStateMachineHash();
				if (currentStateMachineHash == AnimationStateMachineHashes.StateMachine_PushBack.NameHash)
				{
					return false;
				}
			}
			if (flag)
			{
				switch (definition.InteractionType)
				{
				case NpcData.NpcDefinition.ENpcInteractionType.Invalid:
					flag = false;
					break;
				}
			}
			return flag;
		}

		public static int GetAutoPassableNavMeshLayerMask(this Character _character)
		{
			if (_character is AssassinCharacter)
			{
				return LogicConstants.AutoPassableNavMeshLayerMask;
			}
			NpcCharacter npcCharacter = _character as NpcCharacter;
			if (npcCharacter != null)
			{
				return (!npcCharacter.EntityStats.Get_bool(StatTypes.EStatType.Npc_CanDoParcour)) ? NavMeshLayers.WalkableLayerMaskPreset : LogicConstants.AutoPassableNavMeshLayerMask;
			}
			return NavMeshLayers.WalkableLayerMaskPreset;
		}

		public static Vector3 GetHeadPosition(this Character _character)
		{
			Transform headTransform = _character.UnityComponentCache.HeadTransform;
			return (!(headTransform == null)) ? headTransform.position : (_character.Position + _character.CharacterDefinition.Definition.DetectionCastOffset);
		}

		public static Vector3 GetDefaultProjectileImpactPos(this Character _target)
		{
			if (_target.IsAttached)
			{
				Transform boneTransform = _target.UnityComponentCache.AnimationController.Animator.GetBoneTransform(HumanBodyBones.Chest);
				if (boneTransform != null)
				{
					return boneTransform.position;
				}
			}
			if (_target.GetDeadOrAboutToDie())
			{
				return _target.Position + new Vector3(0f, 0.1f, 0f);
			}
			return _target.Position + new Vector3(0f, 1.1f, 0f);
		}
	}
}
