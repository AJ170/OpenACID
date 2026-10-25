namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public interface IStateMachine : IUpdateable, ITurnUpdateable
	{
		EStateMachineType Type { get; }

		EStateType CurrentStateType { get; }

		EStateType ScheduledStateType { get; }

		EStateType InitialState { get; }

		IEntity EntityBase { get; }

		IPayload CurrentStatePayload { get; }

		event StateChangedHandler StateChanged;

		bool HasState(EStateType _type);

		bool HasTransition(EStateType _from, EStateType _to);

		bool HasRegularTransition(EStateType _from, EStateType _to);

		bool IsCurrentOrScheduled(EStateType _stateType);

		void ClearScheduledStateChange();

		bool ScheduleStateChange(EStateType _stateType);

		bool ScheduleStateChangeForced(EStateType _stateType);

		bool ChangeState(EStateType _stateType);

		bool ChangeStateForced(EStateType _stateType);

		TPayload GetPayload<TPayload>(EStateType _type) where TPayload : class, IPayload;

		TPayload TryGetPayload<TPayload>(EStateType _type) where TPayload : class, IPayload;

		void Attach();

		void Detach();

		void OnRegistered();

		void OnUnregistered();
	}
	public interface IStateMachine<out TEntity> : IUpdateable, ITurnUpdateable, IStateMachine where TEntity : IEntity
	{
		TEntity Entity { get; }
	}
	public interface IStateMachine<out TEntity, out THis> : IUpdateable, ITurnUpdateable, IStateMachine, IStateMachine<TEntity> where TEntity : IEntity
	{
		THis This { get; }
	}
}
