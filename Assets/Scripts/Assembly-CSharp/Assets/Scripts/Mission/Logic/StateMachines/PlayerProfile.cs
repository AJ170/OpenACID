using AcierProtocol;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class PlayerProfile : StateMachineBase<AssassinCharacter, PlayerProfile>
	{
		private EMovementSpeed m_ChosenSpeed;

		public override EStateMachineType Type
		{
			get
			{
				return EStateMachineType.PlayerProfile;
			}
		}

		public override EStateType InitialState
		{
			get
			{
				return EStateType.PlayerProfile_High;
			}
		}

		public EMovementSpeed ChosenSpeed
		{
			get
			{
				return m_ChosenSpeed;
			}
			set
			{
				m_ChosenSpeed = value;
			}
		}

		public bool SpeedIsFixed { get; private set; }

		public bool FixedSpeedIsOverriden { get; private set; }

		private bool IsSpeedChangeAllowed
		{
			get
			{
				if (Entity.GetIsBeingChased())
				{
					return false;
				}
				if (Entity.GetIsApproachingTarget())
				{
					SkillData.IApproachSkillDefinition buffedApproachSkillDefinition = UbiCore.Utils.Singleton<SkillData>.Instance.GetBuffedApproachSkillDefinition(Entity.AssassinCharacterStateMachines.AssassinApproachingTarget.Reason, Entity);
					return buffedApproachSkillDefinition != null;
				}
				return true;
			}
		}

		public PlayerProfile(AssassinCharacter _entity)
			: base(_entity)
		{
		}

		public static void InitPlayerProfile()
		{
			StateMachineBase<AssassinCharacter, PlayerProfile>.InitStateMachineBase();
			StateMachineBase<AssassinCharacter, PlayerProfile>.AddState<State<AssassinCharacter>>(EStateType.PlayerProfile_Low, OnEnterLowProfile, null, OnUpdate);
			StateMachineBase<AssassinCharacter, PlayerProfile>.AddState<State<AssassinCharacter>>(EStateType.PlayerProfile_High, OnEnterHighProfile, null, OnUpdate);
			StateMachineBase<AssassinCharacter, PlayerProfile>.AddState<State<AssassinCharacter>>(EStateType.PlayerProfile_Chase, OnEnterChasing, null, OnUpdate);
			StateMachineBase<AssassinCharacter, PlayerProfile>.AddTransition(EStateType.PlayerProfile_Low, EStateType.PlayerProfile_High);
			StateMachineBase<AssassinCharacter, PlayerProfile>.AddTransition(EStateType.PlayerProfile_Low, EStateType.PlayerProfile_Chase);
			StateMachineBase<AssassinCharacter, PlayerProfile>.AddTransition(EStateType.PlayerProfile_High, EStateType.PlayerProfile_Low);
			StateMachineBase<AssassinCharacter, PlayerProfile>.AddTransition(EStateType.PlayerProfile_High, EStateType.PlayerProfile_Chase);
			StateMachineBase<AssassinCharacter, PlayerProfile>.AddTransition(EStateType.PlayerProfile_Chase, EStateType.PlayerProfile_Low);
			StateMachineBase<AssassinCharacter, PlayerProfile>.AddTransition(EStateType.PlayerProfile_Chase, EStateType.PlayerProfile_High);
			StateMachineBase<AssassinCharacter, PlayerProfile>.AddControlling(EStateType.PlayerProfile_Chase, EStateMachineType.PlayerMain, EStateType.PlayerMain_BeingChased);
			StateMachineBase<AssassinCharacter, PlayerProfile>.AddControlling(EStateType.PlayerProfile_High, EStateMachineType.PlayerMain, EStateType.PlayerMain_BeingChased);
			StateMachineBase<AssassinCharacter, PlayerProfile>.AddControlling(EStateType.PlayerProfile_Low, EStateMachineType.PlayerMain, EStateType.PlayerMain_HidingInPatrol);
			StateMachineBase<AssassinCharacter, PlayerProfile>.AddControlling(EStateType.PlayerProfile_Low, EStateMachineType.PlayerStalkingZone, EStateType.PlayerStalkingZone_NotStalking);
		}

		public override void Attach()
		{
			base.Attach();
			AnimationController animationController = Entity.UnityComponentCache.AnimationController;
			ChosenSpeed = animationController.ReferenceMovementSpeed;
		}

		public void OverrideFixedSpeed()
		{
			FixedSpeedIsOverriden = true;
		}

		private static void OnEnterLowProfile(IStateMachine<AssassinCharacter> _statemachine, IPayload _payload, IState<Character> _prevstate, IState<Character> _currentstate)
		{
			OnChangeAnimationProfile(_statemachine, EStateType.PlayerProfile_Low);
		}

		private static void OnEnterHighProfile(IStateMachine<AssassinCharacter> _statemachine, IPayload _payload, IState<Character> _prevstate, IState<Character> _currentstate)
		{
			OnChangeAnimationProfile(_statemachine, EStateType.PlayerProfile_High);
		}

		private static void OnEnterChasing(IStateMachine<AssassinCharacter> _statemachine, IPayload _payload, IState<Character> _prevstate, IState<Character> _currentstate)
		{
			OnChangeAnimationProfile(_statemachine, EStateType.PlayerProfile_Chase);
		}

		private static void OnUpdate(IStateMachine<AssassinCharacter> stateMachine, IPayload payload, IState<Character> currentState)
		{
			PlayerProfile playerProfile = (PlayerProfile)stateMachine;
			playerProfile.CheckChosenSpeed();
			CheckForSprintingInHighProfileOverride(stateMachine.Entity);
		}

		private void CheckChosenSpeed()
		{
			SpeedIsFixed = false;
			PlayerCharacter playerCharacter = Entity as PlayerCharacter;
			EStateType eStateType = ChosenSpeed.ToStateType();
			if (Entity.GetIsHidingInPatrol())
			{
				eStateType = EStateType.PlayerProfile_Low;
				FixedSpeedIsOverriden = false;
			}
			else if (Entity.GetIsHidingInObject())
			{
				eStateType = EStateType.PlayerProfile_Low;
				FixedSpeedIsOverriden = false;
			}
			else if (!FixedSpeedIsOverriden)
			{
				if (Entity.GetIsInDisguise())
				{
					eStateType = EStateType.PlayerProfile_Low;
					SpeedIsFixed = true;
				}
				else if (playerCharacter != null && playerCharacter.GetIsUsingEagleEye())
				{
					eStateType = EStateType.PlayerProfile_Low;
					SpeedIsFixed = true;
				}
				else if (playerCharacter != null && playerCharacter.PlayerCharacterStateMachines.PlayerStalkingZone.WantsLowProfile)
				{
					eStateType = EStateType.PlayerProfile_Low;
					SpeedIsFixed = true;
				}
			}
			if ((CurrentStateType != eStateType || ScheduledStateType != 0) && ScheduledStateType != eStateType && IsSpeedChangeAllowed)
			{
				ScheduleStateChangeForced(eStateType);
			}
		}

		private static void CheckForSprintingInHighProfileOverride(IPlayerProfileCarrierEntity _entity)
		{
			IPlayerStatsCarrier playerStatsCarrier = _entity as IPlayerStatsCarrier;
			if (playerStatsCarrier != null && _entity.PlayerProfileCarrier.PlayerProfile.CurrentStateType == EStateType.PlayerProfile_High && playerStatsCarrier.PlayerStats.Get_bool(StatTypes.EStatType.Player_SprintInHighProfileOverride))
			{
				_entity.ConfigureAnimationController(EStateType.PlayerProfile_Chase);
			}
		}

		private static void OnChangeAnimationProfile(IStateMachine<AssassinCharacter> _stateMachine, EStateType _stateType)
		{
			PlayerProfile playerProfile = (PlayerProfile)_stateMachine;
			playerProfile.FixedSpeedIsOverriden = false;
			_stateMachine.Entity.ConfigureAnimationController(_stateType);
			AnimationController animationController = _stateMachine.Entity.UnityComponentCache.AnimationController;
			if (animationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) == AnimationTaskType.Idle)
			{
				animationController.TaskManager.CreateAnimationTaskIdle();
			}
			_stateMachine.Entity.AssassinInteraction.Reevaluate();
			CheckForSprintingInHighProfileOverride(_stateMachine.Entity);
		}
	}
}
