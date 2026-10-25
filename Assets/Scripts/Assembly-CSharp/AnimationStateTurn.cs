using Assets.Scripts.Animation;
using UnityEngine;

public class AnimationStateTurn : AnimationStateBase, IAnimatorEventListener
{
	public static float TurnMinAngle = 45f;

	private readonly Vector3 m_targetDirection;

	private bool m_waitForTurn;

	private NpcTurnDirection m_turnDirection;

	private float m_turnCorrectionAngle;

	public AnimationStateTurn(AnimationController _animationController, Vector3 _targetDirection)
		: base(AnimationStateType.Turn, _animationController)
	{
		m_targetDirection = _targetDirection;
		AnimationController.GetComponent<AnimatorDataController>().RegisterEventListener(this);
	}

	public override void OnEnter()
	{
		base.OnEnter();
		AnimationController.ApplyRootMotion = true;
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		float num = Vector3.Angle(m_targetDirection, AnimationController.transform.forward) * Mathf.Sign(Vector3.Dot(m_targetDirection, AnimationController.transform.right));
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
			base.IsFinished = true;
		}
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.GetComponent<AnimatorDataController>().UnregisterEventListener(this);
	}

	public override void OnDispose()
	{
		base.OnDispose();
		AnimationController.GetComponent<AnimatorDataController>().UnregisterEventListener(this);
	}

	public override void OnUpdate()
	{
		if (m_waitForTurn)
		{
			float num = Damping.Damp(m_turnCorrectionAngle, 0f, 0.2f, 1f);
			AnimationController.NextRotation = Quaternion.Euler(0f, m_turnCorrectionAngle - num, 0f) * AnimationController.NextRotation;
			m_turnCorrectionAngle = num;
		}
	}

	public override bool CanCancel()
	{
		return false;
	}

	public bool OnAnimatorEvent(AnimatorEvent _event)
	{
		if (_event.Type == AnimatorEventType.TurnEnd && m_waitForTurn)
		{
			base.IsFinished = true;
			m_waitForTurn = false;
		}
		return false;
	}
}
