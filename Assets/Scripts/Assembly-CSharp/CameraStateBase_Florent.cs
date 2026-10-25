using System;
using Assets.Scripts.Mission.Camera;
using SharedBaseLib.System;
using UbiCore.Common;
using UbiCore.Debug;
using UbiCore.Messenger;
using UbiCore.Utils;
using UnityEngine;

public class CameraStateBase_Florent : CameraStateBase, IInterpreter
{
	public float MinDistanceFromObstacles = 0.35f;

	protected Vector3 m_leadOffset;

	public float LeadDistance = 4f;

	public float LeadDamping = 0.1f;

	public float Fov = 70f;

	public float FovForward = 70f;

	public float FovBack = 70f;

	protected Vector3 m_position;

	protected Vector3 m_targetPosition;

	public float PositionDamping = 0.1f;

	public Vector3 PositionOffset = new Vector3(0f, 3f, -3.4f);

	public Vector3 PositionOffsetForward = new Vector3(0f, 5f, 1.5f);

	public Vector3 PositionOffsetBack = new Vector3(0f, 2f, -1f);

	protected Vector3 m_lookAt;

	protected Vector3 m_targetLookAt;

	public float LookAtDamping = 0.1f;

	public Vector3 LookAtOffset = new Vector3(0f, 1f, 0f);

	public Vector3 LookAtOffsetForward = new Vector3(0f, 1f, 2f);

	public Vector3 LookAtOffsetBack = new Vector3(0f, 2f, 1f);

	protected float m_zoomAmount;

	protected float m_grabZoomAmount;

	public static bool FlipZoomDirection;

	public float ZoomDamping = 0.2f;

	public float ZoomDragDistance = 0.15f;

	public float RotateDragDistance = 0.3f;

	protected float m_tapLockTime;

	public float TapLockDuraction = 0.3f;

	protected float m_characterLookAtWeight;

	protected float m_realignSpeed;

	protected Quaternion m_grabViewRotation;

	protected Quaternion m_viewRotation;

	public bool AutoRealign = true;

	public float MaxAngleToRealign = 120f;

	public float RealignDamping = 0.2f;

	public float MinRealignSpeed = 0.1f;

	public float MaxRealignSpeed = 0.6f;

	public bool RealignIfLOSIsBlocked;

	protected Vector3 m_refPosition;

	public float MinDistanceToCut = 10f;

	public static bool GrabMoveInput = true;

	protected readonly int m_raycastLayerMask = Layers.StaticColliderCameraBlockersLayersMaskPreset;

	public bool InputEnabled { get; set; }

	public float InputPriority { get; set; }

	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.None;
		}
	}

	public bool GrabbedInput { get; private set; }

	public CameraStateBase_Florent()
	{
		InputPriority = 3f;
	}

	public void OnDrawGizmos()
	{
	}

	public override void CopyFrom(ICameraState _src)
	{
	}

	public override void StartState(ICameraState _previous)
	{
		InputEnabled = true;
		AcierInputHandler.Instance.RegisterInterpreter(this);
		m_position = Camera.main.transform.position;
		Vector3 vector = Camera.main.transform.rotation * Vector3.forward;
		m_lookAt = Camera.main.transform.position + vector * 5f;
		vector.y = 0f;
		vector.Normalize();
		m_viewRotation = Quaternion.LookRotation(vector);
		m_realignSpeed = 0f;
		m_targetPosition = m_position;
		m_targetLookAt = m_lookAt;
		m_zoomAmount = 0f;
		m_grabViewRotation = m_viewRotation;
		m_grabZoomAmount = 0f;
		GrabbedInput = false;
	}

	public override void EndState()
	{
		InputEnabled = false;
		AcierInputHandler.Instance.UnregisterInterpreter(this);
	}

	public override void UpdateState()
	{
		Profiling.Start("Camera update", float.MaxValue);
		Vector3 basePosition;
		Vector3 leadPosition;
		Quaternion viewRotation;
		GetTargets(out basePosition, out leadPosition, out viewRotation);
		float deltaTime = UbiCore.Utils.Singleton<Timer>.Instance.deltaTime;
		MaxAngleToRealign = Mathf.Max(MaxAngleToRealign, 1f);
		m_realignSpeed = ((!AutoRealign) ? 0f : (Mathf.Lerp(MaxRealignSpeed, MinRealignSpeed, Mathf.Clamp01(GetFlatAngle(m_viewRotation, viewRotation) / MaxAngleToRealign)) * GetAutoRealignFactor()));
		m_viewRotation = Quaternion.Lerp(m_viewRotation, viewRotation, GetDampingFactor(deltaTime, m_realignSpeed, RealignDamping));
		Vector3 normalized = (leadPosition - basePosition).normalized;
		m_leadOffset = Vector3.Lerp(m_leadOffset, normalized * LeadDistance, GetDampingFactor(deltaTime, GetAutoLeadFactor(), LeadDamping));
		m_refPosition = basePosition + m_leadOffset;
		m_targetPosition = m_refPosition + m_viewRotation * GetPositionOffset() * GetPositionScale();
		m_targetLookAt = m_refPosition + m_viewRotation * GetLookAtOffset() * GetLookAtScale();
		CutIfTooFar();
		m_position = Vector3.Lerp(m_position, m_targetPosition, GetDampingFactor(deltaTime, 1f, PositionDamping));
		m_lookAt = Vector3.Lerp(m_lookAt, m_targetLookAt, GetDampingFactor(deltaTime, 1f, LookAtDamping));
		m_zoomAmount = Mathf.Lerp(m_zoomAmount, 0f, GetDampingFactor(deltaTime, GetAutoZoomFactor(), ZoomDamping));
		m_outputPosition = m_position;
		Vector3 vector = m_lookAt - m_position;
		if (vector.sqrMagnitude > 1E-05f)
		{
			m_outputRotation = Quaternion.LookRotation(vector.normalized);
		}
		m_outputFieldOfView = GetFov();
		PreventGimbalLock();
		KeepLineOfSightToCharacter();
		RetargetCharacterLookAt();
		Profiling.Stop();
	}

	protected virtual void PreventGimbalLock()
	{
		Vector3 vector = m_lookAt - CurrentPosition;
		float num = Vector3.Angle(vector, Vector3.down);
		float num2 = 5f;
		if (num < num2)
		{
			Vector3 vector2 = vector;
			vector2.y = 0f;
			vector2.Normalize();
			m_outputPosition = m_lookAt - (vector2 * Mathf.Sin(num2 * ((float)Math.PI / 180f)) + Vector3.down * Mathf.Cos(num2 * ((float)Math.PI / 180f))) * vector.magnitude;
			if ((m_lookAt - CurrentPosition).sqrMagnitude > 1E-05f)
			{
				m_outputRotation = Quaternion.LookRotation(m_lookAt - CurrentPosition);
			}
		}
	}

	protected void CutIfTooFar()
	{
		if ((m_position - m_targetPosition).magnitude > MinDistanceToCut)
		{
			m_position = m_targetPosition;
			m_lookAt = m_targetLookAt;
		}
	}

	protected virtual void RetargetCharacterLookAt()
	{
	}

	protected virtual float GetPositionScale()
	{
		return 1f;
	}

	protected virtual float GetLookAtScale()
	{
		return 1f;
	}

	protected virtual float GetAutoLeadFactor()
	{
		return (!AcierCameraController.NavMeshAgent.hasPath) ? 0f : (1f - Mathf.Abs(m_zoomAmount));
	}

	protected virtual float GetAutoRealignFactor()
	{
		return (!AcierCameraController.NavMeshAgent.hasPath) ? 0f : 1f;
	}

	protected virtual float GetAutoZoomFactor()
	{
		return (!AcierCameraController.NavMeshAgent.hasPath) ? 0f : 1f;
	}

	protected virtual void GetTargets(out Vector3 basePosition, out Vector3 leadPosition, out Quaternion viewRotation)
	{
		NavMeshAgent navMeshAgent = AcierCameraController.NavMeshAgent;
		Transform transform = navMeshAgent.transform;
		viewRotation = transform.rotation;
		basePosition = transform.position;
		viewRotation = AcierCameraController.AgentDataFilter.Rotation;
		basePosition = AcierCameraController.AgentDataFilter.Position;
		leadPosition = basePosition + viewRotation * Vector3.forward;
		if (navMeshAgent.hasPath)
		{
			if (navMeshAgent.nextOffMeshLinkData.valid)
			{
				leadPosition = navMeshAgent.nextOffMeshLinkData.endPos;
			}
			else if (navMeshAgent.hasPath)
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
		leadPosition = basePosition + vector.normalized * LeadDistance;
	}

	protected virtual float GetFov()
	{
		return Mathf.Lerp(Mathf.Lerp(Fov, FovBack, Mathf.Clamp01(0f - m_zoomAmount)), FovForward, Mathf.Clamp01(m_zoomAmount));
	}

	protected virtual Vector3 GetPositionOffset()
	{
		return Vector3.Lerp(Vector3.Lerp(PositionOffset, PositionOffsetBack, Mathf.Clamp01(0f - m_zoomAmount)), PositionOffsetForward, Mathf.Clamp01(m_zoomAmount));
	}

	protected virtual Vector3 GetLookAtOffset()
	{
		return Vector3.Lerp(Vector3.Lerp(LookAtOffset, LookAtOffsetBack, Mathf.Clamp01(0f - m_zoomAmount)), LookAtOffsetForward, Mathf.Clamp01(m_zoomAmount));
	}

	protected float GetFlatAngle(Quaternion refRotation, Quaternion targetRotation)
	{
		Vector3 from = refRotation * Vector3.forward;
		from.y = 0f;
		from.Normalize();
		Vector3 to = targetRotation * Vector3.forward;
		to.y = 0f;
		to.Normalize();
		return Vector3.Angle(from, to);
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

	public virtual void Realign()
	{
		Vector3 basePosition;
		Vector3 leadPosition;
		Quaternion viewRotation;
		GetTargets(out basePosition, out leadPosition, out viewRotation);
		m_viewRotation = viewRotation;
		m_zoomAmount = 0f;
		m_leadOffset = Vector3.zero;
	}

	public virtual void Rotate(Vector2 _input)
	{
		float num = _input.x / RotateDragDistance;
		m_viewRotation = m_grabViewRotation * Quaternion.Euler(0f, num * 180f, 0f);
		float num2 = _input.y / ZoomDragDistance;
		m_zoomAmount = Mathf.Clamp(m_grabZoomAmount + num2 * (float)((!FlipZoomDirection) ? 1 : (-1)), -1f, 1f);
		m_leadOffset = Vector3.Lerp(m_leadOffset, Vector3.zero, GetDampingFactor((Mathf.Abs(num) + Mathf.Abs(num2)) * 0.1f, 1f, LeadDamping));
	}

	protected virtual void KeepLineOfSightToCharacter()
	{
		NavMeshAgent navMeshAgent = AcierCameraController.NavMeshAgent;
		Transform transform = navMeshAgent.transform;
		Vector3 position = transform.position;
		Vector3 vector = position + Vector3.up * 1.5f;
		Vector3 vector2 = m_position - vector;
		Vector3 normalized = vector2.normalized;
		float num = vector2.magnitude + MinDistanceFromObstacles;
		Vector3 origin = vector + normalized * num;
		RaycastHit[] array = Physics.RaycastAll(origin, -normalized, num, m_raycastLayerMask);
		RaycastHit[] array2 = Physics.RaycastAll(vector, normalized, num, m_raycastLayerMask);
		if (array2.Length <= 0)
		{
			return;
		}
		float num2 = float.PositiveInfinity;
		float num3 = float.PositiveInfinity;
		RaycastHit raycastHit = array2[0];
		RaycastHit[] array3 = array2;
		for (int i = 0; i < array3.Length; i++)
		{
			RaycastHit raycastHit2 = array3[i];
			float num4 = num - raycastHit2.distance;
			if (num4 < num3)
			{
				num3 = num4;
				raycastHit = raycastHit2;
			}
		}
		RaycastHit[] array4 = array;
		foreach (RaycastHit raycastHit3 in array4)
		{
			num2 = Mathf.Min(num2, raycastHit3.distance);
		}
		if (num2 > num3)
		{
			m_outputPosition = raycastHit.point + raycastHit.normal * MinDistanceFromObstacles;
		}
		else if (RealignIfLOSIsBlocked)
		{
			Realign();
		}
	}

	protected virtual void GrabRotation()
	{
		m_grabViewRotation = m_viewRotation;
		m_grabZoomAmount = m_zoomAmount;
	}

	public bool OnTouch(TouchEvent _event)
	{
		return GrabbedInput;
	}

	public bool OnDoubleTap(TapEvent _event)
	{
		return GrabbedInput;
	}

	public bool OnTap(TapEvent _event)
	{
		if (UbiCore.Utils.Singleton<Timer>.Instance.realTime - m_tapLockTime < TapLockDuraction)
		{
			return true;
		}
		return GrabbedInput;
	}

	public bool OnDrag(DragEvent _event)
	{
		return GrabbedInput;
	}

	public bool OnSelect(SelectEvent _event)
	{
		return GrabbedInput;
	}

	public IconMaterial GetDefaultActionIcon()
	{
		return IconMaterial.Invalid;
	}

	public bool OnTriggerDefaultAction()
	{
		return false;
	}

	public bool OnPrepareDefaultAction()
	{
		return false;
	}

	public bool OnCancelDefaultAction()
	{
		return false;
	}

	public bool OnSecondTouch(TouchEvent _event)
	{
		m_tapLockTime = Time.time;
		GrabbedInput = GrabMoveInput;
		return GrabbedInput;
	}

	public bool OnSecondTap(TapEvent _event)
	{
		Realign();
		GrabbedInput = false;
		return true;
	}

	public bool OnSecondDrag(DragEvent _event)
	{
		if (_event.Phase == InputEvent.InputPhase.Started)
		{
			GrabRotation();
		}
		Rotate(_event.Distance / AcierInputHandler.Instance.PixelScaleFactor * AcierInputHandler.Instance.SensitivityFactor);
		GrabbedInput = _event.Phase != InputEvent.InputPhase.Ended && GrabMoveInput;
		return true;
	}

	public bool OnSecondSelect(SelectEvent _event)
	{
		if (_event.Phase == InputEvent.InputPhase.Started)
		{
			GrabRotation();
		}
		Rotate(_event.Distance / AcierInputHandler.Instance.PixelScaleFactor * AcierInputHandler.Instance.SensitivityFactor);
		GrabbedInput = _event.Phase != InputEvent.InputPhase.Ended && GrabMoveInput;
		return true;
	}

	public bool OnSwipe(SwipeEvent _event)
	{
		return false;
	}

	public bool OnTwist(TwistEvent _event)
	{
		return false;
	}

	public bool OnPinch(PinchEvent _event)
	{
		return false;
	}

	public void OnUpdate()
	{
	}
}
