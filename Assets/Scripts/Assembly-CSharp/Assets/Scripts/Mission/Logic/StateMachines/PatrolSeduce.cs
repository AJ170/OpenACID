using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.AI.Formations;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.States;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Selection;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class PatrolSeduce : StateMachineBase<Assets.Scripts.Mission.Logic.Entities.Patrol, PatrolSeduce>
	{
		public class DancingState : State<Assets.Scripts.Mission.Logic.Entities.Patrol>
		{
			public class Payload : IPayload
			{
				public SimpleGameTimer ActiveTimer { get; private set; }

				public BBList<EnemyCharacter> Enemies { get; private set; }

				public BBList<EntityId> Courtesans { get; private set; }

				public EntityId TargetId { get; set; }

				public Payload()
				{
					ActiveTimer = new SimpleGameTimer();
					Enemies = new BBList<EnemyCharacter>(10);
					Courtesans = new BBList<EntityId>(5);
				}

				public void Reset()
				{
					ActiveTimer.Reset();
					Enemies.Clear();
					Courtesans.Clear();
					TargetId = EntityId.InvalidId;
				}
			}

			public const int DefaultCapacityEnemies = 10;

			public const int DefaultCapacityCourtesans = 5;

			public DancingState(EStateType _stateType, OnEnterHandler _onEnterHandler, OnExitHandler _onExitHandler, OnUpdateHandler _onUpdateHandler)
				: base(_stateType, _onEnterHandler, _onExitHandler, _onUpdateHandler)
			{
			}

			public override IPayload CreatePayload(IStateMachine<Assets.Scripts.Mission.Logic.Entities.Patrol> _stateMachine)
			{
				return new Payload();
			}
		}

		public class ApproachingState : State<Assets.Scripts.Mission.Logic.Entities.Patrol>
		{
			public class Payload : IPayload
			{
				private const int MaxMeetingPointIterations = 5;

				public EntityId Target { get; private set; }

				public bool IsTargetMoving
				{
					get
					{
						GameEntity gameEntity = Target.GameEntity;
						if (gameEntity == null)
						{
							return false;
						}
						NavMeshAgent navMeshAgent = gameEntity.UnityComponentCache.NavMeshAgent;
						return navMeshAgent != null && navMeshAgent.velocity.sqrMagnitude > 0f;
					}
				}

				public SimpleGameTimer StopGuardsTimer { get; private set; }

				public Vector3 MeetingPoint { get; private set; }

				public Payload()
				{
					StopGuardsTimer = new SimpleGameTimer();
				}

				public void Init(EntityId _courtesanPatrol, EntityId _target)
				{
					Target = _target;
					Vector3 vector = (MeetingPoint = _target.GameEntity.Position);
					Vector3 position2 = _courtesanPatrol.GameEntity.Position;
					Vector3 v = vector - position2;
					float num = v.LengthXZ();
					float num2 = ((!(num < 4f)) ? (-1f) : 1f);
					Vector3 vector2 = vector + num2 * v.normalized * 4f;
					float num3 = float.MinValue;
					for (int i = 0; i < 5; i++)
					{
						NavMeshHit _hit;
						if (!NavigationHelper.CascadingSamplePosition(vector2, out _hit, NavMeshLayers.GroundSurfaceLayerMask))
						{
							break;
						}
						vector2 = _hit.position;
						if (NavMesh.FindClosestEdge(vector2, out _hit, NavMeshLayers.GroundSurfaceLayerMask))
						{
							float num4 = vector2.DistanceXZ(_hit.position);
							if (num4 > num3 && vector2.DistanceXZ(vector) < 7.9f)
							{
								num3 = num4;
								MeetingPoint = vector2;
							}
							if (num4 > 3f)
							{
								vector2 += _hit.normal * (3f - Mathf.Sqrt(num4));
							}
							else
							{
								vector2 += _hit.normal * 3f;
							}
							continue;
						}
						break;
					}
				}

				public void Reset()
				{
					StopGuardsTimer.Reset();
				}
			}

			public ApproachingState(EStateType _stateType, OnEnterHandler _onEnterHandler, OnExitHandler _onExitHandler, OnUpdateHandler _onUpdateHandler)
				: base(_stateType, _onEnterHandler, _onExitHandler, _onUpdateHandler)
			{
			}

			public override IPayload CreatePayload(IStateMachine<Assets.Scripts.Mission.Logic.Entities.Patrol> _stateMachine)
			{
				return new Payload();
			}
		}

		private const float StopGuardsDelay = 2f;

		private static readonly BBList<EnemyCharacter> GuardsInRange = new BBList<EnemyCharacter>();

		public override EStateMachineType Type
		{
			get
			{
				return EStateMachineType.PatrolSeduce;
			}
		}

		public override EStateType InitialState
		{
			get
			{
				return EStateType.PatrolSeduce_Undefined;
			}
		}

		public PatrolSeduce(Assets.Scripts.Mission.Logic.Entities.Patrol _entity)
			: base(_entity)
		{
		}

		public static void InitPatrolSeduce()
		{
			StateMachineBase<Assets.Scripts.Mission.Logic.Entities.Patrol, PatrolSeduce>.InitStateMachineBase();
			StateMachineBase<Assets.Scripts.Mission.Logic.Entities.Patrol, PatrolSeduce>.AddState<State<Assets.Scripts.Mission.Logic.Entities.Patrol>>(EStateType.PatrolSeduce_Undefined, null, null, OnUpdateUndefined);
			StateMachineBase<Assets.Scripts.Mission.Logic.Entities.Patrol, PatrolSeduce>.AddState<ApproachingState>(EStateType.PatrolSeduce_Approaching, OnEnterApproaching, OnExitApproaching, OnUpdateApproaching);
			StateMachineBase<Assets.Scripts.Mission.Logic.Entities.Patrol, PatrolSeduce>.AddState<DancingState>(EStateType.PatrolSeduce_Dancing, OnEnterDancing, OnExitDancing, OnUpdateDancing);
			StateMachineBase<Assets.Scripts.Mission.Logic.Entities.Patrol, PatrolSeduce>.AddTransition(EStateType.PatrolSeduce_Undefined, EStateType.PatrolSeduce_Approaching);
			StateMachineBase<Assets.Scripts.Mission.Logic.Entities.Patrol, PatrolSeduce>.AddTransition(EStateType.PatrolSeduce_Approaching, EStateType.PatrolSeduce_Undefined);
			StateMachineBase<Assets.Scripts.Mission.Logic.Entities.Patrol, PatrolSeduce>.AddTransition(EStateType.PatrolSeduce_Approaching, EStateType.PatrolSeduce_Dancing);
			StateMachineBase<Assets.Scripts.Mission.Logic.Entities.Patrol, PatrolSeduce>.AddTransition(EStateType.PatrolSeduce_Dancing, EStateType.PatrolSeduce_Undefined);
		}

		private static void OnUpdateUndefined(IStateMachine<Assets.Scripts.Mission.Logic.Entities.Patrol> _statemachine, IPayload _payload, IState<Assets.Scripts.Mission.Logic.Entities.Patrol> _currentstate)
		{
			Assets.Scripts.Mission.Logic.Entities.Patrol entity = _statemachine.Entity;
			if (entity.PatrolComponent.MissionData.Data.Type != EPatrolType.CourtesanHidingSpot || entity.PatrolComponent.EntityAtHidingSpot == EntityId.InvalidId || UbiCore.Utils.Singleton<SelectionManager>.Instance.Selection != null)
			{
				return;
			}
			EnemyCharacter enemyCharacter = null;
			Vector3 position = entity.Position;
			IEnumerable<EnemyCharacter> enumerable = LogicMain.EntityManager.CollectEntitiesInRadius<EnemyCharacter>(position, 20f);
			bool flag = false;
			float num = float.MinValue;
			foreach (EnemyCharacter item in enumerable)
			{
				if (!item.IsAttached || item.GetIsDead())
				{
					continue;
				}
				Vector3 headPosition = item.GetHeadPosition();
				UnityEngine.Camera main = UnityEngine.Camera.main;
				Vector3 vector = main.transform.position - headPosition;
				float magnitude = vector.magnitude;
				if ((!(magnitude < num) || !flag) && !Physics.Raycast(headPosition, vector.normalized, magnitude, Layers.StaticCollidersLayersMaskPreset))
				{
					bool flag2 = IsPositionOnScreen(main, headPosition);
					if (enemyCharacter == null || flag2)
					{
						flag = flag2;
						num = magnitude;
						enemyCharacter = item;
					}
				}
			}
			ESkillAvailabilityFlags _availabilityFlags;
			if (enemyCharacter != null && PlayerHelper.Entity.PlayerCharacterStateMachines.AssassinApproachingTarget.EvaluateInteractionReason(enemyCharacter.Id, EInteractionReason.Invalid, out _availabilityFlags) == EInteractionReason.LetCourtesansSeduce)
			{
				UbiCore.Utils.Singleton<SelectionManager>.Instance.Selection = enemyCharacter;
			}
		}

		private static void OnEnterApproaching(IStateMachine<Assets.Scripts.Mission.Logic.Entities.Patrol> _statemachine, IPayload _payload, IState<Assets.Scripts.Mission.Logic.Entities.Patrol> _prevstate, IState<Assets.Scripts.Mission.Logic.Entities.Patrol> _currentstate)
		{
			_statemachine.Entity.EntityStats.Set_bool(StatTypes.EStatType.Interaction_Triggered, true);
			LetCourtesansApproachMeetingPoint(_statemachine, _payload);
			ApproachingState.Payload payload = (ApproachingState.Payload)_payload;
			if (payload.IsTargetMoving)
			{
				payload.StopGuardsTimer.Start(2f);
			}
			DancingState.Payload payload2 = _statemachine.GetPayload<DancingState.Payload>(EStateType.PatrolSeduce_Dancing);
			BBList<MemberInfo> entities = _statemachine.Entity.PatrolComponent.Formation.Entities;
			foreach (MemberInfo item in entities)
			{
				payload2.Courtesans.Add(item.m_id);
			}
		}

		private static void OnUpdateApproaching(IStateMachine<Assets.Scripts.Mission.Logic.Entities.Patrol> _statemachine, IPayload _payload, IState<Assets.Scripts.Mission.Logic.Entities.Patrol> _currentstate)
		{
			if (HaveCourtesansArrived(_statemachine, _payload) && _statemachine.Entity.PatrolComponent.Idles && _statemachine.Entity.PatrolComponent.Approacher.MinMemberDeviationSqr < 1f)
			{
				_statemachine.ScheduleStateChange(EStateType.PatrolSeduce_Dancing);
				return;
			}
			ApproachingState.Payload payload = (ApproachingState.Payload)_payload;
			if (!payload.IsTargetMoving || !payload.StopGuardsTimer.IsFinished)
			{
				return;
			}
			EnemyCharacter enemyCharacter = payload.Target.GameEntity as EnemyCharacter;
			if (enemyCharacter == null)
			{
				return;
			}
			GuardsInRange.Clear();
			Assets.Scripts.Mission.Logic.Entities.Patrol patrol = enemyCharacter.Patrol;
			DancingState.Payload payload2 = _statemachine.GetPayload<DancingState.Payload>(EStateType.PatrolSeduce_Dancing);
			payload2.TargetId = enemyCharacter.Id;
			for (int i = 0; i < patrol.PatrolComponent.Formation.MemberCount; i++)
			{
				EnemyCharacter enemyCharacter2 = patrol.PatrolComponent.Formation.Entities[i].m_id.TryGetEntity<EnemyCharacter>();
				if (enemyCharacter2 != null)
				{
					GuardsInRange.Add(enemyCharacter2);
				}
			}
			SeduceGuards(payload2, GuardsInRange, _statemachine.Entity);
		}

		private static void OnExitApproaching(IStateMachine<Assets.Scripts.Mission.Logic.Entities.Patrol> _statemachine, IPayload _payload, IState<Assets.Scripts.Mission.Logic.Entities.Patrol> _currentstate, IState<Assets.Scripts.Mission.Logic.Entities.Patrol> _nextstate)
		{
			if (_nextstate.StateType != EStateType.PatrolSeduce_Dancing)
			{
				DancingState.Payload payload = (DancingState.Payload)_payload;
				StopSeduceGuards(payload);
			}
		}

		private static void OnEnterDancing(IStateMachine<Assets.Scripts.Mission.Logic.Entities.Patrol> _statemachine, IPayload _payload, IState<Assets.Scripts.Mission.Logic.Entities.Patrol> _prevstate, IState<Assets.Scripts.Mission.Logic.Entities.Patrol> _currentstate)
		{
			DancingState.Payload payload = (DancingState.Payload)_payload;
			payload.ActiveTimer.Start(10f);
			Assets.Scripts.Mission.Logic.Entities.Patrol entity = _statemachine.Entity;
			EnemyHelper.FindEnemiesInRange(entity.PatrolComponent.CurrentWaypoint.Position, 7.9f, EnemyHelper.CanBeSeduced, GuardsInRange);
			SeduceGuards(payload, GuardsInRange, entity);
		}

		private static void OnUpdateDancing(IStateMachine<Assets.Scripts.Mission.Logic.Entities.Patrol> _statemachine, IPayload _payload, IState<Assets.Scripts.Mission.Logic.Entities.Patrol> _currentstate)
		{
			DancingState.Payload payload = (DancingState.Payload)_payload;
			if (payload.ActiveTimer.IsFinished)
			{
				_statemachine.ScheduleStateChange(EStateType.PatrolSeduce_Undefined);
			}
		}

		private static void OnExitDancing(IStateMachine<Assets.Scripts.Mission.Logic.Entities.Patrol> _statemachine, IPayload _payload, IState<Assets.Scripts.Mission.Logic.Entities.Patrol> _currentstate, IState<Assets.Scripts.Mission.Logic.Entities.Patrol> _nextstate)
		{
			if (_statemachine.Entity.PatrolComponent.IsOnSidetrack)
			{
				_statemachine.Entity.PatrolComponent.StopSidetrack();
			}
			DancingState.Payload payload = (DancingState.Payload)_payload;
			StopSeduceGuards(payload);
			payload.ActiveTimer.Reset();
		}

		private static void LetCourtesansApproachMeetingPoint(IStateMachine<Assets.Scripts.Mission.Logic.Entities.Patrol> _statemachine, IPayload _payload)
		{
			ApproachingState.Payload payload = (ApproachingState.Payload)_payload;
			Assets.Scripts.Mission.Logic.Components.Patrol patrolComponent = _statemachine.Entity.PatrolComponent;
			patrolComponent.StartSidetrack(payload.MeetingPoint, false);
		}

		private static bool HaveCourtesansArrived(IStateMachine<Assets.Scripts.Mission.Logic.Entities.Patrol> _statemachine, IPayload _payload)
		{
			Vector3 position = _statemachine.Entity.Position;
			ApproachingState.Payload payload = (ApproachingState.Payload)_payload;
			float magnitude = (payload.MeetingPoint - position).magnitude;
			return magnitude < 1f;
		}

		private static void SeduceGuards(DancingState.Payload _payload, BBList<EnemyCharacter> _enemiesToSeduce, Assets.Scripts.Mission.Logic.Entities.Patrol _courtesanPatrol)
		{
			int count = _payload.Enemies.Count;
			for (int i = 0; i < count; i++)
			{
				EnemyCharacter item = _payload.Enemies[i];
				_enemiesToSeduce.Remove(item);
			}
			int count2 = _enemiesToSeduce.Count;
			for (int j = 0; j < count2; j++)
			{
				EnemyCharacter enemyCharacter = _enemiesToSeduce[j];
				CharacterCondition characterCondition = enemyCharacter.EnemyCharacterStateMachines.CharacterCondition;
				TargetConditionState.Payload payload = characterCondition.GetPayload<TargetConditionState.Payload>(EStateType.CharacterCondition_Seduced);
				payload.Target = _courtesanPatrol.Id;
				enemyCharacter.Buffs.AddBuff(new AdditiveNumberBuff(EBuffType.Seduced, 1f));
				characterCondition.ExternalChange(EStateType.CharacterCondition_Seduced);
			}
			_payload.Enemies.AddRange(_enemiesToSeduce);
		}

		private static void StopSeduceGuards(DancingState.Payload _payload)
		{
			List<EnemyCharacter> enemies = _payload.Enemies;
			int count = enemies.Count;
			for (int i = 0; i < count; i++)
			{
				EnemyCharacter enemyCharacter = enemies[i];
				enemyCharacter.Buffs.RemoveBuffs(EBuffType.Seduced, int.MaxValue);
			}
			enemies.Clear();
		}

		public static bool IsPositionOnScreen(UnityEngine.Camera _camera, Vector3 _position)
		{
			Vector3 vector = _camera.WorldToScreenPoint(_position);
			if (vector.z < 0f)
			{
				return false;
			}
			if (vector.x >= 0f && vector.x < (float)Screen.width && vector.y >= 0f && vector.y < (float)Screen.height)
			{
				return true;
			}
			return false;
		}
	}
}
