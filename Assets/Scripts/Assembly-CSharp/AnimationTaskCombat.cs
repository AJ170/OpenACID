using Assets.Scripts.Mission.Logic.Combat;

public class AnimationTaskCombat : AnimationTaskBase, IAnimatorEventListener
{
	public class Parameters : AnimationTaskParameters
	{
		public CombatParticipant Participant { get; set; }

		public Parameters()
			: base(AnimationTaskType.Combat)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.Combat;

	private bool m_rootMotion;

	private CombatParticipant m_participant;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.Combat;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public bool Dead { get; private set; }

	public CombatParticipant Participant
	{
		get
		{
			return m_participant;
		}
	}

	public AnimationTaskCombat()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskCombat(Parameters _parameters)
		: base(_parameters)
	{
		Dead = false;
		m_participant = _parameters.Participant;
		AnimationController.DataController.RegisterEventListener(this);
	}

	public void OnExitCombat(bool _dead)
	{
		Dead = _dead;
		base.IsFinished = true;
	}

	public override void OnEnter()
	{
		base.OnEnter();
		m_rootMotion = AnimationController.ApplyRootMotion;
		AnimationController.ApplyRootMotion = true;
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.ApplyRootMotion = m_rootMotion;
	}

	public override bool CanCancel()
	{
		if (!base.CanCancel())
		{
			return false;
		}
		if (base.IsFinished)
		{
			return true;
		}
		return m_participant.ActiveBehaviour != null && m_participant.ActiveBehaviour.React(null);
	}

	public override void OnDispose()
	{
		base.OnDispose();
		AnimationController.DataController.UnregisterEventListener(this);
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
		}
		return false;
	}
}
