using Assets.Scripts.Animation.States;

namespace Assets.Scripts.Animation.Tasks
{
	public class AnimationTaskPokeIntoHaystack : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public Parameters()
				: base(AnimationTaskType.PokeIntoHaystack)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.PokeIntoHaystack;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.Base,
			ParametersTemplate = new Parameters()
		};

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.PokeIntoHaystack;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskPokeIntoHaystack()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskPokeIntoHaystack(Parameters _parameters)
			: base(_parameters)
		{
			OnAddState(new AnimationStatePlaySimple(_parameters.AnimationController, new ParameterValues
			{
				ActionType = ActionType.SearchHaystack
			}, AnimationStateMachineHashes.StateMachine_SearchHaystack, "PokeIntoHaystack"));
		}

		public override void OnEnter()
		{
			base.OnEnter();
			AnimationController.ApplyRootMotion = false;
		}

		public override bool CanCancel()
		{
			return false;
		}
	}
}
