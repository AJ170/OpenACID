using Assets.Scripts.Animation;
using UnityEngine;
using UnityEngine.AI;

public class AnimationTaskAssassinateAirInstigator : AnimationTaskBase, IAnimatorEventListener
{
	public class Parameters : AnimationTaskParameters
	{
		public GameObject Instigator { get; set; }

		public GameObject Target { get; set; }

		public bool WithKillCam { get; set; }

		public Parameters()
			: base(AnimationTaskType.AssassinateAirInstigator)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.AssassinateAirInstigator;

	public static Vector3 FirstMatchOffset = new Vector3(0f, 2.6f, -0.2f);

	public static Vector3 SecondMatchOffset = new Vector3(-0.05f, -0.05f, 0.2f);

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private Quaternion m_targetRotation = Quaternion.identity;

	private AnimationController.Direction m_targetDir;

	private Vector3 m_targetPosition = Vector3.zero;

	private Quaternion m_targetOrientation = Quaternion.identity;

	private bool m_firstMatch = true;

	private bool m_allowAlignment;

	private bool m_askMatch;

	private AvatarTarget m_matchBodyPart;

	private bool m_reactionStarted;

	private Vector3 m_matchPosition = Vector3.zero;

	private Transform m_matchTransform;

	private static MatchTargetWeightMask s_weightMask = new MatchTargetWeightMask(new Vector3(1f, 1f, 1f), 0f);

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.AssassinateAirInstigator;
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

	public AnimationTaskAssassinateAirInstigator()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskAssassinateAirInstigator(Parameters _parameters)
		: base(_parameters)
	{
		Instigator = _parameters.Instigator;
		Target = _parameters.Target;
		m_targetPosition = Target.transform.position;
		AnimationController.StateMachine.StateMachineEntered += OnStateMachineEntered;
		AnimationController.StateMachine.StateMachineExited += OnStateMachineExited;
		AnimationController.DataController.RegisterEventListener(this);
		m_targetDir = AnimationHelper.DetermineDirection(Instigator, Target);
		m_targetOrientation = AnimationHelper.AlignTargetToTargetDirection(m_targetDir, Instigator, Target);
	}

	public override void OnEnter()
	{
		base.OnEnter();
		AnimationController.ApplyRootMotion = true;
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		AnimationController.Agent.Stop();
		AnimationController.Agent.ResetPath();
		Vector3 vector = Instigator.transform.position - Target.transform.position;
		Vector3 vector2 = vector;
		vector2.y = 0f;
		float magnitude = vector.magnitude;
		AnimationController.Animator.SetCurrentActionType(ActionType.AssassinateAir);
		AnimationController.Animator.SetCurrentJumpDistanceY(vector.y);
		AnimationController.Animator.SetCurrentJumpDistanceZ(magnitude);
		m_targetRotation = AnimationHelper.AlignInstigatorToTarget(Instigator, Target);
	}

	private void OnStateMachineEntered(object _sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		if (_event.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_AssassinationAir))
		{
			AnimationController.Animator.SetCurrentActionType(ActionType.Default);
			AnimationController.StateMachine.StateMachineEntered -= OnStateMachineEntered;
		}
	}

	private void OnStateMachineExited(object _sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		if (_event.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_AssassinationAir))
		{
			AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
			base.IsFinished = true;
			AnimationController.StateMachine.StateMachineEntered -= OnStateMachineExited;
		}
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (!m_reactionStarted)
		{
			AnimationController.FaceDirection(m_targetRotation * Vector3.forward);
		}
		if (m_askMatch)
		{
			TryStartMatching();
		}
		if (AnimationController.Animator.isMatchingTarget && m_matchTransform != null)
		{
			Vector3 position = m_matchTransform.position;
			Vector3 vector = m_matchPosition - position;
			if (m_reactionStarted && m_allowAlignment)
			{
				AnimationHelper.AlignEntityToNavMeshPlane(AnimationController.EntityId, AnimationController.Agent.areaMask, LogicMain.FrameTime / (LogicMain.FrameTime + 0.1f * Mathf.Clamp01(vector.magnitude)));
			}
		}
	}

	private void TryStartMatching()
	{
		float _matchStart;
		float _matchEnd;
		bool matchTargetData = AnimationHelper.GetMatchTargetData(AnimationController, out _matchStart, out _matchEnd, 0);
		AnimatorStateInfo currentAnimatorStateInfo = AnimationController.Animator.GetCurrentAnimatorStateInfo(0);
		if (matchTargetData)
		{
			m_matchTransform = AnimationController.Agent.transform;
			if (m_firstMatch)
			{
				m_matchPosition = m_targetPosition + m_targetRotation * FirstMatchOffset;
				m_firstMatch = false;
			}
			else
			{
				m_matchPosition = m_targetPosition + m_targetRotation * SecondMatchOffset;
				m_allowAlignment = true;
			}
			AnimationController.Animator.MatchTarget(m_matchPosition, Quaternion.identity, m_matchBodyPart, s_weightMask, currentAnimatorStateInfo.normalizedTime, _matchEnd);
			m_askMatch = false;
		}
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
	}

	public override void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawCube(Instigator.transform.position, new Vector3(0.25f, 0.25f, 0.25f));
		Gizmos.DrawCube(Target.transform.position, new Vector3(0.25f, 0.25f, 0.25f));
		Gizmos.color = Color.red;
		if (AnimationController.Agent.hasPath)
		{
			NavMeshPath path = AnimationController.Agent.path;
			Vector3[] corners = path.corners;
			Gizmos.DrawCube(corners[corners.Length - 1], new Vector3(0.25f, 0.25f, 0.25f));
		}
	}

	public override void OnDispose()
	{
		base.OnDispose();
		AnimationController.DataController.UnregisterEventListener(this);
		AnimationController.StateMachine.StateMachineEntered -= OnStateMachineEntered;
		AnimationController.StateMachine.StateMachineExited -= OnStateMachineExited;
		AnimationController.Agent.Warp(m_matchPosition);
	}

	public bool OnAnimatorEvent(AnimatorEvent _event)
	{
		switch (_event.Type)
		{
		case AnimatorEventType.CameraStart:
			CameraStateHelper.OnCameraStart();
			break;
		case AnimatorEventType.CameraEnd:
			CameraStateHelper.OnCameraEnd();
			break;
		case AnimatorEventType.MatchStart:
			if (!m_firstMatch)
			{
				AnimationController.FaceDirection(m_targetRotation * Vector3.forward, 0f);
				AnimationController.ApplyRootMotion = true;
			}
			m_askMatch = true;
			TryStartMatching();
			break;
		case AnimatorEventType.ReactionStart:
			m_reactionStarted = true;
			break;
		}
		return false;
	}

	public override bool CanCancel()
	{
		return false;
	}

	public Vector3 GetInitialTargetPos()
	{
		return m_targetPosition;
	}

	public AnimationController.Direction GetTargetDirection()
	{
		return m_targetDir;
	}

	public void GetCameraRef(out Vector3 _pos, out Quaternion _rot)
	{
		_pos = m_targetPosition + m_targetOrientation * Vector3.right * -0.057f + m_targetOrientation * Vector3.forward * -0.764f;
		_rot = m_targetOrientation;
	}
}
