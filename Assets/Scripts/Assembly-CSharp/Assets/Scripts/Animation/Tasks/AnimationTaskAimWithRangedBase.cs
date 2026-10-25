using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.StateMachines;
using UnityEngine;

namespace Assets.Scripts.Animation.Tasks
{
	public abstract class AnimationTaskAimWithRangedBase : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public Animator WeaponAnimator { get; set; }

			public IGameEntity TargetEntity { get; set; }

			public Parameters(AnimationTaskType _taskType)
				: base(_taskType)
			{
			}
		}

		public const float TurningSpeed = 2f;

		public const float LookAtThresholdY = 0.1f;

		public Animator WeaponAnimator { get; private set; }

		public IGameEntity TargetEntity { get; private set; }

		protected AutoAnimationRotation AutoAnimationRotation { get; private set; }

		protected AnimationTaskAimWithRangedBase(Parameters _parameters)
			: base(_parameters)
		{
			WeaponAnimator = _parameters.WeaponAnimator;
			TargetEntity = _parameters.TargetEntity;
		}

		public bool Shoot()
		{
			WeaponHandler weaponHandler = AnimationController.TaskManager.GetWeaponHandler(EWeaponCategory.Ranged);
			WeaponHandler.WeaponDesc currentWeaponDesc = weaponHandler.CurrentWeaponDesc;
			AnimationTaskManager.ELayer unsheathAnimationLayer = CharacterDrawWeaponBase.GetUnsheathAnimationLayer(WeaponHandler.WeaponTypesToType(currentWeaponDesc.Type));
			if (AnimationController.StateMachine.GetCurrentState(unsheathAnimationLayer) == currentWeaponDesc.AimState.NameHash)
			{
				AnimationController.StateMachine.StateEntered += OnShootAnimEntered;
				ShootSetParameters(AnimationController.Animator);
				ShootSetParametersWeapon(WeaponAnimator);
				return true;
			}
			return false;
		}

		public void StopAiming()
		{
			base.IsFinished = true;
		}

		public virtual void RestartAiming()
		{
			base.IsFinished = false;
			OnEnterSetParameters(AnimationController.Animator);
			OnEnterSetParametersWeapon(WeaponAnimator);
			RestartAimingInternal();
		}

		public override void OnEnter()
		{
			base.OnEnter();
			AutoAnimationRotation = AddAutoUpdateable(new AutoAnimationRotationToTarget(AnimationController.EntityId.GameEntity, TargetEntity));
			AutoAnimationRotation.Speed = 2f;
			AutoAnimationRotation.FireAndForget = false;
			AutoAnimationRotation.Enter();
			RestartAiming();
		}

		public override void OnExit(bool _isCanceled)
		{
			base.OnExit(_isCanceled);
			AnimationController.StateMachine.StateEntered -= OnShootAnimEntered;
			AutoAnimationRotation.Exit();
			OnExitSetParameters(AnimationController.Animator);
			OnExitSetParametersWeapon(WeaponAnimator);
		}

		public override void OnDispose()
		{
			base.OnDispose();
			AnimationController.StateMachine.StateEntered -= OnShootAnimEntered;
		}

		public override bool CanCancel()
		{
			return true;
		}

		public override string ToString()
		{
			if (AutoAnimationRotation == null)
			{
				return string.Format("{0} ({1}) aNULL:rNULL", GetType().ToString(), base.ID);
			}
			return string.Format("{0} ({1}) a{2}:r{3}", GetType().ToString(), base.ID, AutoAnimationRotation.AutoStop, AutoAnimationRotation.IsRunning);
		}

		private void OnShootAnimEntered(object _sender, AnimatorStateMachine.StateEventArgs _stateEventArgs)
		{
			WeaponHandler weaponHandler = AnimationController.TaskManager.GetWeaponHandler(EWeaponCategory.Ranged);
			WeaponHandler.WeaponDesc currentWeaponDesc = weaponHandler.CurrentWeaponDesc;
			if (_stateEventArgs.State.CompareNameHash(currentWeaponDesc.ShootState))
			{
				OnEnterSetParameters(AnimationController.Animator);
				OnEnterSetParametersWeapon(WeaponAnimator);
				AnimationController.StateMachine.StateEntered -= OnShootAnimEntered;
				OnShootAnimEnteredInternal();
			}
		}

		protected void ShootSetParameters(Animator _animator)
		{
			_animator.SetCurrentAttackRangeType(AttackRangeType.ReleaseArrow);
		}

		protected void OnEnterSetParameters(Animator _animator)
		{
			_animator.SetCurrentStopAction(false);
			_animator.SetCurrentAttackRangeType(AttackRangeType.StartAim);
			OnEnterSetParametersInternal(_animator);
		}

		protected void OnExitSetParameters(Animator _animator)
		{
			OnExitSetParametersInternal(_animator);
		}

		protected abstract void OnEnterSetParametersWeapon(Animator _animator);

		protected abstract void OnExitSetParametersWeapon(Animator _animator);

		protected abstract void ShootSetParametersWeapon(Animator _animator);

		protected abstract void RestartAimingInternal();

		protected abstract void OnEnterSetParametersInternal(Animator _animator);

		protected abstract void OnExitSetParametersInternal(Animator _animator);

		protected abstract void OnShootAnimEnteredInternal();
	}
}
