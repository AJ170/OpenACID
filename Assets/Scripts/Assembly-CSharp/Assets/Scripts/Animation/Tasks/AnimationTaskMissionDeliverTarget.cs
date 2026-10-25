using Assets.Scripts.Animation.States;

namespace Assets.Scripts.Animation.Tasks
{
	public class AnimationTaskMissionDeliverTarget : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public Parameters()
				: base(AnimationTaskType.MissionDeliverTarget)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.MissionDeliverTarget;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.UpperBody,
			ParametersTemplate = new Parameters()
		};

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.MissionDeliverTarget;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskMissionDeliverTarget()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskMissionDeliverTarget(Parameters _parameters)
			: base(_parameters)
		{
			OnAddState(new AnimationStatePlaySimple(_parameters.AnimationController, new ParameterValues
			{
				ActionTypeUpperBody = ActionTypeUpperBody.ReceiveItem
			}, AnimationStateHashes.StateMachine_ReceiveItem, "Receive"));
		}

		public override bool CanCancel()
		{
			return false;
		}

		public override void OnEnter()
		{
			base.OnEnter();
			AnimationController.Animator.SetLayerWeight(1, 1f);
		}

		public override void OnExit(bool _isCanceled)
		{
			base.OnExit(_isCanceled);
			AnimationController.Animator.SetLayerWeight(1, 0f);
		}
	}
}
