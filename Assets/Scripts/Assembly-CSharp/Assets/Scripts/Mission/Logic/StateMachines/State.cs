namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class State<TEntity> : IState, IState<TEntity> where TEntity : IEntity
	{
		public delegate void OnEnterHandler(IStateMachine<TEntity> _stateMachine, IPayload _payload, IState<TEntity> _prevState, IState<TEntity> _currentState);

		public delegate void OnExitHandler(IStateMachine<TEntity> _stateMachine, IPayload _payload, IState<TEntity> _currentState, IState<TEntity> _nextState);

		public delegate void OnUpdateHandler(IStateMachine<TEntity> _stateMachine, IPayload _payload, IState<TEntity> _currentState);

		private OnExitHandler m_OnExit;

		private OnEnterHandler m_OnEnter;

		private OnUpdateHandler m_OnUpdate;

		public EStateType StateType { get; private set; }

		public State(EStateType _stateType, OnEnterHandler _onEnterHandler, OnExitHandler _onExitHandler, OnUpdateHandler _onUpdateHandler)
		{
			StateType = _stateType;
			m_OnEnter = _onEnterHandler;
			m_OnExit = _onExitHandler;
			m_OnUpdate = _onUpdateHandler;
		}

		public void OverrideStateHandlers(OnEnterHandler _onEnterHandler, OnExitHandler _onExitHandler, OnUpdateHandler _onUpdateHandler)
		{
			if (_onEnterHandler != null)
			{
				m_OnEnter = _onEnterHandler;
			}
			if (_onExitHandler != null)
			{
				m_OnExit = _onExitHandler;
			}
			if (_onUpdateHandler != null)
			{
				m_OnUpdate = _onUpdateHandler;
			}
		}

		public void Enter(IStateMachine<TEntity> _stateMachine, IPayload _payload, IState<TEntity> _prevState)
		{
			if (m_OnEnter != null)
			{
				m_OnEnter(_stateMachine, _payload, _prevState, this);
			}
		}

		public void Exit(IStateMachine<TEntity> _stateMachine, IPayload _payload, IState<TEntity> _nextState)
		{
			if (m_OnExit != null)
			{
				m_OnExit(_stateMachine, _payload, this, _nextState);
			}
		}

		public void Update(IStateMachine<TEntity> _stateMachine, IPayload _payload)
		{
			if (m_OnUpdate != null)
			{
				m_OnUpdate(_stateMachine, _payload, this);
			}
		}

		public virtual IPayload CreatePayload(IStateMachine<TEntity> _stateMachine)
		{
			return null;
		}
	}
}
