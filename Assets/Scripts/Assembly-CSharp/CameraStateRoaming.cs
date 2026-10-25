using AcierProtocol;
using Assets.Scripts.Mission.Camera;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.UI.LogicInterface;
using UnityEngine;

public class CameraStateRoaming : CameraStateAutoRotation
{
	[SerializeField]
	public RoamingValues m_roamingValues;

	[SerializeField]
	public ClimbValues m_autoRotateValuesWhileCimbing = new ClimbValues();

	protected Vector3 m_leadOffset;

	protected Vector3 m_referenceDirection;

	private float m_sprintVelocity;

	private RaycastValues m_climbRaycastValues;

	public override RaycastValues Values
	{
		get
		{
			return (CameraStateHelper.CachedAnimationTaskType != AnimationTaskType.Climb) ? m_raycastValues : m_climbRaycastValues;
		}
	}

	private float CurrentLeadDistance
	{
		get
		{
			if (AcierCameraController.Speed == EMovementSpeed.Sprint)
			{
				return m_roamingValues.sprintLeadingDistance;
			}
			if (AcierCameraController.Speed == EMovementSpeed.Walk)
			{
				return m_roamingValues.walkLeadingDistance;
			}
			return m_roamingValues.runLeadingDistance;
		}
	}

	public override bool SpherecastAvoidance
	{
		get
		{
			return true;
		}
	}

	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.Roaming;
		}
	}

	public override void CopyFrom(ICameraState _src)
	{
		base.CopyFrom(_src);
		CameraStateRoaming cameraStateRoaming = (CameraStateRoaming)_src;
		m_roamingValues = cameraStateRoaming.m_roamingValues;
	}

	public override void StartState(ICameraState _previous)
	{
		base.StartState(_previous);
		if (m_roamingValues == null)
		{
			m_roamingValues = AcierCameraController.Instance.GlobalValues.m_roamingValues;
		}
		m_climbRaycastValues = AcierCameraController.Instance.GlobalValues.m_climbRaycastValues;
		m_referenceDampedPosition = (m_referenceTargetPosition = AcierCameraController.AgentDataFilter.Position);
	}

	protected void GetTargets(out Vector3 basePosition, out Vector3 leadPosition, out Quaternion viewRotation)
	{
		NavMeshAgent navMeshAgent = AcierCameraController.NavMeshAgent;
		viewRotation = AcierCameraController.AgentDataFilter.Rotation;
		basePosition = AcierCameraController.AgentDataFilter.Position;
		leadPosition = basePosition + viewRotation * Vector3.forward;
		if (navMeshAgent.hasPath)
		{
			if (navMeshAgent.nextOffMeshLinkData.valid)
			{
				leadPosition = navMeshAgent.nextOffMeshLinkData.endPos;
			}
			else
			{
				NavMeshPath path = navMeshAgent.path;
				Vector3[] corners = path.corners;
				if (corners.Length > 0)
				{
					leadPosition = corners[corners.Length - 1];
				}
			}
		}
		Vector3 vector = leadPosition - basePosition;
		vector.y = 0f;
		leadPosition = basePosition + vector.normalized * CurrentLeadDistance;
	}

	protected override Vector3 CalculateCameraOffset()
	{
		Vector3 vector = base.CalculateCameraOffset();
		return vector + m_roamingValues.sprintCameraOffset * m_sprintVelocity;
	}

	protected override void OffsetRaycastStart(ref Vector3 _raycastStart)
	{
		Vector3 vector = ((CameraStateHelper.CachedAnimationTaskType != AnimationTaskType.Climb) ? m_orbitValues.raycastOffset : m_roamingValues.overrideRaycastOffsetWhileClimbing.raycastOffset);
		_raycastStart += Vector3.Cross(m_agentFacing, Vector3.up) * vector.x;
		_raycastStart += m_agentFacing * vector.z;
		_raycastStart += Vector3.up * vector.y;
	}

	protected float GetDampingFactor(float dt, float speed, float damping)
	{
		if (damping <= 0f)
		{
			return 1f;
		}
		float num = dt * speed;
		if (dt <= 0f)
		{
			return 0f;
		}
		return num / (num + damping);
	}

	protected override void FetchAgentFacing()
	{
		AnimationTaskClimb animationTaskClimb = CameraStateHelper.CurrentAnimationTask as AnimationTaskClimb;
		if (animationTaskClimb != null)
		{
			m_agentFacing = FlatDirection(animationTaskClimb.OffMeshLinkEntity.Rotation);
		}
		else
		{
			base.FetchAgentFacing();
		}
		if (!CameraStateBase.Debugging)
		{
		}
	}

	protected override void CalculateReference()
	{
		Vector3 basePosition;
		Vector3 leadPosition;
		Quaternion viewRotation;
		GetTargets(out basePosition, out leadPosition, out viewRotation);
		bool flag = AcierCameraController.Events.isRotating;
		if (!flag && IsInPatrol() && IsIdlingInPatrol())
		{
			flag = true;
		}
		if (flag)
		{
			m_leadOffset = Damping.Damp(m_leadOffset, Vector3.zero, m_roamingValues.relaxDamping, 0.75f);
		}
		else if (AcierCameraController.Events.isMoving)
		{
			Vector3 normalized = (leadPosition - basePosition).normalized;
			m_leadOffset = Damping.Damp(m_leadOffset, normalized * CurrentLeadDistance, m_roamingValues.leadingDamping, 1f);
		}
		m_referenceDampedPosition = basePosition + m_leadOffset;
	}

	protected bool IsRealignmentNeeded()
	{
		CameraEvents events = AcierCameraController.Events;
		if (events.isRotating)
		{
			return false;
		}
		bool flag = events.isMoving || CameraStateHelper.CachedAnimationTaskType == AnimationTaskType.Climb;
		if (flag && IsInPatrol() && IsIdlingInPatrol())
		{
			flag = false;
		}
		return flag;
	}

	protected override void CalculateRotation()
	{
		CameraEvents events = AcierCameraController.Events;
		if (events.isRotating)
		{
			CalculateZoomRotationWhenUserRotates();
		}
		CalculateZoomRotationDamp();
		if (IsRealignmentNeeded())
		{
			m_zoomRotationTarget.x = Damping.Damp(m_zoomRotationTarget.x, 0f, m_roamingValues.resetVerticalDamping, 1f);
			CalculateAutoAlignment();
		}
	}

	protected override void CalculateAutoAlignment()
	{
		bool flag = AcierCameraController.Instance.CurrentPlayerState == EPlayerState.HidingInPatrol;
		CalculateAutoAlignment((!flag) ? GetBestAlignmentSpeedCurve() : m_roamingValues.m_alignmentSpeedInPatrol.curve);
	}

	protected override Vector3 CalculateRealignDirection()
	{
		if (CameraStateHelper.CachedAnimationTaskType != AnimationTaskType.Climb)
		{
			return AcierCameraController.AgentDataFilter.Direction;
		}
		if (Vector3.Angle(m_cameraForward, m_agentFacing) > m_autoRotateValuesWhileCimbing.maxUserYawAngle)
		{
			Vector3 vector = Quaternion.Euler(0f, m_autoRotateValuesWhileCimbing.maxUserYawAngle, 0f) * m_agentFacing;
			Vector3 vector2 = Quaternion.Euler(0f, 0f - m_autoRotateValuesWhileCimbing.maxUserYawAngle, 0f) * m_agentFacing;
			float num = Vector3.Angle(m_cameraForward, vector);
			float num2 = Vector3.Angle(m_cameraForward, vector2);
			return (!(num < num2)) ? vector2 : vector;
		}
		return m_cameraForward;
	}

	protected bool IsInPatrol()
	{
		return AcierCameraController.Instance.CurrentPlayerState == EPlayerState.HidingInPatrol;
	}

	protected bool IsIdlingInPatrol()
	{
		IPayload currentStatePayload = LogicMain.PlayerEntity.PlayerCharacterStateMachines.PlayerMain.CurrentStatePayload;
		PlayerMain.HidingInPatrolState.HidingInPatrolPayload hidingInPatrolPayload = currentStatePayload as PlayerMain.HidingInPatrolState.HidingInPatrolPayload;
		if (hidingInPatrolPayload != null && hidingInPatrolPayload.PatrolId != EntityId.InvalidId)
		{
			Patrol patrol = hidingInPatrolPayload.PatrolId.Entity as Patrol;
			return patrol != null && patrol.PatrolComponent.Idles;
		}
		return false;
	}

	public override void UpdateState()
	{
		float b = ((AcierCameraController.Speed != EMovementSpeed.Sprint) ? 0f : 1f);
		float t = Damping.CalculateFactor(m_roamingValues.speedTransitionDamping, 1f);
		m_sprintVelocity = Mathf.Lerp(m_sprintVelocity, b, t);
		base.UpdateState();
		m_outputFieldOfView = m_fieldOfViewValues.fieldOfView + m_roamingValues.sprintFieldOfViewModifier * m_sprintVelocity;
	}
}
