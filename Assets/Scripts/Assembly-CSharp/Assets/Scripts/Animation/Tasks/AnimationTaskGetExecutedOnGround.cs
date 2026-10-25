using Assets.Scripts.Animation.States;

namespace Assets.Scripts.Animation.Tasks
{
	internal class AnimationTaskGetExecutedOnGround : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public Parameters()
				: base(AnimationTaskType.GetExecutedOnGround)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.GetExecutedOnGround;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.Base,
			ParametersTemplate = new Parameters()
		};

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.GetExecutedOnGround;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskGetExecutedOnGround()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskGetExecutedOnGround(Parameters _parameters)
			: base(_parameters)
		{
			OnAddState(new AnimationStatePlaySimple(_parameters.AnimationController, new ParameterValues
			{
				ReactionType = ReactionType.GetExecutedOnGround
			}, AnimationStateHashes.State_GetKilledCombo_GetKilledCombo_02, "Get Killed Combo 02"));
		}
	}
}
