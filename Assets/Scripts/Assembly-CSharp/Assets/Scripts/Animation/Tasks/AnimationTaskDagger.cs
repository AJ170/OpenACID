using Assets.Scripts.Animation.States;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;

namespace Assets.Scripts.Animation.Tasks
{
	public class AnimationTaskDagger : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public EntityId TargetEntityId { get; set; }

			public Parameters()
				: base(AnimationTaskType.Dagger)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.Dagger;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.Base,
			ParametersTemplate = new Parameters()
		};

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.Dagger;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskDagger()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskDagger(Parameters _parameters)
			: base(_parameters)
		{
			GameEntity gameEntity = _parameters.TargetEntityId.GameEntity;
			Vector3 vector = gameEntity.Position - AnimationController.EntityId.GameEntity.Position;
			float f = vector.y / vector.magnitude;
			float value = Mathf.Asin(f) * 57.29578f;
			AnimationStatePlaySimple animationStatePlaySimple = OnAddState(new AnimationStatePlaySimple(_parameters.AnimationController, new ParameterValues
			{
				AttackMeleeType = AttackMeleeType.Dagger,
				TargetDirectionY = value
			}, AnimationStateMachineHashes.StateMachine_SilentDagger, "SilentDagger"));
			AutoAnimationRotation autoAnimationRotation = animationStatePlaySimple.AddAutoAnimationRotation(gameEntity);
			autoAnimationRotation.Speed = 2f;
		}

		public override bool CanCancel()
		{
			return false;
		}
	}
}
