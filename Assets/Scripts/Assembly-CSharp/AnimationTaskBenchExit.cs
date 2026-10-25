using Assets.Scripts.Animation;
using UnityEngine;

public class AnimationTaskBenchExit : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public bool StartRunning { get; set; }

		public Vector3 Destination { get; set; }

		public Parameters()
			: base(AnimationTaskType.BenchExit)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.BenchExit;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private bool m_startRunning;

	private Vector3 m_destination;

	private Quaternion m_initialRotation;

	private Quaternion m_targetRotation;

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.BenchExit;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public bool StartRunning
	{
		get
		{
			return m_startRunning;
		}
		private set
		{
			m_startRunning = value;
		}
	}

	public AnimationTaskBenchExit()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskBenchExit(Parameters _parameters)
		: base(_parameters)
	{
		AnimationController.StateMachine.StateMachineExited += OnStateMachineExited;
		m_startRunning = _parameters.StartRunning;
		m_destination = _parameters.Destination;
	}

	public override bool CanCancel()
	{
		return false;
	}

	public override void OnEnter()
	{
		AnimationTaskHideoutExitBase.SetPlayerCondition(EffectManager.ECondition.ExitBench, true);
		base.OnEnter();
		Vector3 vector = m_destination - AnimationController.NextPosition;
		Vector3 forward = AnimationController.transform.forward;
		vector.y = (forward.y = 0f);
		vector.Normalize();
		forward.Normalize();
		float num = Mathf.Clamp(Vector3.Angle(forward, vector), 0f, 90f);
		num *= ((!(Vector3.Cross(forward, vector).y <= 0f)) ? 1f : (-1f));
		m_initialRotation = AnimationController.NextRotation;
		m_targetRotation = Quaternion.LookRotation(Quaternion.AngleAxis(num, AnimationController.transform.up) * forward);
		AnimationController.Animator.SetCurrentActionType(ActionType.GetUpBench);
		AnimationController.Animator.SetCurrentMovementState(m_startRunning ? MovementState.Movement : MovementState.Idle);
	}

	private void OnStateMachineExited(object _sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		if (_event.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_SitOnBench))
		{
			base.IsFinished = true;
			AnimationController.Agent.enabled = true;
			AnimationController.StateMachine.StateMachineExited -= OnStateMachineExited;
		}
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (AnimationController.StateMachine.GetCurrentState() == AnimationStateHashes.State_GetUpBenchRun.NameHash)
		{
			AnimatorStateInfo currentAnimatorStateInfo = AnimationController.Animator.GetCurrentAnimatorStateInfo(0);
			AnimationController.NextRotation = Quaternion.Slerp(m_initialRotation, m_targetRotation, currentAnimatorStateInfo.normalizedTime);
		}
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationTaskHideoutExitBase.SetPlayerCondition(EffectManager.ECondition.ExitBench, false);
	}

	public override void OnDispose()
	{
		base.OnDispose();
		AnimationController.StateMachine.StateMachineExited -= OnStateMachineExited;
	}
}
