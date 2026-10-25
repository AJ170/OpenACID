using System;
using System.Collections.Generic;
using SharedBaseLib.Core;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Crowd
{
	public class CrowdFollowCrowdGraph : MonoBehaviour
	{
		public struct NodePathStep
		{
			public CrowdGraph.CrowdNode.Flow Flow;

			public Crowd.CrowdNodeData NodeData;
		}

		public static float FlowFollowDamping = 2f;

		public static float FlowAnticipationTime = 1f;

		public static float FlowCenterDamping = 1f;

		public static float FlowNextDamping = 0.6f;

		public static float EdgeAvoidDamping = 0.6f;

		public static float EdgeCenterDamping = 1.5f;

		public static float PushDirectionDamping = 0.4f;

		public static int NodePathLength = 4;

		public static float CutNodePathWeightFactor = 0.95f;

		public static float NodePathMaxWeightFactorDeltaIgnored = 0.1f;

		[HideInInspector]
		public CrowdBehavior CrowdBehavior;

		public CrowdGraph.CrowdNode.Flow Flow;

		public Vector3 FlowDirection;

		public BBSimpleList<NodePathStep> NodePath = new BBSimpleList<NodePathStep>();

		private Crowd.CrowdNodeData m_nodeData;

		public Crowd.CrowdNodeData NodeData
		{
			get
			{
				return m_nodeData;
			}
			set
			{
				if (m_nodeData == value)
				{
					return;
				}
				if (m_nodeData != null)
				{
					BBList<CrowdBehavior> behaviors = m_nodeData.Behaviors;
					behaviors.SwapRemove(CrowdBehavior);
				}
				m_nodeData = value;
				if (m_nodeData != null)
				{
					m_nodeData.Behaviors.Add(CrowdBehavior);
					if (CrowdAvoidance.EnableCrowdAvoidance)
					{
						CrowdBehavior.CrowdAvoidance.UpdateNodeCache();
					}
				}
			}
		}

		public void OnDrawGizmosSelected()
		{
			Vector3 position = CrowdBehavior.Position;
			NavMeshAgent agent = CrowdBehavior.Agent;
			if (Flow != null)
			{
			}
			if (NodeData != null)
			{
			}
			Vector3 vector = position;
			for (int i = 0; i < NodePath.Count; i++)
			{
				vector = NodePath[i].NodeData.Node.CenterPosition;
			}
		}

		public void Reset(Crowd.CrowdNodeData node = null, CrowdGraph.CrowdNode.Flow flow = null)
		{
			NodeData = null;
			Flow = null;
			NodePath.Clear();
			FlowDirection = ((Flow == null) ? Vector3.zero : Flow.Direction);
		}

		public void FollowCrowdGraphInit()
		{
			NavMeshAgent agent = CrowdBehavior.Agent;
			agent.ResetPath();
			NodeData = null;
			Flow = null;
			CrowdBehavior.CrowdDetectIsStuck.Reset(CrowdBehavior.Position);
			CrowdBehavior.RandomizeAvoidancePriority();
			agent.speed = CrowdBehavior.BaseSpeed;
			NodePath.Clear();
		}

		public bool FollowCrowdGraphUpdate()
		{
			UpdateClosestNode();
			if (NodeData == null)
			{
				return false;
			}
			UpdateNodePath();
			FollowCrowdGraph();
			return true;
		}

		public void UpdateNodePath()
		{
			BBSimpleList<NodePathStep> nodePath = NodePath;
			if (NodeData == null || Flow == null)
			{
				return;
			}
			BBList<Crowd.CrowdNodeData> crowdNodes = Crowd.Instance.CrowdNodes;
			Crowd.CrowdNodeData crowdNodeData = NodeData;
			CrowdGraph.CrowdNode.Flow flow = Flow;
			for (int i = 0; i < nodePath.Count; i++)
			{
				NodePathStep nodePathStep = nodePath[i];
				if (nodePathStep.NodeData.Node.DynamicWeight < crowdNodeData.Node.DynamicWeight * CutNodePathWeightFactor || (!crowdNodeData.Flee && nodePathStep.NodeData.Flee))
				{
					for (int num = nodePath.Count - 1; num >= i; num--)
					{
						nodePath.RemoveAt(num);
					}
					break;
				}
			}
			if (nodePath.Count > 0)
			{
				NodePathStep nodePathStep2 = nodePath[nodePath.Count - 1];
				crowdNodeData = nodePathStep2.NodeData;
				flow = nodePathStep2.Flow;
			}
			while (nodePath.Count < NodePathLength)
			{
				Crowd.CrowdNodeData crowdNodeData2 = null;
				float num2 = 0f;
				CrowdGraph.CrowdNode.Flow flow2 = null;
				List<int> neighbors = crowdNodeData.Node.Neighbors;
				float num3 = float.MaxValue;
				for (int j = 0; j < neighbors.Count; j++)
				{
					int index = neighbors[j];
					Crowd.CrowdNodeData crowdNodeData3 = crowdNodes[index];
					bool flag = false;
					for (int k = 0; k < nodePath.Count; k++)
					{
						if (crowdNodeData3 == nodePath[k].NodeData || crowdNodeData3 == NodeData)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						float num4 = crowdNodeData3.Node.DynamicWeight / Math.Max(crowdNodeData.Node.DynamicWeight, 0.0001f);
						float num5 = Mathf.Abs(Vector3.Angle(flow.Direction, crowdNodeData3.Node.CenterPosition - crowdNodeData.Node.CenterPosition));
						if (num4 > num2 || (num2 - num4 <= NodePathMaxWeightFactorDeltaIgnored && num5 < num3))
						{
							num3 = num5;
							num2 = Mathf.Max(num4, num2);
							crowdNodeData2 = crowdNodeData3;
						}
					}
				}
				if (crowdNodeData2 == null)
				{
					break;
				}
				List<int> neighbors2 = crowdNodeData2.Node.Neighbors;
				List<CrowdGraph.CrowdNode.Flow> flows = crowdNodeData2.Node.Flows;
				BBSimpleList<double> flowWeights = CrowdStaticCache.FlowWeights;
				flowWeights.Clear();
				double num6 = 0.0;
				for (int l = 0; l < flows.Count; l++)
				{
					CrowdGraph.CrowdNode.Flow flow3 = flows[l];
					if (flow3.NormalizedWeight < double.Epsilon)
					{
						flowWeights.Add(0.0);
						continue;
					}
					Crowd.CrowdNodeData crowdNodeData4 = null;
					float num7 = float.MaxValue;
					for (int m = 0; m < neighbors2.Count; m++)
					{
						int index2 = neighbors2[m];
						Crowd.CrowdNodeData crowdNodeData5 = crowdNodes[index2];
						if (crowdNodeData5 != crowdNodeData2)
						{
							float num8 = Mathf.Abs(Vector3.Angle(flow3.Direction, crowdNodeData5.Node.CenterPosition - crowdNodeData2.Node.CenterPosition));
							if (num8 < num7)
							{
								num7 = num8;
								crowdNodeData4 = crowdNodeData5;
							}
						}
					}
					double num9 = ((crowdNodeData4 == null) ? 0f : crowdNodeData4.Node.DynamicWeight);
					double num10 = crowdNodeData2.Node.DynamicWeight;
					double val = num9 - num10;
					double normalizedWeight = flow3.NormalizedWeight;
					double num11 = 1f + Math.Max(Vector3.Dot(flow3.Direction, flow.Direction), 0f) * CrowdSettings.DirectionResponse;
					double num12 = 1.0 + Math.Max(0.0, val) * (double)CrowdSettings.DynamicWeightResponse;
					normalizedWeight *= num11 * num12;
					num6 += normalizedWeight;
					flowWeights.Add(num6);
				}
				double num13 = (double)UnityEngine.Random.value * num6;
				for (int n = 0; n < flows.Count; n++)
				{
					if (num13 <= flowWeights[n] && flow2 == null)
					{
						flow2 = flows[n];
						break;
					}
				}
				if (flow2 == null)
				{
					break;
				}
				NodePathStep nodePathStep3 = default(NodePathStep);
				nodePathStep3.Flow = flow2;
				nodePathStep3.NodeData = crowdNodeData2;
				NodePathStep value = nodePathStep3;
				nodePath.Add(value);
				flow = flow2;
				crowdNodeData = crowdNodeData2;
			}
		}

		private void FollowCrowdGraph()
		{
			Crowd.CrowdNodeData crowdNodeData = null;
			if (NodePath.Count > 0)
			{
				crowdNodeData = NodePath[0].NodeData;
			}
			CrowdGraph.CrowdNode crowdNode = ((crowdNodeData == null) ? null : crowdNodeData.Node);
			if (Flow == null || NodeData == null)
			{
				return;
			}
			Crowd.CrowdNodeData nodeData = NodeData;
			CrowdGraph.CrowdNode node = nodeData.Node;
			CrowdGraph.CrowdNode.Flow flow = Flow;
			Vector3 flowDirection = FlowDirection;
			Vector3 position = CrowdBehavior.Position;
			NavMeshAgent agent = CrowdBehavior.Agent;
			Animator animator = CrowdBehavior.Animator;
			float localDt = CrowdBehavior.LocalDt;
			Vector3 from = flowDirection;
			if (localDt > float.Epsilon)
			{
				float t = ((!nodeData.Flee) ? Mathf.Clamp01(Mathf.Abs(1f - node.DynamicWeight) * CrowdSettings.FleeResponse) : 1f);
				float to = Mathf.Lerp(CrowdBehavior.BaseSpeed, CrowdSettings.FleeSpeed, t);
				float num = Damping.Damp(agent.speed, to, CrowdSettings.FleeDamping, 1f, localDt);
				if (Mathf.Abs(num - agent.speed) > 0.01f)
				{
					agent.speed = num;
				}
			}
			from = Damping.Damp(from, flow.Direction, FlowFollowDamping, 1f, localDt);
			if (crowdNode != null)
			{
				from = Damping.Damp(from, (crowdNode.CenterPosition - node.CenterPosition).normalized, FlowNextDamping, 1f, localDt);
			}
			if (CrowdBehavior.CrowdReactToPush.ReactionNoticeable)
			{
				from = Damping.Damp(from, CrowdBehavior.CrowdReactToPush.ReactionDirection, PushDirectionDamping, 1f, localDt);
			}
			if (CrowdBehavior.HasNavMeshEdgeHit)
			{
				from = Damping.Damp(from, CrowdBehavior.NavMeshEdgeHit.normal, EdgeAvoidDamping, 1f, localDt);
				from = Damping.Damp(from, (node.CenterPosition - position).normalized, EdgeCenterDamping, 1f, localDt);
			}
			float num2 = Vector3.Dot(position + from * FlowAnticipationTime - node.CenterPosition, flowDirection);
			Vector3 vector = node.CenterPosition + from * num2;
			from = Damping.Damp(from, (vector - position).normalized, FlowCenterDamping, 1f, localDt);
			from.y = 0f;
			from.Normalize();
			if (from.sqrMagnitude < float.Epsilon)
			{
				from = CrowdBehavior.Rotation * Vector3.forward;
			}
			FlowDirection = from;
			CrowdBehavior.TargetRotation = Quaternion.LookRotation(from);
			Vector3 b = from * agent.speed;
			float t2 = Damping.CalculateFactor(CrowdSettings.VelocityDamping, 1f, localDt);
			CrowdBehavior.TargetVelocity = Vector3.Lerp(CrowdBehavior.TargetVelocity, b, t2);
			if (CrowdAvoidance.EnableCrowdAvoidance)
			{
				CrowdBehavior.CrowdAvoidance.ApplyAvoidance(ref CrowdBehavior.TargetVelocity, ref CrowdBehavior.TargetRotation, localDt);
				FlowDirection = Vector3.Lerp(FlowDirection, CrowdBehavior.TargetVelocity.normalized, Damping.CalculateFactor(CrowdAvoidance.FeedbackDamping, 1f, localDt));
				float speed = Mathf.Lerp(agent.speed, Mathf.Clamp(CrowdBehavior.TargetVelocity.magnitude, CrowdSettings.MinSpeed, CrowdSettings.FleeSpeed), Damping.CalculateFactor(CrowdAvoidance.SpeedDamping, 1f, localDt));
				agent.speed = speed;
			}
			Vector3 vector2 = Vector3.Lerp(agent.velocity, CrowdBehavior.TargetVelocity, t2);
			if ((agent.velocity - vector2).sqrMagnitude > float.Epsilon)
			{
				agent.velocity = vector2;
			}
			float currentSpeed = animator.GetCurrentSpeed();
			float b2 = Mathf.Min(agent.velocity.magnitude, agent.speed) / 6.27f;
			float num3 = Mathf.Lerp(currentSpeed, b2, Damping.CalculateFactor(CrowdSettings.AnimatorSpeedDamping, 1f, localDt));
			if (Mathf.Abs(currentSpeed - num3) > 0.0001f)
			{
				animator.SetCurrentSpeed(num3);
			}
		}

		public void UpdateClosestNode()
		{
			Vector3 position = CrowdBehavior.Position;
			BBList<Crowd.CrowdNodeData> crowdNodes = Crowd.Instance.CrowdNodes;
			Crowd.CrowdNodeData crowdNodeData = NodeData;
			CrowdGraph.CrowdNode.Flow flow = null;
			if (NodeData == null)
			{
				crowdNodeData = Crowd.Instance.GetClosestNode(position);
				NodePath.Clear();
			}
			else if (NodePath.Count > 0)
			{
				NodePathStep nodePathStep = NodePath[0];
				if (nodePathStep.NodeData == NodeData || (position - nodePathStep.NodeData.Node.CenterPosition).magnitude < (position - NodeData.Node.CenterPosition).magnitude)
				{
					crowdNodeData = nodePathStep.NodeData;
					flow = nodePathStep.Flow;
					NodePath.RemoveAt(0);
				}
			}
			else
			{
				CrowdGraph.CrowdNode node = NodeData.Node;
				float num = (position - node.CenterPosition).magnitude;
				List<int> neighbors = node.Neighbors;
				for (int i = 0; i < neighbors.Count; i++)
				{
					Crowd.CrowdNodeData crowdNodeData2 = crowdNodes[NodeData.Node.Neighbors[i]];
					CrowdGraph.CrowdNode node2 = crowdNodeData2.Node;
					float magnitude = (position - node2.CenterPosition).magnitude;
					NavMeshHit _hit;
					if (magnitude < num && !NavigationHelper.SafeRaycast(position, node2.CenterPosition, out _hit, CrowdBehavior.Agent.areaMask))
					{
						num = magnitude;
						crowdNodeData = crowdNodeData2;
					}
				}
			}
			if (crowdNodeData == null || (crowdNodeData == NodeData && Flow != null))
			{
				return;
			}
			if (flow == null)
			{
				BBSimpleList<double> flowWeights = CrowdStaticCache.FlowWeights;
				CrowdGraph.CrowdNode node3 = crowdNodeData.Node;
				List<CrowdGraph.CrowdNode.Flow> flows = node3.Flows;
				List<int> neighbors2 = node3.Neighbors;
				flowWeights.Clear();
				double num2 = 0.0;
				for (int j = 0; j < flows.Count; j++)
				{
					CrowdGraph.CrowdNode.Flow flow2 = flows[j];
					CrowdGraph.CrowdNode crowdNode = null;
					float num3 = float.MaxValue;
					for (int k = 0; k < neighbors2.Count; k++)
					{
						int index = neighbors2[k];
						Crowd.CrowdNodeData crowdNodeData3 = crowdNodes[index];
						if (crowdNodeData3 != crowdNodeData)
						{
							CrowdGraph.CrowdNode node4 = crowdNodeData3.Node;
							float num4 = Mathf.Abs(Vector3.Angle(flow2.Direction, node4.CenterPosition - node3.CenterPosition));
							if (num4 < num3)
							{
								num3 = num4;
								crowdNode = node4;
							}
						}
					}
					double num5 = ((crowdNode == null) ? 0f : crowdNode.DynamicWeight);
					double num6 = node3.DynamicWeight;
					double val = num5 - num6;
					double normalizedWeight = flow2.NormalizedWeight;
					double num7 = 1f + Math.Max(Vector3.Dot(flow2.Direction, CrowdBehavior.Rotation * Vector3.forward), 0f) * CrowdSettings.DirectionResponse;
					double num8 = 1.0 + Math.Max(0.0, val) * (double)CrowdSettings.DynamicWeightResponse;
					normalizedWeight *= num7 * num8;
					num2 += normalizedWeight;
					flowWeights.Add(num2);
				}
				double num9 = (double)UnityEngine.Random.value * num2;
				for (int l = 0; l < flows.Count; l++)
				{
					if (num9 <= flowWeights[l] && Flow == null)
					{
						flow = flows[l];
						break;
					}
				}
			}
			NodeData = crowdNodeData;
			Flow = flow;
		}
	}
}
