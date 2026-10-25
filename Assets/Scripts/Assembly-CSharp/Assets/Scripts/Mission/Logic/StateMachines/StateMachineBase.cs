using System;
using System.Runtime.InteropServices;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using SharedBaseLib.Core;
using UbiCore.Messenger;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public abstract class StateMachineBase<T, TThis> : StateMachineBaseBase, IUpdateable, ITurnUpdateable, IStateMachine, IStateMachine<T, TThis>, IStateMachine<T> where T : IEntity, IStateMachinesCarrier where TThis : StateMachineBase<T, TThis>
	{
		private enum ETransitionType
		{
			None = 0,
			Regular = 1,
			Undesired = 2
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct Transition
		{
			public ETransitionType TransitionType { get; set; }
		}

		private static readonly State<T>[] s_States = new State<T>[83];

		private static readonly BBList<State<T>> s_StateList = new BBList<State<T>>();

		private static readonly Transition[,] s_Transitions = new Transition[83, 83];

		private readonly IPayload[] m_Payload = new IPayload[83];

		private readonly StateChangedMessage<TThis> m_StateChangedMessage;

		private readonly T m_entity;

		private EStateType m_CurrentStateType;

		private EStateType m_ScheduledStateType;

		private EStateMachineType m_LastSchedulingStateMachineType = EStateMachineType.Invalid;

		private EStateType m_LastSchedulingStateType;

		public bool IsActive { get; set; }

		public int BucketTurnUpdateIntervalTurns
		{
			get
			{
				return Entity.StateMachines.BucketTurnUpdateIntervalTurns;
			}
		}

		public virtual int TurnUpdateIntervalTurns
		{
			get
			{
				return 0;
			}
		}

		public int TurnUpdateOffsetTurns
		{
			get
			{
				return (int)(uint)Entity.Id;
			}
		}

		public int LastTurnUpdateTurn { get; private set; }

		public int NextTurnUpdateTurn { get; private set; }

		public int TurnsSinceTurnUpdate
		{
			get
			{
				return LogicMain.CurrentTurn - LastTurnUpdateTurn;
			}
		}

		public float TimeSinceTurnUpdate
		{
			get
			{
				return (float)TurnsSinceTurnUpdate * 0.1f;
			}
		}

		public abstract EStateType InitialState { get; }

		public TThis This
		{
			get
			{
				return (TThis)this;
			}
		}

		public IEntity EntityBase
		{
			get
			{
				return m_entity;
			}
		}

		public T Entity
		{
			get
			{
				return m_entity;
			}
		}

		public EStateType CurrentStateType
		{
			get
			{
				return m_CurrentStateType;
			}
		}

		public IPayload CurrentStatePayload
		{
			get
			{
				return m_Payload[(int)m_CurrentStateType];
			}
		}

		public EStateType ScheduledStateType
		{
			get
			{
				return m_ScheduledStateType;
			}
		}

		private State<T> CurrentState
		{
			get
			{
				return s_States[(int)m_CurrentStateType];
			}
		}

		public event StateChangedHandler StateChanged
		{
			add
			{
				this.m_StateChanged = (StateChangedHandler)Delegate.Combine(this.m_StateChanged, value);
			}
			remove
			{
				this.m_StateChanged = (StateChangedHandler)Delegate.Remove(this.m_StateChanged, value);
			}
		}

		private event StateChangedHandler m_StateChanged;

		protected StateMachineBase(T _entity)
		{
			m_entity = _entity;
			for (int i = 0; i < s_StateList.Count; i++)
			{
				m_Payload[(int)s_StateList[i].StateType] = s_StateList[i].CreatePayload(this);
			}
			m_StateChangedMessage = new StateChangedMessage<TThis>((TThis)this);
		}

		public void ScheduleTurnUpdate(bool _resetLastTurnUpdateTurn)
		{
			NextTurnUpdateTurn = LogicMain.CurrentTurn;
			if (_resetLastTurnUpdateTurn)
			{
				LastTurnUpdateTurn = LogicMain.CurrentTurn;
			}
		}

		public bool ChangeState(EStateType _stateType)
		{
			if (!CheckTransition(_stateType))
			{
				return false;
			}
			if (!CheckScheduledState(_stateType))
			{
				return false;
			}
			InternalChangeState(_stateType);
			return true;
		}

		public bool ChangeStateForced(EStateType _stateType)
		{
			if (!CheckTransition(_stateType))
			{
				return false;
			}
			InternalChangeState(_stateType);
			return true;
		}

		public void ClearScheduledStateChange()
		{
			if (m_ScheduledStateType != 0)
			{
				m_ScheduledStateType = EStateType.Invalid;
				StoreSchedulingInfo();
			}
		}

		public bool ScheduleStateChange(EStateType _stateType)
		{
			if (_stateType == m_ScheduledStateType)
			{
				return true;
			}
			if (!CheckTransition(_stateType))
			{
				return false;
			}
			if (!CheckScheduledState(_stateType))
			{
				return false;
			}
			m_ScheduledStateType = _stateType;
			StoreSchedulingInfo();
			return true;
		}

		public bool ScheduleStateChangeForced(EStateType _stateType)
		{
			if (_stateType == m_ScheduledStateType)
			{
				return true;
			}
			if (!CheckTransition(_stateType))
			{
				return false;
			}
			m_ScheduledStateType = _stateType;
			StoreSchedulingInfo();
			return true;
		}

		public TPayload GetPayload<TPayload>(EStateType _type) where TPayload : class, IPayload
		{
			return (TPayload)m_Payload[(int)_type];
		}

		public TPayload TryGetPayload<TPayload>(EStateType _type) where TPayload : class, IPayload
		{
			return m_Payload[(int)_type] as TPayload;
		}

		public bool HasState(EStateType _type)
		{
			return HasStateStatic(_type);
		}

		public static bool HasStateStatic(EStateType _type)
		{
			return s_States[(int)_type] != null;
		}

		public bool HasTransition(EStateType _from, EStateType _to)
		{
			return HasTransitionStatic(_from, _to);
		}

		public bool HasRegularTransition(EStateType _from, EStateType _to)
		{
			return HasRegularTransitionStatic(_from, _to);
		}

		public static bool HasTransitionStatic(EStateType _from, EStateType _to)
		{
			return s_Transitions[(int)_from, (int)_to].TransitionType != ETransitionType.None;
		}

		public static bool HasRegularTransitionStatic(EStateType _from, EStateType _to)
		{
			return s_Transitions[(int)_from, (int)_to].TransitionType == ETransitionType.Regular;
		}

		public bool IsCurrentOrScheduled(EStateType _stateType)
		{
			return CurrentStateType == _stateType || ScheduledStateType == _stateType;
		}

		protected static void InitStateMachineBase()
		{
			StateMachineBaseBase.InitStateMachineBaseBase();
			AddState<State<T>>(EStateType.Invalid, null, null, null);
		}

		public virtual void OnRegistered()
		{
			IsActive = true;
			Reset();
		}

		public virtual void OnUnregistered()
		{
		}

		public void DoTurnUpdate(int _deltaTurns)
		{
			if (m_ScheduledStateType != 0)
			{
				InternalChangeState(m_ScheduledStateType);
			}
			else
			{
				CurrentState.Update(this, CurrentStatePayload);
			}
			LastTurnUpdateTurn = LogicMain.CurrentTurn;
			int turnUpdateIntervalTurns = TurnUpdateIntervalTurns;
			NextTurnUpdateTurn = ((turnUpdateIntervalTurns > 0) ? (LastTurnUpdateTurn + turnUpdateIntervalTurns - LastTurnUpdateTurn % turnUpdateIntervalTurns + TurnUpdateOffsetTurns % turnUpdateIntervalTurns) : 0);
		}

		public virtual void Attach()
		{
		}

		public virtual void Detach()
		{
		}

		protected virtual void InitState(EStateType _initStateType)
		{
			m_CurrentStateType = _initStateType;
		}

		protected static void AddState<TState>(EStateType _stateType, State<T>.OnEnterHandler _onEnterHandler, State<T>.OnExitHandler _onExitHandler, State<T>.OnUpdateHandler _onUpdateHandler) where TState : State<T>
		{
			State<T> state = (State<T>)Activator.CreateInstance(typeof(TState), _stateType, _onEnterHandler, _onExitHandler, _onUpdateHandler);
			s_States[(int)_stateType] = state;
			s_StateList.Add(state);
		}

		protected static void OverrideStateHandlers(EStateType _stateType, State<T>.OnEnterHandler _onEnterHandler, State<T>.OnExitHandler _onExitHandler, State<T>.OnUpdateHandler _onUpdateHandler)
		{
			if (Assets.Scripts.Engine.Helpers.Verify(s_States[(int)_stateType] != null))
			{
				s_States[(int)_stateType].OverrideStateHandlers(_onEnterHandler, _onExitHandler, _onUpdateHandler);
			}
		}

		protected static void AddTransition(EStateType _from, EStateType _to)
		{
			if (Assets.Scripts.Engine.Helpers.Verify(HasStateStatic(_from)) && Assets.Scripts.Engine.Helpers.Verify(HasStateStatic(_to)) && Assets.Scripts.Engine.Helpers.Verify(!HasTransitionStatic(_from, _to)))
			{
				s_Transitions[(int)_from, (int)_to].TransitionType = ETransitionType.Regular;
			}
		}

		protected static void AddUndesiredTransition(EStateType _from, EStateType _to)
		{
			if (Assets.Scripts.Engine.Helpers.Verify(HasStateStatic(_from)) && Assets.Scripts.Engine.Helpers.Verify(HasStateStatic(_to)) && Assets.Scripts.Engine.Helpers.Verify(!HasTransitionStatic(_from, _to)))
			{
				s_Transitions[(int)_from, (int)_to].TransitionType = ETransitionType.Undesired;
			}
		}

		protected static void AddControlling(EStateType _controlledState, EStateMachineType _controllingStateMachineType, EStateType _controllingState)
		{
		}

		private void Reset()
		{
			InitState(InitialState);
			IPayload payload = null;
			for (int i = 0; i < s_StateList.Count; i++)
			{
				payload = m_Payload[(int)s_StateList[i].StateType];
				if (payload != null)
				{
					payload.Reset();
				}
			}
		}

		private void OnStateChanged(IStateMachine _statemachine, EStateType _from, EStateType _to)
		{
			StateChangedHandler stateChanged = this.m_StateChanged;
			if (stateChanged != null)
			{
				stateChanged(_statemachine, _from, _to);
			}
			m_StateChangedMessage.From = _from;
			m_StateChangedMessage.To = _to;
			Messenger.Instance.Broadcast(m_StateChangedMessage);
		}

		private void InternalChangeState(EStateType _stateType)
		{
			m_ScheduledStateType = EStateType.Invalid;
			m_LastSchedulingStateMachineType = EStateMachineType.Invalid;
			m_LastSchedulingStateType = EStateType.Invalid;
			StateMachineBaseBase.IsInStateChange = true;
			CurrentState.Exit(this, CurrentStatePayload, s_States[(int)_stateType]);
			EStateType currentStateType = m_CurrentStateType;
			m_CurrentStateType = _stateType;
			CurrentState.Enter(this, CurrentStatePayload, s_States[(int)currentStateType]);
			if (m_CurrentStateType == _stateType)
			{
				OnStateChanged(this, currentStateType, m_CurrentStateType);
			}
			StateMachineBaseBase.IsInStateChange = false;
		}

		private bool CheckTransition(EStateType _stateType)
		{
			return HasTransition(m_CurrentStateType, _stateType);
		}

		private bool CheckScheduledState(EStateType _stateType)
		{
			return Assets.Scripts.Engine.Helpers.Verify(m_ScheduledStateType == EStateType.Invalid, "Entity {0} ({1}), state machine {2}: Must not change to state {3} because scheduled state is already {4} (was scheduled by state machine {5} in state {6}).", Entity.Id, Entity.Type, this, _stateType, m_ScheduledStateType, m_LastSchedulingStateMachineType, m_LastSchedulingStateType);
		}

		private void StoreSchedulingInfo()
		{
		}
	}
}
