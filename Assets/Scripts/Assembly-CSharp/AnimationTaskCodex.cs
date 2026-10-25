using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;

public class AnimationTaskCodex : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public Codex Codex { get; set; }

		public InteractionLocators.Locator SelectedLocator { get; set; }

		public Parameters()
			: base(AnimationTaskType.Codex)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.Codex;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.Codex;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public EntityId CodexId { get; private set; }

	public AnimationTaskCodex()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskCodex(Parameters _parameters)
		: base(_parameters)
	{
		CodexId = _parameters.Codex.Id;
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		AnimationController.Animator.SetCurrentLinkType(eLinkType.Invalid);
		AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
		OnAddState(new AnimationStateApproachLink(_parameters.AnimationController, _parameters.SelectedLocator.Position.transform.position, _parameters.SelectedLocator.Orientation));
	}

	public override bool CanCancel()
	{
		return false;
	}
}
