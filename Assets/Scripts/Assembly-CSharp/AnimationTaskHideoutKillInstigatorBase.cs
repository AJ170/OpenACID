using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic.Components;
using UnityEngine;

public class AnimationTaskHideoutKillInstigatorBase : AnimationTaskBase, IAnimatorEventListener
{
	public class Parameters : AnimationTaskParameters
	{
		public GameObject Instigator { get; set; }

		public GameObject Target { get; set; }

		public float DistanceHideoutToTarget { get; set; }

		public InteractionLocators.Locator Locator { get; set; }

		public Parameters(AnimationTaskType _taskType)
			: base(_taskType)
		{
		}
	}

	public readonly AnimationTaskType sTaskType = AnimationTaskType.HayStackKillInstigator;

	private static float[] DistanceInHaystackMin = new float[5] { -1f, 1.15f, 1.15f, 1.15f, 1.15f };

	private static float[] DistanceInHaystackMax = new float[5] { -1f, 1.15f, 1.3f, 1f, 1.3f };

	private Vector3 m_InitialPosition = Vector3.zero;

	private float m_DistanceHayStackToTarget;

	private AnimationController.Direction m_TargetDir;

	public override AnimationTaskType TaskType
	{
		get
		{
			return sTaskType;
		}
	}

	public GameObject Target { get; private set; }

	public GameObject Instigator { get; private set; }

	public AnimationTaskHideoutKillInstigatorBase(Parameters _parameters, AnimationTaskType _taskType)
		: base(_parameters)
	{
		sTaskType = _taskType;
		Target = _parameters.Target;
		Instigator = _parameters.Instigator;
		m_DistanceHayStackToTarget = _parameters.DistanceHideoutToTarget;
		AnimationController.Animator.SetCurrentActionType(ActionType.KillFromHayStack);
		AnimationController.Animator.SetTargetDirection(AnimationHelper.DetermineDirection(Instigator, Target));
		AnimationController.StateMachine.StateEntered += OnStateEntered;
		AnimationController.StateMachine.StateExited += OnStateExited;
		AnimationController.DataController.RegisterEventListener(this);
	}

	private void OnStateEntered(object sender, AnimatorStateMachine.StateEventArgs _event)
	{
		if (_event.State.CompareNameHash(AnimationStateHashes.State_HayStack_Idle))
		{
			base.IsFinished = true;
			AnimationController.StateMachine.StateEntered -= OnStateEntered;
		}
	}

	private void OnStateExited(object sender, AnimatorStateMachine.StateEventArgs _event)
	{
		if (_event.State.CompareNameHash(AnimationStateHashes.State_HayStack_Idle))
		{
			AnimationController.Animator.SetCurrentActionType(ActionType.Invalid);
			AnimationController.StateMachine.StateExited -= OnStateExited;
		}
	}

	public override void OnEnter()
	{
		base.OnEnter();
		m_InitialPosition = AnimationController.NextPosition;
		m_TargetDir = AnimationHelper.DetermineDirection(Instigator, Target);
		m_DistanceHayStackToTarget = Mathf.Clamp(m_DistanceHayStackToTarget, AnimationController.HaystackTargetDistanceMin, AnimationController.HaystackTargetDistanceMax);
		float num = Mathf.Clamp01((m_DistanceHayStackToTarget - AnimationController.HaystackTargetDistanceMin) / (AnimationController.HaystackTargetDistanceMax - AnimationController.HaystackTargetDistanceMin));
		float num2 = DistanceInHaystackMin[(int)m_TargetDir];
		float num3 = DistanceInHaystackMax[(int)m_TargetDir];
		float num4 = num2 + num * (num3 - num2);
		Vector3 vector = Target.transform.position - m_InitialPosition;
		vector.y = 0f;
		vector.Normalize();
		vector *= num4 + m_DistanceHayStackToTarget;
		Vector3 nextPosition = Target.transform.position + -vector;
		nextPosition.y += 0.5f;
		AnimationController.Animator.SetDistanceToTarget(num);
		AnimationController.NextPosition = nextPosition;
		AnimationController.NextRotation = Quaternion.LookRotation(vector);
		AnimationController.ApplyRootMotion = true;
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.NextPosition = m_InitialPosition;
		AnimationController.ApplyRootMotion = false;
	}

	public override void OnDispose()
	{
		base.OnDispose();
		AnimationController.DataController.UnregisterEventListener(this);
	}

	public virtual bool OnAnimatorEvent(AnimatorEvent _event)
	{
		switch (_event.Type)
		{
		case AnimatorEventType.CameraStart:
			CameraStateHelper.OnCameraStart();
			break;
		case AnimatorEventType.CameraEnd:
			CameraStateHelper.OnCameraEnd();
			break;
		}
		return false;
	}

	public override bool CanCancel()
	{
		return false;
	}
}
