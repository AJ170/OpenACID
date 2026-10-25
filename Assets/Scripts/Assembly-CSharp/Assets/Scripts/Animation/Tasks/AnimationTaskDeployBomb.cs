using Assets.Scripts.Animation.States;

namespace Assets.Scripts.Animation.Tasks
{
	public class AnimationTaskDeployBomb : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public Parameters()
				: base(AnimationTaskType.DeployBomb)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.DeployBomb;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.Base,
			ParametersTemplate = new Parameters()
		};

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.DeployBomb;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskDeployBomb()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskDeployBomb(Parameters _parameters)
			: base(_parameters)
		{
			OnAddState(new AnimationStatePlaySimple(_parameters.AnimationController, new ParameterValues
			{
				GeneralState = GeneralState.Default,
				ActionType = ActionType.DeployBomb
			}, AnimationStateMachineHashes.StateMachine_DeployBomb, "DeployBomb"));
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
