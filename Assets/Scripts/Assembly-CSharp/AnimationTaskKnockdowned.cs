using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using UnityEngine;

public class AnimationTaskKnockdowned : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public IPositionProvider Source { get; set; }

		public Character Target { get; set; }

		public bool ForceToGround { get; set; }

		public Parameters()
			: base(AnimationTaskType.Knockdowned)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.Knockdowned;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private readonly IPositionProvider m_source;

	private readonly Character m_target;

	private readonly bool m_forceToGround;

	private bool m_rootMotion;

	private bool m_stateReached;

	private bool m_taskEntered;

	private bool m_gotHit;

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.Knockdowned;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskKnockdowned()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskKnockdowned(Parameters _parameters)
		: base(_parameters)
	{
		m_source = _parameters.Source;
		m_target = _parameters.Target;
		m_forceToGround = _parameters.ForceToGround;
	}

	private void OnStateEntered(object _sender, AnimatorStateMachine.StateEventArgs _event)
	{
		if (_event.State.CompareNameHash(AnimationStateHashes.State_PushBackFallFront) || _event.State.CompareNameHash(AnimationStateHashes.State_PushBackFallBack))
		{
			SetStateReached();
		}
	}

	public override void OnEnter()
	{
		base.OnEnter();
		m_gotHit = false;
		m_taskEntered = true;
		m_rootMotion = AnimationController.ApplyRootMotion;
		AnimationController.ApplyRootMotion = true;
		if (AnimationController.Agent.enabled)
		{
			AnimationController.Agent.Stop();
		}
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		AnimationController.StateMachine.StateEntered += OnStateEntered;
		AnimationController.Animator.SetCurrentStopAction(false);
		AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
		if (m_forceToGround)
		{
			AnimationController.Animator.Play(AnimationStateHashes.State_PushBackLoopBack.NameHash);
			SetStateReached();
			return;
		}
		int currentState = AnimationController.StateMachine.GetCurrentState();
		if (AnimationStateHashes.IsStateContainedInArray(currentState, AnimationStateHashes.States_PushBack))
		{
			SetStateReached();
			return;
		}
		AnimationController.Animator.SetCurrentReactionType(ReactionType.PushBack);
		float dir = 1f;
		float distance = 0f;
		if (m_source != null)
		{
			Vector3 vector = m_source.Position - AnimationController.NextPosition;
			if ((Quaternion.Inverse(AnimationController.NextRotation) * vector).z >= 0f)
			{
				AnimationController.NextRotation.SetLookRotation(vector);
			}
			else
			{
				AnimationController.NextRotation.SetLookRotation(-vector);
				dir = -1f;
			}
			distance = LogicMain.Random.Rand(0.5f, 1f);
		}
		AnimationController.Animator.SetCurrentReactionDirectionX(0f);
		AnimationController.Animator.SetCurrentReactionDirectionZ(dir);
		AnimationController.Animator.SetCurrentJumpDistanceZ(distance);
	}

	private void SetStateReached()
	{
		AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
		AnimationController.StateMachine.StateEntered -= OnStateEntered;
		m_stateReached = true;
		if (CanCancel())
		{
			base.IsFinished = true;
		}
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		AnimationHelper.AlignEntityToNavMeshPlane(AnimationController.EntityId, AnimationController.Agent.areaMask, 1f);
	}

	public override bool CanCancel()
	{
		if (!m_stateReached || !base.CanCancel())
		{
			return false;
		}
		if (m_gotHit)
		{
			return true;
		}
		ICharacterConditionCarrier characterConditionCarrier = m_target.CharacterStateMachines as ICharacterConditionCarrier;
		return characterConditionCarrier == null || characterConditionCarrier.CharacterCondition.CurrentStateType != EStateType.CharacterCondition_Knockdowned;
	}

	public override void OnDispose()
	{
		if (m_taskEntered && !m_stateReached)
		{
			AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
			AnimationController.StateMachine.StateEntered -= OnStateEntered;
		}
		if (!m_target.GetDeadOrAboutToDie())
		{
			if (!m_gotHit)
			{
				AnimationController.Animator.SetCurrentStopAction(true);
			}
			AnimationController.ApplyRootMotion = m_rootMotion;
		}
		base.OnDispose();
	}

	public void OnGotHit()
	{
		m_gotHit = true;
	}
}
