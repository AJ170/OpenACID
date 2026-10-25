using Assets.Scripts.Mission.Camera;
using UnityEngine;

public abstract class CameraStateOrbit : CameraStateRaycast
{
	[SerializeField]
	protected ZoomAndRotateValues m_zoomAndRotateValues;

	[SerializeField]
	protected OrbitValues m_orbitValues;

	protected Vector3 m_agentPosition;

	protected Vector3 m_agentTargetPosition;

	protected Vector3 m_agentForward;

	protected Vector3 m_agentFacing;

	protected Vector3 m_cameraForward;

	protected Vector3 m_referenceTargetPosition;

	protected Vector3 m_referenceDampedPosition;

	protected Quaternion m_rotation;

	protected Vector3 m_cameraTargetPosition;

	protected Vector3 m_cameraDampedPosition;

	protected Vector3 m_lookAtTargetPosition;

	protected Vector3 m_lookAtDampedPosition;

	protected Vector2 m_zoomRotation;

	protected Vector2 m_zoomRotationTarget;

	protected Vector3 m_dampedRaycastStart;

	public override bool DrawAvatar
	{
		get
		{
			return true;
		}
	}

	protected CameraStateOrbit()
	{
		m_rotation = Quaternion.identity;
	}

	public override void CopyFrom(ICameraState _src)
	{
		base.CopyFrom(_src);
		CameraStateOrbit cameraStateOrbit = (CameraStateOrbit)_src;
		m_zoomAndRotateValues = cameraStateOrbit.m_zoomAndRotateValues;
		m_orbitValues = cameraStateOrbit.m_orbitValues;
	}

	public override void StartState(ICameraState _previous)
	{
		if (_previous is CameraStateIntroduction || _previous is CameraStateDead)
		{
			_previous = null;
		}
		base.StartState(_previous);
		if (m_zoomAndRotateValues == null)
		{
			m_zoomAndRotateValues = AcierCameraController.Instance.GlobalValues.m_zoomAndRotateValues;
		}
		if (m_orbitValues == null)
		{
			m_orbitValues = AcierCameraController.Instance.GlobalValues.m_orbitValues;
		}
		CameraStateOrbit cameraStateOrbit = _previous as CameraStateOrbit;
		if (cameraStateOrbit != null)
		{
			m_rotation = cameraStateOrbit.m_rotation * Quaternion.Euler(0f, cameraStateOrbit.m_zoomRotation.y, 0f);
		}
		else
		{
			Vector3 vector = ((_previous != null) ? base.transform.forward : AcierCameraController.NavMeshAgent.transform.forward);
			vector.y = 0f;
			float magnitude = vector.magnitude;
			if (magnitude > 1E-05f && (vector / magnitude).sqrMagnitude > 1E-05f)
			{
				m_rotation = Quaternion.LookRotation(vector / magnitude);
			}
		}
		m_zoomRotationTarget = (m_zoomRotation = Vector2.zero);
		m_agentPosition = AcierCameraController.NavMeshAgent.transform.position;
		Vector3 cameraOffset = m_orbitValues.cameraOffset;
		Vector3 lookAtOffset = m_orbitValues.lookAtOffset;
		m_referenceTargetPosition = m_agentPosition;
		m_cameraTargetPosition = m_agentPosition + m_rotation * cameraOffset;
		m_lookAtTargetPosition = m_agentPosition + m_rotation * lookAtOffset;
		m_referenceDampedPosition = m_referenceTargetPosition;
		m_cameraDampedPosition = m_cameraTargetPosition;
		m_lookAtDampedPosition = m_lookAtTargetPosition;
		if (cameraStateOrbit != null)
		{
			m_agentTargetPosition = cameraStateOrbit.m_agentTargetPosition;
			m_agentFacing = cameraStateOrbit.m_agentFacing;
		}
		m_dampedRaycastStart = CalculateRaycastStartTarget();
	}

	public override void EndState()
	{
	}

	protected void FetchInputValues()
	{
		NavMeshAgent navMeshAgent = AcierCameraController.NavMeshAgent;
		m_agentPosition = AcierCameraController.AgentDataFilter.Position;
		if (AcierCameraController.Events.isMoving)
		{
			if (navMeshAgent.nextOffMeshLinkData.valid)
			{
				m_agentTargetPosition = navMeshAgent.nextOffMeshLinkData.startPos;
			}
			else if (navMeshAgent.hasPath)
			{
				NavMeshPath path = navMeshAgent.path;
				Vector3[] corners = path.corners;
				if (corners.Length > 0)
				{
					m_agentTargetPosition = corners[corners.Length - 1];
				}
			}
		}
		m_agentForward = AcierCameraController.AgentDataFilter.Rotation * Vector3.forward;
		m_agentForward.y = 0f;
		m_agentForward.Normalize();
		m_cameraForward = base.transform.forward;
		m_cameraForward.y = 0f;
		m_cameraForward.Normalize();
		if (!CameraStateBase.Debugging)
		{
		}
	}

	protected virtual void FetchAgentFacing()
	{
		if (AcierCameraController.Events.isMoving)
		{
			m_agentFacing = m_agentTargetPosition - m_agentPosition;
			m_agentFacing.y = 0f;
			m_agentFacing.Normalize();
		}
		else
		{
			m_agentFacing = m_agentForward;
		}
		if (!CameraStateBase.Debugging)
		{
		}
	}

	protected virtual void CalculateReference()
	{
		DampReference();
		if (CameraStateBase.Debugging)
		{
			DrawReferenceDebugging();
		}
	}

	protected void DampReference()
	{
		m_referenceDampedPosition = Damping.Damp(m_referenceDampedPosition, m_referenceTargetPosition, m_orbitValues.referenceDamping, 1f);
	}

	protected void DrawReferenceDebugging()
	{
	}

	protected virtual Vector3 CalculateCameraOffset()
	{
		float t = m_zoomRotation.x * 0.5f + 0.5f;
		if (m_orbitValues.m_cameraSpline != null)
		{
			return m_orbitValues.m_cameraSpline.Eval(t);
		}
		return Vector3.zero;
	}

	protected virtual Vector3 CalculateLookAtOffset()
	{
		float t = m_zoomRotation.x * 0.5f + 0.5f;
		if (m_orbitValues.m_lookAtSpline != null)
		{
			return m_orbitValues.m_lookAtSpline.Eval(t);
		}
		return Vector3.zero;
	}

	protected virtual void CheckForRealign()
	{
		CameraEvents events = AcierCameraController.Events;
		if (events.secondTapEvent != null)
		{
			Realign();
		}
	}

	protected virtual void Realign()
	{
		m_zoomRotationTarget.x = 0f;
	}

	protected virtual void CalculateRotation()
	{
		CameraEvents events = AcierCameraController.Events;
		if (events.isRotating)
		{
			CalculateZoomRotationWhenUserRotates();
		}
		CalculateZoomRotationDamp();
	}

	protected virtual void CalculateZoomRotationWhenUserRotates()
	{
		CameraEvents events = AcierCameraController.Events;
		float pixelScaleFactor = AcierInputHandler.Instance.PixelScaleFactor;
		float sensitivityFactor = AcierInputHandler.Instance.SensitivityFactor;
		float num = events.cameraDragEvent.Delta.x / pixelScaleFactor * sensitivityFactor;
		m_zoomRotationTarget.y += num / m_zoomAndRotateValues.rotateDragDistance * 180f * events.RotateSensitivity * (float)((!CameraEvents.FlipXAxis) ? 1 : (-1));
		float f = events.cameraDragEvent.Distance.x / pixelScaleFactor * sensitivityFactor;
		float num2 = events.cameraDragEvent.Delta.y / pixelScaleFactor * sensitivityFactor;
		float num3 = Mathf.Lerp(0.5f, 1f, Mathf.Max(1f - Mathf.Abs(f) * 2f, 0f));
		m_zoomRotationTarget.x += num2 / m_zoomAndRotateValues.zoomDragDistance * (float)((!m_zoomAndRotateValues.flip) ? 1 : (-1)) * num3 * (float)((!CameraEvents.FlipYAxis) ? 1 : (-1));
		m_zoomRotationTarget.x = Mathf.Clamp(m_zoomRotationTarget.x, -1f, 1f);
	}

	protected virtual void CalculateZoomRotationDamp()
	{
		float t = Damping.CalculateFactor(m_zoomAndRotateValues.damping, 1f);
		m_zoomRotation.x = Mathf.Lerp(m_zoomRotation.x, m_zoomRotationTarget.x, t);
		m_zoomRotation.y = Mathf.Lerp(m_zoomRotation.y, m_zoomRotationTarget.y, t);
	}

	protected void CalculateCameraTargetPosition()
	{
		m_cameraTargetPosition = m_referenceDampedPosition + m_rotation * m_orbitValues.cameraOffset;
	}

	protected void CalculateLookAtTargetPosition()
	{
		m_lookAtTargetPosition = m_referenceDampedPosition + m_rotation * m_orbitValues.lookAtOffset;
	}

	protected void DampCameraAndLookAtPosition()
	{
		float t = Damping.CalculateFactor(m_orbitValues.cameraDamping, 1f);
		float t2 = Damping.CalculateFactor(m_orbitValues.lookAtDamping, 1f);
		m_cameraDampedPosition = Vector3.Lerp(m_cameraDampedPosition, m_cameraTargetPosition, t);
		m_lookAtDampedPosition = Vector3.Lerp(m_lookAtDampedPosition, m_lookAtTargetPosition, t2);
	}

	protected void LookAtAndOrbit()
	{
		Vector3 vector = m_cameraDampedPosition - m_lookAtDampedPosition;
		vector = Quaternion.Euler(0f, m_zoomRotation.y, 0f) * vector;
		vector += m_lookAtDampedPosition;
		Vector3 lookAtDampedPosition = m_lookAtDampedPosition;
		Vector3 vector2 = CalculateCameraOffset();
		Vector3 vector3 = CalculateLookAtOffset();
		Quaternion quaternion = m_rotation * Quaternion.Euler(0f, m_zoomRotation.y, 0f);
		Vector3 vector4 = vector + quaternion * vector2;
		Vector3 vector5 = lookAtDampedPosition + quaternion * vector3;
		if (CameraStateBase.Debugging)
		{
			Debug.DrawLine(vector4, vector5, Color.yellow);
		}
		if ((vector5 - vector4).sqrMagnitude > 1E-05f)
		{
			Quaternion outputRotation = Quaternion.LookRotation(vector5 - vector4);
			m_outputRotation = outputRotation;
		}
		m_outputPosition = vector4;
		if (CameraStateBase.Debugging)
		{
			if (m_orbitValues.m_cameraSpline != null)
			{
				DrawDebugSpline(m_orbitValues.m_cameraSpline, Color.cyan, vector);
			}
			if (m_orbitValues.m_lookAtSpline != null)
			{
				DrawDebugSpline(m_orbitValues.m_lookAtSpline, Color.red, lookAtDampedPosition);
			}
		}
	}

	private void DrawDebugSpline(CameraSpline spline, Color c, Vector3 _basePos)
	{
		for (int i = 0; i < 19; i++)
		{
			Quaternion quaternion = m_rotation * Quaternion.Euler(0f, m_zoomRotation.y, 0f);
			float t = (float)i / 19f;
			float t2 = (float)(i + 1) / 19f;
			Vector3 start = _basePos + quaternion * spline.Eval(t);
			Vector3 end = _basePos + quaternion * spline.Eval(t2);
			Debug.DrawLine(start, end, c);
		}
		CameraSpline.Keyframe[] keys = spline.m_keys;
		foreach (CameraSpline.Keyframe keyframe in keys)
		{
			Quaternion quaternion2 = m_rotation * Quaternion.Euler(0f, m_zoomRotation.y, 0f);
			Vector3 vector = _basePos + quaternion2 * keyframe.position;
		}
	}

	private Vector3 CalculateRaycastStartTarget()
	{
		Vector3 agentPosition = m_agentPosition;
		agentPosition.y += m_raycastValues.eyeHeight;
		return agentPosition;
	}

	protected override Vector3 CalculateRaycastStart()
	{
		Vector3 vector = CalculateRaycastStartTarget();
		m_dampedRaycastStart = Damping.Damp(m_dampedRaycastStart, vector, m_raycastValues.raycastStartDamping, 1f);
		Vector3 vector2 = m_dampedRaycastStart - vector;
		Vector3 normalized = vector2.normalized;
		float magnitude = vector2.magnitude;
		m_ray.origin = vector;
		m_ray.direction = normalized;
		Vector3 _raycastStart = ((!(magnitude > 1E-05f) || !Physics.Raycast(m_ray, out m_hitInfo, magnitude, AcierCameraController.Instance.LayerMask)) ? m_dampedRaycastStart : (m_ray.origin + m_hitInfo.distance * m_ray.direction));
		OffsetRaycastStart(ref _raycastStart);
		return _raycastStart;
	}

	protected virtual void OffsetRaycastStart(ref Vector3 _raycastStart)
	{
		Vector3 raycastOffset = m_orbitValues.raycastOffset;
		_raycastStart += Vector3.Cross(m_agentFacing, Vector3.up) * raycastOffset.x;
		_raycastStart += m_agentFacing * raycastOffset.z;
		_raycastStart += Vector3.up * raycastOffset.y;
	}

	public override void UpdateState()
	{
		FetchInputValues();
		FetchAgentFacing();
		CalculateReference();
		CheckForRealign();
		CalculateRotation();
		CalculateCameraTargetPosition();
		CalculateLookAtTargetPosition();
		DampCameraAndLookAtPosition();
		LookAtAndOrbit();
		base.UpdateState();
	}
}
