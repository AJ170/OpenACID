using Assets.Scripts.Mission.Logic.Combat;

namespace Assets.Scripts.Animation.Tasks
{
	internal class AnimationTaskHitEscortee : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public Parameters()
				: base(AnimationTaskType.HitEscortee)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.HitEscortee;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.Base,
			ParametersTemplate = new Parameters()
		};

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.HitEscortee;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskHitEscortee()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskHitEscortee(Parameters _parameters)
			: base(_parameters)
		{
		}

		public override void OnEnter()
		{
			base.OnEnter();
			AnimationController.Animator.SetCurrentGeneralState(GeneralState.Combat);
			AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
			AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
			AnimationController.Animator.SetCurrentAttackMeleeType(AttackMeleeType.UpLeft);
			AnimationController.Animator.SetCurrentComboAttack(Flag.False);
			AnimationController.StateMachine.StateMachineExited += OnStateMachineExited;
		}

		public override void OnExit(bool _isCanceled)
		{
			base.OnExit(_isCanceled);
			AnimationController.Animator.SetCurrentGeneralState(GeneralState.Default);
			AnimationController.Animator.SetCurrentAttackMeleeType(AttackMeleeType.Invalid);
			AnimationController.Animator.SetCurrentComboAttack(Flag.False);
			AnimationController.Animator.speed = 1f;
			AnimationController.StateMachine.StateMachineExited -= OnStateMachineExited;
		}

		public void OnStateMachineExited(object _sender, AnimatorStateMachine.StateMachineEventArgs _stateMachineEventArgs)
		{
			if (_stateMachineEventArgs.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_AttackMelee))
			{
				base.IsFinished = true;
			}
		}
	}
}
