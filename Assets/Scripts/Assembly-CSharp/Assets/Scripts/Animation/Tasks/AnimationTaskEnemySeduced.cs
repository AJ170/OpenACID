using Assets.Scripts.Animation.States;

namespace Assets.Scripts.Animation.Tasks
{
	public class AnimationTaskEnemySeduced : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public int AnimatorNpcId { get; set; }

			public Parameters()
				: base(AnimationTaskType.EnemySeduced)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.EnemySeduced;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.Base,
			ParametersTemplate = new Parameters()
		};

		private bool m_rootMotion;

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.EnemySeduced;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskEnemySeduced()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskEnemySeduced(Parameters _parameters)
			: base(_parameters)
		{
			OnAddState(new AnimationStatePlaySimple(_parameters.AnimationController, new ParameterValues
			{
				ReactionType = ReactionType.GetSeduced,
				IdleType = IdleType.Neutral,
				MovementState = MovementState.Idle,
				AnimatorNpcId = _parameters.AnimatorNpcId
			}, AnimationStateMachineHashes.StateMachine_EnemySeduced, "EnemySeduced"));
		}

		public override void OnEnter()
		{
			base.OnEnter();
			m_rootMotion = AnimationController.ApplyRootMotion;
			AnimationController.ApplyRootMotion = true;
			AnimationController.Animator.SetCurrentStopAction(false);
		}

		public override void OnExit(bool _isCanceled)
		{
			base.OnExit(_isCanceled);
			AnimationController.ApplyRootMotion = m_rootMotion;
			AnimationController.Animator.SetCurrentStopAction(true);
		}

		public override bool CanCancel()
		{
			return true;
		}
	}
}
