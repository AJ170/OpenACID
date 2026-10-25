namespace Assets.Scripts.Animation.Tasks
{
	public class AnimationTaskDieFromFall : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public Parameters()
				: base(AnimationTaskType.DieFromFall)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.DieFromFall;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.Base,
			ParametersTemplate = new Parameters()
		};

		private bool m_stateReached;

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.DieFromFall;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskDieFromFall()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskDieFromFall(Parameters _parameters)
			: base(_parameters)
		{
		}

		public override void OnEnter()
		{
			base.OnEnter();
			AnimationController.ApplyRootMotion = true;
			if (AnimationController.Agent.enabled)
			{
				AnimationController.Agent.Stop();
				AnimationController.Agent.updatePosition = false;
				AnimationController.Agent.updateRotation = false;
			}
			AnimationController.StateMachine.StateEntered += OnStateEntered;
			AnimationController.Animator.SetCurrentReactionType(ReactionType.PushBack);
			AnimationController.Animator.SetCurrentAttackMeleeType(AttackMeleeType.Invalid);
			AnimationController.Animator.SetCurrentReactionDirectionX(0f);
			AnimationController.Animator.SetCurrentReactionDirectionZ(-1f);
			AnimationController.Animator.SetCurrentJumpDistanceZ(0f);
		}

		public override void OnExit(bool _isCanceled)
		{
			base.OnExit(_isCanceled);
			if (!m_stateReached)
			{
				AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
			}
			OnDispose();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			AnimationController.CorrectPosition();
		}

		private void OnStateEntered(object _sender, AnimatorStateMachine.StateEventArgs _state)
		{
			if (m_stateReached)
			{
				base.IsFinished = true;
				return;
			}
			for (AnimatorStateMachineStatic.StateMachine parentStateMachine = _state.State.ParentStateMachine; parentStateMachine != null; parentStateMachine = parentStateMachine.ParentStateMachine)
			{
				if (parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_Reaction) || parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_Die))
				{
					AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
					AnimationController.Animator.SetCurrentGeneralState(GeneralState.Default);
					base.IsFinished = true;
					m_stateReached = true;
					break;
				}
			}
		}

		public override void OnDispose()
		{
			base.OnDispose();
			AnimationController.StateMachine.StateEntered -= OnStateEntered;
		}

		public override bool CanCancel()
		{
			return false;
		}
	}
}
