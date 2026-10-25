using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;

public class AnimationTaskBenchKillTarget : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public Bench Bench { get; set; }

		public GameObject Target { get; set; }

		public float DistanceToTarget { get; set; }

		public float DistanceToTargetNormalized { get; set; }

		public AnimationController.Direction Direction { get; set; }

		public Parameters()
			: base(AnimationTaskType.BenchKillTarget)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.BenchKillTarget;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private Vector3 m_initialPos;

	private Quaternion m_initialRotation;

	private AcierCameraController m_cameraController;

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.BenchKillTarget;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskBenchKillTarget()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskBenchKillTarget(Parameters _parameters)
		: base(_parameters)
	{
		m_cameraController = AcierCameraController.Instance;
		m_cameraController.CameraStateStarted += OnCameraStateStarted;
		AnimationController.Agent.enabled = false;
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		Bench.Seat seat = _parameters.Bench.GetSeat(Bench.ESeats.Player);
		m_initialRotation = seat.Orientation;
		if (_parameters.Direction == AnimationController.Direction.Front)
		{
			m_initialRotation *= Quaternion.AngleAxis(180f, Vector3.up);
		}
		Vector3 initialPos = seat.Position + seat.Orientation * Vector3.forward * _parameters.DistanceToTarget;
		initialPos += seat.Orientation * Vector3.right * 0.1f;
		initialPos.y = AnimationController.NextPosition.y;
		m_initialPos = initialPos;
		AnimationController.ApplyRootMotion = false;
		AnimationController.Animator.SetTargetDirection(_parameters.Direction);
		AnimationController.Animator.SetDistanceToTarget(_parameters.DistanceToTargetNormalized);
		AnimationController.StateMachine.StateMachineEntered += OnStateMachineEntered;
		AnimationController.Animator.CrossFade(AnimationStateHashes.State_LowProfile_Idle_Footm.NameHash, 0f);
	}

	public override bool CanCancel()
	{
		return false;
	}

	private void OnStateMachineEntered(object _sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		if (_event.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_GetKilledFromBench))
		{
			AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
			AnimationController.StateMachine.StateMachineEntered -= OnStateMachineEntered;
		}
	}

	private void OnCameraStateStarted(object _sender, AcierCameraController.CameraStateEventArgs _args)
	{
		if (_args.CameraState.StateType == AcierCameraController.ECameraState.BenchAssassination)
		{
			AnimationController.ApplyRootMotion = true;
			AnimationController.transform.position = m_initialPos;
			AnimationController.NextPosition = m_initialPos;
			AnimationController.transform.rotation = m_initialRotation;
			AnimationController.NextRotation = m_initialRotation;
			AnimationController.Animator.SetCurrentReactionType(ReactionType.GetKilledFromBench);
			m_cameraController.CameraStateStarted -= OnCameraStateStarted;
		}
	}

	public override void OnDispose()
	{
		base.OnDispose();
		m_cameraController.CameraStateStarted -= OnCameraStateStarted;
		AnimationController.StateMachine.StateMachineEntered -= OnStateMachineEntered;
	}
}
