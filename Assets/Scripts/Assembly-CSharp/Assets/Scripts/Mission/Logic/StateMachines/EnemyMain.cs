using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class EnemyMain : StateMachineAnimListeningBase<EnemyCharacter, EnemyMain>
	{
		public override EStateMachineType Type
		{
			get
			{
				return EStateMachineType.EnemyMain;
			}
		}

		public override EStateType InitialState
		{
			get
			{
				return EStateType.CombatCharacterMain_Roaming;
			}
		}

		public EnemyMain(EnemyCharacter _entity)
			: base(_entity)
		{
		}

		public static void InitEnemyMain()
		{
			StateMachineAnimListeningBase<EnemyCharacter, EnemyMain>.InitStateMachineAnimListeningBase();
			StateMachineBase<EnemyCharacter, EnemyMain>.AddState<State<EnemyCharacter>>(EStateType.CombatCharacterMain_Roaming, OnEnterRoaming, null, null);
			StateMachineBase<EnemyCharacter, EnemyMain>.AddState<State<EnemyCharacter>>(EStateType.CombatCharacterMain_Combat, OnEnterCombat, OnExitCombat, null);
			StateMachineBase<EnemyCharacter, EnemyMain>.AddState<State<EnemyCharacter>>(EStateType.EnemyMain_Chasing, OnEnterChasing, null, null);
			StateMachineBase<EnemyCharacter, EnemyMain>.AddState<CallbackDelayState<EnemyCharacter>>(EStateType.CombatCharacterMain_Parcour, OnEnterParcour, OnExitParcour, null);
			StateMachineBase<EnemyCharacter, EnemyMain>.AddTransition(EStateType.CombatCharacterMain_Roaming, EStateType.CombatCharacterMain_Combat);
			StateMachineBase<EnemyCharacter, EnemyMain>.AddTransition(EStateType.CombatCharacterMain_Roaming, EStateType.EnemyMain_Chasing);
			StateMachineBase<EnemyCharacter, EnemyMain>.AddTransition(EStateType.CombatCharacterMain_Roaming, EStateType.CombatCharacterMain_Parcour);
			StateMachineBase<EnemyCharacter, EnemyMain>.AddTransition(EStateType.CombatCharacterMain_Combat, EStateType.EnemyMain_Chasing);
			StateMachineBase<EnemyCharacter, EnemyMain>.AddTransition(EStateType.CombatCharacterMain_Combat, EStateType.CombatCharacterMain_Roaming);
			StateMachineBase<EnemyCharacter, EnemyMain>.AddTransition(EStateType.EnemyMain_Chasing, EStateType.CombatCharacterMain_Combat);
			StateMachineBase<EnemyCharacter, EnemyMain>.AddTransition(EStateType.EnemyMain_Chasing, EStateType.CombatCharacterMain_Roaming);
			StateMachineBase<EnemyCharacter, EnemyMain>.AddTransition(EStateType.EnemyMain_Chasing, EStateType.CombatCharacterMain_Parcour);
			StateMachineBase<EnemyCharacter, EnemyMain>.AddTransition(EStateType.CombatCharacterMain_Parcour, EStateType.CombatCharacterMain_Roaming);
			StateMachineBase<EnemyCharacter, EnemyMain>.AddTransition(EStateType.CombatCharacterMain_Parcour, EStateType.EnemyMain_Chasing);
			StateMachineBase<EnemyCharacter, EnemyMain>.AddControlling(EStateType.EnemyMain_Chasing, EStateMachineType.CharacterAwareness, EStateType.CharacterAwareness_Alarmed);
			StateMachineBase<EnemyCharacter, EnemyMain>.AddControlling(EStateType.EnemyMain_Chasing, EStateMachineType.EnemyVitality, EStateType.Character_Alive);
			StateMachineBase<EnemyCharacter, EnemyMain>.AddControlling(EStateType.EnemyMain_Chasing, EStateMachineType.PlayerVitality, EStateType.Character_Alive);
			StateMachineBase<EnemyCharacter, EnemyMain>.AddControlling(EStateType.CombatCharacterMain_Roaming, EStateMachineType.CharacterAwareness, EStateType.CharacterAwareness_Alarmed);
			StateMachineBase<EnemyCharacter, EnemyMain>.AddControlling(EStateType.CombatCharacterMain_Combat, EStateMachineType.AssassinApproachingTarget, EStateType.AssassinApproachingTarget_Approaching);
		}

		private static void OnEnterRoaming(IStateMachine<EnemyCharacter> _statemachine, IPayload _payload, IState<EnemyCharacter> _prevstate, IState<EnemyCharacter> _currentstate)
		{
			IStateMachine<ICharacterVitalityCarrierEntity> characterVitality = _statemachine.Entity.EnemyCharacterStateMachines.CharacterVitality;
			if (characterVitality.CurrentStateType == EStateType.Character_Immortal)
			{
				characterVitality.ScheduleStateChange(EStateType.Character_Alive);
			}
			if (!_statemachine.Entity.GetHasZeroHealth())
			{
				_statemachine.Entity.ChooseWeaponState(EWeaponCategory.Melee, EWeaponState.Sheathed);
			}
		}

		private static void OnEnterChasing(IStateMachine<EnemyCharacter> _statemachine, IPayload _payload, IState<EnemyCharacter> _prevstate, IState<EnemyCharacter> _currentstate)
		{
			IStateMachine<ICharacterVitalityCarrierEntity> characterVitality = _statemachine.Entity.EnemyCharacterStateMachines.CharacterVitality;
			if (characterVitality.CurrentStateType == EStateType.Character_Immortal)
			{
				characterVitality.ScheduleStateChange(EStateType.Character_Alive);
			}
			if (!_statemachine.Entity.GetHasZeroHealth() && !HasRangedWeapon(_statemachine))
			{
				_statemachine.Entity.ChooseWeaponState(EWeaponCategory.Melee, EWeaponState.Unsheathed);
			}
			RemoveFromPatrol(_statemachine);
		}

		private static void OnEnterCombat(IStateMachine<EnemyCharacter> _statemachine, IPayload _payload, IState<EnemyCharacter> _prevstate, IState<EnemyCharacter> _currentstate)
		{
			EnemyCharacter enemyCharacter = _statemachine.Entity.Cast<EnemyCharacter>();
			enemyCharacter.CharacterAi.IsActive = false;
			RemoveFromPatrol(_statemachine);
		}

		private static void OnExitCombat(IStateMachine<EnemyCharacter> _statemachine, IPayload _payload, IState<EnemyCharacter> _prevstate, IState<EnemyCharacter> _currentstate)
		{
			NpcCharacter npcCharacter = _statemachine.Entity.Cast<NpcCharacter>();
			npcCharacter.CharacterAi.IsActive = true;
			IStateMachine stateMachine = npcCharacter.StateMachines.GetStateMachine(EStateMachineType.CharacterAwareness);
			CharacterAwareness.AlarmedState.AlarmedPayload payload = stateMachine.GetPayload<CharacterAwareness.AlarmedState.AlarmedPayload>(EStateType.CharacterAwareness_Alarmed);
			payload.UpdateLastKnownPosition(npcCharacter, LogicMain.PlayerEntity);
		}

		private static void OnEnterParcour(IStateMachine<EnemyCharacter> _statemachine, IPayload _payload, IState<EnemyCharacter> _prevstate, IState<EnemyCharacter> _currentstate)
		{
			EnemyCharacterStateMachines enemyCharacterStateMachines = _statemachine.Entity.EnemyCharacterStateMachines;
			if (enemyCharacterStateMachines.CharacterVitality.CurrentStateType == EStateType.Character_Immortal)
			{
				enemyCharacterStateMachines.CharacterVitality.ScheduleStateChange(EStateType.Character_Alive);
			}
			if (_statemachine.Entity.GetRangedWeaponState() == EWeaponState.Unsheathed)
			{
				_statemachine.Entity.ChooseWeaponState(EWeaponCategory.Ranged, EWeaponState.Sheathed);
			}
			_statemachine.Entity.CanBePooled = false;
		}

		private static void OnExitParcour(IStateMachine<EnemyCharacter> _statemachine, IPayload _payload, IState<EnemyCharacter> _currentstate, IState<EnemyCharacter> _nextstate)
		{
			_statemachine.Entity.CanBePooled = true;
		}

		private static void RemoveFromPatrol(IStateMachine<EnemyCharacter> _statemachine)
		{
			Assets.Scripts.Mission.Logic.Entities.Patrol patrol = _statemachine.Entity.Patrol;
			if (patrol != null)
			{
				patrol.Remove(_statemachine.Entity.Id, false);
			}
		}

		private static bool HasRangedWeapon(IStateMachine<EnemyCharacter> _statemachine)
		{
			EWeaponState rangedWeaponState = _statemachine.Entity.GetRangedWeaponState();
			return rangedWeaponState == EWeaponState.Sheathed || rangedWeaponState == EWeaponState.Unsheathed;
		}
	}
}
