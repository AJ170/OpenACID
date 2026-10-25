using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;

public class AnimationTaskBenchKillInstigator : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public GameObject Instigator { get; set; }

		public GameObject Target { get; set; }

		public float DistanceToTarget { get; set; }

		public float DistanceToTargetNormalized { get; set; }

		public AnimationController.Direction Direction { get; set; }

		public Parameters()
			: base(AnimationTaskType.BenchKillInstigator)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.BenchKillInstigator;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private GameEntity m_targetEntity;

	private AcierCameraController m_cameraController;

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.BenchKillInstigator;
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

	public AnimationTaskBenchKillInstigator()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskBenchKillInstigator(Parameters _parameters)
		: base(_parameters)
	{
		m_cameraController = AcierCameraController.Instance;
		m_cameraController.CameraStateStarted += OnCameraStateStarted;
		Instigator = _parameters.Instigator;
		Target = _parameters.Target;
		EntityWrapperComponent component = Target.GetComponent<EntityWrapperComponent>();
		m_targetEntity = component.GetEntity<GameEntity>();
		m_targetEntity.CanBePooled = false;
		AnimationController.ApplyRootMotion = true;
		AnimationController.Animator.SetTargetDirection(_parameters.Direction);
		AnimationController.Animator.SetDistanceToTarget(_parameters.DistanceToTargetNormalized);
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		AnimationController.StateMachine.StateMachineExited += OnStateMachineExited;
	}

	public override bool CanCancel()
	{
		return false;
	}

	private void OnStateMachineExited(object _sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		if (_event.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_SitOnBench))
		{
			base.IsFinished = true;
			AnimationController.Animator.SetCurrentActionType(ActionType.Invalid);
			AnimationController.StateMachine.StateMachineExited -= OnStateMachineExited;
			m_targetEntity.CanBePooled = true;
			m_targetEntity = null;
		}
	}

	public override void OnDispose()
	{
		base.OnDispose();
		m_cameraController.CameraStateStarted -= OnCameraStateStarted;
		AnimationController.StateMachine.StateMachineExited -= OnStateMachineExited;
		if (m_targetEntity != null)
		{
			m_targetEntity.CanBePooled = true;
			m_targetEntity = null;
		}
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.Agent.enabled = true;
	}

	private void OnCameraStateStarted(object _sender, AcierCameraController.CameraStateEventArgs _args)
	{
		if (_args.CameraState.StateType == AcierCameraController.ECameraState.BenchAssassination)
		{
			AnimationController.Animator.SetCurrentActionType(ActionType.KillFromBench);
			m_cameraController.CameraStateStarted -= OnCameraStateStarted;
		}
	}
}
