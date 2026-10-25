using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;

public class AnimationTaskBlackjackTarget : AnimationTaskBase, IAnimatorEventListener
{
	public class Parameters : AnimationTaskParameters
	{
		public EntityId Instigator { get; set; }

		public EntityId Target { get; set; }

		public AnimationController.Direction TargetDir { get; set; }

		public Parameters()
			: base(AnimationTaskType.BlackjackTarget)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.BlackjackTarget;

	private const float SlerpDurationSec = 0.2f;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private float m_slerpCurrent;

	private bool m_startSlerp;

	private Quaternion m_initialRotation = Quaternion.identity;

	private Quaternion m_targetRotation = Quaternion.identity;

	private EntityId m_instigator = EntityId.InvalidId;

	private readonly Character m_targetChar;

	private readonly AnimationController.Direction m_targetDir;

	private bool m_reactionStarted;

	private bool m_rootMotion;

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.BlackjackTarget;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskBlackjackTarget()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskBlackjackTarget(Parameters _parameters)
		: base(_parameters)
	{
		m_instigator = _parameters.Instigator;
		m_targetChar = _parameters.Target.GetEntity<Character>();
		m_targetDir = _parameters.TargetDir;
		m_instigator.GetEntity<Character>().UnityComponentCache.AnimationController.DataController.RegisterEventListener(this);
		AnimationController.StateMachine.StateMachineEntered += OnStateMachineEntered;
		AnimationController.Agent.Stop();
		AnimationController.Agent.ResetPath();
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		AnimationController.ApplyRootMotion = false;
	}

	private void OnStateMachineEntered(object _sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		if (_event.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_BlackjackReaction))
		{
			AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
			AnimationController.StateMachine.StateMachineEntered -= OnStateMachineEntered;
		}
	}

	public bool OnAnimatorEvent(AnimatorEvent _event)
	{
		if (!AnimationStateHashes.IsStateContainedInArray(_event.StateNameHash, AnimationStateHashes.States_Blackjack))
		{
			return false;
		}
		AnimatorEventType type = _event.Type;
		if (type == AnimatorEventType.ReactionStart)
		{
			OnStartReaction();
		}
		return false;
	}

	private void OnStartReaction()
	{
		if (!m_reactionStarted)
		{
			CameraStateHelper.OnCameraStart();
			m_rootMotion = AnimationController.ApplyRootMotion;
			AnimationController.ApplyRootMotion = true;
			AnimationController.Animator.SetCurrentReactionType(ReactionType.BlackjackReaction);
			AnimationController.Animator.SetTargetDirection(m_targetDir);
			AnimationController.Animator.SetCurrentStopAction(false);
			m_reactionStarted = true;
		}
	}

	public override void OnEnter()
	{
		base.OnEnter();
		m_startSlerp = true;
		AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
		m_initialRotation = AnimationController.NextRotation;
		Character character = m_instigator.TryGetEntity<Character>();
		if (character != null && character.IsAttached)
		{
			m_targetRotation = AnimationHelper.AlignTargetToTargetDirection(m_targetDir, character.GameObject, m_targetChar.GameObject);
		}
		else
		{
			m_targetRotation = m_initialRotation;
		}
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		float num = Mathf.Clamp01(m_slerpCurrent / 0.2f);
		if (m_startSlerp && num < 1f)
		{
			AnimationController.NextRotation = Quaternion.Slerp(m_initialRotation, m_targetRotation, num);
			m_slerpCurrent += LogicMain.FrameTime;
		}
		AnimationHelper.StayOnNavMesh(m_targetChar);
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.Animator.SetCurrentStopAction(true);
		AnimationController.ApplyRootMotion = m_rootMotion;
	}

	public override bool CanCancel()
	{
		return m_reactionStarted;
	}

	public override void OnDispose()
	{
		base.OnDispose();
		Character character = m_instigator.TryGetEntity<Character>();
		if (character != null && character.IsAttached)
		{
			character.UnityComponentCache.AnimationController.DataController.UnregisterEventListener(this);
		}
	}
}
