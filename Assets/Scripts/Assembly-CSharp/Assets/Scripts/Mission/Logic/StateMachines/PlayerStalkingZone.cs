using AcierProtocol;
using Assets.Scripts.Animation;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Motion.Hotspots;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class PlayerStalkingZone : StateMachineBase<AssassinCharacter, PlayerStalkingZone>
	{
		public class StalkingZoneState : State<AssassinCharacter>
		{
			public class StalkingZoneStatePayload : IPayload
			{
				public EntityId StalkingZone { get; set; }

				public StalkingZoneStatePayload()
				{
					Reset();
				}

				public void Reset()
				{
					StalkingZone = EntityId.InvalidId;
				}
			}

			public StalkingZoneState(EStateType _stateType)
				: this(_stateType, null, null, null)
			{
			}

			public StalkingZoneState(EStateType _stateType, OnEnterHandler _onEnterHandler, OnExitHandler _onExitHandler, OnUpdateHandler _onUpdateHandler)
				: base(_stateType, _onEnterHandler, _onExitHandler, _onUpdateHandler)
			{
			}

			public override IPayload CreatePayload(IStateMachine<AssassinCharacter> _stateMachine)
			{
				return new StalkingZoneStatePayload();
			}
		}

		public override EStateType InitialState
		{
			get
			{
				return EStateType.PlayerStalkingZone_NotStalking;
			}
		}

		public override EStateMachineType Type
		{
			get
			{
				return EStateMachineType.PlayerStalkingZone;
			}
		}

		public bool WantsLowProfile
		{
			get
			{
				if (WantsToStalk && (Entity.GetSpeed() != EMovementSpeed.Walk || IsEnterStalkingTimerFinished || IsEnterStalkingTimerActive || IsCurrentOrScheduled(EStateType.PlayerStalkingZone_Stalking)))
				{
					return true;
				}
				return false;
			}
		}

		public bool WantsToStalk { get; private set; }

		public bool IsEnterStalkingTimerFinished
		{
			get
			{
				return EnterStalkingTime > 0f && LogicMain.CurrentTime >= EnterStalkingTime;
			}
		}

		public bool IsEnterStalkingTimerActive
		{
			get
			{
				return EnterStalkingTime > 0f && LogicMain.CurrentTime < EnterStalkingTime;
			}
		}

		public bool PlayerSwitchedToHighProfile { get; set; }

		public float EnterStalkingTime { get; private set; }

		public PlayerStalkingZone(AssassinCharacter _entity)
			: base(_entity)
		{
		}

		public static void InitPlayerStalkingZone()
		{
			StateMachineBase<AssassinCharacter, PlayerStalkingZone>.InitStateMachineBase();
			StateMachineBase<AssassinCharacter, PlayerStalkingZone>.AddState<StalkingZoneState>(EStateType.PlayerStalkingZone_Stalking, OnEnterStalking, OnExitStalking, OnUpdate);
			StateMachineBase<AssassinCharacter, PlayerStalkingZone>.AddState<StalkingZoneState>(EStateType.PlayerStalkingZone_NotStalking, null, null, OnUpdate);
			StateMachineBase<AssassinCharacter, PlayerStalkingZone>.AddTransition(EStateType.PlayerStalkingZone_Stalking, EStateType.PlayerStalkingZone_NotStalking);
			StateMachineBase<AssassinCharacter, PlayerStalkingZone>.AddTransition(EStateType.PlayerStalkingZone_NotStalking, EStateType.PlayerStalkingZone_Stalking);
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			ResetEnterStalkingTimer();
		}

		private static void OnEnterStalking(IStateMachine<AssassinCharacter> _stateMachine, IPayload _payload, IState<Character> _prevState, IState<Character> _currState)
		{
			_stateMachine.Entity.UnityComponentCache.AnimationController.StartCrouch(1f, 1f);
			_stateMachine.Entity.Highlight.SetMode(Highlight.EHighlightLayer.Hiding, Highlight.ESelectionMode.Hiding);
			ChangeIdleTypeIfNeeded(_stateMachine, IdleType.Crouch);
		}

		private static void OnExitStalking(IStateMachine<AssassinCharacter> _stateMachine, IPayload _payload, IState<Character> _prevState, IState<Character> _currState)
		{
			_stateMachine.Entity.UnityComponentCache.AnimationController.StartCrouch(0f, 1f);
			_stateMachine.Entity.Highlight.RemoveMode(Highlight.EHighlightLayer.Hiding);
			ChangeIdleTypeIfNeeded(_stateMachine, (_stateMachine.Entity.GetSpeed() == EMovementSpeed.Walk) ? IdleType.LowProfile : IdleType.HighProfile);
		}

		private static void ChangeIdleTypeIfNeeded(IStateMachine<AssassinCharacter> _stateMachine, IdleType _type)
		{
			AnimationTaskBase currentTask = _stateMachine.Entity.UnityComponentCache.AnimationController.TaskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base);
			if (currentTask != null && currentTask is AnimationTaskIdle)
			{
				currentTask.Cast<AnimationTaskIdle>().OnUpdateIdleType(_type, IdleType.Invalid);
			}
		}

		private static void OnUpdate(IStateMachine<AssassinCharacter> _stateMachine, IPayload _payload, IState<Character> _character)
		{
			if (_stateMachine.CurrentStateType == EStateType.PlayerStalkingZone_Stalking)
			{
				OnUpdateStalking(_stateMachine, _payload, _character);
			}
			else
			{
				OnUpdateNotStalking(_stateMachine, _payload, _character);
			}
		}

		private static void OnUpdateStalking(IStateMachine<AssassinCharacter> _stateMachine, IPayload _payload, IState<Character> _character)
		{
			StalkingZone _stalkingZone = null;
			AssassinCharacter entity = _stateMachine.Entity;
			bool flag = UbiCore.Utils.Singleton<HotspotService>.Instance.StalkingZoneManager.IsInsideStalkingZone(entity, out _stalkingZone);
			bool flag2 = entity.GetIsExposed() && !entity.GetIsUnseen();
			bool isInDisguise = entity.GetIsInDisguise();
			bool isInCombat = entity.GetIsInCombat();
			bool isActive = PlayerHelper.GetIsActive("Silent_Dagger_01".ToEnum<SkillTypes>());
			bool isWaitingForAnim = entity.GetIsWaitingForAnim();
			EMovementSpeed speed = entity.GetSpeed();
			if (isInCombat || !flag || isActive || flag2 || isInDisguise || speed != EMovementSpeed.Walk || isWaitingForAnim)
			{
				PlayerStalkingZone playerStalkingZone = (PlayerStalkingZone)_stateMachine;
				playerStalkingZone.PlayerSwitchedToHighProfile = flag && !flag2 && speed != EMovementSpeed.Walk;
				_stateMachine.ScheduleStateChange(EStateType.PlayerStalkingZone_NotStalking);
			}
		}

		private static void OnUpdateNotStalking(IStateMachine<AssassinCharacter> _stateMachine, IPayload _payload, IState<Character> _character)
		{
			PlayerStalkingZone playerStalkingZone = (PlayerStalkingZone)_stateMachine;
			AssassinCharacter entity = _stateMachine.Entity;
			AnimationController animationController = entity.UnityComponentCache.AnimationController;
			StalkingZone _stalkingZone = null;
			bool flag = UbiCore.Utils.Singleton<HotspotService>.Instance.StalkingZoneManager.IsInsideStalkingZone(entity, out _stalkingZone);
			bool flag2 = entity.GetIsExposed() && !entity.GetIsUnseen();
			bool isRoaming = entity.GetIsRoaming();
			bool isInDisguise = entity.GetIsInDisguise();
			bool isInCombat = entity.GetIsInCombat();
			bool isApproachingTarget = entity.GetIsApproachingTarget();
			bool flag3 = entity.GetIsApproaching() && !UbiCore.Utils.Singleton<HotspotService>.Instance.StalkingZoneManager.IsPositionInStalkingZone(entity.GetApproachPosition());
			EMovementSpeed speed = entity.GetSpeed();
			bool flag4 = speed != EMovementSpeed.Walk && entity.GetIsNavigating();
			if (!flag)
			{
				playerStalkingZone.PlayerSwitchedToHighProfile = false;
			}
			playerStalkingZone.WantsToStalk = !flag3 && !isApproachingTarget && !isInCombat && !flag4 && flag && isRoaming && !flag2 && !isInDisguise && (!playerStalkingZone.PlayerSwitchedToHighProfile || entity.AssassinCharacterStateMachines.PlayerProfile.ChosenSpeed == EMovementSpeed.Walk);
			if (playerStalkingZone.WantsToStalk)
			{
				if (speed == EMovementSpeed.Walk)
				{
					if (playerStalkingZone.IsEnterStalkingTimerFinished)
					{
						playerStalkingZone.ResetEnterStalkingTimer();
						StalkingZoneState.StalkingZoneStatePayload payload = _stateMachine.GetPayload<StalkingZoneState.StalkingZoneStatePayload>(EStateType.PlayerStalkingZone_Stalking);
						payload.StalkingZone = _stalkingZone.Id;
						_stateMachine.ScheduleStateChange(EStateType.PlayerStalkingZone_Stalking);
					}
					else if (!playerStalkingZone.IsEnterStalkingTimerActive)
					{
						playerStalkingZone.StartEnterStalkingTimer();
					}
				}
				if (speed == EMovementSpeed.Walk && !animationController.IsEnteringCrouch())
				{
					animationController.StartCrouch(1f, 1f);
				}
			}
			else if (animationController.CrouchValue.Scalar > 0f && !animationController.IsLeavingCrouch())
			{
				animationController.StartCrouch(0f, 1f);
			}
		}

		public float GetEnterStalkingDurationSec()
		{
			float num = Entity.PlayerStats.Get_float(StatTypes.EStatType.Player_StealthModifier);
			float num2 = Entity.PlayerStats.Get_float(StatTypes.EStatType.Player_StalkingZoneEnterDurationSecBase);
			float num3 = Entity.PlayerStats.Get_float(StatTypes.EStatType.Player_StalkingZoneEnterDurationSecOffset);
			float num4 = 1f / num;
			return num2 + num4 * num3;
		}

		public void StartEnterStalkingTimer()
		{
			EnterStalkingTime = LogicMain.CurrentTime + GetEnterStalkingDurationSec();
		}

		public void ResetEnterStalkingTimer()
		{
			EnterStalkingTime = -1f;
		}
	}
}
