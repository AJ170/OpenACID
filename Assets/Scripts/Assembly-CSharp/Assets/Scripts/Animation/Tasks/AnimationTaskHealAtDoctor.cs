using Assets.Scripts.Animation.States;
using Assets.Scripts.Mission.Logic;

namespace Assets.Scripts.Animation.Tasks
{
	public class AnimationTaskHealAtDoctor : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public EntityId TargetEntityId { get; set; }

			public Parameters()
				: base(AnimationTaskType.PlayerHealAtDoctor)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.PlayerHealAtDoctor;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.Base,
			ParametersTemplate = new Parameters()
		};

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.PlayerHealAtDoctor;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskHealAtDoctor()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskHealAtDoctor(Parameters _parameters)
			: base(_parameters)
		{
			AnimationStatePlaySimple animationStatePlaySimple = OnAddState(new AnimationStatePlaySimple(_parameters.AnimationController, new ParameterValues
			{
				ActionType = ActionType.HealAtDoctor
			}, AnimationStateMachineHashes.StateMachine_HealAtDoctor, "HealAtDoctor"));
			AutoAnimationRotation autoAnimationRotation = animationStatePlaySimple.AddAutoAnimationRotation(_parameters.TargetEntityId.GameEntity);
			autoAnimationRotation.Speed = 2f;
		}

		public override bool CanCancel()
		{
			return false;
		}
	}
}
