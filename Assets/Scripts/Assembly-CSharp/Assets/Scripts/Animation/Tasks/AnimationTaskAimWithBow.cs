using UnityEngine;

namespace Assets.Scripts.Animation.Tasks
{
	internal class AnimationTaskAimWithBow : AnimationTaskAimWithRangedBase
	{
		public const AnimationTaskType sTaskType = AnimationTaskType.AimWithBow;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.Base,
			ParametersTemplate = new Parameters(AnimationTaskType.AimWithBow)
		};

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.AimWithBow;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskAimWithBow()
			: base(ParametersTemplate)
		{
		}

		public AnimationTaskAimWithBow(Parameters _parameters)
			: base(_parameters)
		{
		}

		public override void OnEnter()
		{
			base.OnEnter();
			base.AutoAnimationRotation.AutoStop = false;
		}

		public override void OnUpdate()
		{
			Vector3 position = base.TargetEntity.Position;
			if (!base.IsFinished && Mathf.Abs(AnimationController.EntityId.GameEntity.Position.y - position.y) > 0.1f)
			{
				AnimationController.SetLookAtWeight(1f, 0.75f, 0.35f, 0f, 0.3f);
				AnimationController.SetLookAtPosition(position);
			}
			else
			{
				AnimationController.SetLookAtWeight(0f);
			}
			base.OnUpdate();
		}

		public override void OnExit(bool _isCanceled)
		{
			base.OnExit(_isCanceled);
			AnimationController.SetLookAtWeight(0f);
		}

		protected override void OnEnterSetParametersWeapon(Animator _animator)
		{
			OnEnterSetParameters(_animator);
		}

		protected override void OnExitSetParametersWeapon(Animator _animator)
		{
			OnExitSetParameters(_animator);
		}

		protected override void ShootSetParametersWeapon(Animator _animator)
		{
			ShootSetParameters(_animator);
		}

		protected override void RestartAimingInternal()
		{
			AnimationController.Animator.SetCurrentIdleType(IdleType.Neutral);
			AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
		}

		protected override void OnEnterSetParametersInternal(Animator _animator)
		{
			_animator.SetCurrentGeneralState(GeneralState.CombatRangeBow);
		}

		protected override void OnExitSetParametersInternal(Animator _animator)
		{
			_animator.SetCurrentAttackRangeType(AttackRangeType.Invalid);
			_animator.SetCurrentStopAction(true);
			_animator.SetCurrentGeneralState(GeneralState.Invalid);
		}

		protected override void OnShootAnimEnteredInternal()
		{
		}
	}
}
