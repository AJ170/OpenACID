using Assets.Scripts.Animation.States;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Animation.Tasks
{
	public class AnimationTaskFanOfKnives : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public EntityId TargetEntityId { get; set; }

			public Parameters()
				: base(AnimationTaskType.FanOfKnives)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.FanOfKnives;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.Base,
			ParametersTemplate = new Parameters()
		};

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.FanOfKnives;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskFanOfKnives()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskFanOfKnives(Parameters _parameters)
			: base(_parameters)
		{
			AnimationStatePlaySimple animationStatePlaySimple = OnAddState(new AnimationStatePlaySimple(_parameters.AnimationController, new ParameterValues
			{
				AttackMeleeType = AttackMeleeType.FanOfKnives
			}, AnimationStateMachineHashes.StateMachine_FanOfKnives, "FanOfKnives"));
			AutoAnimationRotation autoAnimationRotation = animationStatePlaySimple.AddAutoAnimationRotation(_parameters.TargetEntityId.GameEntity);
			autoAnimationRotation.Speed = 2f;
		}

		public override void OnDispose()
		{
			Effects effects = AnimationController.EntityId.TryGetEntity<IEffectsCarrier>().Effects;
			effects.OnEffectOff("fan_of_knives_trail_left");
			effects.OnEffectOff("fan_of_knives_trail_right");
			base.OnDispose();
		}

		public override bool CanCancel()
		{
			return false;
		}
	}
}
