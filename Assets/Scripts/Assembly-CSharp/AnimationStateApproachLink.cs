using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;

public class AnimationStateApproachLink : AnimationStateBase, IAnimatorEventListener
{
	public static float TurnMinAngle = 90f;

	public static float SlideDamping = 0.05f;

	private bool m_UpdateRotation = true;

	private bool m_UpdateTranslation = true;

	private Vector3 m_TargetPosition;

	private Quaternion m_TargetOrientation;

	private bool m_waitForTurn;

	private NpcTurnDirection m_turnDirection;

	private float m_turnCorrectionAngle;

	private Vector3 m_faceDirection = Vector3.zero;

	public AnimationStateApproachLink(AnimationController _animationController, Vector3 _targetPosition, Quaternion _targetOrientation)
		: base(AnimationStateType.ApproachLink, _animationController)
	{
		m_TargetPosition = _targetPosition;
		m_TargetOrientation = _targetOrientation;
	}

	public AnimationStateApproachLink(AnimationController _animationController, OffMeshLinkEntity _link)
		: base(AnimationStateType.ApproachLink, _animationController)
	{
		m_TargetPosition = _link.StartPosition;
		m_TargetOrientation = _link.Rotation;
	}

	public override void OnDrawGizmos()
	{
	}

	public override void OnEnter()
	{
		base.OnEnter();
		AnimationController.Agent.Stop();
		AnimationController.ApplyRootMotion = true;
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		m_faceDirection = m_TargetOrientation * Vector3.forward;
		AnimationController.GetComponent<AnimatorDataController>().RegisterEventListener(this);
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
			StartApproachLink();
		}
	}

	public void StartApproachLink()
	{
		AnimationController.ApplyRootMotion = false;
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
	}

	public override void OnDispose()
	{
		base.OnDispose();
		AnimationController.GetComponent<AnimatorDataController>().UnregisterEventListener(this);
	}

	public override void OnUpdate()
	{
		if (base.IsFinished)
		{
			return;
		}
		if (m_waitForTurn)
		{
			float num = Damping.Damp(m_turnCorrectionAngle, 0f, 0.2f, 1f);
			AnimationController.NextRotation = Quaternion.Euler(0f, m_turnCorrectionAngle - num, 0f) * AnimationController.NextRotation;
			m_turnCorrectionAngle = num;
			return;
		}
		if (m_UpdateRotation || m_UpdateTranslation)
		{
			if (m_UpdateTranslation)
			{
				AnimationController.CorrectPosition(m_TargetPosition, SlideDamping);
				m_UpdateTranslation = (m_TargetPosition - AnimationController.NextPosition).magnitude > AnimationController.TranslateMargin;
			}
			if (m_UpdateRotation)
			{
				AnimationController.FaceDirection(m_TargetOrientation * Vector3.forward, SlideDamping);
				m_UpdateRotation = Quaternion.Angle(AnimationController.NextRotation, m_TargetOrientation) > AnimationController.RotationMargin;
			}
		}
		base.IsFinished = !m_UpdateRotation && !m_UpdateTranslation;
	}

	public override bool CanCancel()
	{
		return false;
	}

	public bool OnAnimatorEvent(AnimatorEvent _event)
	{
		AnimatorEventType type = _event.Type;
		if (type == AnimatorEventType.TurnEnd)
		{
			if (m_waitForTurn)
			{
				StartApproachLink();
			}
			m_waitForTurn = false;
		}
		return false;
	}
}
