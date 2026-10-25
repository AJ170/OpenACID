namespace Assets.Scripts.Animation.Tasks
{
	public class AnimationTaskUnsheathCrossbow : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public AttackRangeType AttackRangeType { get; set; }

			public Parameters()
				: base(AnimationTaskType.UnsheathCrossbow)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.UnsheathCrossbow;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.UpperBody,
			ParametersTemplate = new Parameters()
		};

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.UnsheathCrossbow;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AttackRangeType AttackRangeType { get; set; }

		public AnimationTaskUnsheathCrossbow()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskUnsheathCrossbow(Parameters _parameters)
			: base(_parameters)
		{
			AttackRangeType = _parameters.AttackRangeType;
		}

		public override void OnEnter()
		{
			base.OnEnter();
			AnimationController.Animator.SetCurrentAttackRangeType(AttackRangeType);
			AnimationController.Animator.SetCurrentStopAction(false);
			AnimationController.Animator.SetCurrentActionTypeUpperBody(ActionTypeUpperBody.UnsheathCrossbow);
			AnimationController.Animator.SetLayerWeight((int)base.Layer, 1f);
			AnimationController.StateMachine.StateEntered += OnStateEntered;
			AnimationController.StateMachine.StateExited += OnStateExited;
		}

		public override void OnExit(bool _isCanceled)
		{
			base.OnExit(_isCanceled);
			AnimationController.StateMachine.StateEntered -= OnStateEntered;
			AnimationController.StateMachine.StateExited -= OnStateExited;
		}

		private void OnStateEntered(object _sender, AnimatorStateMachine.StateEventArgs _event)
		{
			if (_event.State.CompareNameHash(AnimationStateHashes.State_UnsheathCrossbowWithoutAim))
			{
				AnimationController.Animator.SetCurrentActionTypeUpperBody(ActionTypeUpperBody.Invalid);
			}
		}

		private void OnStateExited(object _sender, AnimatorStateMachine.StateEventArgs _event)
		{
			if (_event.State.CompareNameHash(AnimationStateHashes.State_UnsheathCrossbowWithoutAim))
			{
				base.IsFinished = true;
			}
		}
	}
}
