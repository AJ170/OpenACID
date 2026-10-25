using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Motion.WithoutAgent;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class NavAgentSurrogate : TurnAndFrameUpdateableComponent
	{
		private const float DefaultIntervalTurnsFactor = 6f;

		private const float MaxIntervalOffset = 60f;

		private const int AssertedFrameUpdateCountMax = 20;

		private const float ScheduleTurnUpdateDistSqrThreshold = 0.25f;

		private readonly NavMeshPath m_tempPath;

		private readonly BBList<Vector3> m_corners;

		private int m_targetIndex;

		private int m_demandFrameUpdateCount;

		private Vector3 m_frameTargetPosition;

		private float m_frameTargetRotationXZ;

		private int m_TurnUpdateIntervalTurns;

		public Vector3 Destination
		{
			get
			{
				return (!HasPath) ? GameEntity.Position : m_corners[m_corners.Count - 1];
			}
			set
			{
				if (NavMesh.CalculatePath(GameEntity.Position, value, WalkableMask, m_tempPath))
				{
					Path = m_tempPath;
				}
				else
				{
					ResetPath();
				}
			}
		}

		public bool HasPath
		{
			get
			{
				return m_corners.Count > 0;
			}
		}

		public NavMeshPath Path
		{
			set
			{
				if (value == null || value.corners.Length <= 1)
				{
					ResetPath();
					return;
				}
				int count = m_corners.Count;
				Vector3 lhs = ((count <= 0) ? Vector3.zero : m_corners[count - 1]);
				m_corners.Clear();
				m_corners.AddRange(value.corners);
				m_targetIndex = 1;
				ResetFrameTargets();
				int count2 = m_corners.Count;
				if (count2 == 0 || lhs.DistanceXZSquared(m_corners[count2 - 1]) > 0.25f)
				{
					ScheduleTurnUpdate(true);
				}
			}
		}

		public float Speed { get; set; }

		public Vector3 SteeringTarget
		{
			get
			{
				return (!HasPath) ? GameEntity.Position : m_corners[m_targetIndex];
			}
		}

		public int WalkableMask { get; set; }

		public override int Priority
		{
			get
			{
				return 6;
			}
		}

		private Vector3 FrameTargetPosition
		{
			get
			{
				return m_frameTargetPosition;
			}
			set
			{
				m_frameTargetPosition = value;
				if (!IsFrameUpdateActiveAndRecent)
				{
					GameEntity.Position = value;
				}
			}
		}

		private float FrameTargetRotationXZ
		{
			get
			{
				return m_frameTargetRotationXZ;
			}
			set
			{
				m_frameTargetRotationXZ = value;
				if (!IsFrameUpdateActiveAndRecent)
				{
					GameEntity.Rotation = Quaternion.Euler(0f, value, 0f);
				}
			}
		}

		public override int TurnUpdateIntervalTurns
		{
			get
			{
				return m_TurnUpdateIntervalTurns;
			}
		}

		private bool IsFrameUpdateActive
		{
			get
			{
				return m_demandFrameUpdateCount > 0;
			}
		}

		private bool IsFrameUpdateActiveAndRecent
		{
			get
			{
				return IsFrameUpdateActive && UbiCore.Utils.Singleton<Timer>.Instance.time - LastFrameUpdateTime < 0.1f;
			}
		}

		public NavAgentSurrogate(IEntity _entity)
			: base(_entity)
		{
			m_tempPath = new NavMeshPath();
			m_corners = new BBList<Vector3>();
			IsActive = true;
		}

		public void ResetPath()
		{
			m_corners.Clear();
			m_targetIndex = -1;
			ResetFrameTargets();
		}

		public void DemandFrameUpdate(bool _demand)
		{
			m_demandFrameUpdateCount = ((!_demand) ? (m_demandFrameUpdateCount - 1) : (m_demandFrameUpdateCount + 1));
		}

		public void UpdateFrameTarget(Vector3 _target, float _targetRotationXZ, float _speed)
		{
			ResetPath();
			FrameTargetPosition = _target;
			FrameTargetRotationXZ = _targetRotationXZ;
			Speed = _speed;
		}

		public override void Attach(EntityWrapperComponent _wrapperComponent)
		{
			base.Attach(_wrapperComponent);
			IsActive = false;
			ResetPath();
			ICharacterConditionCarrier characterConditionCarrier = Entity.Cast<NpcCharacter>().CharacterStateMachines as ICharacterConditionCarrier;
			NavMeshHit _hit;
			if ((characterConditionCarrier != null && characterConditionCarrier.CharacterCondition.CurrentStateType == EStateType.CharacterCondition_OnBench) || !NavigationHelper.CascadingSamplePosition(GameEntity.Position, out _hit, NavMeshLayers.WalkableLayerMaskPreset))
			{
				return;
			}
			Vector3 position = _hit.position;
			Vector3 vector = position;
			NpcCharacter npcCharacter = GameEntity as NpcCharacter;
			if (npcCharacter != null)
			{
				Assets.Scripts.Mission.Logic.Entities.Patrol patrol = npcCharacter.Patrol;
				if (patrol != null && patrol.Contains(npcCharacter.Id))
				{
					Patrol patrolComponent = patrol.PatrolComponent;
					if (!NavMesh.CalculatePath(patrolComponent.CurrentWaypoint.Position, position, NavMeshLayers.WalkableLayerMaskPreset, m_tempPath) || m_tempPath.status != 0)
					{
						if (NavMesh.CalculatePath(patrolComponent.LastWaypoint.Position, patrolComponent.CurrentWaypoint.Position, NavMeshLayers.WalkableLayerMaskPreset, m_tempPath))
						{
							Vector3[] corners = m_tempPath.corners;
							int num = corners.Length;
							if (num == 1)
							{
								vector = corners[0];
							}
							else
							{
								float num2 = float.MaxValue;
								for (int i = 0; i < num - 1; i++)
								{
									Vector3 vector2 = corners[i];
									Vector3 vector3 = corners[i + 1] - vector2;
									vector3.Normalize();
									Vector3 lhs = position - vector2;
									float num3 = Vector3.Dot(lhs, vector3);
									Vector3 vector4 = vector2 + vector3 * num3;
									float magnitude = (vector4 - position).magnitude;
									if (magnitude < num2)
									{
										num2 = magnitude;
										vector = vector4;
									}
								}
							}
						}
						if (NavMesh.CalculatePath(vector, position, NavMeshLayers.WalkableLayerMaskPreset, m_tempPath))
						{
							Vector3[] corners2 = m_tempPath.corners;
							vector = corners2[corners2.Length - 1];
						}
					}
				}
			}
			GameEntity.Position = vector;
			IUnityComponentCacheCarrier unityComponentCacheCarrier = Entity as IUnityComponentCacheCarrier;
			unityComponentCacheCarrier.UnityComponentCache.NavMeshAgent.Warp(GameEntity.Position);
		}

		public override void Detach()
		{
			base.Detach();
			ResetFrameTargets();
			IsActive = true;
			IUnityComponentCacheCarrier unityComponentCacheCarrier = Entity as IUnityComponentCacheCarrier;
			if (unityComponentCacheCarrier == null)
			{
				return;
			}
			WalkableMask = ((Character)Entity).GetAutoPassableNavMeshLayerMask();
			NavMeshAgent navMeshAgent = unityComponentCacheCarrier.UnityComponentCache.NavMeshAgent;
			if (!(navMeshAgent != null) || !navMeshAgent.enabled)
			{
				return;
			}
			bool flag = true;
			NpcCharacter npcCharacter = GameEntity as NpcCharacter;
			if (npcCharacter != null)
			{
				Assets.Scripts.Mission.Logic.Entities.Patrol patrol = npcCharacter.Patrol;
				flag = patrol == null || !patrol.Contains(npcCharacter.Id);
			}
			NavMeshHit _hit;
			if (flag && NavigationHelper.CascadingSamplePosition(GameEntity.Position, out _hit, NavMeshLayers.WalkableLayerMaskPreset))
			{
				GameEntity.Position = _hit.position;
				if (navMeshAgent.hasPath)
				{
					Speed = navMeshAgent.speed;
					Destination = navMeshAgent.destination;
				}
			}
			navMeshAgent.Stop();
			navMeshAgent.ResetPath();
		}

		public override void DoTurnUpdate(int _deltaTurns)
		{
			if (HasPath)
			{
				Vector3 _position = GameEntity.Position;
				float _rotationXZ = GameEntity.Rotation.eulerAngles.y;
				bool flag = false;
				float _deltaTime = Mathf.Min(TimeSinceTurnUpdate, (float)TurnUpdateIntervalTurns * 0.1f);
				bool flag2;
				do
				{
					flag2 = Helper.InterpolatePosAndRotation(Speed, SteeringTarget, ref _position, ref _rotationXZ, ref _deltaTime);
					if (flag2)
					{
						m_targetIndex++;
						if (m_targetIndex >= m_corners.Count)
						{
							ResetPath();
							break;
						}
					}
					else
					{
						flag = true;
					}
				}
				while (flag2);
				if (flag)
				{
					FrameTargetPosition = _position;
					FrameTargetRotationXZ = _rotationXZ;
				}
			}
			CalcNumberTurnsBetweenUpdates();
			base.DoTurnUpdate(_deltaTurns);
		}

		public override void DoFrameUpdate(float _deltaTime)
		{
			if (IsFrameUpdateActive)
			{
				Vector3 _position = GameEntity.Position;
				float _rotationXZ = GameEntity.Rotation.eulerAngles.y;
				Helper.InterpolatePosAndRotation(Speed, FrameTargetPosition, FrameTargetRotationXZ, ref _position, ref _rotationXZ, ref _deltaTime);
				GameEntity.Position = _position;
				GameEntity.Rotation = Quaternion.Euler(0f, _rotationXZ, 0f);
			}
			base.DoFrameUpdate(_deltaTime);
		}

		private void CalcNumberTurnsBetweenUpdates()
		{
			if (HasPath)
			{
				m_TurnUpdateIntervalTurns = (int)(6f + GameEntity.DistanceBucket * 60f / Speed);
			}
			else
			{
				m_TurnUpdateIntervalTurns = 60;
			}
		}

		private void ResetFrameTargets()
		{
			FrameTargetPosition = GameEntity.Position;
			FrameTargetRotationXZ = GameEntity.Rotation.eulerAngles.y;
		}
	}
}
