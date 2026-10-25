using Assets.Scripts.Animation;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

public class AnimationTaskCapturedNpc : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public Parameters()
			: base(AnimationTaskType.CapturedNpc)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.CapturedNpc;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.CapturedNpc;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	private bool ReleasedFromCapture { get; set; }

	private bool ScheduleStandupTriggered { get; set; }

	private bool StandupTriggered { get; set; }

	private float StandupStartTime { get; set; }

	public AnimationTaskCapturedNpc()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskCapturedNpc(Parameters _parameters)
		: base(_parameters)
	{
		AnimationController.ApplyRootMotion = false;
		AnimationController.Animator.SetCurrentIdleType(IdleType.Invalid);
		AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
		AnimationController.Animator.Play(AnimationStateHashes.States_CapturedNpcIdle[LogicMain.Random.Rand(0, AnimationStateHashes.States_CapturedNpcIdle.Length - 1)].NameHash);
		AnimationController.StateMachine.StateExited += OnStateExited;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (AnimationController.EntityId.Entity.Cast<NpcCharacter>().Patrol.PatrolStats.Get_bool(StatTypes.EStatType.Interaction_Triggered) && !ReleasedFromCapture)
		{
			ReleasedFromCapture = true;
		}
		else if (ReleasedFromCapture && !ScheduleStandupTriggered)
		{
			ScheduleStandupTriggered = true;
			StandupStartTime = LogicMain.CurrentTime + LogicMain.Random.RandX(0f, 3f);
		}
		else if (!StandupTriggered && ScheduleStandupTriggered && LogicMain.CurrentTime > StandupStartTime)
		{
			StandupTriggered = true;
			AnimationController.Animator.SetCurrentIdleType(IdleType.Neutral);
		}
	}

	public override bool CanCancel()
	{
		return true;
	}

	public override void OnDispose()
	{
		base.OnDispose();
		AnimationController.StateMachine.StateExited -= OnStateExited;
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.Animator.SetCurrentIdleType(IdleType.Neutral);
	}

	private void OnStateExited(object _sender, AnimatorStateMachine.StateEventArgs _event)
	{
		if (_event.State.NameHash == AnimationStateHashes.State_KneelStandup.NameHash)
		{
			base.IsFinished = true;
			AnimationController.StateMachine.StateExited -= OnStateExited;
		}
	}
}
