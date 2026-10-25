using UnityEngine;

namespace Assets.Scripts.Animation.Tasks
{
	public class AnimationTaskHoldCombatIdle : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public Parameters()
				: base(AnimationTaskType.HoldCombatIdle)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.HoldCombatIdle;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.Base,
			ParametersTemplate = new Parameters()
		};

		private float m_clock;

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.HoldCombatIdle;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskHoldCombatIdle()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskHoldCombatIdle(Parameters _parameters)
			: base(_parameters)
		{
			m_clock = float.MaxValue;
		}

		public override void OnEnter()
		{
			base.OnEnter();
			m_clock = CombatController.Instance.ExitCombatHoldIdle;
			AnimationController.Animator.SetCurrentGeneralState(GeneralState.Combat);
			AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
			AnimationController.Animator.SetCurrentActionType(ActionType.Invalid);
			AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
		}

		public override void OnExit(bool _isCanceled)
		{
			base.OnExit(_isCanceled);
			AnimationController.Animator.SetCurrentGeneralState(GeneralState.Default);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			AnimationHelper.StayOnNavMesh(base.Owner);
			m_clock -= Time.deltaTime;
			if (m_clock <= 0f)
			{
				base.IsFinished = true;
			}
		}

		public override bool CanCancel()
		{
			return true;
		}
	}
}
