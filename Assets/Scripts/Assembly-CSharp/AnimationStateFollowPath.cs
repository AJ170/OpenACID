using AcierProtocol;
using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Motion;
using UnityEngine;
using UnityEngine.AI;

public class AnimationStateFollowPath : AnimationStateBase, IAnimatorEventListener
{
	public const float FacingDamping = 0.2f;

	public const float MaxFacingAngle = 5f;

	public const float WalkStoppingDistance = 0.25f;

	public const float RunStoppingDistance = 0.75f;

	public const float Acceleration = 250f;

	public const float AngularSpeed = 1440f;

	public Vector3 FacingDirection;

	public static float FaceDirectionDamping = 0.15f;

	public static float TurnAroundMinSpeed = 0.5f;

	public static float TurnAroundMinAngle = 110f;

	public static float SpeedAtMaxTurnAngle = 0.2f;

	public static float TurnAngleForMaxSlow = 90f;

	public static float SmallTurnMaxSpeed = 0.5f;

	public static float SmallTurnMinAngle = 90f;

	public static float IdleTurnMinAngle = 90f;

	public static float FacingTurnMinAngle = 15f;

	public static float SpeedAtNavMeshEdge = 1f;

	public static float NavMeshEdgeDistance = 1.2f;

	private float m_inertia;

	private float m_avoidanceCorrectionFactor = 1f;

	private bool m_turning;

	private float m_turningMinTime;

	private NpcTurnDirection m_turnDirection = NpcTurnDirection.Invalid;

	private float m_turnCorrectionAngle;

	private bool m_arrived;

	public bool EnableFacing { get; set; }

	public float StoppingDistanceSq { get; set; }

	public bool ResetMovementState { get; set; }

	public AnimationStateFollowPath(AnimationController _animationController, bool _accelerate, bool _resetMovementState, float _stoppingDistanceSq)
		: base(AnimationStateType.FollowPath, _animationController)
	{
		m_inertia = ((!_accelerate) ? 1f : 0f);
		StoppingDistanceSq = _stoppingDistanceSq;
		ResetMovementState = _resetMovementState;
		AnimationController.GetComponent<AnimatorDataController>().RegisterEventListener(this);
	}

	public override void OnEnter()
	{
		base.OnEnter();
		if (AnimationController.CrouchValue.Scalar > 0f)
		{
			AnimationController.Animator.SetCurrentIdleType(IdleType.LowProfile);
		}
		AnimationController.Agent.acceleration = 250f;
		AnimationController.Agent.angularSpeed = 1440f;
		AnimationController.Agent.autoBraking = true;
		AnimationController.Agent.autoTraverseOffMeshLink = false;
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		AnimationController.ApplyRootMotion = true;
		if (AnimationController.EntityId.Entity.Type == EntityTypes.EType.PlayerCharacter)
		{
			m_avoidanceCorrectionFactor = 0f;
		}
		bool flag = AnimationController.Animator.GetCurrentMovementState() == MovementState.Idle;
		m_turning = false;
		m_arrived = false;
		if (flag && AnimationController.Agent.hasPath && AnimationController.CrouchValue.Scalar <= 0f)
		{
			Quaternion quaternion = Quaternion.LookRotation(AnimationController.Agent.steeringTarget - AnimationController.Agent.nextPosition);
			float num = Vector3.Angle(quaternion * Vector3.forward, AnimationController.NextRotation * Vector3.forward) * Mathf.Sign(Vector3.Dot(quaternion * Vector3.forward, AnimationController.NextRotation * Vector3.right));
			if (Mathf.Abs(num) > IdleTurnMinAngle && !m_turning && LogicMain.CurrentTime > m_turningMinTime)
			{
				m_turnDirection = AnimationHelper.DetermineNpcTurnDirection(0f, num);
				m_turnCorrectionAngle = num - AnimationHelper.NpcTurnDirectionToAngle(m_turnDirection);
				AnimationController.Animator.SetCurrentMovementState(MovementState.Turn);
				AnimationController.Animator.SetCurrentNpcTurnDirection(m_turnDirection);
				m_turning = true;
			}
		}
	}

	public override void OnDispose()
	{
		base.OnDispose();
		AnimationController.GetComponent<AnimatorDataController>().UnregisterEventListener(this);
	}

	public bool OnAnimatorEvent(AnimatorEvent _event)
	{
		AnimatorEventType type = _event.Type;
		if (type == AnimatorEventType.TurnEnd)
		{
			if (m_turning)
			{
				AnimationController.Animator.SetCurrentMovementState((!m_arrived) ? MovementState.Movement : MovementState.Idle);
			}
			m_turningMinTime = LogicMain.CurrentTime + 0.3f;
			m_turning = false;
		}
		return false;
	}

	public override void OnUpdate()
	{
		float magnitude = (AnimationController.Agent.pathEndPosition - AnimationController.NextPosition).magnitude;
		NavMeshAgent agent = AnimationController.Agent;
		float b = Mathf.Lerp(0.25f, 0.75f, (AnimationController.Agent.speed - AnimationController.CalculateSpeed(EMovementSpeed.Walk)) / (AnimationController.CalculateSpeed(EMovementSpeed.Run) - AnimationController.CalculateSpeed(EMovementSpeed.Walk)));
		b = Mathf.Min(Mathf.Sqrt(StoppingDistanceSq), b);
		AnimationController.Agent.stoppingDistance = b;
		m_arrived = m_arrived || magnitude <= b;
		Vector3 vector = agent.velocity - agent.desiredVelocity;
		float magnitude2 = vector.magnitude;
		Quaternion quaternion = ((AnimationController.Agent.hasPath && !m_arrived) ? Quaternion.LookRotation(agent.steeringTarget - agent.nextPosition) : ((!EnableFacing) ? AnimationController.NextRotation : Quaternion.LookRotation(FacingDirection)));
		float currentSpeed = AnimationController.Animator.GetCurrentSpeed();
		float num = Vector3.Angle(quaternion * Vector3.forward, AnimationController.NextRotation * Vector3.forward) * Mathf.Sign(Vector3.Dot(quaternion * Vector3.forward, AnimationController.NextRotation * Vector3.right));
		float num2 = ((!(AnimationController.gameObject == LogicMain.Player)) ? NavMeshEdgeDistance : ParcourHelper.ClearDistanceForward(agent.nextPosition, AnimationController.NextRotation * Vector3.forward, 2f));
		float speed = AnimationController.GetSpeed(agent.velocity);
		float a = Mathf.Lerp(1f, SpeedAtMaxTurnAngle, (!m_turning) ? Mathf.Clamp01(num / TurnAngleForMaxSlow) : 0f);
		a = Mathf.Min(a, Mathf.Lerp(SpeedAtNavMeshEdge, 1f, num2 / NavMeshEdgeDistance));
		speed = Mathf.Min(Mathf.Lerp(speed / 2f, speed, m_inertia), a);
		float damping = ((!(AnimationController.CrouchValue.Scalar > 0f)) ? 0.1f : 0.5f);
		float speed2 = AnimationController.Damping(currentSpeed, speed, damping);
		AnimationController.Animator.SetCurrentSpeed(speed2);
		float num3 = (1f - Mathf.Exp((0f - magnitude2) * 0.25f) - 0.2f) / 0.8f * m_avoidanceCorrectionFactor;
		if (num3 > 0f)
		{
			AnimationController.NextPosition += vector * num3 * LogicMain.FrameTime;
		}
		AnimationController.CorrectPosition(Mathf.Lerp(0.5f, 0.025f, m_inertia));
		if (AnimationController.Agent.hasPath && !m_arrived && !AnimationController.Agent.isOnOffMeshLink)
		{
			float currentBankLeftRight = AnimationController.Animator.GetCurrentBankLeftRight();
			float bankAmount = AnimationController.GetBankAmount(AnimationController.Agent);
			AnimationController.Animator.SetCurrentBankLeftRight(AnimationController.Damping(currentBankLeftRight, bankAmount, 0.25f));
			if (!m_turning)
			{
				AnimationController.FaceDirection(quaternion * Vector3.forward, FaceDirectionDamping);
			}
			else
			{
				float num4 = Damping.Damp(m_turnCorrectionAngle, 0f, 0.2f, 1f);
				AnimationController.NextRotation = Quaternion.Euler(0f, m_turnCorrectionAngle - num4, 0f) * AnimationController.NextRotation;
				m_turnCorrectionAngle = num4;
			}
			if (Mathf.Abs(num) > TurnAroundMinAngle && currentSpeed > TurnAroundMinSpeed && !m_turning && LogicMain.CurrentTime > m_turningMinTime && AnimationController.CrouchValue.Scalar <= 0f && num2 > 1f)
			{
				m_turnDirection = ((num < 0f) ? NpcTurnDirection.Ccw180 : NpcTurnDirection.Cw180);
				m_turnCorrectionAngle = num - AnimationHelper.NpcTurnDirectionToAngle(m_turnDirection);
				m_turning = true;
			}
			if (Mathf.Abs(num) > SmallTurnMinAngle && currentSpeed <= SmallTurnMaxSpeed && !m_turning && LogicMain.CurrentTime > m_turningMinTime && AnimationController.CrouchValue.Scalar <= 0f)
			{
				m_turnDirection = AnimationHelper.DetermineNpcTurnDirection(0f, num);
				m_turnCorrectionAngle = num - AnimationHelper.NpcTurnDirectionToAngle(m_turnDirection);
				m_turning = true;
			}
			if (!m_turning)
			{
				AnimationController.Animator.SetCurrentMovementState(MovementState.Movement);
			}
			else
			{
				AnimationController.Animator.SetCurrentNpcTurnDirection(m_turnDirection);
				AnimationController.Animator.SetCurrentMovementState(MovementState.Turn);
			}
			m_inertia = Mathf.Clamp01(m_inertia + LogicMain.FrameTime / (1f + num / 45f) * 5f);
		}
		else if (!AnimationController.Agent.isOnOffMeshLink)
		{
			if (ResetMovementState)
			{
				AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
			}
			if (EnableFacing)
			{
				if (!m_turning)
				{
					AnimationController.FaceDirection(FacingDirection, 0.2f);
				}
				else
				{
					float num5 = Damping.Damp(m_turnCorrectionAngle, 0f, 0.2f, 1f);
					AnimationController.NextRotation = Quaternion.Euler(0f, m_turnCorrectionAngle - num5, 0f) * AnimationController.NextRotation;
					m_turnCorrectionAngle = num5;
				}
				if (Mathf.Abs(num) > FacingTurnMinAngle && !m_turning && AnimationController.CrouchValue.Scalar <= 0f)
				{
					m_turnDirection = AnimationHelper.DetermineNpcTurnDirection(0f, num);
					m_turnCorrectionAngle = num - AnimationHelper.NpcTurnDirectionToAngle(m_turnDirection);
					m_turning = true;
				}
				if (!m_turning)
				{
					AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
				}
				else
				{
					AnimationController.Animator.SetCurrentNpcTurnDirection(m_turnDirection);
					AnimationController.Animator.SetCurrentMovementState(MovementState.Turn);
				}
				if (!m_turning && Mathf.Abs(num) < FacingTurnMinAngle)
				{
					base.IsFinished = true;
				}
			}
			else
			{
				base.IsFinished = true;
			}
		}
		else
		{
			base.IsFinished = true;
		}
		if (base.IsFinished)
		{
			SetCrouchIdleTypeIfNeeded();
		}
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		if (m_turning)
		{
			AnimationController.Animator.SetCurrentMovementState(MovementState.Movement);
		}
		if (_isCanceled)
		{
			SetCrouchIdleTypeIfNeeded();
			if (ResetMovementState)
			{
				AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
			}
		}
	}

	private void SetCrouchIdleTypeIfNeeded()
	{
		if (AnimationController.CrouchValue.Scalar > 0f && !AnimationController.IsLeavingCrouch())
		{
			AnimationController.Animator.SetCurrentIdleType(IdleType.Crouch);
		}
	}
}
