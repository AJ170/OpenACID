using System;
using AcierProtocol;
using Assets.Scripts.Animation;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.NpcBehavior;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Motion;
using UnityEngine;

namespace Assets.Scripts.Mission.AI.Helper
{
	public static class AiHelper
	{
		public const float MaxRemainingDistanceToStop = 0.1f;

		public static bool CanApproachWithSampling(NpcCharacter _character, Vector3 _destination, int _navMeshMask, bool _checkRemainingDistance, NavMeshPath _path)
		{
			int passableNavMeshLayerMask = GetPassableNavMeshLayerMask(_character);
			passableNavMeshLayerMask &= _navMeshMask;
			NavMeshHit _hit;
			if (NavigationHelper.CascadingSamplePosition(_destination, out _hit, passableNavMeshLayerMask) && NavMesh.CalculatePath(_character.Position, _hit.position, passableNavMeshLayerMask, _path))
			{
				if (_checkRemainingDistance && _path.CalculateLength() < 0.1f)
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public static void AttachApproachTarget(NpcCharacter _character, NavMeshPath _path, EMovementSpeed _speed)
		{
			AnimationController animationController = _character.UnityComponentCache.AnimationController;
			if (NavMesh.CalculatePath(_character.Position, _character.NavAgentSurrogate.Destination, GetPassableNavMeshLayerMask(_character), _path))
			{
				animationController.TaskManager.CreateAnimationTaskMoveToForcePlay(_path, animationController.CalculateSpeed(_speed));
			}
		}

		public static BehaviorReturnCode ApproachCurrentTargetAi(this TargetApproacher _targetApproacher)
		{
			_targetApproacher.SetTarget(_targetApproacher.SourceEntityId.GetEntity<NpcCharacter>().GetTargetEntity().Id);
			return _targetApproacher.ApproachTargetAi();
		}

		public static BehaviorReturnCode ApproachCurrentTargetAi(this TargetApproacher _targetApproacher, Vector3 _targetDestination)
		{
			_targetApproacher.SetTarget(_targetDestination);
			return _targetApproacher.ApproachTargetAi();
		}

		public static BehaviorReturnCode ApproachSteeringTargetAi(this TargetApproacher _targetApproacher, GameEntity _targetGameEntity)
		{
			_targetApproacher.SetTarget(_targetGameEntity.GetSteeringTarget());
			return _targetApproacher.ApproachTargetAi();
		}

		public static BehaviorReturnCode ApproachTargetAi(this TargetApproacher _targetApproacher)
		{
			if (_targetApproacher.SourceEntityId == EntityId.InvalidId)
			{
				_targetApproacher.Reset();
				return BehaviorReturnCode.Failure;
			}
			_targetApproacher.Approach();
			_targetApproacher.Update(true);
			switch (_targetApproacher.Status)
			{
			case TargetApproacher.EStatus.Running:
				return BehaviorReturnCode.Running;
			case TargetApproacher.EStatus.Arrived:
				return BehaviorReturnCode.Success;
			case TargetApproacher.EStatus.Failed:
				return BehaviorReturnCode.Failure;
			default:
				return BehaviorReturnCode.Failure;
			}
		}

		public static BehaviorReturnCode ActionWait(NpcCharacter _character, BehaviorReturnCode _lastReturnCode)
		{
			return ActionWait(_character, IdleType.Neutral, _lastReturnCode);
		}

		public static BehaviorReturnCode ActionWait(NpcCharacter _character, IdleType _idleType, BehaviorReturnCode _lastReturnCode)
		{
			if (_lastReturnCode != BehaviorReturnCode.Running)
			{
				if (_character.IsAttached)
				{
					NavMeshAgent navMeshAgent = _character.UnityComponentCache.NavMeshAgent;
					if (navMeshAgent != null && navMeshAgent.enabled)
					{
						navMeshAgent.destination = _character.Position;
						navMeshAgent.Stop();
					}
					AnimationController animationController = _character.UnityComponentCache.AnimationController;
					if (animationController != null)
					{
						animationController.TaskManager.CreateAnimationTaskIdle(_idleType);
					}
				}
				else
				{
					_character.NavAgentSurrogate.Path = null;
				}
			}
			return BehaviorReturnCode.Running;
		}

		public static BehaviorReturnCode ActionApproach(BehaviorReturnCode _lastReturnCode, NpcCharacter _character, Func<bool> _enter, TargetApproacher _targetApproacher)
		{
			if (_lastReturnCode == BehaviorReturnCode.Running)
			{
				return _targetApproacher.ApproachTargetAi();
			}
			return (_enter == null || !_enter()) ? BehaviorReturnCode.Failure : BehaviorReturnCode.Running;
		}

		public static BehaviorReturnCode ActionGeneric(BehaviorReturnCode _lastReturnCode, NpcCharacter _character, Func<bool> _enter, Func<bool> _update, bool _failureOnDetached = true)
		{
			if (_failureOnDetached && !_character.IsAttached)
			{
				return BehaviorReturnCode.Failure;
			}
			if (_lastReturnCode == BehaviorReturnCode.Running)
			{
				return (_update != null && !_update()) ? BehaviorReturnCode.Success : BehaviorReturnCode.Running;
			}
			return (_enter == null || !_enter()) ? BehaviorReturnCode.Failure : BehaviorReturnCode.Running;
		}

		public static BehaviorReturnCode ActionWaitForAnim(BehaviorReturnCode _lastReturnCode, NpcCharacter _character, Func<bool> _enter, int[] _runningTaskTypes, bool _failureOnDetached = true)
		{
			BehaviorReturnCode behaviorReturnCode = ActionWaitForAnim(_lastReturnCode, _character, _runningTaskTypes, _failureOnDetached);
			return (behaviorReturnCode != 0) ? behaviorReturnCode : ((_enter == null || !_enter()) ? BehaviorReturnCode.Failure : BehaviorReturnCode.Running);
		}

		public static BehaviorReturnCode ActionWaitForAnim(BehaviorReturnCode _lastReturnCode, NpcCharacter _character, Func<IParameterContainer, bool> _enter, IParameterContainer _enterParameters, int[] _runningTaskTypes, bool _failureOnDetached = true)
		{
			BehaviorReturnCode behaviorReturnCode = ActionWaitForAnim(_lastReturnCode, _character, _runningTaskTypes, _failureOnDetached);
			return (behaviorReturnCode != 0) ? behaviorReturnCode : ((_enter == null || !_enter(_enterParameters)) ? BehaviorReturnCode.Failure : BehaviorReturnCode.Running);
		}

		public static bool PlayDismiss(NpcCharacter _npcCharacter, int[] _npcIds, Vector3 _lookAt)
		{
			if (_npcCharacter.IsAttached)
			{
				AnimationController animationController = _npcCharacter.UnityComponentCache.AnimationController;
				return animationController.TaskManager.CreateAnimationTasksDismiss(_lookAt, _npcIds);
			}
			return true;
		}

		public static bool PlayStare(NpcCharacter _npcCharacter, Vector3 _lookAt)
		{
			if (_npcCharacter.IsAttached)
			{
				AnimationController animationController = _npcCharacter.UnityComponentCache.AnimationController;
				return animationController.TaskManager.CreateAnimationTaskNpcTurningAndIdle(IdleType.Neutral, _lookAt);
			}
			return true;
		}

		public static CharacterAwareness.AlarmedState.AlarmedPayload GetAlarmedPayload(NpcCharacter _character)
		{
			ICharacterAwarenessCarrier characterAwarenessCarrier = _character.StateMachines as ICharacterAwarenessCarrier;
			return characterAwarenessCarrier.CharacterAwareness.GetPayload<CharacterAwareness.AlarmedState.AlarmedPayload>(EStateType.CharacterAwareness_Alarmed);
		}

		public static void RestartAlarmedActivity(EnemyCharacter _enemy)
		{
			CharacterAwareness characterAwareness = _enemy.EnemyCharacterStateMachines.CharacterAwareness;
			CharacterAwareness.AlarmedState.AlarmedPayload payload = characterAwareness.GetPayload<CharacterAwareness.AlarmedState.AlarmedPayload>(EStateType.CharacterAwareness_Alarmed);
			payload.RestartActiveTimer(characterAwareness);
		}

		public static void UnsheathWeapon(NpcCharacter _character, CharacterDrawWeaponBase _drawWeapon, EWeaponCategory _weaponCategory, bool _cancelMovement)
		{
			if (_cancelMovement)
			{
				_character.CancelMovement(false);
			}
			if (_drawWeapon.ChosenWeaponState != EWeaponState.Unsheathed)
			{
				_character.ChooseWeaponState(_weaponCategory, EWeaponState.Unsheathed);
			}
		}

		public static void SheathWeapon(NpcCharacter _character, CharacterDrawWeaponBase _drawWeapon, EWeaponCategory _weaponCategory, bool _cancelMovement)
		{
			if (_cancelMovement)
			{
				_character.CancelMovement(false);
			}
			if (_drawWeapon.ChosenWeaponState != 0)
			{
				_character.ChooseWeaponState(_weaponCategory, EWeaponState.Sheathed);
			}
		}

		public static int GetPassableNavMeshLayerMask(NpcCharacter _character)
		{
			return _character.GetAutoPassableNavMeshLayerMask();
		}

		public static bool AddToPatrol(NpcRuntimeData _npcRuntimeData)
		{
			NpcCharacter npcCharacter = _npcRuntimeData.NpcCharacter;
			Patrol patrol = npcCharacter.Patrol;
			return patrol != null && patrol.Add(npcCharacter.Id);
		}

		public static void RemoveFromPatrol(NpcRuntimeData _npcRuntimeData, bool _lowPrio)
		{
			NpcCharacter npcCharacter = _npcRuntimeData.NpcCharacter;
			Patrol patrol = npcCharacter.Patrol;
			if (patrol != null)
			{
				patrol.Remove(npcCharacter.Id, _lowPrio);
			}
		}

		private static BehaviorReturnCode ActionWaitForAnim(BehaviorReturnCode _lastReturnCode, NpcCharacter _character, int[] _runningTaskTypes, bool _failureOnDetached = true)
		{
			if (_failureOnDetached && !_character.IsAttached)
			{
				return BehaviorReturnCode.Failure;
			}
			return (_lastReturnCode == BehaviorReturnCode.Running) ? ((!_character.IsAttached || !ContainsCurrentTask(_character, _runningTaskTypes)) ? BehaviorReturnCode.Success : BehaviorReturnCode.Running) : BehaviorReturnCode.NotCalled;
		}

		private static bool ContainsCurrentTask(NpcCharacter _character, int[] _runningTaskTypes)
		{
			AnimationTaskType currentTaskType = _character.UnityComponentCache.AnimationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base);
			int num = _runningTaskTypes.Length;
			for (int i = 0; i < num; i++)
			{
				AnimationTaskType animationTaskType = (AnimationTaskType)_runningTaskTypes[i];
				if (animationTaskType == currentTaskType)
				{
					return true;
				}
			}
			return false;
		}
	}
}
