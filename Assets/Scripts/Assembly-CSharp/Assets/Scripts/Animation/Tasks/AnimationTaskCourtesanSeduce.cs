using Assets.Scripts.Animation.States;

namespace Assets.Scripts.Animation.Tasks
{
	public class AnimationTaskCourtesanSeduce : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public int AnimatorNpcId { get; set; }

			public Parameters()
				: base(AnimationTaskType.CourtesanSeduce)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.CourtesanSeduce;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.Base,
			ParametersTemplate = new Parameters()
		};

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.CourtesanSeduce;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskCourtesanSeduce()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskCourtesanSeduce(Parameters _parameters)
			: base(_parameters)
		{
			OnAddState(new AnimationStatePlaySimple(_parameters.AnimationController, new ParameterValues
			{
				ActionType = ActionType.Seduce,
				IdleType = IdleType.Neutral,
				MovementState = MovementState.Idle,
				AnimatorNpcId = _parameters.AnimatorNpcId
			}, AnimationStateMachineHashes.StateMachine_CourtesanSeduce, "CourtesanSeduce"));
		}

		public override void OnEnter()
		{
			base.OnEnter();
			AnimationController.ApplyRootMotion = true;
			AnimationController.Agent.updatePosition = false;
			AnimationController.Agent.updateRotation = false;
			AnimationController.Animator.SetCurrentStopAction(false);
		}

		public override void OnExit(bool _isCanceled)
		{
			base.OnExit(_isCanceled);
			AnimationController.Animator.SetCurrentStopAction(true);
		}

		public override bool CanCancel()
		{
			return true;
		}
	}
}
