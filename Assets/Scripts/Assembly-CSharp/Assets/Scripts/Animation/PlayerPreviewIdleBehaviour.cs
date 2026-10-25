using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Animation
{
	[RequireComponent(typeof(AnimationController))]
	public class PlayerPreviewIdleBehaviour : MonoBehaviour
	{
		private readonly AnimatorStateMachineStatic.StateHashValue PlayerIdleLeft = new AnimatorStateMachineStatic.StateHashValue("Base.Player_IdleTypes.PlayerIdle_base_animus_footl");

		private readonly AnimatorStateMachineStatic.StateHashValue PlayerIdleRight = new AnimatorStateMachineStatic.StateHashValue("Base.Player_IdleTypes.PlayerIdle_base_animus_footr");

		private readonly AnimatorStateMachineStatic.StateHashValue PlayerIdleUnsheathWeapon = new AnimatorStateMachineStatic.StateHashValue("Base.Player_IdleTypes.Idle_Unsheath_weapon");

		private float m_idleTimeMin = 6f;

		private float m_idleTimeMax = 14f;

		private float m_nextIdleChange;

		private IdleType m_nextIdleType = IdleType.Invalid;

		private bool m_isPlayingWeaponEquipAnimation;

		private AnimationController m_animationController;

		public WeaponHandler WeaponHandler { get; private set; }

		public bool IsPlayingWeaponEquipAnimation
		{
			get
			{
				return m_isPlayingWeaponEquipAnimation;
			}
			set
			{
				m_isPlayingWeaponEquipAnimation = value;
			}
		}

		public void EquipSword()
		{
			m_animationController.Animator.SetCurrentActionType(ActionType.AnimusUnsheathWeapon);
			IsPlayingWeaponEquipAnimation = true;
		}

		public void CancelSwordAnimation()
		{
			if (IsPlayingWeaponEquipAnimation)
			{
				m_animationController.Animator.SetCurrentActionType(ActionType.Default);
				m_animationController.Animator.SetCurrentStopAction(true);
			}
		}

		private void Awake()
		{
			m_animationController = GetComponent<AnimationController>();
			WeaponHandler = base.gameObject.AddComponentIfNotExist<WeaponHandler>();
		}

		private void OnEnable()
		{
			m_animationController.StateMachine.StateEntered += OnStateEntered;
			WeaponHandler.GetComponent<AnimatorDataController>().RegisterEventListener(WeaponHandler);
			OnEnterBaseIdle();
		}

		private void OnDisable()
		{
			m_animationController.StateMachine.StateEntered -= OnStateEntered;
			WeaponHandler.GetComponent<AnimatorDataController>().UnregisterEventListener(WeaponHandler);
		}

		private void OnStateEntered(object _sender, AnimatorStateMachine.StateEventArgs _args)
		{
			if (_args.Layer == 0 && _args.LoopCount == 0)
			{
				if (_args.State.CompareNameHash(PlayerIdleUnsheathWeapon))
				{
					m_animationController.Animator.SetCurrentActionType(ActionType.Invalid);
				}
				if (!_args.State.CompareNameHash(PlayerIdleLeft) && !_args.State.CompareNameHash(PlayerIdleRight))
				{
					OnEnterSpecialIdle();
				}
				else
				{
					OnEnterBaseIdle();
				}
			}
		}

		private void OnEnterSpecialIdle()
		{
			m_animationController.Animator.SetCurrentIdleType(IdleType.LowProfile);
		}

		private void OnEnterBaseIdle()
		{
			m_nextIdleChange = Time.time + Random.Range(m_idleTimeMin, m_idleTimeMax);
			m_nextIdleType = IdleType.RandomTrait;
		}

		private void Update()
		{
			if (m_nextIdleType != IdleType.Invalid && Time.time >= m_nextIdleChange)
			{
				m_animationController.Animator.SetCurrentIdleType(m_nextIdleType);
				m_nextIdleType = IdleType.Invalid;
			}
		}
	}
}
