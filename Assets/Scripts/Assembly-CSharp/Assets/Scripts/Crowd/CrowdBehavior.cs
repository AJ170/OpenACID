using System.Collections.Generic;
using Assets.Scripts.Animation;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.AI.Helper;
using Assets.Scripts.Mission.AI.System;
using Database;
using Humanoids;
using SharedBaseLib.System;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Crowd
{
	public class CrowdBehavior : MonoBehaviour
	{
		public string BehaviorTreeXMLPath;

		public GameID GameID;

		public bool IsFemale;

		public Vector3 Position;

		public Quaternion Rotation;

		public Vector3 Velocity;

		public Vector3 TargetVelocity;

		public Quaternion TargetRotation;

		public float BaseSpeed;

		public NavMeshHit NavMeshEdgeHit;

		public bool HasNavMeshEdgeHit;

		[HideInInspector]
		public NavMeshAgent Agent;

		[HideInInspector]
		public Animator Animator;

		[HideInInspector]
		public Transform Transform;

		[HideInInspector]
		public ObjectFade ObjectFade;

		[HideInInspector]
		public CrowdAvoidance CrowdAvoidance;

		[HideInInspector]
		public CrowdReactToPush CrowdReactToPush;

		[HideInInspector]
		public CrowdLookAt CrowdLookAt;

		[HideInInspector]
		public CrowdDetectIsStuck CrowdDetectIsStuck;

		[HideInInspector]
		public CrowdRelevance CrowdRelevance;

		[HideInInspector]
		public CrowdCorrectPosition CrowdCorrectPosition;

		[HideInInspector]
		public CrowdReactToHotspot CrowdReactToHotspot;

		[HideInInspector]
		public CrowdFollowCrowdGraph CrowdFollowCrowdGraph;

		[HideInInspector]
		public CrowdFollowPath CrowdFollowPath;

		[HideInInspector]
		public CrowdStopAtStation CrowdStopAtStation;

		private LODGroup m_LODGroup;

		private List<Renderer> m_renderers = new List<Renderer>();

		private float m_lastAnimatorUpdateTime;

		private float m_lastUpdateTime;

		private float m_nextUpdateTime;

		private float m_nextAnimatorRandomUpdateTime;

		private float m_movementStateVelocity;

		public float LocalDt { get; private set; }

		public IBehaviorTreeHolder BehaviorTreeHolder { get; private set; }

		public CrowdBehaviorRuntimeData BehaviorRuntimeData { get; private set; }

		private void Start()
		{
			Initialize();
			Disable();
		}

		public void Initialize()
		{
			BehaviorRuntimeData = new CrowdBehaviorRuntimeData();
			BehaviorRuntimeData.Init(this);
			Transform = base.transform;
			Animator = GetComponent<Animator>();
			Animator.applyRootMotion = false;
			Agent = GetComponent<NavMeshAgent>();
			Agent.avoidancePriority = CrowdSettings.MaxAvoidancePriority;
			Agent.areaMask = NavMeshLayers.GroundSurfaceLayerMask;
			Agent.autoBraking = false;
			Agent.updatePosition = false;
			Agent.updateRotation = false;
			Agent.stoppingDistance = 0.25f;
			Agent.autoRepath = false;
			Agent.radius = CrowdSettings.AgentRadius;
			BehaviorTreeHolder = UbiCore.Utils.Singleton<BehaviorTreeService>.Instance.RequestBehaviorTreeHolder(BehaviorTreeXMLPath, BehaviorTreeCategory.Crowd);
			CrowdAvoidance = base.gameObject.AddComponentIfNotExist<CrowdAvoidance>();
			CrowdAvoidance.CrowdBehavior = this;
			CrowdReactToPush = base.gameObject.AddComponentIfNotExist<CrowdReactToPush>();
			CrowdReactToPush.Animator = Animator;
			CrowdReactToPush.CrowdBehavior = this;
			CrowdLookAt = base.gameObject.AddComponentIfNotExist<CrowdLookAt>();
			CrowdLookAt.Animator = Animator;
			CrowdDetectIsStuck = base.gameObject.AddComponentIfNotExist<CrowdDetectIsStuck>();
			CrowdRelevance = base.gameObject.AddComponentIfNotExist<CrowdRelevance>();
			CrowdRelevance.OnEnterCloseToCamera += OnEnterCloseToCamera;
			CrowdRelevance.OnLeaveCloseToCamera += OnLeaveCloseToCamera;
			CrowdCorrectPosition = base.gameObject.AddComponentIfNotExist<CrowdCorrectPosition>();
			CrowdCorrectPosition.Agent = Agent;
			CrowdStopAtStation = base.gameObject.AddComponentIfNotExist<CrowdStopAtStation>();
			CrowdStopAtStation.CrowdBehavior = this;
			CrowdReactToHotspot = base.gameObject.AddComponentIfNotExist<CrowdReactToHotspot>();
			CrowdReactToHotspot.CrowdBehavior = this;
			CrowdFollowCrowdGraph = base.gameObject.AddComponentIfNotExist<CrowdFollowCrowdGraph>();
			CrowdFollowCrowdGraph.CrowdBehavior = this;
			CrowdFollowPath = base.gameObject.AddComponentIfNotExist<CrowdFollowPath>();
			CrowdFollowPath.CrowdBehavior = this;
			m_lastAnimatorUpdateTime = LogicMain.CurrentTime;
			m_lastUpdateTime = LogicMain.CurrentTime;
			m_nextUpdateTime = LogicMain.CurrentTime;
			m_nextAnimatorRandomUpdateTime = LogicMain.CurrentTime;
			IsFemale = Animator.runtimeAnimatorController.name.Contains("female");
			m_LODGroup = base.gameObject.GetComponent<LODGroup>();
			m_renderers.Clear();
			SkinnedMeshRenderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				m_renderers.Add(componentsInChildren[i]);
			}
			MeshRenderer[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<MeshRenderer>();
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				m_renderers.Add(componentsInChildren2[j]);
			}
			Crowd.Instance.AddCrowdBehavior(this);
		}

		private void OnDestroy()
		{
			CrowdFollowCrowdGraph.NodeData = null;
			CrowdStopAtStation.LeaveStation();
			Crowd.Instance.RemoveCrowdBehavior(this);
		}

		public void Enable()
		{
			if (!base.enabled)
			{
				base.enabled = true;
				Agent.enabled = true;
				Rendering.ConfigureLightProbes(base.gameObject);
				Rendering.Instance.OnRegisterShadows(base.gameObject);
				if (m_LODGroup != null)
				{
					m_LODGroup.enabled = true;
				}
				for (int i = 0; i < m_renderers.Count; i++)
				{
					m_renderers[i].enabled = true;
				}
			}
		}

		public void Disable()
		{
			if (base.enabled)
			{
				base.enabled = false;
				Agent.enabled = false;
				if (m_LODGroup != null)
				{
					m_LODGroup.enabled = false;
				}
				for (int i = 0; i < m_renderers.Count; i++)
				{
					m_renderers[i].enabled = false;
				}
				Rendering.ConfigureLightProbes(base.gameObject);
				Rendering.Instance.OnUnregisterShadows(base.gameObject);
				CrowdAvoidance.Reset();
				CrowdRelevance.ResetCold();
				CrowdFollowCrowdGraph.Reset();
				CrowdStopAtStation.LeaveStation();
			}
		}

		public void Warp(Crowd.CrowdNodeData node, CrowdGraph.CrowdNode.Flow flow, Vector3 target)
		{
			Transform.localPosition = target;
			Position = target;
			Agent.Warp(target);
			CrowdFollowCrowdGraph.Reset(node, flow);
			CrowdCorrectPosition.Reset(target);
			float value = Random.value;
			BaseSpeed = Mathf.Lerp(CrowdSettings.MinSpeed, CrowdSettings.MaxSpeed, value);
			Agent.speed = BaseSpeed;
			Agent.acceleration = CrowdSettings.AgentAcceleration;
			Agent.angularSpeed = CrowdSettings.AgentAngularSpeed;
			TargetVelocity = Vector3.zero;
			Agent.velocity = Vector3.zero;
			CrowdReactToPush.ResetDelay();
			m_lastAnimatorUpdateTime = LogicMain.CurrentTime;
			m_lastUpdateTime = LogicMain.CurrentTime;
			m_nextUpdateTime = LogicMain.CurrentTime;
			m_nextAnimatorRandomUpdateTime = LogicMain.CurrentTime;
			CrowdDetectIsStuck.Reset(Position);
			CrowdRelevance.ResetHot();
			Quaternion rotation = ((flow == null) ? Quaternion.Euler(0f, 360f * Random.value, 0f) : Quaternion.LookRotation(flow.Direction));
			Transform.rotation = rotation;
			Rotation = rotation;
			CrowdStopAtStation.Reset();
			CrowdReactToHotspot.Reset();
			if (CrowdSettings.RandomizeOnWarp)
			{
				CharacterAppearance appearance = new CharacterAppearance();
				HumanoidDef item = Main.SafeGameDb.GetItem<HumanoidDef>(GameID);
				Singleton<HumanoidFactory>.Instance.RandomizeHead(item, appearance);
				Singleton<HumanoidFactory>.Instance.RandomizeColor(item, appearance, false);
				Singleton<HumanoidFactory>.Instance.AdjustAppearance(item, appearance, base.gameObject);
			}
			if (CrowdSettings.FadeCharacters)
			{
				ObjectFade = base.gameObject.AddComponentIfNotExist<ObjectFade>();
				ObjectFade.OnFadeDoneCallback = ObjectFadeCallbacks.OnFadeFinishedDestroySelf;
				ObjectFade.FadeIn();
			}
			Animator.Play(AnimationStateHashes.State_LowProfile_Idle_Footm.NameHash);
		}

		private void LateUpdate()
		{
			if (CrowdSettings.UpdatePeriod > 0f && LogicMain.CurrentTime < m_nextUpdateTime)
			{
				LocalDt = LogicMain.FrameTime;
				Agent.velocity = Damping.Damp(Agent.velocity, TargetVelocity, CrowdSettings.VelocityDamping, 1f, LocalDt);
				return;
			}
			LocalDt = Mathf.Max(LogicMain.CurrentTime - m_lastUpdateTime, LogicMain.FrameTime);
			m_lastUpdateTime = LogicMain.CurrentTime;
			m_nextUpdateTime = m_lastUpdateTime + CrowdSettings.UpdatePeriod * Random.Range(0.8f, 1.2f);
			HasNavMeshEdgeHit = NavigationHelper.SafeRaycast(Position, Position + TargetRotation * Vector3.forward * CrowdSettings.EdgeAnticipationDistance, out NavMeshEdgeHit, Agent.areaMask);
			if (CrowdAvoidance.EnableCrowdAvoidance)
			{
				CrowdAvoidance.UpdateAvoidance();
			}
			CrowdDetectIsStuck.UpdateIsStuck(Position);
			if (LogicMain.CurrentTime > m_nextAnimatorRandomUpdateTime)
			{
				Animator.SetFloat(AnimatorParameters.RandomHash, Random.value);
				m_nextAnimatorRandomUpdateTime = LogicMain.CurrentTime + 1f;
			}
			if (BehaviorTreeHolder != null)
			{
				BehaviorTreeHolder.UpdateTurn(BehaviorRuntimeData);
			}
			CrowdRelevance.UpdateRelevance(Position, LocalDt);
		}

		public void OnEnterCloseToCamera()
		{
			Animator.enabled = true;
			Agent.obstacleAvoidanceType = CrowdSettings.AvoidanceType;
		}

		public void OnLeaveCloseToCamera()
		{
			if (CrowdSettings.AnimatorLOD)
			{
				Animator.enabled = false;
			}
			if (CrowdSettings.AvoidanceLOD)
			{
				Agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
			}
		}

		private void OnAnimatorMove()
		{
			float currentTime = LogicMain.CurrentTime;
			NavMeshAgent agent = Agent;
			Animator animator = Animator;
			LocalDt = currentTime - m_lastAnimatorUpdateTime;
			m_lastAnimatorUpdateTime = currentTime;
			if (animator.GetCurrentReactionType() != ReactionType.Invalid)
			{
				animator.SetCurrentReactionType(ReactionType.Invalid);
			}
			if (LocalDt <= float.Epsilon)
			{
				return;
			}
			Quaternion rotation = Rotation;
			Quaternion currentRotation = rotation;
			Vector3 facingDirection = rotation * Vector3.forward;
			Vector3 position = Position;
			Vector3 targetPosition = position;
			Vector3 velocity = TargetVelocity;
			if (!CrowdSettings.IgnoreRootMotion)
			{
				velocity = Animator.deltaPosition / LocalDt;
			}
			if (CrowdReactToPush.EnablePushReaction)
			{
				CrowdReactToPush.ReactToPush(targetPosition, TargetVelocity, agent.velocity, facingDirection, agent.speed);
				if (CrowdReactToPush.ReactionNoticeable)
				{
					velocity.x = agent.velocity.x;
					velocity.z = agent.velocity.z;
				}
			}
			Velocity = velocity;
			targetPosition += Velocity * LocalDt;
			targetPosition = CrowdCorrectPosition.CorrectPositionUpdate(position, targetPosition, LocalDt);
			bool _turnAnimPlaying;
			currentRotation = UpdateTurning(currentRotation, TargetRotation, out _turnAnimPlaying);
			UpdateMovementState(_turnAnimPlaying);
			Transform.localPosition = targetPosition;
			Position = targetPosition;
			Transform.localRotation = currentRotation;
			Rotation = currentRotation;
		}

		private void OnAnimatorIK()
		{
			if (CrowdLookAt.EnableLookAt && CrowdRelevance.IsCloseToCamera)
			{
				CrowdLookAt.LookAtPlayer(Position, Rotation * Vector3.forward, LocalDt);
			}
			else
			{
				CrowdLookAt.ResetLookAt();
			}
		}

		private Quaternion UpdateTurning(Quaternion _currentRotation, Quaternion _targetRotation, out bool _turnAnimPlaying)
		{
			Animator animator = Animator;
			_turnAnimPlaying = false;
			_currentRotation = Animator.deltaRotation * _currentRotation;
			Vector3 to = _currentRotation * Vector3.forward;
			Vector3 vector = _targetRotation * Vector3.forward;
			float num = Vector3.Angle(vector, to) * Mathf.Sign(Vector3.Dot(vector, _currentRotation * Vector3.right));
			float magnitude = Agent.velocity.magnitude;
			if (Mathf.Abs(num) > 45f && magnitude < CrowdSettings.MaxSpeedToTurn)
			{
				if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash != AnimationStateHashes.State_NpcTurning.NameHash)
				{
					NpcTurnDirection npcTurnDirection = AnimationHelper.DetermineNpcTurnDirection(0f, num);
					if (npcTurnDirection != NpcTurnDirection.Invalid)
					{
						animator.SetCurrentNpcTurnDirection(npcTurnDirection);
						_turnAnimPlaying = true;
					}
				}
			}
			else
			{
				float num2 = Quaternion.Angle(_currentRotation, _targetRotation);
				float b = Mathf.Clamp01(CrowdSettings.MinRotationSpeed * LocalDt / num2);
				_currentRotation = Quaternion.Slerp(_currentRotation, _targetRotation, Mathf.Max(Damping.CalculateFactor(CrowdSettings.RotationDamping, 1f, LocalDt), b));
			}
			return _currentRotation;
		}

		private void UpdateMovementState(bool _turnAnimPlaying)
		{
			Animator animator = Animator;
			NavMeshAgent agent = Agent;
			m_movementStateVelocity = Damping.Damp(m_movementStateVelocity, agent.velocity.magnitude, CrowdSettings.MovementStateVelocityDamping, 1f, LocalDt);
			MovementState currentMovementState = animator.GetCurrentMovementState();
			MovementState movementState = currentMovementState;
			if (_turnAnimPlaying)
			{
				movementState = MovementState.Turn;
			}
			else if (CrowdStopAtStation.OnStation || m_movementStateVelocity < CrowdSettings.MaxSpeedToIdle)
			{
				movementState = MovementState.Idle;
			}
			else if (m_movementStateVelocity > CrowdSettings.MinSpeedToMove)
			{
				movementState = MovementState.Movement;
			}
			if (movementState != currentMovementState)
			{
				animator.SetCurrentMovementState(movementState);
				animator.SetCurrentIdleType(IdleType.Neutral);
			}
		}

		public bool SetDestinationToNextNode()
		{
			Agent.ResetPath();
			CrowdFollowCrowdGraph.UpdateClosestNode();
			CrowdFollowCrowdGraph.UpdateNodePath();
			if (CrowdFollowCrowdGraph.NodePath.Count <= 0)
			{
				return false;
			}
			NavMeshHit _hit;
			if (!NavigationHelper.CascadingSamplePosition(Position, out _hit, Agent.areaMask))
			{
				return false;
			}
			NavMeshPath navMeshPath = new NavMeshPath();
			NavMesh.CalculatePath(_hit.position, CrowdFollowCrowdGraph.NodePath[0].NodeData.Node.CenterPosition, Agent.areaMask, navMeshPath);
			if (navMeshPath.status == NavMeshPathStatus.PathComplete)
			{
				Agent.Warp(_hit.position);
				bool result = Agent.SetPath(navMeshPath);
				Agent.Resume();
				return result;
			}
			return false;
		}

		public void RandomizeAvoidancePriority()
		{
			Agent.avoidancePriority = Mathf.RoundToInt(Mathf.Lerp(CrowdSettings.MaxAvoidancePriority - 1, CrowdSettings.MinAvoidancePriority, Random.value));
		}
	}
}
