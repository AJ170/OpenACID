using Assets.Scripts.Animation;
using UbiCore.Messenger;
using UnityEngine;

public class AnimationTaskClimbRebound : AnimationTaskBase, IAnimation, IParkourAnimation, IAnimatorEventListener
{
	public class Parameters : AnimationTaskParameters
	{
		public ClimbReboundEvent ClimbReboundEvent { get; set; }

		public Parameters()
			: base(AnimationTaskType.ClimbRebound)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.ClimbRebound;

	public static float TargetDistanceToWall = 0.75f;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private ClimbReboundEvent m_event;

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.ClimbRebound;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public Vector3 Destination { get; private set; }

	public float Height { get; private set; }

	public AnimationTaskClimbRebound()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskClimbRebound(Parameters _parameters)
		: base(_parameters)
	{
		m_event = _parameters.ClimbReboundEvent;
		Destination = m_event.WallPoint;
		Height = 0f;
	}

	public override void OnEnter()
	{
		base.OnEnter();
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		AnimationController.ApplyRootMotion = true;
		AnimationController.Animator.SetCurrentClimbDirection(eDirection.Up);
		AnimationController.Animator.SetCurrentLinkType(eLinkType.Climb);
		AnimationController.Animator.SetCurrentEdgeDistance(float.MaxValue);
		AnimationController.Animator.SetCurrentGeneralState(GeneralState.OnLink);
		AnimationController.GetComponent<AnimatorDataController>().RegisterEventListener(this);
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.GetComponent<AnimatorDataController>().UnregisterEventListener(this);
	}

	public bool OnAnimatorEvent(AnimatorEvent _event)
	{
		AnimatorEventType type = _event.Type;
		if (type == AnimatorEventType.TaskEnd)
		{
			Finish();
		}
		return false;
	}

	private void Finish()
	{
		base.IsFinished = true;
		AnimationController.Animator.SetCurrentClimbDirection(eDirection.Invalid);
		AnimationController.Animator.SetCurrentLinkType(eLinkType.Invalid);
		AnimationController.Animator.SetCurrentEdgeDistance(AnimationController.Invalid);
		AnimationController.Animator.SetCurrentGeneralState(GeneralState.Default);
		AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
	}

	public override bool CanCancel()
	{
		return false;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		AnimationController.FaceDirection(-m_event.WallNormal);
		AnimationController.KeepDistance(m_event.WallPoint, m_event.WallNormal, TargetDistanceToWall, true);
	}
}
