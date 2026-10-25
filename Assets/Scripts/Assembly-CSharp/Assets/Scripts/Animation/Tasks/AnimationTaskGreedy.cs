using Assets.Scripts.Animation.States;

namespace Assets.Scripts.Animation.Tasks
{
	public class AnimationTaskGreedy : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public Parameters()
				: base(AnimationTaskType.Greedy)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.Greedy;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.Base,
			ParametersTemplate = new Parameters()
		};

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.Greedy;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskGreedy()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskGreedy(Parameters _parameters)
			: base(_parameters)
		{
			OnAddState(new AnimationStatePlaySimple(_parameters.AnimationController, new ParameterValues
			{
				ReactionType = ReactionType.CoinThrowReaction,
				MovementState = MovementState.Idle
			}, AnimationStateMachineHashes.StateMachine_CoinThrowReaction, "Greedy"));
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
