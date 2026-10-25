using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Motion;
using UbiCore.Messenger;
using UnityEngine;

public class AnimationStateBalance : AnimationStateBase, IAnimation, IParkourAnimation
{
	public static float BalanceFadeLength = 3f;

	public static int BezierSegmentCount = 100;

	public static float TurnMinAngle = 45f;

	public static float BlockLinkDuration = 1f;

	private float m_progress;

	private float m_length;

	private float m_speedFactor;

	private Vector3 m_linkDirection;

	private MovementState m_nextMovementState = MovementState.Movement;

	private Vector3[] m_refPoints;

	private float[] m_lengthSum;

	private Vector3 m_targetPosition;

	private Vector3 m_start;

	private Vector3 m_end;

	private Vector3 m_startControlPoint;

	private Vector3 m_endControlPoint;

	private AnimatorIK m_animatorIK;

	private bool m_fadeStart = true;

	private bool m_fadeEnd = true;

	public float Height
	{
		get
		{
			return (m_start - m_end).y;
		}
	}

	public Vector3 Destination
	{
		get
		{
			return m_end;
		}
	}

	public AnimationStateBalance(AnimationController _animationController, Vector3 _start, Vector3 _end, Vector3 _startControlPoint, Vector3 _endControlPoint, bool _fadeStart = true, bool _fadeEnd = true)
		: base(AnimationStateType.Balance, _animationController)
	{
		m_start = _start;
		m_end = _end;
		m_startControlPoint = _startControlPoint;
		m_endControlPoint = _endControlPoint;
		Vector3 vector = m_end - m_start;
		m_length = vector.magnitude;
		vector.y = 0f;
		m_linkDirection = vector.normalized;
		m_fadeStart = _fadeStart;
		m_fadeEnd = _fadeEnd;
		int bezierSegmentCount = BezierSegmentCount;
		m_refPoints = new Vector3[bezierSegmentCount];
		m_lengthSum = new float[bezierSegmentCount];
		for (int i = 0; i < bezierSegmentCount; i++)
		{
			float t = (float)i / (float)(bezierSegmentCount - 1);
			m_refPoints[i] = CubicBezier(m_start, m_startControlPoint, m_endControlPoint, m_end, t);
		}
		m_length = 0f;
		m_lengthSum[0] = 0f;
		for (int j = 1; j < bezierSegmentCount; j++)
		{
			m_length += (m_refPoints[j] - m_refPoints[j - 1]).magnitude;
			m_lengthSum[j] = m_length;
		}
		if (this.GetHandlesParkourInputEvents())
		{
			Messenger.Instance.AddListener<ParcourInputEvent>(OnParcourInputEvent);
		}
	}

	public void OnParcourInputEvent(ParcourInputEvent _event)
	{
		m_nextMovementState = ((ParcourHelper.ComputeRelativeDirection(_event.Direction, m_linkDirection) != eDirection.Invalid) ? MovementState.Movement : MovementState.Idle);
	}

	public override void OnEnter()
	{
		base.OnEnter();
		AnimationController.ApplyRootMotion = true;
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		m_animatorIK = AnimationController.Animator.GetComponent<AnimatorIK>();
		if ((bool)m_animatorIK)
		{
			m_animatorIK.GetIKPositionOverride = GetIKPosition;
		}
		StartBalance();
	}

	private void StartBalance()
	{
		AnimationController.Animator.SetCurrentLinkType(eLinkType.Balance);
		AnimationController.Animator.SetCurrentBankLeftRight(0f);
		AnimationController.Animator.SetCurrentMovementState(MovementState.Movement);
		m_progress = GetProjectedProgress(AnimationController.NextPosition);
		m_targetPosition = GetTargetPosition(m_progress);
	}

	public float GetProjectedProgress(Vector3 _position)
	{
		float value = Vector3.Dot(_position - m_start, m_linkDirection) / (m_end - m_start).magnitude;
		return Mathf.Clamp01(value);
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		if (m_fadeEnd)
		{
			AnimationController.Animator.SetCurrentBalanceWalk(0f);
		}
		AnimationController.Animator.SetCurrentGeneralState(GeneralState.Default);
		AnimationController.Animator.SetCurrentLinkType(eLinkType.Invalid);
		AnimationController.Animator.SetCurrentMovementState(m_nextMovementState);
		if ((bool)m_animatorIK)
		{
			m_animatorIK.GetIKPositionOverride = null;
		}
		Messenger.Instance.RemoveListener<ParcourInputEvent>(OnParcourInputEvent);
	}

	public override void OnDispose()
	{
		base.OnDispose();
		Messenger.Instance.RemoveListener<ParcourInputEvent>(OnParcourInputEvent);
	}

	public override void OnUpdate()
	{
		float currentLinkLength = AnimationController.Animator.GetCurrentLinkLength();
		AnimationController.Animator.SetCurrentLinkLength(Damping.Damp(currentLinkLength, m_length, 0.5f, 1f));
		if (!base.IsFinished)
		{
			float num = Mathf.Abs(Vector3.Dot(AnimationController.Animator.deltaPosition, m_linkDirection));
			m_progress += num / m_length;
			m_targetPosition = GetTargetPosition(m_progress);
			Quaternion quaternion = AnimationController.FaceDirection(m_linkDirection, 0.2f);
			float num2 = Vector3.Angle(quaternion * Vector3.forward, m_linkDirection);
			Vector3 rhs = AnimationController.NextPosition - m_targetPosition;
			float num3 = rhs.magnitude - Mathf.Clamp(Vector3.Dot(m_linkDirection, rhs), 0f, float.MaxValue);
			m_speedFactor = Mathf.Max(m_speedFactor, Mathf.Clamp01(1.1f - num2 / 10f - num3 / 1f));
			AnimationController.CorrectPosition(m_targetPosition, Mathf.Lerp(0.5f, 0f, m_speedFactor));
			float num4 = 1f;
			if (m_fadeStart)
			{
				num4 = Mathf.Min(num4, m_progress * m_length / BalanceFadeLength);
			}
			if (m_fadeEnd)
			{
				num4 = Mathf.Min(num4, (1f - m_progress) * m_length / BalanceFadeLength);
			}
			num4 = Mathf.Clamp01(num4);
			AnimationController.Animator.SetCurrentBalanceWalk(num4);
			float currentSpeed = AnimationController.Animator.GetCurrentSpeed();
			float speedFactor = AnimationController.GetSpeedFactor(AnimationController.ReferenceMovementSpeed);
			currentSpeed = Damping.Damp(currentSpeed, speedFactor, 0.2f, 1f);
			AnimationController.Animator.SetCurrentSpeed(currentSpeed);
			base.IsFinished = m_progress > 1f;
		}
	}

	private bool GetIKPosition(Vector3 _bonePosition, ref Vector3 _groundPosition, ref Vector3 _groundNormal)
	{
		if (m_animatorIK == null)
		{
			return false;
		}
		float num = Vector3.Dot(_bonePosition - m_targetPosition, m_linkDirection);
		float progress = m_progress + num / m_length;
		_groundPosition = _bonePosition;
		_groundPosition.y = GetTargetPosition(progress).y;
		_groundNormal = Vector3.up;
		return true;
	}

	public Vector3 GetTargetPosition(float _progress)
	{
		int num = 1;
		int num2 = m_lengthSum.Length - 1;
		while (num < num2)
		{
			int num3 = (num + num2) / 2;
			float num4 = m_lengthSum[num3] / m_length;
			if (_progress <= num4)
			{
				num2 = num3;
			}
			else
			{
				num = num3 + 1;
			}
		}
		float num5 = m_lengthSum[num] / m_length;
		float num6 = m_lengthSum[num - 1] / m_length;
		if (_progress <= num5)
		{
			return Vector3.Lerp(m_refPoints[num - 1], m_refPoints[num], (_progress - num6) / (num5 - num6));
		}
		return m_end;
	}

	private static Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		float num = 1f - t;
		return p0 * (num * num * num) + p1 * (3f * num * num * t) + p2 * (3f * num * t * t) + p3 * (t * t * t);
	}

	public override bool CanCancel()
	{
		return false;
	}
}
