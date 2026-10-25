using AcierProtocol;
using Assets.Scripts.Animation;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Motion;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.System;
using UbiCore;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.Logic
{
	public static class PatrolHelper
	{
		private const float MaxRemainingDistanceToRecalculate = 1.5f;

		private const float MaxRemainingDistanceToRecalculateSqr = 2.25f;

		private const float MinVelocityToRecalculate = 3f;

		private const float MinVelocityToRecalculateSqr = 9f;

		private const float StopApproachinDistance = 0.3f;

		private const float StopApproachinDistanceSqr = 0.09f;

		private const float MaxRemainingDistanceToIdle = 0.4f;

		private const float MinDistanceToLookToWaypointSqr = 0.01f;

		private const float MinDeltaInterpolateSpeed = 3f;

		private const float MaxDeltaInterpolateSpeed = 6f;

		private const float MinMaxDeltaInterpolateSpeed = 3f;

		private const float WalkToIdleMaxDelta = 1f;

		private const float TargetDeltaMagnitudeOnAttach = 8f;

		public static void AttachPatrol(NpcCharacter _character, NavMeshPath _path, TargetApproacher _targetApproacher)
		{
			if (!_character.GetIsDead())
			{
				Assets.Scripts.Mission.Logic.Entities.Patrol patrol = _character.Patrol;
				Assets.Scripts.Mission.Logic.Components.Patrol patrolComponent = patrol.PatrolComponent;
				if (patrolComponent.Idles)
				{
					ForceIdleOfWaypoint(_character, patrolComponent);
				}
				else
				{
					FollowPatrol(_character, patrolComponent, _path, _targetApproacher, true, true);
				}
			}
		}

		public static void ForceIdleOfWaypoint(NpcCharacter _character, Assets.Scripts.Mission.Logic.Components.Patrol _patrolComponent)
		{
			Assets.Scripts.Mission.Logic.Components.Patrol.Waypoint waypoint = ForceRotationOfWaypoint(_character, _patrolComponent);
			AnimationController animationController = _character.UnityComponentCache.AnimationController;
			IdleType idleType = animationController.CharacterDefinition.GetIdleType(waypoint.IdleCategory);
			if (idleType == IdleType.CapturedNpcMale)
			{
				animationController.TaskManager.CreateAnimationTaskCapturedNpc();
			}
			else
			{
				animationController.TaskManager.CreateAnimationTaskIdleForcePlay(idleType);
			}
		}

		public static Assets.Scripts.Mission.Logic.Components.Patrol.Waypoint ForceRotationOfWaypoint(NpcCharacter _character, Assets.Scripts.Mission.Logic.Components.Patrol _patrolComponent)
		{
			Assets.Scripts.Mission.Logic.Components.Patrol.Waypoint currentWaypoint = _patrolComponent.CurrentWaypoint;
			switch (currentWaypoint.RotationType)
			{
			case ERotationAtWaypoint.RotationOfWaypoint:
				SetYRotation(_character, currentWaypoint.Rotation.eulerAngles.y);
				break;
			case ERotationAtWaypoint.LookingToCenter:
			{
				float _angle;
				if (CalcLookToCenterRotation(_character, currentWaypoint, out _angle))
				{
					SetYRotation(_character, _angle);
				}
				break;
			}
			default:
				SetYRotation(_character, currentWaypoint.Rotation.eulerAngles.y);
				break;
			}
			return currentWaypoint;
		}

		public static void Patrol(NpcCharacter _character, NavMeshPath _path, TargetApproacher _targetApproacher, bool _forcePathRecalculation, ref bool _idling)
		{
			Assets.Scripts.Mission.Logic.Entities.Patrol patrol = _character.Patrol;
			Patrol(_character, patrol.PatrolComponent, _path, _targetApproacher, _forcePathRecalculation, ref _idling);
		}

		public static void Patrol(AssassinCharacter _player, Assets.Scripts.Mission.Logic.Entities.Patrol _patrol, NavMeshPath _path, TargetApproacher _targetApproacher, ref bool _idling)
		{
			Patrol(_player, _patrol.PatrolComponent, _path, _targetApproacher, true, ref _idling);
		}

		public static bool Add(this Assets.Scripts.Mission.Logic.Entities.Patrol _patrol, EntityId _id)
		{
			bool _newlyAdded;
			if (_patrol.PatrolComponent.Formation.Add(_id, out _newlyAdded))
			{
				if (_newlyAdded && _patrol.PatrolComponent.EntityAtHidingSpot != EntityId.InvalidId)
				{
					AddHighlight(_id);
				}
				NpcCharacter npcCharacter = _id.Entity as NpcCharacter;
				if (npcCharacter != null)
				{
					int value = 0;
					int _entityIndex;
					if (_patrol.PatrolComponent.Formation.GetEntityIndex(_id, out _entityIndex))
					{
						value = AnimationHelper.ClampAnimatorNpcId(_entityIndex);
					}
					npcCharacter.NpcStats.Set_int(StatTypes.EStatType.Npc_AnimatorNpcId, value);
				}
				return true;
			}
			return false;
		}

		public static void Remove(this Assets.Scripts.Mission.Logic.Entities.Patrol _patrol, EntityId _id, bool _lowPrio)
		{
			bool _newlyRemoved;
			_patrol.PatrolComponent.Formation.Remove(_id, out _newlyRemoved, _lowPrio);
			if (_newlyRemoved && _patrol.PatrolComponent.EntityAtHidingSpot != EntityId.InvalidId)
			{
				RemoveHighlight(_id);
			}
		}

		public static bool Contains(this Assets.Scripts.Mission.Logic.Entities.Patrol _patrol, EntityId _id)
		{
			return _patrol.PatrolComponent.Formation.Contains(_id);
		}

		public static bool AddAtHidingSpot(this Assets.Scripts.Mission.Logic.Entities.Patrol _patrol, EntityId _id)
		{
			if (_patrol.PatrolComponent.Formation.AddAtHidingSpot(_id))
			{
				_patrol.PatrolComponent.Formation.HighlightAllMembers(true);
				AddHighlight(_id);
				return true;
			}
			return false;
		}

		public static bool RemoveFromHidingSpot(this Assets.Scripts.Mission.Logic.Entities.Patrol _patrol, EntityId _id)
		{
			if (_patrol.PatrolComponent.Formation.RemoveFromHidingSpot(_id))
			{
				RemoveHighlight(_id);
				_patrol.PatrolComponent.Formation.HighlightAllMembers(false);
				return true;
			}
			return false;
		}

		public static bool CanInteract(this Assets.Scripts.Mission.Logic.Entities.Patrol _patrol)
		{
			Assets.Scripts.Mission.Logic.Components.Patrol patrolComponent = _patrol.PatrolComponent;
			bool flag = _patrol.PatrolStats.Get_bool(StatTypes.EStatType.Patrol_Enabled);
			bool flag2 = _patrol.PatrolStats.Get_bool(StatTypes.EStatType.Interaction_Triggered);
			switch (_patrol.PatrolDefinition.Definition.Type)
			{
			case EPatrolType.CourtesanHidingSpot:
				if (!EntitlementExtensions.AreCourtesansUnlocked())
				{
					return false;
				}
				break;
			case EPatrolType.CevilianCouple:
			case EPatrolType.CevilianNarrow:
			case EPatrolType.CevilianWide:
			case EPatrolType.CevilianHidingSpot:
				if (!EntitlementExtensions.AreMonksUnlocked())
				{
					return false;
				}
				break;
			}
			return _patrol.PatrolDefinition.Definition.InteractionReason != EInteractionReason.Invalid && flag && !flag2 && !_patrol.IsOnSidetrack() && patrolComponent.Formation.EntityAtHidingSpot == EntityId.InvalidId && (patrolComponent.Idles || patrolComponent.WaypointCount > 1) && patrolComponent.Formation.MemberCount > 0 && (_patrol.VictimComponent == null || !_patrol.VictimComponent.IsVictimAttacked) && (_patrol.CaptiveComponent == null || _patrol.CaptiveComponent.CanLiberate);
		}

		public static bool IsSeducing(this Assets.Scripts.Mission.Logic.Entities.Patrol _patrol)
		{
			EStateType scheduledStateType = _patrol.PatrolStateMachines.PatrolSeduce.ScheduledStateType;
			EStateType eStateType = ((scheduledStateType != 0 && scheduledStateType != EStateType.PatrolSeduce_Undefined) ? scheduledStateType : _patrol.PatrolStateMachines.PatrolSeduce.CurrentStateType);
			return eStateType == EStateType.PatrolSeduce_Approaching || eStateType == EStateType.PatrolSeduce_Dancing;
		}

		public static bool IsOnSidetrack(this Assets.Scripts.Mission.Logic.Entities.Patrol _patrol)
		{
			return _patrol.PatrolComponent.IsOnSidetrack;
		}

		public static bool HidingEntityShouldExit(this Assets.Scripts.Mission.Logic.Entities.Patrol _patrol)
		{
			return _patrol.PatrolComponent.HidingEntityShouldExit;
		}

		private static void SetYRotation(NpcCharacter _character, float _targetY)
		{
			if (!MathHelper.Approximately(_character.Rotation.eulerAngles.y, _targetY))
			{
				_character.Rotation = Quaternion.Euler(0f, _targetY, 0f);
			}
		}

		private static bool CalcLookToCenterRotation(Character _character, Assets.Scripts.Mission.Logic.Components.Patrol.Waypoint _currentTarget, out float _angle)
		{
			Vector3 direction = _currentTarget.Position - _character.Position;
			_angle = 0f;
			bool flag = direction.sqrMagnitude > 0.01f;
			if (flag)
			{
				_angle = direction.AngleZX();
			}
			return flag;
		}

		private static void Patrol(Character _character, Assets.Scripts.Mission.Logic.Components.Patrol _patrolComponent, NavMeshPath _path, TargetApproacher _targetApproacher, bool _forcePathRecalculation, ref bool _idling)
		{
			bool isAttached = _character.IsAttached;
			bool flag = !_patrolComponent.PatrolEntity.PatrolStats.Get_bool(StatTypes.EStatType.Interaction_Triggered) && _patrolComponent.MissionData.Data.Type == EPatrolType.CapturedNpcs;
			bool flag2 = true;
			if (isAttached && !flag)
			{
				ICharacterConditionCarrier characterConditionCarrier = _character.StateMachines as ICharacterConditionCarrier;
				if (characterConditionCarrier != null && characterConditionCarrier.CharacterCondition.CurrentStateType != EStateType.CharacterCondition_Normal)
				{
					flag2 = false;
				}
			}
			if (isAttached && !flag && flag2)
			{
				if (_patrolComponent.ShouldMembersPerformUpdate() && !_character.GetIsInParcour())
				{
					if (_patrolComponent.Idles && !_forcePathRecalculation)
					{
						ApproachWaypoint(_character, _patrolComponent, _path, _targetApproacher, ref _idling);
						return;
					}
					_idling = false;
					FollowPatrol(_character, _patrolComponent, _path, _targetApproacher, _forcePathRecalculation, false);
				}
			}
			else
			{
				_targetApproacher.Reset();
				_idling = _patrolComponent.WaypointCount <= 1;
			}
		}

		private static void FollowPatrol(Character _character, Assets.Scripts.Mission.Logic.Components.Patrol _patrol, NavMeshPath _path, TargetApproacher _targetApproacher, bool _forcePathRecalculation, bool _handleAttach)
		{
			NavMeshAgent navMeshAgent = _character.UnityComponentCache.NavMeshAgent;
			AnimationController animationController = _character.UnityComponentCache.AnimationController;
			if (animationController != null && navMeshAgent.enabled && (_forcePathRecalculation || _handleAttach || _targetApproacher.Status != TargetApproacher.EStatus.Running || (_targetApproacher.TargetPosition - _character.Position).sqrMagnitude < 2.25f || navMeshAgent.velocity.sqrMagnitude > 9f))
			{
				Vector3 _position;
				_patrol.GetTargetWorldPosition(_character.Id, out _position);
				Assets.Scripts.Mission.Logic.Components.Patrol.Waypoint currentWaypoint = _patrol.CurrentWaypoint;
				EMovementSpeed minSpeed = currentWaypoint.MinSpeed;
				float num = animationController.CalculateSpeed(minSpeed);
				if (_handleAttach)
				{
					Vector3 vector = currentWaypoint.Position - _patrol.Position;
					vector.Normalize();
					Vector3 targetPosition = _character.Position + vector * 8f;
					if (navMeshAgent.enabled && navMeshAgent.CalculatePath(targetPosition, _path))
					{
						animationController.TaskManager.CreateAnimationTaskMoveToForcePlay(_path, num);
						_targetApproacher.Reset();
					}
				}
				else if (currentWaypoint.WarpTarget)
				{
					RestartTargetApproacher(_character, _targetApproacher, _position, num);
				}
				else
				{
					float magnitude = (_position - _character.Position).magnitude;
					float num2 = magnitude - 3f;
					float num3;
					if (num2 < 0f)
					{
						num3 = animationController.CalculateSpeed(EMovementSpeed.Walk);
						if (num3 < num)
						{
							num3 = Mathf.Lerp(num3, num, magnitude / 3f);
						}
					}
					else
					{
						num3 = Mathf.Max(num, animationController.CalculateSpeed(currentWaypoint.MaxSpeed), num2 / 3f);
					}
					RestartTargetApproacher(_character, _targetApproacher, _position, num3);
				}
			}
			if (_targetApproacher.Status == TargetApproacher.EStatus.Running)
			{
				_targetApproacher.Update(_forcePathRecalculation);
			}
		}

		private static void ApproachWaypoint(Character _character, Assets.Scripts.Mission.Logic.Components.Patrol _patrol, NavMeshPath _path, TargetApproacher _targetApproacher, ref bool _idling)
		{
			bool flag = _idling;
			NavMeshAgent navMeshAgent = _character.UnityComponentCache.NavMeshAgent;
			AnimationController animationController = _character.UnityComponentCache.AnimationController;
			if (!flag)
			{
				Vector3 position = _character.Position;
				Vector3 _position;
				_patrol.GetTargetWorldPosition(_character.Id, out _position);
				if (NavMesh.CalculatePath(position, _position, NavMeshLayers.FullParcourMaskPreset, _path))
				{
					_idling = _path.corners.Length <= 1;
					if (!_idling)
					{
						float num = _path.CalculateLength();
						_idling = num < 0.4f;
					}
					if (!_idling)
					{
						float magnitude = (_position - position).magnitude;
						float speed = Mathf.Lerp(animationController.CalculateSpeed(EMovementSpeed.WalkSlow), animationController.CalculateSpeed(EMovementSpeed.Walk), magnitude / 1f);
						RestartTargetApproacher(_character, _targetApproacher, _position, speed);
						_targetApproacher.Update(true);
					}
				}
			}
			if (!_idling || flag)
			{
				return;
			}
			if (navMeshAgent.enabled)
			{
				navMeshAgent.Stop();
				navMeshAgent.ResetPath();
			}
			Assets.Scripts.Mission.Logic.Components.Patrol.Waypoint currentWaypoint = _patrol.CurrentWaypoint;
			IdleType idleType = animationController.CharacterDefinition.GetIdleType(currentWaypoint.IdleCategory);
			switch (currentWaypoint.RotationType)
			{
			case ERotationAtWaypoint.RotationOfWaypoint:
				animationController.TaskManager.CreateAnimationTaskNpcTurningAndIdle(idleType, currentWaypoint.Rotation.eulerAngles.y);
				break;
			case ERotationAtWaypoint.LookingToCenter:
			{
				float _angle;
				if (CalcLookToCenterRotation(_character, currentWaypoint, out _angle))
				{
					animationController.TaskManager.CreateAnimationTaskNpcTurningAndIdle(idleType, _angle);
				}
				else
				{
					animationController.TaskManager.CreateAnimationTaskIdle(idleType);
				}
				break;
			}
			default:
				animationController.TaskManager.CreateAnimationTaskIdle(idleType);
				break;
			}
		}

		private static void RestartTargetApproacher(Character _character, TargetApproacher _targetApproacher, Vector3 _target, float _speed)
		{
			_targetApproacher.Reset();
			_targetApproacher.SetData(_character.Id, _target, _speed, 0.09f);
			_targetApproacher.Approach();
		}

		private static void AddHighlight(EntityId _id)
		{
			_id.GameEntity.Highlight.SetMode(Highlight.EHighlightLayer.Hiding, Highlight.ESelectionMode.Hiding);
		}

		private static void RemoveHighlight(EntityId _id)
		{
			_id.GameEntity.Highlight.RemoveMode(Highlight.EHighlightLayer.Hiding);
		}
	}
}
