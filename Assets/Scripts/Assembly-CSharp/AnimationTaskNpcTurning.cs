using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic;
using UnityEngine;

public class AnimationTaskNpcTurning : AnimationTaskBase, IAnimatorEventListener
{
	public class Parameters : AnimationTaskParameters
	{
		public float TargetAngle { get; set; }

		public IPositionProvider TargetProvider { get; set; }

		public TurnAccuracy TurnAccuracy { get; set; }

		public Parameters()
			: base(AnimationTaskType.NpcTurning)
		{
		}
	}

	public enum TurnAccuracy
	{
		High = 0,
		Low = 1
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.NpcTurning;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	public static readonly float[] MaxFacingAngle = new float[2] { 2.5f, 15f };

	private NpcTurnDirection m_turnDirection;

	private bool m_turning;

	private float m_turnCorrectionAngle;

	private Quaternion m_targetRotation;

	private readonly float m_maxAngle;

	private readonly TurnAccuracy m_turnAccuracy;

	private float m_lastAgentSpeed;

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.NpcTurning;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskNpcTurning()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskNpcTurning(Parameters _parameters)
		: base(_parameters)
	{
		if (_parameters.TargetProvider != null)
		{
			Vector3 forward = _parameters.TargetProvider.Position - AnimationController.transform.position;
			forward.y = 0f;
			forward.Normalize();
			m_targetRotation = Quaternion.LookRotation(forward, Vector3.up);
		}
		else
		{
			m_targetRotation = Quaternion.Euler(0f, _parameters.TargetAngle, 0f);
		}
		m_turnAccuracy = _parameters.TurnAccuracy;
		m_maxAngle = MaxFacingAngle[(int)m_turnAccuracy];
		AnimationController.GetComponent<AnimatorDataController>().RegisterEventListener(this);
	}

	public override void OnEnter()
	{
		base.OnEnter();
		m_lastAgentSpeed = AnimationController.Agent.speed;
		float num = Vector3.Angle(m_targetRotation * Vector3.forward, AnimationController.NextRotation * Vector3.forward) * Mathf.Sign(Vector3.Dot(m_targetRotation * Vector3.forward, AnimationController.NextRotation * Vector3.right));
		if (Mathf.Abs(num) <= m_maxAngle)
		{
			base.IsFinished = true;
			return;
		}
		AnimationController.Agent.updatePosition = true;
		AnimationController.Agent.updateRotation = false;
		AnimationController.ApplyRootMotion = true;
		AnimationController.Agent.speed = 0f;
		m_turnDirection = AnimationHelper.DetermineNpcTurnDirection(0f, num);
		m_turnCorrectionAngle = num - AnimationHelper.NpcTurnDirectionToAngle(m_turnDirection);
		if (m_turnDirection != NpcTurnDirection.Invalid)
		{
			AnimationController.Animator.SetCurrentIdleType(IdleType.Neutral);
			AnimationController.Animator.SetCurrentMovementState(MovementState.Turn);
			AnimationController.Animator.SetCurrentNpcTurnDirection(m_turnDirection);
			m_turning = true;
		}
		else
		{
			AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
			m_turning = false;
		}
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (m_turning)
		{
			float num = Damping.Damp(m_turnCorrectionAngle, 0f, 0.2f, 1f);
			AnimationController.NextRotation = Quaternion.Euler(0f, m_turnCorrectionAngle - num, 0f) * AnimationController.NextRotation;
			m_turnCorrectionAngle = num;
			return;
		}
		AnimationController.FaceDirection(m_targetRotation * Vector3.forward);
		float f = Vector3.Angle(m_targetRotation * Vector3.forward, AnimationController.NextRotation * Vector3.forward) * Mathf.Sign(Vector3.Dot(m_targetRotation * Vector3.forward, AnimationController.NextRotation * Vector3.right));
		if (Mathf.Abs(f) <= m_maxAngle)
		{
			base.IsFinished = true;
		}
	}

	public override void OnDispose()
	{
		base.OnDispose();
		AnimationController.GetComponent<AnimatorDataController>().UnregisterEventListener(this);
		AnimationController.Agent.speed = m_lastAgentSpeed;
	}

	public override bool CanCancel()
	{
		return true;
	}

	public bool OnAnimatorEvent(AnimatorEvent _event)
	{
		AnimatorEventType type = _event.Type;
		if (type == AnimatorEventType.TurnEnd)
		{
			if (m_turning)
			{
				AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
			}
			m_turning = false;
		}
		return false;
	}
}
