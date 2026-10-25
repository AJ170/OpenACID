using Assets.Scripts.Animation.States;

namespace Assets.Scripts.Animation.Tasks
{
	public class AnimationTaskThrowSand : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public Parameters()
				: base(AnimationTaskType.ThrowSand)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.ThrowSand;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.Base,
			ParametersTemplate = new Parameters()
		};

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.ThrowSand;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskThrowSand()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskThrowSand(Parameters _parameters)
			: base(_parameters)
		{
			OnAddState(new AnimationStatePlaySimple(_parameters.AnimationController, new ParameterValues
			{
				ActionType = ActionType.ThrowSand,
				MovementState = MovementState.Idle,
				AttackMeleeType = AttackMeleeType.Invalid
			}, AnimationStateMachineHashes.StateMachine_ThrowSand, "ThrowSand"));
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			AnimationHelper.StayOnNavMesh(base.Owner);
		}

		public override bool CanCancel()
		{
			return false;
		}
	}
}
