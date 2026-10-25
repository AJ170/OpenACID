using System;
using Assets.Scripts.Engine;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public static class DelayState
	{
		public static void EnterCallbackDelayState(this IStateMachine _stateMachine, EStateType _type, EStateType _nextStateType, EStateType _cancelationStateType, AnimationTaskType _taskType = AnimationTaskType.Unknown, AnimationTaskManager.ELayer _layer = AnimationTaskManager.ELayer.Base, bool _immediateStateChange = false)
		{
			_stateMachine.HandleCurrentState(_type);
			_stateMachine.ResetDelayState(_type);
			_stateMachine.SetDelayedNextState(_type, _nextStateType);
			_stateMachine.SetDelayedCancelationState(_type, _cancelationStateType);
			_stateMachine.SetImmediateStateChange(_type, _immediateStateChange);
			if (_stateMachine.CurrentStateType != _type)
			{
				_stateMachine.ScheduleStateChangeForced(_type);
			}
			Action finishedAction = _stateMachine.Cast<IStateMachineAnimListening>().StartAnimationListening(_taskType, AnimatorEventType.TriggerDelayAction, _layer, _stateMachine.GetDelayedTriggerAction(_type));
			_stateMachine.SetFinishedAction(_type, finishedAction);
		}

		public static void EnterCallbackDelayState(this IStateMachine _stateMachine, EStateType _type, EStateType _nextStateType, EStateType _cancelationStateType, AnimationPlotSequence _sequence, bool _immediateStateChange = false)
		{
			_stateMachine.HandleCurrentState(_type);
			_stateMachine.ResetDelayState(_type);
			_stateMachine.SetDelayedNextState(_type, _nextStateType);
			_stateMachine.SetDelayedCancelationState(_type, _cancelationStateType);
			_stateMachine.SetImmediateStateChange(_type, _immediateStateChange);
			if (_stateMachine.CurrentStateType != _type)
			{
				_stateMachine.ScheduleStateChangeForced(_type);
			}
			_sequence.Start(_stateMachine.GetDelayedTriggerAction(_type));
		}

		public static void EnterPredicateDelayState(this IStateMachine _stateMachine, EStateType _type, EStateType _nextStateType, EStateType _cancelationStateType, Func<bool> _predicate, bool _immediateStateChange = false)
		{
			_stateMachine.HandleCurrentState(_type);
			_stateMachine.ResetDelayState(_type);
			_stateMachine.SetDelayedNextState(_type, _nextStateType);
			_stateMachine.SetDelayedCancelationState(_type, _cancelationStateType);
			_stateMachine.SetDelayedTriggerPredicate(_type, _predicate);
			_stateMachine.SetImmediateStateChange(_type, _immediateStateChange);
			if (_stateMachine.CurrentStateType != _type)
			{
				_stateMachine.ScheduleStateChangeForced(_type);
			}
		}

		public static void EnterCountdownDelayState(this IStateMachine _stateMachine, EStateType _type, EStateType _nextStateType, EStateType _cancelationStateType, int _countdownTurns, bool _immediateStateChange = false)
		{
			_stateMachine.HandleCurrentState(_type);
			_stateMachine.ResetDelayState(_type);
			_stateMachine.SetDelayedNextState(_type, _nextStateType);
			_stateMachine.SetDelayedCancelationState(_type, _cancelationStateType);
			_stateMachine.SetDelayedTriggerCountdown(_type, _countdownTurns);
			_stateMachine.SetImmediateStateChange(_type, _immediateStateChange);
			if (_stateMachine.CurrentStateType != _type)
			{
				_stateMachine.ScheduleStateChangeForced(_type);
			}
		}

		public static void AbortDelayState(this IStateMachine _stateMachine, EStateType _stateType)
		{
			_stateMachine.GetPayload<IDelayPayload>(_stateType).Cancel();
		}

		public static void AbortCurrentDelayState(this IStateMachine _stateMachine)
		{
			_stateMachine.AbortDelayState(_stateMachine.CurrentStateType);
		}

		public static void ResetDelayState(this IStateMachine _stateMachine, EStateType _type)
		{
			_stateMachine.GetPayload<IDelayPayload>(_type).ResetDelayState();
		}

		public static void SetDelayedTriggerAction(this IStateMachine _stateMachine, EStateType _type, TriggerAction _triggerAction)
		{
			_stateMachine.GetPayload<IDelayPayload>(_type).TriggerAction = _triggerAction;
		}

		public static void SetDelayedNextState(this IStateMachine _stateMachine, EStateType _type, EStateType _nextStateType)
		{
			_stateMachine.GetPayload<IDelayPayload>(_type).NextState = _nextStateType;
		}

		public static EStateType GetDelayedNextState(this IStateMachine _stateMachine, EStateType _type)
		{
			return _stateMachine.GetPayload<IDelayPayload>(_type).NextState;
		}

		public static void SetDelayedCancelationState(this IStateMachine _stateMachine, EStateType _type, EStateType _cancelationStateType)
		{
			_stateMachine.GetPayload<IDelayPayload>(_type).CancelationState = _cancelationStateType;
		}

		public static EStateType GetDelayedCancelationState(this IStateMachine _stateMachine, EStateType _type)
		{
			return _stateMachine.GetPayload<IDelayPayload>(_type).CancelationState;
		}

		public static void SetImmediateStateChange(this IStateMachine _stateMachine, EStateType _type, bool _immediateStateChange)
		{
			_stateMachine.GetPayload<IDelayPayload>(_type).ImmediateStateChange = _immediateStateChange;
		}

		public static Action<bool> GetDelayedTriggerAction(this IStateMachine _stateMachine, EStateType _type)
		{
			return _stateMachine.GetPayload<ICallbackPayload>(_type).Callback;
		}

		public static void SetFinishedAction(this IStateMachine _stateMachine, EStateType _type, Action _finishedAction)
		{
			_stateMachine.GetPayload<ICallbackPayload>(_type).FinishedAction = _finishedAction;
		}

		public static void SetDelayedTriggerPredicate(this IStateMachine _stateMachine, EStateType _type, Func<bool> _predicate)
		{
			_stateMachine.GetPayload<IPredicatePayload>(_type).Predicate = _predicate;
		}

		public static void SetDelayedTriggerCountdown(this IStateMachine _stateMachine, EStateType _type, int _turnCountDown)
		{
			int triggerTurn = LogicMain.CurrentTurn + _turnCountDown;
			_stateMachine.GetPayload<IPredicatePayload>(_type).Predicate = () => LogicMain.CurrentTurn >= triggerTurn;
		}

		public static bool IsDelayedState(this IStateMachine _stateMachine, EStateType _type)
		{
			return _stateMachine.TryGetPayload<IDelayPayload>(_type) != null;
		}

		public static void HandleCurrentState(this IStateMachine _stateMachine, EStateType _intendedType)
		{
			if (!Assets.Scripts.Engine.Helpers.Verify(!_stateMachine.IsDelayedState(_stateMachine.CurrentStateType) || !_stateMachine.GetPayload<IDelayPayload>(_stateMachine.CurrentStateType).IsActive, "Entity {0}, SM {1} is trying to enter delay state {2} although it is already in delay state {3}.", _stateMachine.EntityBase.Id, _stateMachine.Type, _intendedType, _stateMachine.CurrentStateType))
			{
				EStateType currentStateType = _stateMachine.CurrentStateType;
				_stateMachine.AbortCurrentDelayState();
				_stateMachine.ResetDelayState(currentStateType);
			}
			if (!Assets.Scripts.Engine.Helpers.Verify(_stateMachine.ScheduledStateType == EStateType.Invalid || !_stateMachine.IsDelayedState(_stateMachine.ScheduledStateType), "Entity {0}, SM {1} is trying to enter delay state {2} although it is already scheduled for delay state {3}.", _stateMachine.EntityBase.Id, _stateMachine.Type, _intendedType, _stateMachine.ScheduledStateType))
			{
				_stateMachine.ResetDelayState(_stateMachine.ScheduledStateType);
			}
		}
	}
	public abstract class DelayState<TEntity> : State<TEntity> where TEntity : IEntity
	{
		public abstract class Payload : IDelayPayload, IPayload
		{
			protected readonly IStateMachine<TEntity> m_stateMachine;

			private bool m_hasBeenCanceled;

			public EStateType NextState { get; set; }

			public EStateType CancelationState { get; set; }

			public bool ImmediateStateChange { get; set; }

			public TriggerAction TriggerAction { get; set; }

			public Action FinishedAction { get; set; }

			public bool IsActive
			{
				get
				{
					return NextState != 0 || TriggerAction != null;
				}
			}

			public bool HasBeenCanceled
			{
				get
				{
					return m_hasBeenCanceled;
				}
			}

			protected Payload(IStateMachine<TEntity> _stateMachine)
			{
				m_stateMachine = _stateMachine;
			}

			public virtual void Reset()
			{
				ResetDelayState();
			}

			public virtual void ResetDelayState()
			{
				NextState = EStateType.Invalid;
				CancelationState = EStateType.Invalid;
				ImmediateStateChange = false;
				TriggerAction = null;
				if (FinishedAction != null)
				{
					FinishedAction();
				}
				FinishedAction = null;
				m_hasBeenCanceled = false;
			}

			public void Trigger(bool _canceled)
			{
				EStateType eStateType = ((!_canceled) ? NextState : CancelationState);
				CancelationState = EStateType.Invalid;
				NextState = EStateType.Invalid;
				m_hasBeenCanceled = _canceled;
				if (TriggerAction != null)
				{
					TriggerAction(m_stateMachine, _canceled);
				}
				TriggerAction = null;
				if (_canceled && FinishedAction != null)
				{
					FinishedAction();
				}
				FinishedAction = null;
				if (eStateType == EStateType.Invalid)
				{
					return;
				}
				if (m_stateMachine.CurrentStateType == eStateType)
				{
					m_stateMachine.ClearScheduledStateChange();
				}
				else if (m_stateMachine.ScheduledStateType != eStateType)
				{
					if (ImmediateStateChange)
					{
						m_stateMachine.ChangeStateForced(eStateType);
					}
					else
					{
						m_stateMachine.ScheduleStateChangeForced(eStateType);
					}
				}
			}

			public virtual void Cancel()
			{
				Trigger(true);
			}
		}

		protected DelayState(EStateType _stateType)
			: base(_stateType, (OnEnterHandler)null, (OnExitHandler)null, (OnUpdateHandler)null)
		{
		}

		protected DelayState(EStateType _stateType, OnEnterHandler _onEnterHandler, OnExitHandler _onExitHandler, OnUpdateHandler _onUpdateHandler)
			: base(_stateType, _onEnterHandler, _onExitHandler, _onUpdateHandler)
		{
		}
	}
}
