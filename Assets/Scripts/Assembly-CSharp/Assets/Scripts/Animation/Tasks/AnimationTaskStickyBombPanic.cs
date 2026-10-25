using Assets.Scripts.Animation.States;

namespace Assets.Scripts.Animation.Tasks
{
	public class AnimationTaskStickyBombPanic : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public Parameters()
				: base(AnimationTaskType.StickyBombPanic)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.StickyBombPanic;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.Base,
			ParametersTemplate = new Parameters()
		};

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.StickyBombPanic;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskStickyBombPanic()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskStickyBombPanic(Parameters _parameters)
			: base(_parameters)
		{
			OnAddState(new AnimationStatePlaySimple(_parameters.AnimationController, new ParameterValues
			{
				ReactionType = ReactionType.StickyBombReaction,
				MovementState = MovementState.Idle
			}, AnimationStateMachineHashes.StateMachine_StickyBombReaction, "StickyBombPanic"));
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			AnimationHelper.StayOnNavMesh(base.Owner);
		}

		public override bool CanCancel()
		{
			return true;
		}
	}
}
