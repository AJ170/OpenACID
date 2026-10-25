using UnityEngine;

namespace Assets.Scripts.Animation.Tasks
{
	public class AnimationTaskSheathCrossbow : AnimationTaskSimpleUpperbodyBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public Parameters()
				: base(AnimationTaskType.SheathCrossbow)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.SheathCrossbow;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.UpperBody,
			ParametersTemplate = new Parameters()
		};

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.SheathCrossbow;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskSheathCrossbow()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskSheathCrossbow(Parameters _parameters)
			: base(_parameters, ActionTypeUpperBody.Invalid, AnimationStateHashes.State_SheathCrossbow, 0f, 0.3f, EFade.Out)
		{
		}

		public override void OnEnter()
		{
			base.OnEnter();
			Animator animator = AnimationController.Animator;
			animator.SetCurrentStopAction(true);
			animator.SetCurrentAttackRangeType(AttackRangeType.Invalid);
			AnimationController.StateMachine.StateEntered += OnStateEnteredAdditional;
		}

		private void OnStateEnteredAdditional(object _sender, AnimatorStateMachine.StateEventArgs _event)
		{
			if (_event.State.CompareNameHash(AnimationStateHashes.State_NoUpperBodyAnim))
			{
				AnimationController.Animator.SetCurrentActionTypeUpperBody(ActionTypeUpperBody.Invalid);
				base.IsFinished = true;
			}
		}

		public override void OnDispose()
		{
			base.OnDispose();
			AnimationController.StateMachine.StateEntered -= OnStateEnteredAdditional;
		}
	}
}
