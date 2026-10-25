using Assets.Scripts.Animation.States;
using Assets.Scripts.Mission.Logic;

namespace Assets.Scripts.Animation.Tasks
{
	internal class AnimationTaskExecuteOnGround : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public EntityId VictimId { get; set; }

			public Parameters()
				: base(AnimationTaskType.ExecuteOnGround)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.ExecuteOnGround;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.Base,
			ParametersTemplate = new Parameters()
		};

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.ExecuteOnGround;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskExecuteOnGround()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskExecuteOnGround(Parameters _parameters)
			: base(_parameters)
		{
			AutoAnimationRotationToTarget autoAnimationRotationToTarget = new AutoAnimationRotationToTarget(AnimationController.EntityId.GameEntity, _parameters.VictimId.GameEntity)
			{
				Speed = 2f,
				FireAndForget = true
			};
			OnAddState(new AnimationStatePlaySimple(_parameters.AnimationController, new ParameterValues
			{
				GeneralState = GeneralState.Combat,
				MovementState = MovementState.Idle,
				AttackMeleeType = (AttackMeleeType)1102
			}, _parameters.VictimId, AnimatorEventType.ReactionStart, AnimationStateMachineHashes.StateMachine_ComboKill, "Combo Kill 02"));
		}

		public override void OnEnter()
		{
			base.OnEnter();
			AnimationController.Animator.SetCurrentGeneralState(GeneralState.Combat);
		}
	}
}
