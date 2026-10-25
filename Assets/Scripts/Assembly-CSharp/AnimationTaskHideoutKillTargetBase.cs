using Assets.Scripts.Animation;
using UnityEngine;

public class AnimationTaskHideoutKillTargetBase : AnimationTaskBase, IAnimatorEventListener
{
	public class Parameters : AnimationTaskParameters
	{
		public GameObject Instigator { get; set; }

		public GameObject Target { get; set; }

		public float DistanceHideoutToTarget { get; set; }

		public Parameters(AnimationTaskType _taskType)
			: base(_taskType)
		{
		}
	}

	public readonly AnimationTaskType sTaskType;

	private GameObject m_Instigator;

	private GameObject m_Target;

	private Quaternion m_InitialRotation = Quaternion.identity;

	private Quaternion m_TargetRotation = Quaternion.identity;

	private bool m_reactionStarted;

	private float m_SlerpDurationSec = 0.2f;

	private float m_SlerpCurrent;

	private bool m_StartSlerp;

	private AnimationController.Direction m_TargetDir;

	private float m_DistanceHideoutToTarget;

	public override AnimationTaskType TaskType
	{
		get
		{
			return sTaskType;
		}
	}

	public AnimationTaskHideoutKillTargetBase(Parameters _parameters, AnimationTaskType _taskType)
		: base(_parameters)
	{
		sTaskType = _taskType;
		m_Instigator = _parameters.Instigator;
		m_Target = _parameters.Target;
		m_DistanceHideoutToTarget = _parameters.DistanceHideoutToTarget;
		m_Instigator.GetComponent<AnimationController>().DataController.RegisterEventListener(this);
		AnimationController.DataController.RegisterEventListener(this);
		AnimationController.StateMachine.StateEntered += OnStateEntered;
		AnimationController.Agent.Stop();
		AnimationController.ApplyRootMotion = false;
		AnimationController.Animator.CrossFade(AnimationStateHashes.State_LowProfile_Idle_Footm.NameHash, 0f);
	}

	private void OnStateEntered(object sender, AnimatorStateMachine.StateEventArgs _event)
	{
		if (AnimationStateHashes.IsStateContainedInArray(_event.State, AnimationStateHashes.States_HayStackGetKilled))
		{
			AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
			AnimationController.StateMachine.StateEntered -= OnStateEntered;
			m_DistanceHideoutToTarget = Mathf.Clamp(m_DistanceHideoutToTarget, AnimationController.HaystackTargetDistanceMin, AnimationController.HaystackTargetDistanceMax);
			AnimationController.Animator.SetDistanceToTarget(Mathf.Clamp01((m_DistanceHideoutToTarget - AnimationController.HaystackTargetDistanceMin) / (AnimationController.HaystackTargetDistanceMax - AnimationController.HaystackTargetDistanceMin)));
		}
	}

	public override void OnEnter()
	{
		base.OnEnter();
		m_StartSlerp = true;
		m_TargetDir = AnimationHelper.DetermineDirection(m_Instigator, m_Target);
		m_InitialRotation = m_Target.transform.rotation;
		m_TargetRotation = AnimationHelper.AlignTargetToTargetDirection(m_TargetDir, m_Instigator, m_Target);
	}

	private void OnStartReaction()
	{
		if (!m_reactionStarted)
		{
			AnimationController.Agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
			AnimationController.Agent.updatePosition = false;
			AnimationController.Agent.updateRotation = false;
			AnimationController.Animator.SetCurrentReactionType(ReactionType.GetKilledFromHaystack);
			AnimationController.Animator.SetTargetDirection(m_TargetDir);
			AnimationController.ApplyRootMotion = true;
			m_reactionStarted = true;
		}
	}

	public virtual bool OnAnimatorEvent(AnimatorEvent _event)
	{
		AnimatorEventType type = _event.Type;
		if (type == AnimatorEventType.ReactionStart)
		{
			OnStartReaction();
		}
		return false;
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
		if (!base.IsFinished)
		{
			AnimatorStateInfo currentAnimatorStateInfo = AnimationController.Animator.GetCurrentAnimatorStateInfo(0);
			if (AnimationStateHashes.IsStateContainedInArray(currentAnimatorStateInfo.fullPathHash, AnimationStateHashes.States_HayStackReactionFallIntoHaystack))
			{
				base.IsFinished = currentAnimatorStateInfo.normalizedTime > 0.9f;
			}
		}
	}

	public override void OnDispose()
	{
		base.OnDispose();
		m_Instigator.GetComponent<AnimationController>().DataController.UnregisterEventListener(this);
		AnimationController.DataController.UnregisterEventListener(this);
	}

	public override bool CanCancel()
	{
		return false;
	}
}
