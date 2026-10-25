using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public abstract class CharacterVitalityBase<TEntity, TThis> : StateMachineBase<TEntity, TThis> where TEntity : ICharacterVitalityCarrierEntity where TThis : CharacterVitalityBase<TEntity, TThis>
	{
		public sealed override EStateType InitialState
		{
			get
			{
				return EStateType.Character_Alive;
			}
		}

		protected CharacterVitalityBase(TEntity _entity)
			: base(_entity)
		{
		}

		protected static void InitCharacterVitalityBase()
		{
			StateMachineBase<TEntity, TThis>.InitStateMachineBase();
			StateMachineBase<TEntity, TThis>.AddState<State<TEntity>>(EStateType.Character_Alive, null, null, null);
			StateMachineBase<TEntity, TThis>.AddState<State<TEntity>>(EStateType.Character_Dead, OnEnterDead, null, null);
			StateMachineBase<TEntity, TThis>.AddState<State<TEntity>>(EStateType.Character_Immortal, null, null, null);
			StateMachineBase<TEntity, TThis>.AddTransition(EStateType.Character_Alive, EStateType.Character_Dead);
			StateMachineBase<TEntity, TThis>.AddTransition(EStateType.Character_Alive, EStateType.Character_Immortal);
			StateMachineBase<TEntity, TThis>.AddTransition(EStateType.Character_Immortal, EStateType.Character_Alive);
			StateMachineBase<TEntity, TThis>.AddControlling(EStateType.Character_Immortal, EStateMachineType.PlayerMain, EStateType.Generic_WaitingForAnim);
			StateMachineBase<TEntity, TThis>.AddControlling(EStateType.Character_Immortal, EStateMachineType.PlayerMain, EStateType.PlayerMain_ObjectInteraction);
			StateMachineBase<TEntity, TThis>.AddControlling(EStateType.Character_Immortal, EStateMachineType.PlayerMain, EStateType.PlayerMain_NpcInteraction);
			StateMachineBase<TEntity, TThis>.AddControlling(EStateType.Character_Immortal, EStateMachineType.PlayerMain, EStateType.CombatCharacterMain_Parcour);
			StateMachineBase<TEntity, TThis>.AddControlling(EStateType.Character_Immortal, EStateMachineType.EnemyMain, EStateType.CombatCharacterMain_Parcour);
			StateMachineBase<TEntity, TThis>.AddControlling(EStateType.Character_Immortal, EStateMachineType.InteractionCharacterMain, EStateType.CharacterInteractionMain_Busy);
			StateMachineBase<TEntity, TThis>.AddControlling(EStateType.Character_Alive, EStateMachineType.InteractionCharacterMain, EStateType.CharacterInteractionMain_Busy);
			StateMachineBase<TEntity, TThis>.AddControlling(EStateType.Character_Alive, EStateMachineType.PlayerMain, EStateType.CombatCharacterMain_Roaming);
			StateMachineBase<TEntity, TThis>.AddControlling(EStateType.Character_Alive, EStateMachineType.PlayerMain, EStateType.PlayerMain_BeingChased);
			StateMachineBase<TEntity, TThis>.AddControlling(EStateType.Character_Alive, EStateMachineType.EnemyMain, EStateType.CombatCharacterMain_Roaming);
			StateMachineBase<TEntity, TThis>.AddControlling(EStateType.Character_Alive, EStateMachineType.EnemyMain, EStateType.EnemyMain_Chasing);
		}

		public static void OnEnterDead(IStateMachine<TEntity> _stateMachine, IPayload _payload, IState<TEntity> _prevState, IState<TEntity> _currentState)
		{
			_stateMachine.Entity.KillCharacter(EKillType.None);
			_stateMachine.Entity.CharacterStats.Set_int(StatTypes.EStatType.Character_DeathTurn, LogicMain.CurrentTurn);
			_stateMachine.Entity.InvokeCharacterDied();
		}
	}
}
