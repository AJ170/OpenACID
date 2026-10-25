using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;
using UnityEngine.AI;

public class AnimationTaskAssassinateStealthTarget : AnimationTaskBase, IAnimatorEventListener
{
	public class Parameters : AnimationTaskParameters
	{
		public Character Instigator { get; set; }

		public Character Target { get; set; }

		public Parameters()
			: base(sTaskType)
		{
		}
	}

	public static readonly AnimationTaskType sTaskType = AnimationTaskType.AssassinateStealthTarget;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private Character m_Instigator;

	private Character m_Target;

	private bool m_reactionStarted;

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

	public AnimationTaskAssassinateStealthTarget()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskAssassinateStealthTarget(Parameters _parameters)
		: base(_parameters)
	{
		m_Instigator = _parameters.Instigator;
		m_Target = _parameters.Target;
		m_Instigator.UnityComponentCache.AnimationController.DataController.RegisterEventListener(this);
		AnimationController.StateMachine.StateMachineEntered += OnStateMachineEntered;
		AnimationController.Agent.Stop();
		AnimationController.Agent.ResetPath();
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		AnimationController.Agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
		AnimationController.ApplyRootMotion = false;
	}

	private void OnStateMachineEntered(object sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		if (_event.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_GetAssassinatedStealth))
		{
			AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
			AnimationController.StateMachine.StateMachineEntered -= OnStateMachineEntered;
		}
	}

	public bool OnAnimatorEvent(AnimatorEvent _event)
	{
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
			AnimationTaskAssassinateStealthInstigator animationTaskAssassinateStealthInstigator = m_Instigator.UnityComponentCache.AnimationController.TaskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base) as AnimationTaskAssassinateStealthInstigator;
			AnimationController.Direction finalDirection = animationTaskAssassinateStealthInstigator.GetFinalDirection();
			Quaternion quaternion = AnimationHelper.AlignTargetToTargetDirection(finalDirection, m_Instigator.GameObject, m_Target.GameObject);
			AnimationController.gameObject.transform.rotation = quaternion;
			AnimationController.NextRotation = quaternion;
			AnimationController.Animator.SetCurrentReactionType(ReactionType.GetAssassinatedStealth);
			AnimationController.Animator.SetTargetDirection(finalDirection);
			AnimationController.ApplyRootMotion = true;
			m_reactionStarted = true;
		}
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		AnimatorStateInfo currentAnimatorStateInfo = AnimationController.Animator.GetCurrentAnimatorStateInfo(0);
		OnUpdateNavMeshAlignment(currentAnimatorStateInfo);
		AnimationController.CorrectPosition();
		base.IsFinished = currentAnimatorStateInfo.normalizedTime >= 1f && AnimationController.StateMachine.GetCurrentStateMachineHash() == AnimationStateMachineHashes.StateMachine_GetAssassinatedStealth.NameHash;
	}

	private void OnUpdateNavMeshAlignment(AnimatorStateInfo _currentState)
	{
		float num = 0.75f;
		float num2 = 0f;
		if (m_reactionStarted && AnimationController.StateMachine.GetCurrentStateMachineHash() == AnimationStateMachineHashes.StateMachine_GetAssassinatedStealth.NameHash && _currentState.normalizedTime >= num)
		{
			num2 = (_currentState.normalizedTime - num) / (1f - num);
			AnimationHelper.AlignEntityToNavMeshPlane(AnimationController.EntityId, AnimationController.Agent.areaMask, num2);
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
