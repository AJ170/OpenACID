using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Motion;
using SharedBaseLib.System;
using UnityEngine;
using UnityEngine.AI;

public class AnimationTaskRunPassOverDown : AnimationTaskBase, IAnimation, IParkourAnimation, IAnimatorEventListener
{
	public class Parameters : AnimationTaskParameters
	{
		public OffMeshLinkEntity OffMeshLink { get; set; }

		public Parameters()
			: base(AnimationTaskType.RunPassOverDown)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.RunPassOverDown;

	public static float LandOffset = 0.5f;

	public static float TurnMinAngle = 90f;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private OffMeshLinkEntity m_link;

	private Vector3 m_startPosition;

	private Vector3 m_endPosition;

	private EdgeObject m_edge;

	private bool m_passedEdge;

	private bool m_askMatch;

	private Transform m_matchTransform;

	private Vector3 m_matchPoint = Vector3.zero;

	private AvatarTarget m_matchBodyPart = AvatarTarget.LeftHand;

	private static MatchTargetWeightMask s_weightMask = new MatchTargetWeightMask(new Vector3(1f, 1f, 1f), 0f);

	private HumanBodyBones m_matchBone;

	private bool m_waitForTurn;

	private NpcTurnDirection m_turnDirection;

	private float m_turnCorrectionAngle;

	private Vector3 m_faceDirection;

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.RunPassOverDown;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public float Height { get; private set; }

	public Vector3 Destination
	{
		get
		{
			return m_endPosition;
		}
	}

	public AnimationTaskRunPassOverDown()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskRunPassOverDown(Parameters _parameters)
		: base(_parameters)
	{
		m_link = _parameters.OffMeshLink;
		Height = _parameters.OffMeshLink.StartPosition.y - _parameters.OffMeshLink.EndPosition.y;
	}

	public override void OnEnter()
	{
		base.OnEnter();
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		AnimationController.ApplyRootMotion = true;
		m_startPosition = AnimationController.NextPosition;
		NavMeshHit hit;
		if (NavMesh.SamplePosition(m_link.EndPosition + m_link.Rotation * Vector3.forward * LandOffset, out hit, LandOffset, NavMeshLayers.WalkableLayerMaskPreset))
		{
			m_endPosition = hit.position;
		}
		else
		{
			m_endPosition = m_link.EndPosition;
			Debug.LogWarning("Could not find proper NavMesh position to land, using link end position");
		}
		if (m_link.ClimbRoute == null || m_link.ClimbRoute.ClimbRouteEntries.Count < 1)
		{
			base.IsFinished = true;
			return;
		}
		AnimationController.GetComponent<AnimatorDataController>().RegisterEventListener(this);
		m_faceDirection = m_endPosition - m_startPosition;
		m_faceDirection.y = 0f;
		m_faceDirection.Normalize();
		float num = Vector3.Angle(m_faceDirection, AnimationController.transform.forward) * Mathf.Sign(Vector3.Dot(m_faceDirection, AnimationController.transform.right));
		if (Mathf.Abs(num) > TurnMinAngle)
		{
			float currentSpeed = AnimationController.Animator.GetCurrentSpeed();
			float speed = Mathf.Min(currentSpeed, 0.4f);
			AnimationController.Animator.SetCurrentSpeed(speed);
			m_turnDirection = AnimationHelper.DetermineNpcTurnDirection(0f, num);
			m_turnCorrectionAngle = num - AnimationHelper.NpcTurnDirectionToAngle(m_turnDirection);
			AnimationController.Animator.SetCurrentNpcTurnDirection(m_turnDirection);
			AnimationController.Animator.SetCurrentMovementState(MovementState.Turn);
			m_waitForTurn = true;
		}
		else
		{
			StartRunPassOverDown();
		}
	}

	private void StartRunPassOverDown()
	{
		ClimbRouteEntry climbRouteEntry = m_link.ClimbRoute.ClimbRouteEntries[0];
		float value = climbRouteEntry.ClosestPoint.y - m_startPosition.y;
		value = Mathf.Clamp(value, 0.01f, 1.49f);
		m_edge = climbRouteEntry.Edge;
		Vector3 vector = m_endPosition - climbRouteEntry.ClosestPoint;
		float distance = 0f - vector.y;
		vector.y = 0f;
		float magnitude = vector.magnitude;
		AnimationController.Animator.SetCurrentJumpDistanceY(distance);
		AnimationController.Animator.SetCurrentJumpDistanceZ(magnitude);
		AnimationController.Animator.SetCurrentClimbDirection(eDirection.Up);
		AnimationController.Animator.SetCurrentRoofEdge(true);
		AnimationController.Animator.SetCurrentLinkType(eLinkType.RunPassOverDown);
		AnimationController.Animator.SetCurrentEdgeDistance(value);
		AnimationController.Animator.SetCurrentGeneralState(GeneralState.OnLink);
		Vector3 vector2 = m_endPosition - m_startPosition;
		vector2.y = 0f;
		float distance2 = ParcourHelper.ClearDistanceForward(m_endPosition, vector2.normalized, 2f);
		AnimationController.Animator.SetCurrentEdgeDistanceZ(distance2);
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.Animator.SetCurrentRoofEdge(false);
		AnimationController.Animator.SetCurrentClimbDirection(eDirection.Invalid);
		AnimationController.Animator.SetCurrentLinkType(eLinkType.Invalid);
		AnimationController.Animator.SetCurrentEdgeDistance(AnimationController.Invalid);
		AnimationController.Animator.SetCurrentGeneralState(GeneralState.Default);
		NavMeshHit hit;
		if (NavMesh.SamplePosition(AnimationController.NextPosition, out hit, 20f, AnimationController.Agent.areaMask))
		{
			AnimationController.Agent.Warp(hit.position);
		}
	}

	public override void OnDispose()
	{
		base.OnDispose();
		AnimationController.GetComponent<AnimatorDataController>().UnregisterEventListener(this);
	}

	public bool OnAnimatorEvent(AnimatorEvent _event)
	{
		switch (_event.Type)
		{
		case AnimatorEventType.TaskEnd:
			base.IsFinished = true;
			break;
		case AnimatorEventType.SwitchToLeftHand:
			m_matchBodyPart = AvatarTarget.LeftHand;
			m_matchBone = HumanBodyBones.LeftHand;
			break;
		case AnimatorEventType.SwitchToTwoHandsFromLeft:
			m_matchBodyPart = AvatarTarget.LeftHand;
			m_matchBone = HumanBodyBones.LeftHand;
			break;
		case AnimatorEventType.SwitchToRightHand:
			m_matchBodyPart = AvatarTarget.RightHand;
			m_matchBone = HumanBodyBones.RightHand;
			break;
		case AnimatorEventType.SwitchToTwoHandsFromRight:
			m_matchBodyPart = AvatarTarget.RightHand;
			m_matchBone = HumanBodyBones.RightHand;
			break;
		case AnimatorEventType.SwitchToLeftFoot:
			m_matchBodyPart = AvatarTarget.LeftFoot;
			m_matchBone = HumanBodyBones.LeftFoot;
			break;
		case AnimatorEventType.SwitchToRightFoot:
			m_matchBodyPart = AvatarTarget.RightFoot;
			m_matchBone = HumanBodyBones.RightFoot;
			break;
		case AnimatorEventType.MatchStart:
			if (!m_passedEdge)
			{
				Transform transform = AnimationController.Animator.GetBoneTransform(m_matchBone);
				if (transform == null)
				{
					transform = AnimationController.transform;
				}
				m_matchPoint = m_edge.ClosestPointCharacterFoot(transform.position, AnimationController.NextPosition);
			}
			else
			{
				m_matchPoint = AnimationController.NextPosition + AnimationController.NextRotation * Vector3.forward * 1f;
				m_matchPoint.y = m_endPosition.y;
				NavMeshHit hit;
				if (NavMesh.SamplePosition(m_matchPoint, out hit, 0.5f, NavMeshLayers.WalkableLayerMaskPreset))
				{
					m_matchPoint = hit.position;
				}
				else
				{
					m_matchPoint = m_endPosition;
				}
				PlayerHelper.Entity.SetLastFallHeight(Height);
			}
			m_askMatch = true;
			TryStartMatching();
			break;
		case AnimatorEventType.MatchEnd:
			m_passedEdge = true;
			break;
		case AnimatorEventType.TurnEnd:
			if (m_waitForTurn)
			{
				StartRunPassOverDown();
			}
			m_waitForTurn = false;
			break;
		}
		return false;
	}

	private void TryStartMatching()
	{
		float _matchStart;
		float _matchEnd;
		bool matchTargetData = AnimationHelper.GetMatchTargetData(AnimationController, out _matchStart, out _matchEnd, 0);
		AnimatorStateInfo currentAnimatorStateInfo = AnimationController.Animator.GetCurrentAnimatorStateInfo(0);
		m_matchTransform = AnimationController.Agent.transform.FindChildRecursive(m_matchBodyPart.ToString());
		if (matchTargetData)
		{
			AnimationController.MatchTarget(m_matchPoint, Quaternion.identity, m_matchBodyPart, s_weightMask, currentAnimatorStateInfo.normalizedTime, _matchEnd);
			m_askMatch = false;
		}
	}

	public override bool CanCancel()
	{
		return false;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (m_waitForTurn)
		{
			float num = Damping.Damp(m_turnCorrectionAngle, 0f, 0.2f, 1f);
			AnimationController.NextRotation = Quaternion.Euler(0f, m_turnCorrectionAngle - num, 0f) * AnimationController.NextRotation;
			m_turnCorrectionAngle = num;
			AnimationHelper.StayOnNavMesh(AnimationController.EntityId);
			return;
		}
		AnimationController.FaceDirection(m_link.Rotation * Vector3.forward);
		if (m_askMatch)
		{
			TryStartMatching();
		}
		if (AnimationController.IsMatchingTarget && m_matchTransform != null)
		{
			Vector3 position = m_matchTransform.position;
			Vector3 vector = m_matchPoint - position;
		}
	}
}
