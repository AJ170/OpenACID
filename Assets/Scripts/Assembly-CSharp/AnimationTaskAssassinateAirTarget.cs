using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;
using UnityEngine.AI;

public class AnimationTaskAssassinateAirTarget : AnimationTaskBase, IAnimatorEventListener
{
	public class Parameters : AnimationTaskParameters
	{
		public Character Instigator { get; set; }

		public Character Target { get; set; }

		public Parameters()
			: base(AnimationTaskType.AssassinateAirTarget)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.AssassinateAirTarget;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private Quaternion m_TargetRotation = Quaternion.identity;

	private Vector3 m_TargetPosition = Vector3.zero;

	private Character m_Instigator;

	private Character m_Target;

	private AnimationController.Direction m_TargetDir;

	private bool m_reactionStarted;

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.AssassinateAirTarget;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskAssassinateAirTarget()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskAssassinateAirTarget(Parameters _parameters)
		: base(_parameters)
	{
		m_Instigator = _parameters.Instigator;
		m_Target = _parameters.Target;
		m_Instigator.UnityComponentCache.AnimationController.DataController.RegisterEventListener(this);
		AnimationController.StateMachine.StateMachineEntered += OnStateMachineEntered;
		AnimationController.StateMachine.StateMachineExited += OnStateMachineExited;
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		AnimationController.Agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
		AnimationController.ApplyRootMotion = false;
	}

	private void OnStateMachineEntered(object sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		if (_event.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_GetAssassinatedAir))
		{
			AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
			AnimationController.StateMachine.StateMachineEntered -= OnStateMachineEntered;
		}
	}

	private void OnStateMachineExited(object sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		if (_event.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_GetAssassinatedAir))
		{
			base.IsFinished = true;
			AnimationController.StateMachine.StateMachineExited -= OnStateMachineExited;
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
			AnimationController.Animator.SetCurrentReactionType(ReactionType.GetAssassinatedAirSingle);
			AnimationController.Animator.SetTargetDirection(m_TargetDir);
			m_reactionStarted = true;
			AnimationController.FaceDirection(m_TargetRotation * Vector3.forward, 0f);
			AnimationController.CorrectPosition(m_TargetPosition, 0f);
		}
	}

	public override void OnEnter()
	{
		base.OnEnter();
		AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
		AnimationController.Agent.Stop();
		m_TargetDir = AnimationHelper.DetermineDirection(m_Instigator.GameObject, m_Target.GameObject);
		m_TargetRotation = AnimationHelper.AlignTargetToTargetDirection(m_TargetDir, m_Instigator.GameObject, m_Target.GameObject);
		m_TargetPosition = m_Target.Position;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (m_reactionStarted && AnimationController.StateMachine.GetCurrentStateMachineHash() == AnimationStateMachineHashes.StateMachine_GetAssassinatedAir.NameHash)
		{
			AnimatorStateInfo currentAnimatorStateInfo = AnimationController.Animator.GetCurrentAnimatorStateInfo(0);
			AnimationHelper.AlignEntityToNavMeshPlane(AnimationController.EntityId, AnimationController.Agent.areaMask, currentAnimatorStateInfo.normalizedTime);
		}
		else
		{
			AnimationController.FaceDirection(m_TargetRotation * Vector3.forward);
			AnimationController.CorrectPosition(m_TargetPosition);
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
