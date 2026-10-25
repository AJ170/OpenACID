using Assets.Scripts.Animation;
using UnityEngine;

public class AnimationTaskAssassinateStealthInstigator : AnimationTaskBase, IAnimatorEventListener
{
	public class Parameters : AnimationTaskParameters
	{
		public GameObject Instigator { get; set; }

		public GameObject Target { get; set; }

		public Parameters()
			: base(sTaskType)
		{
		}
	}

	public static readonly AnimationTaskType sTaskType = AnimationTaskType.AssassinateStealthInstigator;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private bool m_triggeredIdle;

	private bool m_enteredIdle;

	private bool m_cameraStarted;

	private bool m_realignmentDone;

	private bool m_initiated;

	private AnimationController.Direction m_finalDirection;

	public override AnimationTaskType TaskType
	{
		get
		{
			return sTaskType;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public GameObject Target { get; private set; }

	public GameObject Instigator { get; private set; }

	public AnimationTaskAssassinateStealthInstigator()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskAssassinateStealthInstigator(Parameters _parameters)
		: base(_parameters)
	{
		Instigator = _parameters.Instigator;
		Target = _parameters.Target;
		AnimationController.ApplyRootMotion = false;
		AnimationController.StateMachine.StateEntered += OnStateEntered;
		AnimationController.StateMachine.StateMachineEntered += OnStateMachineEntered;
		AnimationController.StateMachine.StateMachineExited += OnStateMachineExited;
		AnimationController.DataController.RegisterEventListener(this);
		AnimationController.Agent.Stop();
		AnimationController.Agent.ResetPath();
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
	}

	private void OnStateEntered(object sender, AnimatorStateMachine.StateEventArgs _event)
	{
		if (_event.State.NameHash == AnimationStateHashes.State_HighProfile_Idle_Footl.NameHash)
		{
			m_enteredIdle = true;
		}
	}

	private void OnStateMachineEntered(object _sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		if (_event.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_AssassinationStealth))
		{
			AnimationController.Animator.SetCurrentActionType(ActionType.Default);
			AnimationController.StateMachine.StateMachineEntered -= OnStateMachineEntered;
		}
	}

	private void OnStateMachineExited(object _sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		if (_event.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_AssassinationStealth))
		{
			AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
			base.IsFinished = true;
			AnimationController.StateMachine.StateMachineEntered -= OnStateMachineExited;
		}
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (m_realignmentDone && !m_initiated)
		{
			m_initiated = true;
			AnimationController.ApplyRootMotion = true;
			AnimationController.Animator.SetCurrentActionType(ActionType.AssassinateStealth);
			AnimationController.Animator.SetTargetDirection(GetFinalDirection());
		}
		if (!m_triggeredIdle)
		{
			AnimationController.Animator.CrossFade(AnimationStateHashes.State_HighProfile_Idle_Footl.NameHash, 0.1f);
			m_triggeredIdle = true;
		}
		if (m_cameraStarted && !m_realignmentDone)
		{
			UpdatePosition(1f);
			UpdateRotation(1f);
			m_realignmentDone = true;
		}
		if (!m_enteredIdle)
		{
			int fullPathHash = AnimationController.Animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
			if (fullPathHash == AnimationStateHashes.State_HighProfile_Idle_Footl.NameHash)
			{
				m_enteredIdle = true;
			}
		}
		if (!m_cameraStarted && m_enteredIdle)
		{
			CameraStateHelper.OnCameraStart();
			m_cameraStarted = true;
		}
	}

	public override bool CanCancel()
	{
		return false;
	}

	public override void OnDispose()
	{
		base.OnDispose();
		AnimationController.DataController.UnregisterEventListener(this);
		AnimationController.StateMachine.StateMachineEntered -= OnStateMachineEntered;
		AnimationController.StateMachine.StateMachineExited -= OnStateMachineExited;
		AnimationController.StateMachine.StateEntered -= OnStateEntered;
	}

	public bool OnAnimatorEvent(AnimatorEvent _event)
	{
		AnimatorEventType type = _event.Type;
		if (type == AnimatorEventType.CameraEnd)
		{
			CameraStateHelper.OnCameraEnd();
		}
		return false;
	}

	private Vector3 GetTargetPosition()
	{
		Vector3 vector = Instigator.transform.position - Target.transform.position;
		vector.Normalize();
		switch (GetFinalDirection())
		{
		case AnimationController.Direction.Left:
			vector *= 0.95f;
			vector += new Vector3(0.2f, 0f);
			break;
		case AnimationController.Direction.Right:
			vector *= 0.9f;
			break;
		case AnimationController.Direction.Back:
			vector *= 0.8f;
			vector += new Vector3(0.1f, 0f);
			break;
		case AnimationController.Direction.Front:
			vector *= 1f;
			break;
		}
		return Target.transform.position + vector;
	}

	public AnimationController.Direction GetFinalDirection()
	{
		if (m_finalDirection == AnimationController.Direction.Unknown)
		{
			m_finalDirection = AnimationHelper.DetermineDirection(Instigator, Target);
		}
		return m_finalDirection;
	}

	private void UpdateRotation(float _scalar)
	{
		Vector3 forward = Target.transform.position - Instigator.transform.position;
		forward.y = 0f;
		AnimationController.NextRotation = Quaternion.Slerp(Instigator.transform.rotation, Quaternion.LookRotation(forward), _scalar);
	}

	private void UpdatePosition(float _scalar)
	{
		Vector3 position = Instigator.transform.position;
		Vector3 targetPosition = GetTargetPosition();
		Vector3 vector = targetPosition - position;
		AnimationController.NextPosition = position + vector * _scalar;
	}
}
