using System;
using System.Collections.Generic;
using Assets.Scripts.Mission.Logic.Entities;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.Crowd
{
	public class CrowdAvoidance : MonoBehaviour
	{
		public struct Neighbor
		{
			public CrowdBehavior CrowdBehavior;

			public NavMeshAgent Agent;

			public Vector3 Position;

			public float Radius;

			public Vector3 Velocity;

			public float AvoidanceWeight;
		}

		public static bool EnableCrowdAvoidance = true;

		public static float NeighborSearchRadius = 5f;

		public static float VelocityDamping;

		public static float RotationDamping;

		public static float SpeedDamping = 0.1f;

		public static float FeedbackDamping = 0.1f;

		public static float PlayerVelocityAmount = 0.5f;

		public static float AgentRadius = 0.5f;

		public static float CollisionTimeWeight = 10f;

		public static float CorrectionFactor = 1.25f;

		public static float SeparateVelocity = 2f;

		[HideInInspector]
		public CrowdBehavior CrowdBehavior;

		public BBSimpleList<Neighbor> Neighbors = new BBSimpleList<Neighbor>();

		public BBList<Crowd.CrowdNodeData> NeighborNodes = new BBList<Crowd.CrowdNodeData>();

		public Vector3 TargetVelocity;

		public float TargetWeight;

		public bool AvoidanceWasApplied;

		public void Reset()
		{
			Neighbors.Clear();
			NeighborNodes.Clear();
			AvoidanceWasApplied = false;
		}

		public void OnDrawGizmosSelected()
		{
			if (base.enabled && EnableCrowdAvoidance)
			{
				Gizmos.color = Color.cyan * 0.5f;
				for (int i = 0; i < Neighbors.Count; i++)
				{
					Neighbor neighbor = Neighbors[i];
					Gizmos.DrawLine(CrowdBehavior.Position, neighbor.Position);
				}
				if (!(TargetWeight > 0f))
				{
				}
			}
		}

		public void ApplyAvoidance(ref Vector3 _velocityRef, ref Quaternion _rotationRef, float _dt)
		{
			AvoidanceWasApplied = true;
			float t = Damping.CalculateFactor(VelocityDamping, TargetWeight, _dt);
			_velocityRef = Vector3.Lerp(_velocityRef, TargetVelocity, t);
			if (_velocityRef.sqrMagnitude > 0.05f)
			{
				float t2 = Damping.CalculateFactor(RotationDamping, TargetWeight, _dt);
				_rotationRef = Quaternion.Slerp(_rotationRef, Quaternion.LookRotation(_velocityRef), t2);
			}
		}

		public void UpdateAvoidance()
		{
			AvoidanceWasApplied = false;
			if (LogicMain.IsRunning)
			{
				float agentRadius = AgentRadius;
				Vector2 position = FlattenXZ(CrowdBehavior.Position);
				Vector2 velocity = FlattenXZ(CrowdBehavior.TargetVelocity);
				TargetWeight = 0f;
				TargetVelocity = Vector3.zero;
				UpdateNeighbors();
				BBSimpleList<Neighbor> neighbors = Neighbors;
				for (int i = 0; i < neighbors.Count; i++)
				{
					Neighbor neighbor = neighbors[i];
					Vector2 _targetVelocity;
					float _targetWeight;
					ComputeRVO(position, velocity, agentRadius, FlattenXZ(neighbor.Position), FlattenXZ(neighbor.Agent.velocity), neighbor.Radius, neighbor.AvoidanceWeight, out _targetVelocity, out _targetWeight);
					TargetVelocity += ExpandXZ(_targetVelocity);
					TargetWeight += _targetWeight;
				}
				if (TargetWeight > 0f)
				{
					TargetVelocity /= TargetWeight;
					TargetWeight = 1f;
				}
			}
		}

		private void ComputeRVO(Vector2 _position, Vector2 _velocity, float _radius, Vector2 _neighborPosition, Vector2 _neighborVelocity, float _neighborRadius, float _correctionFactor, out Vector2 _targetVelocity, out float _targetWeight)
		{
			Vector2 from = _neighborPosition - _position;
			float magnitude = from.magnitude;
			Vector2 vector = _velocity - _neighborVelocity;
			Vector2 normalized = from.normalized;
			float num = _radius + _neighborRadius;
			_targetVelocity = Vector2.zero;
			_targetWeight = 0f;
			float num2 = Vector2.Angle(from, _velocity);
			float num3 = Mathf.Clamp01(Mathf.Lerp(1f, 0f, num2 / 90f));
			_correctionFactor *= num3;
			if (num > magnitude)
			{
				float t = (num - magnitude) / num;
				_targetWeight = _correctionFactor;
				float num4 = Vector2.Dot(vector, -normalized);
				Vector2 vector2 = vector - normalized * Mathf.Lerp(0f, Mathf.Max(SeparateVelocity - num4, 0f), t);
				_targetVelocity = Vector2.Lerp(_velocity, vector2 + _neighborVelocity, _correctionFactor) * _targetWeight;
				return;
			}
			float num5 = (magnitude - num) / Vector2.Dot(normalized, vector);
			if (!(num5 <= 0f))
			{
				float num6 = (float)Math.PI / 2f - Mathf.Acos(num / magnitude);
				float num7 = Mathf.Cos(num6);
				float num8 = Mathf.Sin(num6);
				Vector2 from2 = new Vector2(normalized.x * num7 - normalized.y * num8, normalized.x * num8 + normalized.y * num7);
				Vector2 from3 = new Vector2(normalized.x * num7 + normalized.y * num8, (0f - normalized.x) * num8 + normalized.y * num7);
				float num9 = Vector2.Angle(from2, vector) * ((float)Math.PI / 180f);
				float num10 = Vector2.Angle(from3, vector) * ((float)Math.PI / 180f);
				if (num9 < num6 * 2f && num10 < num6 * 2f)
				{
					Vector2 vector3 = ((!(num9 < num10)) ? (Rotate(vector, num10) * Mathf.Cos(num10)) : (Rotate(vector, 0f - num9) * Mathf.Cos(num9)));
					_targetWeight = 1f / (1f + num5 * CollisionTimeWeight) * _correctionFactor;
					_targetVelocity = Vector2.Lerp(_velocity, vector3 + _neighborVelocity, _correctionFactor * CorrectionFactor) * _targetWeight;
				}
			}
		}

		public void UpdateNodeCache()
		{
			BBList<Crowd.CrowdNodeData> neighborNodes = NeighborNodes;
			neighborNodes.Clear();
			Crowd.CrowdNodeData nodeData = CrowdBehavior.CrowdFollowCrowdGraph.NodeData;
			if (nodeData == null)
			{
				return;
			}
			float neighborSearchRadius = NeighborSearchRadius;
			float num = neighborSearchRadius * neighborSearchRadius;
			float num2 = nodeData.Node.Radius * nodeData.Node.Radius;
			Vector3 centerPosition = nodeData.Node.CenterPosition;
			BBList<Crowd.CrowdNodeData> crowdNodes = Crowd.Instance.CrowdNodes;
			HashSet<Crowd.CrowdNodeData> nodeQuerySet = CrowdStaticCache.NodeQuerySet;
			nodeQuerySet.Clear();
			BBList<Crowd.CrowdNodeData> nodeQueryList = CrowdStaticCache.NodeQueryList;
			nodeQueryList.Clear();
			nodeQueryList.Add(nodeData);
			while (nodeQueryList.Count > 0)
			{
				Crowd.CrowdNodeData crowdNodeData = nodeQueryList[nodeQueryList.Count - 1];
				neighborNodes.Add(crowdNodeData);
				List<int> neighbors = crowdNodeData.Node.Neighbors;
				nodeQueryList.RemoveAt(nodeQueryList.Count - 1);
				CrowdStaticCache.NodeQuerySet.Add(crowdNodeData);
				for (int i = 0; i < neighbors.Count; i++)
				{
					int index = neighbors[i];
					Crowd.CrowdNodeData crowdNodeData2 = crowdNodes[index];
					CrowdGraph.CrowdNode node = crowdNodeData2.Node;
					if (!CrowdStaticCache.NodeQuerySet.Contains(crowdNodeData2) && (node.CenterPosition - centerPosition).sqrMagnitude - node.Radius * node.Radius - num2 <= num)
					{
						nodeQueryList.Add(crowdNodeData2);
					}
				}
			}
		}

		public void UpdateNeighbors()
		{
			BBSimpleList<Neighbor> neighbors = Neighbors;
			neighbors.Clear();
			float neighborSearchRadius = NeighborSearchRadius;
			BBList<Crowd.CrowdNodeData> neighborNodes = NeighborNodes;
			Vector3 position = CrowdBehavior.Position;
			float num = neighborSearchRadius * neighborSearchRadius;
			for (int i = 0; i < neighborNodes.Count; i++)
			{
				Crowd.CrowdNodeData crowdNodeData = neighborNodes[i];
				BBList<CrowdBehavior> behaviors = crowdNodeData.Behaviors;
				for (int j = 0; j < behaviors.Count; j++)
				{
					CrowdBehavior crowdBehavior = behaviors[j];
					if (!(crowdBehavior == CrowdBehavior) && (crowdBehavior.Position - position).sqrMagnitude <= num)
					{
						neighbors.Add(new Neighbor
						{
							CrowdBehavior = crowdBehavior,
							Agent = crowdBehavior.Agent,
							Position = crowdBehavior.Position,
							Velocity = crowdBehavior.Agent.velocity,
							Radius = AgentRadius,
							AvoidanceWeight = ((!crowdBehavior.CrowdAvoidance.AvoidanceWasApplied) ? 1f : 0.5f)
						});
					}
				}
			}
			PlayerCharacter playerEntity = LogicMain.PlayerEntity;
			if (playerEntity != null && (playerEntity.Position - position).sqrMagnitude <= num)
			{
				neighbors.Add(new Neighbor
				{
					CrowdBehavior = null,
					Agent = playerEntity.UnityComponentCache.NavMeshAgent,
					Position = playerEntity.Position,
					Velocity = AcierCameraController.AgentDataFilter.FlatVelocity * PlayerVelocityAmount,
					Radius = AgentRadius,
					AvoidanceWeight = 1f
				});
			}
			List<GameEntity> avoidanceAgents = Crowd.Instance.AvoidanceAgents;
			for (int k = 0; k < avoidanceAgents.Count; k++)
			{
				GameEntity gameEntity = avoidanceAgents[k];
				if (gameEntity != null && gameEntity.IsValid && gameEntity.IsAttached && gameEntity.UnityComponentCache.NavMeshAgent != null && (gameEntity.Position - position).sqrMagnitude <= num)
				{
					neighbors.Add(new Neighbor
					{
						CrowdBehavior = null,
						Agent = gameEntity.UnityComponentCache.NavMeshAgent,
						Position = gameEntity.Position,
						Velocity = gameEntity.UnityComponentCache.NavMeshAgent.velocity,
						Radius = AgentRadius,
						AvoidanceWeight = 1f
					});
				}
			}
		}

		private Vector2 FlattenXZ(Vector3 _v)
		{
			return new Vector2(_v.x, _v.z);
		}

		private Vector3 ExpandXZ(Vector2 _v)
		{
			return new Vector3(_v.x, 0f, _v.y);
		}

		private Vector2 Rotate(Vector2 _v, float _angleRad)
		{
			float num = Mathf.Cos(_angleRad);
			float num2 = Mathf.Sin(_angleRad);
			return new Vector2(_v.x * num + _v.y * num2, (0f - _v.x) * num2 + _v.y * num);
		}
	}
}
