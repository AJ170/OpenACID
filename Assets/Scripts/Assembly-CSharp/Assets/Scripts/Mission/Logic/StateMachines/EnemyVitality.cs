using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using UbiCore.Messenger;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class EnemyVitality : CombatCharacterVitalityBase<EnemyCharacter, EnemyVitality>
	{
		public override EStateMachineType Type
		{
			get
			{
				return EStateMachineType.EnemyVitality;
			}
		}

		public EnemyVitality(EnemyCharacter _entity)
			: base(_entity)
		{
		}

		public static void InitEnemyVitality()
		{
			CombatCharacterVitalityBase<EnemyCharacter, EnemyVitality>.InitCombatCharacterVitalityBase();
			StateMachineBase<EnemyCharacter, EnemyVitality>.OverrideStateHandlers(EStateType.Character_Alive, null, null, OnUpdateAlive);
			StateMachineBase<EnemyCharacter, EnemyVitality>.OverrideStateHandlers(EStateType.Character_Dead, OnEnterDead, null, OnUpdateDead);
		}

		private new static void OnUpdateAlive(IStateMachine<EnemyCharacter> stateMachine, IPayload payload, IState<EnemyCharacter> currentState)
		{
			CombatCharacterVitalityBase<EnemyCharacter, EnemyVitality>.OnUpdateAlive(stateMachine, payload, currentState);
		}

		public new static void OnEnterDead(IStateMachine<EnemyCharacter> _stateMachine, IPayload _payload, IState<EnemyCharacter> _prevState, IState<EnemyCharacter> _currentState)
		{
			_stateMachine.Entity.CharacterAi.CancelRunning();
			CombatCharacterVitalityBase<EnemyCharacter, EnemyVitality>.OnEnterDead(_stateMachine, _payload, _prevState, _currentState);
			Messenger.Instance.Broadcast(new NPCKillEvent(_stateMachine.Entity));
			Patrol patrol = _stateMachine.Entity.Patrol;
			if (patrol != null)
			{
				patrol.Remove(_stateMachine.Entity.Id, false);
			}
			LogicMain.GameStats.AddInt(StatTypes.EStatType.Game_RecentlyKilledEnemyCount, 1);
			LogicMain.GameStats.Set_int(StatTypes.EStatType.Game_RecentlyKilledEnemyTurn, LogicMain.CurrentTurn);
			if (_stateMachine.Entity.IsAttached)
			{
				_stateMachine.Entity.CanBePooled = false;
			}
		}

		public static void OnUpdateDead(IStateMachine<EnemyCharacter> _stateMachine, IPayload _payload, IState<EnemyCharacter> _currentState)
		{
			int num = _stateMachine.Entity.EntityStats.Get_int(StatTypes.EStatType.Character_DeathTurn);
			if (num != 0 && LogicMain.CurrentTurn - num > 70)
			{
				while (!_stateMachine.Entity.CanBePooled)
				{
					_stateMachine.Entity.CanBePooled = true;
				}
			}
			if (!_stateMachine.Entity.IsAttached)
			{
				LogicMain.EntityManager.UnregisterEntity(_stateMachine.Entity);
			}
		}
	}
}
