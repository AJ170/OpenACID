using System;
using AcierProtocol;
using Assets.Scripts.Animation;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.Logic
{
	public static class Helpers
	{
		public static T CreateEntity<T>(EntityTypes.EType _type) where T : IEntity
		{
			return (T)Activator.CreateInstance(EntityTypes.GetEntityClassType(_type));
		}

		public static T CreateEntity<T>(EntityData.IEntityDefinition _definition) where T : IEntity
		{
			T result = CreateEntity<T>(_definition.EntityType);
			result.Definition.EntityDefinition = _definition;
			return result;
		}

		public static T CreateRegisterAttachEntity<T>(EntityData.IEntityDefinition _definition, EntityWrapperComponent _wrapper, Transform _parent = null) where T : GameEntity
		{
			T val = CreateEntity<T>(_definition);
			val.GameObjectParent = _parent;
			LogicMain.EntityManager.RegisterEntity(val);
			_wrapper.Entity = val;
			return val;
		}

		public static T GetEntity<T>(this GameObject _go) where T : class, IEntity
		{
			return _go.GetComponent<EntityWrapperComponent>().ForceSet().Entity.Cast<T>();
		}

		public static T TryGetEntity<T>(this GameObject _go) where T : class, IEntity
		{
			EntityWrapperComponent component = _go.GetComponent<EntityWrapperComponent>();
			if (component == null)
			{
				return (T)null;
			}
			return component.Entity.As<T>();
		}

		public static T GetEntity<T>(this UnityEngine.Component _component) where T : class, IEntity
		{
			return _component.GetComponent<EntityWrapperComponent>().ForceSet().Entity.Cast<T>();
		}

		public static T TryGetEntity<T>(this UnityEngine.Component _component) where T : class, IEntity
		{
			EntityWrapperComponent component = _component.GetComponent<EntityWrapperComponent>();
			if (component == null)
			{
				return (T)null;
			}
			return component.Entity.As<T>();
		}

		public static IdleType ToIdleType(this EStateType _type)
		{
			IdleType result = IdleType.Invalid;
			switch (_type)
			{
			case EStateType.PlayerProfile_Low:
				result = IdleType.LowProfile;
				break;
			case EStateType.PlayerProfile_High:
				result = IdleType.HighProfile;
				break;
			case EStateType.PlayerProfile_Chase:
				result = IdleType.HighProfile;
				break;
			}
			return result;
		}

		public static EMovementSpeed ToPlayerSpeed(this EStateType _type, bool _overrideHighWithSprint)
		{
			EMovementSpeed result = EMovementSpeed.Invalid_EMovementSpeed;
			switch (_type)
			{
			case EStateType.PlayerProfile_Low:
				result = EMovementSpeed.Walk;
				break;
			case EStateType.PlayerProfile_High:
				result = ((!_overrideHighWithSprint) ? EMovementSpeed.Run : EMovementSpeed.Sprint);
				break;
			case EStateType.PlayerProfile_Chase:
				result = EMovementSpeed.Sprint;
				break;
			}
			return result;
		}

		public static EMovementSpeed ToPlayerSpeed(this EStateType _type, IPlayerStatsCarrier _playerStatsCarrier)
		{
			return _type.ToPlayerSpeed(_playerStatsCarrier != null && _playerStatsCarrier.PlayerStats.Get_bool(StatTypes.EStatType.Player_SprintInHighProfileOverride));
		}

		public static EStateType ToStateType(this EMovementSpeed _speed)
		{
			EStateType result = EStateType.Invalid;
			switch (_speed)
			{
			case EMovementSpeed.WalkSlow:
			case EMovementSpeed.Walk:
			case EMovementSpeed.WalkFast:
				result = EStateType.PlayerProfile_Low;
				break;
			case EMovementSpeed.Run:
				result = EStateType.PlayerProfile_High;
				break;
			case EMovementSpeed.Sprint:
				result = EStateType.PlayerProfile_Chase;
				break;
			}
			return result;
		}

		public static bool GetIsAssassination(this EInteractionReason _reason)
		{
			switch (_reason)
			{
			case EInteractionReason.AssassinationLeap:
			case EInteractionReason.AssassinationStealth:
			case EInteractionReason.AssassinationStealthFallback:
			case EInteractionReason.AssassinationAir:
				return true;
			default:
				return false;
			}
		}

		public static EWeaponState ToWeaponState(this EStateType _type)
		{
			if (_type == EStateType.Generic_WaitingForAnim)
			{
				return EWeaponState.Invalid;
			}
			return (EWeaponState)(_type - 32);
		}

		public static EStateType ToStateType(this EWeaponState _weaponState)
		{
			return (EStateType)(_weaponState + 32);
		}

		public static GameObjectEntity TryGetEntity(this GameObject _gameObject)
		{
			if (_gameObject == null)
			{
				return null;
			}
			EntityWrapperComponent entityWrapperComponent = null;
			do
			{
				entityWrapperComponent = _gameObject.GetComponent<EntityWrapperComponent>();
				Transform parent = _gameObject.transform.parent;
				_gameObject = ((!(parent == null)) ? parent.gameObject : null);
			}
			while (entityWrapperComponent == null && _gameObject != null);
			if (entityWrapperComponent == null)
			{
				return null;
			}
			return entityWrapperComponent.Entity;
		}

		public static float GetDistance(IGameEntity _entity1, IGameEntity _entity2)
		{
			return (_entity1.Position - _entity2.Position).magnitude;
		}

		public static float GetPlaneDistance(IGameEntity _entity1, IGameEntity _entity2)
		{
			return (_entity1.Position - _entity2.Position).LengthXZ();
		}

		public static float GetDistanceFromPlayer(this IGameEntity _entity)
		{
			if (LogicMain.Player == null)
			{
				return float.MaxValue;
			}
			return GetDistance(LogicMain.PlayerEntity, _entity);
		}

		public static float GetPlaneDistanceFromPlayer(this IGameEntity _entity)
		{
			if (LogicMain.Player == null)
			{
				return float.MaxValue;
			}
			return GetPlaneDistance(LogicMain.PlayerEntity, _entity);
		}

		public static bool IsInState(this IStateMachinesCarrier _entity, EStateMachineType _stateMachineType, int[] _stateTypes)
		{
			IStateMachine stateMachine = _entity.StateMachines.GetStateMachine(_stateMachineType);
			if (stateMachine == null)
			{
				return false;
			}
			for (int i = 0; i < _stateTypes.Length; i++)
			{
				EStateType eStateType = (EStateType)_stateTypes[i];
				if (eStateType == stateMachine.CurrentStateType)
				{
					return true;
				}
			}
			return false;
		}

		public static Enum<SkillTypes> Plus(this Enum<SkillTypes> _skillType, int i)
		{
			return (Enum<SkillTypes>)((int)_skillType + i);
		}

		public static SkillData.ISkillDefinition GetDefinition(this Enum<SkillTypes> _skillType)
		{
			return UbiCore.Utils.Singleton<SkillData>.Instance.GetDefinition(_skillType);
		}

		public static RuleData.IRuleDefinition GetDefinition(this Enum<RuleTypes> _ruleType)
		{
			return UbiCore.Utils.Singleton<RuleData>.Instance.GetDefinition(_ruleType);
		}

		public static BuffData.IBuffDefinition GetDefinition(this EBuffType _buffType)
		{
			return UbiCore.Utils.Singleton<BuffData>.Instance.GetDefinition(_buffType);
		}

		public static Vector3 GetSteeringTarget(this GameEntity _entity)
		{
			bool flag = false;
			Vector3 result = _entity.Position;
			AnimationController animationController = _entity.UnityComponentCache.AnimationController;
			if (animationController != null)
			{
				if (animationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) == AnimationTaskType.Climb)
				{
					AnimationTaskClimb animationTaskClimb = (AnimationTaskClimb)animationController.TaskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base);
					result = animationTaskClimb.OffMeshLinkEntity.EndPosition;
					flag = true;
				}
				else if (animationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) == AnimationTaskType.JumpDownAnywhere)
				{
					AnimationTaskJumpDownAnywhere animationTaskJumpDownAnywhere = (AnimationTaskJumpDownAnywhere)animationController.TaskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base);
					result = animationTaskJumpDownAnywhere.LandPosition;
					flag = true;
				}
				else if (animationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) == AnimationTaskType.JumpDown)
				{
					AnimationTaskJumpDown animationTaskJumpDown = (AnimationTaskJumpDown)animationController.TaskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base);
					result = animationTaskJumpDown.OffMeshLinkEntity.EndPosition;
					flag = true;
				}
			}
			if (!flag)
			{
				NavMeshAgent navMeshAgent = _entity.UnityComponentCache.NavMeshAgent;
				if (navMeshAgent != null)
				{
					result = navMeshAgent.steeringTarget;
				}
			}
			return result;
		}

		public static void GetHidingSpots(BBSimpleList<GameEntity> _result)
		{
			if (_result.Count > 0)
			{
				_result.Clear();
			}
			int entityCount = LogicMain.EntityManager.GetEntityCount(EntityTypes.EType.Haystack);
			for (int i = 0; i < entityCount; i++)
			{
				_result.Add(LogicMain.EntityManager.GetEntity<Haystack>(EntityTypes.EType.Haystack, i));
			}
			entityCount = LogicMain.EntityManager.GetEntityCount(EntityTypes.EType.RooftopGarden);
			for (int j = 0; j < entityCount; j++)
			{
				_result.Add(LogicMain.EntityManager.GetEntity<RooftopGarden>(EntityTypes.EType.RooftopGarden, j));
			}
			entityCount = LogicMain.EntityManager.GetEntityCount(EntityTypes.EType.Bench);
			for (int k = 0; k < entityCount; k++)
			{
				_result.Add(LogicMain.EntityManager.GetEntity<Bench>(EntityTypes.EType.Bench, k));
			}
			entityCount = LogicMain.EntityManager.GetEntityCount(EntityTypes.EType.StalkingZone);
			for (int l = 0; l < entityCount; l++)
			{
				_result.Add(LogicMain.EntityManager.GetEntity<StalkingZone>(EntityTypes.EType.StalkingZone, l));
			}
			entityCount = LogicMain.EntityManager.GetEntityCount(EntityTypes.EType.CrowdCharacter);
			for (int m = 0; m < entityCount; m++)
			{
				CrowdCharacter entity = LogicMain.EntityManager.GetEntity<CrowdCharacter>(EntityTypes.EType.CrowdCharacter, m);
				EHotspot _hotspotType;
				ELimitType _limitType;
				if (Assets.Scripts.Mission.Logic.Components.MissionResult.ConvertTrackedHotspotType(entity.Patrol.PatrolDefinition.Definition.Type, out _hotspotType, out _limitType) && (_hotspotType == EHotspot.EHotspot_CivilianCover || _hotspotType == EHotspot.EHotspot_Courtesans))
				{
					_result.Add(entity);
				}
			}
		}
	}
}
