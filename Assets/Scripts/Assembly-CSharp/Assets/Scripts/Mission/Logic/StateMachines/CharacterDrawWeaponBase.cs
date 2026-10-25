using System;
using Assets.Scripts.Animation;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Entities;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public abstract class CharacterDrawWeaponBase : StateMachineAnimListeningBase<Character, CharacterDrawWeaponBase>
	{
		private class StateSwitchAnimationParams
		{
			public Func<AnimationTaskManager, bool> m_animationTaskFactoryMethod;

			public Func<AnimationTaskManager, bool> m_attachAnimationTaskFactoryMethod;

			public AnimationTaskType m_animationTaskType;

			public AnimationTaskManager.ELayer m_layer;
		}

		private const float WeaponSheathDelay = 2f;

		private static readonly StateSwitchAnimationParams[] UnsheathParams = new StateSwitchAnimationParams[3];

		private static readonly StateSwitchAnimationParams[] SheathParams = new StateSwitchAnimationParams[3];

		private readonly EWeaponCategory m_weaponCategory;

		private readonly EWeaponType m_weaponType;

		private EWeaponState m_chosenWeaponState;

		private float m_lastWeaponSwitchRequested;

		public EWeaponCategory WeaponCatgory
		{
			get
			{
				return m_weaponCategory;
			}
		}

		public EWeaponType WeaponType
		{
			get
			{
				return m_weaponType;
			}
		}

		public override EStateType InitialState
		{
			get
			{
				return EStateType.CharacterDrawWeapon_Sheathed;
			}
		}

		public EWeaponState ChosenWeaponState
		{
			get
			{
				return m_chosenWeaponState;
			}
			set
			{
				if (m_chosenWeaponState != value && CurrentStateType != EStateType.CharacterDrawWeapon_Detached)
				{
					m_chosenWeaponState = value;
					EStateType eStateType = ChosenWeaponState.ToStateType();
					if (eStateType == EStateType.CharacterDrawWeapon_Sheathed)
					{
						m_lastWeaponSwitchRequested = LogicMain.CurrentTime;
					}
					else
					{
						CheckWeaponState();
					}
				}
			}
		}

		public override int TurnUpdateIntervalTurns
		{
			get
			{
				return BucketTurnUpdateIntervalTurns;
			}
		}

		protected CharacterDrawWeaponBase(Character _entity, EWeaponCategory _weaponCategory, EWeaponType _type)
			: base(_entity)
		{
			m_weaponCategory = _weaponCategory;
			m_weaponType = _type;
		}

		public static void InitCharacterDrawWeaponBase()
		{
			StateMachineAnimListeningBase<Character, CharacterDrawWeaponBase>.InitStateMachineAnimListeningBase();
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddState<State<Character>>(EStateType.CharacterDrawWeapon_Sheathed, OnEnterSheath, null, OnUpdateSheathed);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddState<State<Character>>(EStateType.CharacterDrawWeapon_Unsheathed, OnEnterUnsheath, null, OnUpdateUnsheathed);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddState<State<Character>>(EStateType.CharacterDrawWeapon_Detached, OnEnterDetached, null, null);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddState<CallbackDelayState<Character>>(EStateType.Generic_WaitingForAnim, OnEnterWaitingForAnim, OnExitWaitingForAnim, null);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddTransition(EStateType.CharacterDrawWeapon_Sheathed, EStateType.Generic_WaitingForAnim);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddTransition(EStateType.CharacterDrawWeapon_Sheathed, EStateType.CharacterDrawWeapon_Unsheathed);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddTransition(EStateType.CharacterDrawWeapon_Sheathed, EStateType.CharacterDrawWeapon_Detached);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddTransition(EStateType.CharacterDrawWeapon_Unsheathed, EStateType.Generic_WaitingForAnim);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddTransition(EStateType.CharacterDrawWeapon_Unsheathed, EStateType.CharacterDrawWeapon_Sheathed);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddTransition(EStateType.CharacterDrawWeapon_Unsheathed, EStateType.CharacterDrawWeapon_Detached);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddTransition(EStateType.Generic_WaitingForAnim, EStateType.CharacterDrawWeapon_Sheathed);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddTransition(EStateType.Generic_WaitingForAnim, EStateType.CharacterDrawWeapon_Unsheathed);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddTransition(EStateType.Generic_WaitingForAnim, EStateType.CharacterDrawWeapon_Detached);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddTransition(EStateType.CharacterDrawWeapon_Detached, EStateType.Generic_WaitingForAnim);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddTransition(EStateType.CharacterDrawWeapon_Detached, EStateType.CharacterDrawWeapon_Sheathed);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddControlling(EStateType.CharacterDrawWeapon_Sheathed, EStateMachineType.AssassinApproachingTarget, EStateType.AssassinApproachingTarget_Approaching);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddControlling(EStateType.CharacterDrawWeapon_Sheathed, EStateMachineType.PlayerMain, EStateType.CombatCharacterMain_Roaming);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddControlling(EStateType.CharacterDrawWeapon_Sheathed, EStateMachineType.EnemyMain, EStateType.CombatCharacterMain_Roaming);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddControlling(EStateType.CharacterDrawWeapon_Unsheathed, EStateMachineType.EnemyMain, EStateType.EnemyMain_Chasing);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddControlling(EStateType.CharacterDrawWeapon_Unsheathed, EStateMachineType.EnemyMain, EStateType.CombatCharacterMain_Combat);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddControlling(EStateType.CharacterDrawWeapon_Unsheathed, EStateMachineType.PlayerMain, EStateType.CombatCharacterMain_Combat);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddControlling(EStateType.CharacterDrawWeapon_Unsheathed, EStateMachineType.AssassinApproachingTarget, EStateType.AssassinApproachingTarget_Approaching);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddControlling(EStateType.Generic_WaitingForAnim, EStateMachineType.EnemyMain, EStateType.EnemyMain_Chasing);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddControlling(EStateType.Generic_WaitingForAnim, EStateMachineType.EnemyMain, EStateType.CombatCharacterMain_Combat);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddControlling(EStateType.Generic_WaitingForAnim, EStateMachineType.PlayerMain, EStateType.CombatCharacterMain_Combat);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddControlling(EStateType.Generic_WaitingForAnim, EStateMachineType.AssassinApproachingTarget, EStateType.AssassinApproachingTarget_Approaching);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddControlling(EStateType.CharacterDrawWeapon_Detached, EStateMachineType.CharacterVitality, EStateType.Character_Dead);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddControlling(EStateType.CharacterDrawWeapon_Detached, EStateMachineType.PlayerVitality, EStateType.Character_Dead);
			StateMachineBase<Character, CharacterDrawWeaponBase>.AddControlling(EStateType.CharacterDrawWeapon_Detached, EStateMachineType.EnemyVitality, EStateType.Character_Dead);
			UnsheathParams[0] = new StateSwitchAnimationParams
			{
				m_animationTaskFactoryMethod = AnimationTaskManagerExtensions.CreateAnimationTaskUnsheathWeapon,
				m_attachAnimationTaskFactoryMethod = null,
				m_animationTaskType = AnimationTaskType.UnsheathWeapon,
				m_layer = AnimationTaskManager.ELayer.UpperBody
			};
			SheathParams[0] = new StateSwitchAnimationParams
			{
				m_animationTaskFactoryMethod = AnimationTaskManagerExtensions.CreateAnimationTaskSheathWeapon,
				m_attachAnimationTaskFactoryMethod = null,
				m_animationTaskType = AnimationTaskType.SheathWeapon,
				m_layer = AnimationTaskManager.ELayer.UpperBody
			};
			UnsheathParams[1] = new StateSwitchAnimationParams
			{
				m_animationTaskFactoryMethod = AnimationTaskManagerExtensions.CreateAnimationTaskUnsheathBow,
				m_attachAnimationTaskFactoryMethod = AnimationTaskManagerExtensions.CreateAnimationTaskUnsheathBowForcePlay,
				m_animationTaskType = AnimationTaskType.UnsheathBow,
				m_layer = AnimationTaskManager.ELayer.Base
			};
			SheathParams[1] = new StateSwitchAnimationParams
			{
				m_animationTaskFactoryMethod = AnimationTaskManagerExtensions.CreateAnimationTaskSheathBow,
				m_attachAnimationTaskFactoryMethod = null,
				m_animationTaskType = AnimationTaskType.SheathBow,
				m_layer = AnimationTaskManager.ELayer.Base
			};
			UnsheathParams[2] = new StateSwitchAnimationParams
			{
				m_animationTaskFactoryMethod = AnimationTaskManagerExtensions.CreateAnimationTaskUnsheathCrossbow,
				m_attachAnimationTaskFactoryMethod = AnimationTaskManagerExtensions.CreateAnimationTaskUnsheathCrossbowForcePlay,
				m_animationTaskType = AnimationTaskType.UnsheathCrossbow,
				m_layer = AnimationTaskManager.ELayer.UpperBody
			};
			SheathParams[2] = new StateSwitchAnimationParams
			{
				m_animationTaskFactoryMethod = AnimationTaskManagerExtensions.CreateAnimationTaskSheathCrossbow,
				m_attachAnimationTaskFactoryMethod = null,
				m_animationTaskType = AnimationTaskType.SheathCrossbow,
				m_layer = AnimationTaskManager.ELayer.UpperBody
			};
		}

		public static AnimationTaskManager.ELayer GetUnsheathAnimationLayer(EWeaponType _weaponType)
		{
			return UnsheathParams[(int)_weaponType].m_layer;
		}

		public override void OnAttachedAndInitialized()
		{
			base.OnAttachedAndInitialized();
			bool flag = CurrentStateType == EStateType.CharacterDrawWeapon_Unsheathed;
			bool flag2 = CurrentStateType == EStateType.CharacterDrawWeapon_Detached;
			if (!flag2)
			{
				StateSwitchAnimationParams[] stateSwitchParams = ((!flag) ? SheathParams : UnsheathParams);
				StateSwitchAnimationParams stateSwitchParams2 = GetStateSwitchParams(stateSwitchParams);
				Func<AnimationTaskManager, bool> attachAnimationTaskFactoryMethod = stateSwitchParams2.m_attachAnimationTaskFactoryMethod;
				if (attachAnimationTaskFactoryMethod != null)
				{
					attachAnimationTaskFactoryMethod(Entity.UnityComponentCache.AnimationController.TaskManager);
				}
			}
			WeaponHandler weaponHandler = GetWeaponHandler();
			if (weaponHandler != null)
			{
				weaponHandler.WeaponDrawn = flag;
				weaponHandler.ForceAttach(!flag2);
			}
		}

		public void ReattachDroppedWeapon()
		{
			if (CurrentStateType == EStateType.CharacterDrawWeapon_Detached)
			{
				ForceInstantSheathedWeapon();
			}
		}

		public void ForceInstantSheathedWeapon()
		{
			if (CurrentStateType != EStateType.CharacterDrawWeapon_Sheathed && !Entity.GetHasZeroHealth())
			{
				m_chosenWeaponState = EWeaponState.Sheathed;
				ChangeStateForced(EStateType.CharacterDrawWeapon_Sheathed);
				AnimationTaskManager taskManager = Entity.UnityComponentCache.AnimationController.TaskManager;
				if (taskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.UpperBody) == AnimationTaskType.SheathWeapon)
				{
					AnimationTaskSheathWeapon animationTaskSheathWeapon = taskManager.GetCurrentTask(AnimationTaskManager.ELayer.UpperBody) as AnimationTaskSheathWeapon;
					animationTaskSheathWeapon.SheathInstant();
				}
			}
		}

		private void CheckWeaponState()
		{
			if (CurrentStateType == EStateType.CharacterDrawWeapon_Detached)
			{
				return;
			}
			EStateType eStateType = ChosenWeaponState.ToStateType();
			if (eStateType == EStateType.CharacterDrawWeapon_Detached)
			{
				if (IsCurrentOrScheduled(EStateType.Generic_WaitingForAnim))
				{
					this.AbortDelayState(EStateType.Generic_WaitingForAnim);
				}
				ChangeStateForced(EStateType.CharacterDrawWeapon_Detached);
			}
			else
			{
				if (Entity.GetDeadOrAboutToDie())
				{
					return;
				}
				ICharacterConditionCarrier characterConditionCarrier = Entity.StateMachines as ICharacterConditionCarrier;
				if (characterConditionCarrier != null)
				{
					EStateType currentStateType = characterConditionCarrier.CharacterCondition.CurrentStateType;
					if (currentStateType != EStateType.CharacterCondition_Normal && currentStateType != EStateType.CharacterCondition_ThreateningVictim && eStateType == EStateType.CharacterDrawWeapon_Unsheathed)
					{
						return;
					}
				}
				if (Entity.GetIsInParcour() || ScheduledStateType != 0)
				{
					return;
				}
				if (CurrentStateType == EStateType.CharacterDrawWeapon_Sheathed && eStateType == EStateType.CharacterDrawWeapon_Unsheathed)
				{
					if (AllowUnsheathWeapon())
					{
						if (Entity.IsAttached)
						{
							DelayUnsheathWeapon();
						}
						else
						{
							ScheduleStateChangeForced(eStateType);
						}
					}
				}
				else
				{
					if (CurrentStateType != EStateType.CharacterDrawWeapon_Unsheathed || eStateType != EStateType.CharacterDrawWeapon_Sheathed)
					{
						return;
					}
					if (Entity.IsAttached)
					{
						if (LogicMain.CurrentTime - 2f >= m_lastWeaponSwitchRequested)
						{
							DelaySheathWeapon();
						}
					}
					else
					{
						ScheduleStateChangeForced(eStateType);
					}
				}
			}
		}

		private static void OnEnterSheath(IStateMachine<Character> stateMachine, IPayload payload, IState<Character> currentState, IState<Character> nextState)
		{
			ForceWeaponVisibility(stateMachine, false);
			stateMachine.Cast<CharacterDrawWeaponBase>().CheckWeaponState();
		}

		private static void OnEnterUnsheath(IStateMachine<Character> stateMachine, IPayload payload, IState<Character> currentState, IState<Character> nextState)
		{
			ForceWeaponVisibility(stateMachine, true);
			stateMachine.Cast<CharacterDrawWeaponBase>().CheckWeaponState();
		}

		private static void OnUpdateSheathed(IStateMachine<Character> stateMachine, IPayload payload, IState<Character> currentState)
		{
			stateMachine.Cast<CharacterDrawWeaponBase>().CheckWeaponState();
		}

		private static void OnUpdateUnsheathed(IStateMachine<Character> stateMachine, IPayload payload, IState<Character> currentState)
		{
			stateMachine.Cast<CharacterDrawWeaponBase>().CheckWeaponState();
		}

		private static void OnEnterDetached(IStateMachine<Character> _stateMachine, IPayload _payload, IState<Character> _currentState, IState<Character> _nextState)
		{
			if (_stateMachine.Entity.IsAttached)
			{
				WeaponHandler weaponHandler = _stateMachine.Cast<CharacterDrawWeaponBase>().GetWeaponHandler();
				if (weaponHandler != null)
				{
					weaponHandler.DropToFloor(_stateMachine.Entity);
				}
			}
		}

		private static void OnEnterWaitingForAnim(IStateMachine<Character> _statemachine, IPayload _payload, IState<Character> _prevstate, IState<Character> _currentstate)
		{
			if (_statemachine.Entity.IsAttached)
			{
				WeaponHandler weaponHandler = _statemachine.Cast<CharacterDrawWeaponBase>().GetWeaponHandler();
				if (weaponHandler != null)
				{
					weaponHandler.StartListenForAnimatorEvents();
				}
			}
		}

		private static void OnExitWaitingForAnim(IStateMachine<Character> _statemachine, IPayload _payload, IState<Character> _currentstate, IState<Character> _nextstate)
		{
			if (_statemachine.Entity.IsAttached)
			{
				WeaponHandler weaponHandler = _statemachine.Cast<CharacterDrawWeaponBase>().GetWeaponHandler();
				if (weaponHandler != null)
				{
					weaponHandler.StopListenForAnimatorEvents();
				}
			}
		}

		private bool DelayUnsheathWeapon()
		{
			return StartStateSwitchAnimation(UnsheathParams, EStateType.CharacterDrawWeapon_Unsheathed);
		}

		private bool DelaySheathWeapon()
		{
			return StartStateSwitchAnimation(SheathParams, EStateType.CharacterDrawWeapon_Sheathed);
		}

		private bool StartStateSwitchAnimation(StateSwitchAnimationParams[] _stateSwitchParams, EStateType _delayedState)
		{
			StateSwitchAnimationParams stateSwitchParams = GetStateSwitchParams(_stateSwitchParams);
			if (stateSwitchParams != null && stateSwitchParams.m_animationTaskFactoryMethod(Entity.UnityComponentCache.AnimationController.TaskManager))
			{
				this.EnterCallbackDelayState(EStateType.Generic_WaitingForAnim, _delayedState, _delayedState, stateSwitchParams.m_animationTaskType, stateSwitchParams.m_layer);
				return true;
			}
			return false;
		}

		private bool AllowUnsheathWeapon()
		{
			if (!Entity.IsAttached)
			{
				return true;
			}
			for (AnimatorStateMachineStatic.StateMachine stateMachine = Entity.UnityComponentCache.AnimationController.StateMachine.GetCurrentStateMachine(); stateMachine != null; stateMachine = stateMachine.ParentStateMachine)
			{
				if (stateMachine.NameHash == AnimationStateMachineHashes.StateMachine_CoinThrowReaction.NameHash)
				{
					return false;
				}
			}
			return true;
		}

		private StateSwitchAnimationParams GetStateSwitchParams(StateSwitchAnimationParams[] _stateSwitchParams)
		{
			return _stateSwitchParams[(int)m_weaponType];
		}

		public WeaponHandler GetWeaponHandler()
		{
			return Entity.WeaponHandlerCache.GetWeaponHandler(m_weaponCategory);
		}

		private static void ForceWeaponVisibility(IStateMachine<Character> _statemachine, bool _visible)
		{
			if (_statemachine.Entity.IsAttached)
			{
				WeaponHandler weaponHandler = _statemachine.Cast<CharacterDrawWeaponBase>().GetWeaponHandler();
				if (weaponHandler != null)
				{
					weaponHandler.ForceAttach(true);
					weaponHandler.WeaponDrawn = _visible;
				}
			}
		}
	}
}
