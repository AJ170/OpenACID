using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;

public class AnimationTaskAssassinateLeapTarget : AnimationTaskBase, IAnimatorEventListener
{
	public class Parameters : AnimationTaskParameters
	{
		public Character Instigator { get; set; }

		public Character Target { get; set; }

		public Parameters()
			: base(AnimationTaskType.AssassinateLeapTarget)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.AssassinateLeapTarget;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private float m_SlerpDurationSec = 0.2f;

	private float m_SlerpCurrent;

	private bool m_StartSlerp;

	private Quaternion m_InitialRotation = Quaternion.identity;

	private Quaternion m_TargetRotation = Quaternion.identity;

	private Character m_Instigator;

	private Character m_Target;

	private AnimationController.Direction m_TargetDir;

	private bool m_reactionStarted;

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.AssassinateLeapTarget;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskAssassinateLeapTarget()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskAssassinateLeapTarget(Parameters _parameters)
		: base(_parameters)
	{
		m_Instigator = _parameters.Instigator;
		m_Target = _parameters.Target;
		m_Instigator.UnityComponentCache.AnimationController.DataController.RegisterEventListener(this);
		AnimationController.StateMachine.StateMachineEntered += OnStateMachineEntered;
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		AnimationController.Agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
	}

	private void OnStateMachineEntered(object sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		if (_event.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_GetAssassinatedLeap))
		{
			AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
			AnimationController.StateMachine.StateMachineEntered -= OnStateMachineEntered;
		}
	}

	public bool OnAnimatorEvent(AnimatorEvent _event)
	{
		switch (_event.Type)
		{
		default:
			return false;
		}
	}

	private void OnStartReaction()
	{
		if (!m_reactionStarted)
		{
			CameraStateHelper.OnCameraStart();
			AnimationController.Animator.SetCurrentReactionType(ReactionType.GetAssassinatedLeap);
			AnimationController.Animator.SetTargetDirection(AnimationHelper.DetermineDirection(m_Instigator.GameObject, m_Target.GameObject));
			AnimationController.ApplyRootMotion = true;
			m_reactionStarted = true;
		}
	}

	public override void OnEnter()
	{
		base.OnEnter();
		m_StartSlerp = true;
		AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
		m_TargetDir = AnimationHelper.DetermineDirection(m_Instigator.GameObject, m_Target.GameObject);
		m_InitialRotation = AnimationController.NextRotation;
		m_TargetRotation = AnimationHelper.AlignTargetToTargetDirection(m_TargetDir, m_Instigator.GameObject, m_Target.GameObject);
		OnStartReaction();
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		float num = Mathf.Clamp01(m_SlerpCurrent / m_SlerpDurationSec);
		if (m_StartSlerp && num < 1f)
		{
			AnimationController.NextRotation = Quaternion.Slerp(m_InitialRotation, m_TargetRotation, num);
			m_SlerpCurrent += LogicMain.FrameTime;
		}
		AnimationController.CorrectPosition();
		if (m_reactionStarted && AnimationController.StateMachine.GetCurrentStateMachineHash() == AnimationStateMachineHashes.StateMachine_GetAssassinatedLeap.NameHash)
		{
			AnimatorStateInfo currentAnimatorStateInfo = AnimationController.Animator.GetCurrentAnimatorStateInfo(0);
			AnimationHelper.AlignEntityToNavMeshPlane(AnimationController.EntityId, AnimationController.Agent.areaMask, currentAnimatorStateInfo.normalizedTime);
			base.IsFinished = currentAnimatorStateInfo.normalizedTime >= 1f;
		}
	}

	public override bool CanCancel()
	{
		return false;
	}

	public override void OnDispose()
	{
		base.OnDispose();
		if (m_Instigator.IsValid && m_Instigator.IsAttached)
		{
			m_Instigator.UnityComponentCache.AnimationController.DataController.UnregisterEventListener(this);
		}
	}
}
