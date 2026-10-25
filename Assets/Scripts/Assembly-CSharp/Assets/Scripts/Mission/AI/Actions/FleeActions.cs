using System;
using AcierProtocol;
using Assets.Scripts.Animation;
using Assets.Scripts.Mission.AI.Actions.Parameters;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.Helper;
using Assets.Scripts.Mission.AI.NpcBehavior;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Motion;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.System;
using UbiCore;
using UbiCore.Debug;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.AI.Actions
{
	public class FleeActions : NpcBehaviorComponent
	{
		public class RuntimeData : BaseRuntimeData
		{
			public readonly SimpleGameTimer FleeTimer = new SimpleGameTimer();

			public RuntimeData(NpcRuntimeData _npcRuntimeData)
				: base(_npcRuntimeData)
			{
			}
		}

		private const float CrowdFleeApproachDistance = 2f;

		private const float FleeApproachDistanceSqr = 4f;

		private const float FleeStepAngle = 30f;

		private const float FleeMaxAngle = 180f;

		private const float FleeTurnAwayFromEdgeThreshold = 1f;

		private const int FleeStepsAngle = 6;

		private const float FleeSampleDistFactor = (float)Math.PI / 12f;

		private const float CevilianFleeStartMaxdistance = 30f;

		private const float CevilianFleeStartMaxdistanceSqr = 900f;

		private readonly Vector3 InvalidPosition = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

		public SimpleGameTimer FleeTimer
		{
			get
			{
				return base.NpcRuntimeData.Flee.FleeTimer;
			}
		}

		private TargetApproacher TargetApproacher
		{
			get
			{
				return base.NpcRuntimeData.Flee.TargetApproacher;
			}
		}

		private NavMeshPath DummyPath
		{
			get
			{
				return base.NpcRuntimeData.Flee.DummyPath;
			}
		}

		public FleeActions(NpcTreeHolder _npcTreeHolder)
			: base(_npcTreeHolder)
		{
		}

		public bool IsFleeTimerRunning()
		{
			return FleeTimer.IsRunning;
		}

		public BehaviorReturnCode ActionUpdateFleeSpeed(IParameterContainer _fleeReason, BehaviorReturnCode _lastReturnCode)
		{
			FleeReasonParameters fleeReasonParameters = _fleeReason as FleeReasonParameters;
			NpcStats npcStats = base.NpcCharacter.NpcStats;
			switch (fleeReasonParameters.Reason)
			{
			case FleeReasonParameters.FleeReason.FleeFromCapture:
			case FleeReasonParameters.FleeReason.BombExplosion:
				npcStats.Set_int(StatTypes.EStatType.Npc_FleeSpeed, 4);
				break;
			case FleeReasonParameters.FleeReason.WalkFromCombat:
				npcStats.Set_int(StatTypes.EStatType.Npc_FleeSpeed, 1);
				npcStats.Set_Vector3(StatTypes.EStatType.Npc_FleeSourcePosition, InvalidPosition);
				break;
			default:
				npcStats.Set_int(StatTypes.EStatType.Npc_FleeSpeed, 4);
				npcStats.Set_Vector3(StatTypes.EStatType.Npc_FleeSourcePosition, InvalidPosition);
				break;
			}
			return BehaviorReturnCode.Success;
		}

		public BehaviorReturnCode ActionUpdateFleeTimer(IParameterContainer _fleeReason, BehaviorReturnCode _lastReturnCode)
		{
			FleeReasonParameters fleeReasonParameters = _fleeReason as FleeReasonParameters;
			NpcStats npcStats = base.NpcCharacter.NpcStats;
			switch (fleeReasonParameters.Reason)
			{
			case FleeReasonParameters.FleeReason.StrikingAssassination:
				FleeTimer.Start(LogicMain.GameDefinition.CrowdFleeAfterStrikingAssasinationExtensionTime);
				npcStats.Set_int(StatTypes.EStatType.Npc_FleeSpeed, 4);
				npcStats.Set_Vector3(StatTypes.EStatType.Npc_FleeSourcePosition, InvalidPosition);
				break;
			case FleeReasonParameters.FleeReason.FleeFromCombat:
				FleeTimer.Start(LogicMain.GameDefinition.EnemyFleeFromCombatDuration);
				npcStats.Set_int(StatTypes.EStatType.Npc_FleeSpeed, 4);
				npcStats.Set_Vector3(StatTypes.EStatType.Npc_FleeSourcePosition, InvalidPosition);
				break;
			case FleeReasonParameters.FleeReason.FleeFromCapture:
				FleeTimer.Start(LogicMain.GameDefinition.NpcFleeFromCaptureMinTime);
				npcStats.Set_int(StatTypes.EStatType.Npc_FleeSpeed, 4);
				break;
			case FleeReasonParameters.FleeReason.BombExplosion:
				FleeTimer.Start(LogicMain.GameDefinition.CrowdFleeFromBombExplosionMinTime);
				npcStats.Set_int(StatTypes.EStatType.Npc_FleeSpeed, 4);
				npcStats.Set_bool(StatTypes.EStatType.Npc_DidSeeBombExplosion, false);
				break;
			case FleeReasonParameters.FleeReason.WalkFromCombat:
				FleeTimer.Start(LogicMain.GameDefinition.EnemyWalkFromCombatDuration);
				npcStats.Set_int(StatTypes.EStatType.Npc_FleeSpeed, 1);
				npcStats.Set_Vector3(StatTypes.EStatType.Npc_FleeSourcePosition, InvalidPosition);
				break;
			default:
				FleeTimer.Start(LogicMain.GameDefinition.CrowdFleeDefaultExtensionTime);
				npcStats.Set_int(StatTypes.EStatType.Npc_FleeSpeed, 4);
				npcStats.Set_Vector3(StatTypes.EStatType.Npc_FleeSourcePosition, InvalidPosition);
				break;
			}
			return BehaviorReturnCode.Success;
		}

		public BehaviorReturnCode ActionCrowdFlee(BehaviorReturnCode _lastReturnCode)
		{
			return AiHelper.ActionApproach(_lastReturnCode, base.NpcCharacter, StartCrowdFlee, TargetApproacher);
		}

		public void AttachCrowdFlee()
		{
			StartCrowdFlee();
		}

		public void CancelCrowdFlee()
		{
			TargetApproacher.Reset();
		}

		public BehaviorReturnCode ActionEnemyFleeFromCombat(BehaviorReturnCode _lastReturnCode)
		{
			return AiHelper.ActionApproach(_lastReturnCode, base.NpcCharacter, StartEnemyFlee, TargetApproacher);
		}

		public BehaviorReturnCode ActionEnemyWait(BehaviorReturnCode _lastReturnCode)
		{
			if (_lastReturnCode != BehaviorReturnCode.Running)
			{
				Vector3 position = base.NpcCharacter.Position;
				bool flag = false;
				NavMeshHit hit;
				if (NavMesh.FindClosestEdge(position, out hit, NavMeshLayers.GroundSurfaceLayerMask))
				{
					float num = hit.position.DistanceXZ(position);
					if (num < 1f)
					{
						base.NpcCharacter.Rotation = Quaternion.LookRotation(hit.normal);
						flag = true;
					}
				}
				if (!flag)
				{
					Vector3 eulerAngles = base.NpcCharacter.Rotation.eulerAngles;
					base.NpcCharacter.Rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y + 180f, eulerAngles.z);
				}
			}
			return AiHelper.ActionWait(base.NpcCharacter, IdleType.Searching, _lastReturnCode);
		}

		public BehaviorReturnCode ActionAlarmNearGuards(BehaviorReturnCode _lastReturnCode)
		{
			float enemyFleeFromCombatViralDistance = LogicMain.GameDefinition.EnemyFleeFromCombatViralDistance;
			float num = enemyFleeFromCombatViralDistance * enemyFleeFromCombatViralDistance;
			CharacterAwareness.AlarmedState.AlarmedPayload alarmedPayload = AiHelper.GetAlarmedPayload(base.NpcCharacter);
			int enemyCharacterCount = LogicMain.EntityManager.EnemyCharacterCount;
			for (int i = 0; i < enemyCharacterCount; i++)
			{
				EnemyCharacter enemyCharacter = LogicMain.EntityManager.GetEnemyCharacter(i);
				if (enemyCharacter != base.NpcCharacter && enemyCharacter.Id != EntityId.InvalidId && !((base.NpcCharacter.Position - enemyCharacter.Position).sqrMagnitude > num) && !enemyCharacter.GetDeadOrAboutToDie() && !enemyCharacter.GetIsInCombat() && !enemyCharacter.GetIsUnconscious())
				{
					enemyCharacter.SetAlarmed(alarmedPayload);
				}
			}
			return BehaviorReturnCode.Success;
		}

		private bool StartCrowdFlee()
		{
			return StartFlee(LogicMain.GameDefinition.CrowdFleeDistance, false);
		}

		private bool StartEnemyFlee()
		{
			return StartFlee(LogicMain.GameDefinition.EnemyFleeFromCombatDistance, true);
		}

		private bool StartFlee(float _distance, bool _avoidView)
		{
			bool _fallbackFound = false;
			bool _goodTargetFound = false;
			NpcCharacter npcCharacter = base.NpcCharacter;
			if (CanNpcStartFlee(npcCharacter))
			{
				Profiling.Start("StartFlee:FindFleeTarget", float.MaxValue);
				TargetApproacher.Reset();
				Vector3 position = npcCharacter.Position;
				Vector3 vector = npcCharacter.NpcStats.Get_Vector3(StatTypes.EStatType.Npc_FleeSourcePosition);
				float x = vector.x;
				Vector3 invalidPosition = InvalidPosition;
				if (MathHelper.Approximately(x, invalidPosition.x))
				{
					vector = LogicMain.PlayerEntity.Position;
				}
				Vector3 vector2 = position - vector;
				vector2.y = 0f;
				if (vector2.sqrMagnitude < 900f)
				{
					Vector3 vector3 = vector2.normalized;
					float sqrMagnitude = vector3.sqrMagnitude;
					if (sqrMagnitude < MathHelper.Epsilon)
					{
						vector3 = npcCharacter.Rotation * Vector3.forward;
					}
					Vector3 _destination = position;
					int layerMask = NavMeshLayers.GroundSurfaceLayerMask;
					NavMeshHit hit;
					if (NavMesh.SamplePosition(base.NpcCharacter.Position, out hit, 1f, -1) && (hit.mask & NavMeshLayers.GroundSurfaceLayerMask) == 0)
					{
						layerMask = AiHelper.GetPassableNavMeshLayerMask(base.NpcCharacter);
					}
					CheckFleePosition(vector, vector3, _distance, layerMask, ref _destination, ref _goodTargetFound, ref _fallbackFound);
					int num = 1;
					while (!_goodTargetFound && num < 6)
					{
						float num2 = 30f * (float)num;
						Vector3 normalizedDirection = Quaternion.Euler(0f, num2, 0f) * vector3;
						CheckFleePosition(vector, normalizedDirection, _distance, layerMask, ref _destination, ref _goodTargetFound, ref _fallbackFound);
						if (!_goodTargetFound)
						{
							Vector3 normalizedDirection2 = Quaternion.Euler(0f, 0f - num2, 0f) * vector3;
							CheckFleePosition(vector, normalizedDirection2, _distance, layerMask, ref _destination, ref _goodTargetFound, ref _fallbackFound);
						}
						num++;
					}
					if (_goodTargetFound || _fallbackFound)
					{
						ApproachFleePos(_destination, 4f);
					}
				}
				Profiling.Stop();
			}
			return _goodTargetFound || _fallbackFound;
		}

		private static bool CanNpcStartFlee(NpcCharacter _npc)
		{
			if (_npc.IsAttached)
			{
				AnimationController animationController = _npc.UnityComponentCache.AnimationController;
				AnimationTaskBase currentTask = animationController.TaskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base);
				if (currentTask != null)
				{
					return currentTask.CanCancel();
				}
			}
			return true;
		}

		private void CheckFleePosition(Vector3 _fleeSourcePos, Vector3 _normalizedDirection, float _distance, int _layerMask, ref Vector3 _destination, ref bool _goodTargetFound, ref bool _fallbackFound)
		{
			Vector3 vector = _fleeSourcePos + _normalizedDirection * _distance;
			Vector3 _destination2 = vector;
			float maxDistance = _distance * ((float)Math.PI / 12f);
			if (CanApproachFleePos(ref _destination2, maxDistance, _layerMask))
			{
				_fallbackFound = true;
				PlayerCharacter playerEntity = LogicMain.PlayerEntity;
				Vector3 direction = _destination2 - playerEntity.Position;
				_goodTargetFound = Physics.Raycast(playerEntity.GetHeadPosition(), direction, direction.magnitude, Layers.StaticCollidersLayersMaskPreset);
				_destination = _destination2;
			}
		}

		private void ApproachFleePos(Vector3 _destination, float _approachDistSqr)
		{
			EMovementSpeed speed = (EMovementSpeed)base.NpcCharacter.NpcStats.Get_int(StatTypes.EStatType.Npc_FleeSpeed);
			TargetApproacher.SetData(base.NpcCharacter.Id, _destination, speed, _approachDistSqr);
			TargetApproacher.Approach();
		}

		private bool CanApproachFleePos(ref Vector3 _destination, float _maxDistance, int _layerMask)
		{
			NavMeshHit hit;
			if (NavMesh.SamplePosition(_destination, out hit, _maxDistance, _layerMask) && NavMesh.CalculatePath(base.NpcCharacter.Position, hit.position, AiHelper.GetPassableNavMeshLayerMask(base.NpcCharacter), DummyPath))
			{
				_destination = DummyPath.corners[DummyPath.corners.Length - 1];
				return true;
			}
			return false;
		}
	}
}
