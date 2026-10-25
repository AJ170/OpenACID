using UnityEngine;

namespace Assets.Scripts.Animation.Tasks
{
	internal class AnimationTaskSheathBow : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public Animator BowAnimator { get; set; }

			public Parameters()
				: base(AnimationTaskType.SheathBow)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.SheathBow;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.Base,
			ParametersTemplate = new Parameters()
		};

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.SheathBow;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public Animator BowAnimator { get; set; }

		public AnimationTaskSheathBow()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskSheathBow(Parameters _parameters)
			: base(_parameters)
		{
			BowAnimator = _parameters.BowAnimator;
		}

		public override void OnEnter()
		{
			base.OnEnter();
			if (AnimationController.StateMachine.GetCurrentStateMachineHash() == AnimationStateMachineHashes.StateMachine_CombatRangeBow.NameHash)
			{
				AnimationController.Animator.SetCurrentIdleType(IdleType.Neutral);
				AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
				OnEnterSetParameters(AnimationController.Animator);
				OnEnterSetParameters(BowAnimator);
				AnimationController.StateMachine.StateMachineExited += OnCombatRangeBowExited;
			}
			else
			{
				base.IsFinished = true;
			}
		}

		public override bool CanCancel()
		{
			return true;
		}

		public override void OnDispose()
		{
			base.OnDispose();
			AnimationController.StateMachine.StateMachineExited -= OnCombatRangeBowExited;
		}

		private void OnCombatRangeBowExited(object _sender, AnimatorStateMachine.StateMachineEventArgs _e)
		{
			if (_e.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_CombatRangeBow))
			{
				AnimationController.StateMachine.StateMachineExited -= OnCombatRangeBowExited;
				base.IsFinished = true;
			}
		}

		private void OnEnterSetParameters(Animator _animator)
		{
			_animator.SetCurrentStopAction(true);
		}
	}
}
