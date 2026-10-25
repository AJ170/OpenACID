using UnityEngine;

namespace Assets.Scripts.Animation.Tasks
{
	public class AnimationTaskAimWithCrossbow : AnimationTaskAimWithRangedBase
	{
		public const AnimationTaskType sTaskType = AnimationTaskType.AimWithCrossbow;

		private const float TargetDirectionXCorrection = -12f;

		private const float TargetDirectionYCorrection = 5f;

		public float AutoRotationAngleThreshold = 80f;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.UpperBody,
			ParametersTemplate = new Parameters(AnimationTaskType.AimWithCrossbow)
		};

		private bool m_rotationAutoStopInternal;

		private bool m_rotationAutoStopExternal;

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.AimWithCrossbow;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskAimWithCrossbow()
			: base(ParametersTemplate)
		{
		}

		public AnimationTaskAimWithCrossbow(Parameters _parameters)
			: base(_parameters)
		{
		}

		public override void OnEnter()
		{
			base.OnEnter();
			base.AutoAnimationRotation.AutoStop = true;
		}

		public override void OnUpdate()
		{
			Vector3 forward = base.TargetEntity.Position - AnimationController.EntityId.GameEntity.Position;
			float num = Mathf.DeltaAngle(0f, (Quaternion.LookRotation(forward) * Quaternion.Inverse(AnimationController.EntityId.GameEntity.Rotation)).eulerAngles.y);
			AnimationController.Animator.SetTargetDirectionX(num + -12f);
			if (Mathf.Abs(num) > AutoRotationAngleThreshold)
			{
				SetRotationAutoStopInternal(false);
			}
			else
			{
				SetRotationAutoStopInternal(true);
			}
			float f = forward.y / forward.magnitude;
			float num2 = Mathf.Asin(f) * 57.29578f;
			AnimationController.Animator.SetTargetDirectionY(num2 + 5f);
			base.OnUpdate();
		}

		public override void OnExit(bool _isCanceled)
		{
			base.OnExit(_isCanceled);
			AnimationController.StateMachine.StateExited -= OnShootAnimExited;
		}

		public override void OnDispose()
		{
			base.OnDispose();
			AnimationController.StateMachine.StateExited -= OnShootAnimExited;
		}

		public void SetRotationAutoStop(bool _autoStop)
		{
			m_rotationAutoStopExternal = _autoStop;
			UpdateRotationAutoStop();
		}

		private void SetRotationAutoStopInternal(bool _autoStop)
		{
			m_rotationAutoStopInternal = _autoStop;
			UpdateRotationAutoStop();
		}

		private void UpdateRotationAutoStop()
		{
			if (base.AutoAnimationRotation == null)
			{
				return;
			}
			bool flag = m_rotationAutoStopInternal && m_rotationAutoStopExternal;
			if (!flag)
			{
				if (base.AutoAnimationRotation.IsRunning)
				{
					if (base.AutoAnimationRotation.AutoStop != flag)
					{
						base.AutoAnimationRotation.Stop();
						base.AutoAnimationRotation.Start();
					}
				}
				else
				{
					base.AutoAnimationRotation.Start();
				}
			}
			base.AutoAnimationRotation.AutoStop = flag;
		}

		protected override void OnEnterSetParametersWeapon(Animator _animator)
		{
		}

		protected override void OnExitSetParametersWeapon(Animator _animator)
		{
		}

		protected override void ShootSetParametersWeapon(Animator _animator)
		{
			_animator.SetShootCrossbow(true);
		}

		protected override void RestartAimingInternal()
		{
			AnimationController.Animator.SetLayerWeight((int)Properties.Layer, 1f);
		}

		protected override void OnEnterSetParametersInternal(Animator _animator)
		{
		}

		protected override void OnExitSetParametersInternal(Animator _animator)
		{
			_animator.SetCurrentAttackRangeType(AttackRangeType.WeaponReady);
		}

		protected override void OnShootAnimEnteredInternal()
		{
			AnimationController.StateMachine.StateExited += OnShootAnimExited;
		}

		private void OnShootAnimExited(object _sender, AnimatorStateMachine.StateEventArgs _e)
		{
			base.WeaponAnimator.SetLoadCrossbow(true);
			base.WeaponAnimator.SetShootCrossbow(false);
			AnimationController.StateMachine.StateExited -= OnShootAnimExited;
		}
	}
}
