using System;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class PredicateDelayState<TEntity> : DelayState<TEntity> where TEntity : IEntity
	{
		public class PredicatePayload : Payload, IDelayPayload, IPredicatePayload, IPayload
		{
			public Func<bool> Predicate { get; set; }

			public PredicatePayload(IStateMachine<TEntity> _stateMachine)
				: base(_stateMachine)
			{
			}

			public override void ResetDelayState()
			{
				base.ResetDelayState();
				Predicate = null;
			}
		}

		public PredicateDelayState(EStateType _stateType)
			: this(_stateType, (OnEnterHandler)null, (OnExitHandler)null, (OnUpdateHandler)null)
		{
		}

		public PredicateDelayState(EStateType _stateType, OnEnterHandler _onEnterHandler, OnExitHandler _onExitHandler, OnUpdateHandler _onUpdateHandler)
			: base(_stateType, _onEnterHandler, _onExitHandler, (OnUpdateHandler)delegate(IStateMachine<TEntity> _stateMachine, IPayload _payload, IState<TEntity> _currentState)
			{
				OnUpdateHandler(_onUpdateHandler, _stateMachine, _payload, _currentState);
			})
		{
		}

		public override IPayload CreatePayload(IStateMachine<TEntity> _stateMachine)
		{
			return new PredicatePayload(_stateMachine);
		}

		private static void OnUpdateHandler(OnUpdateHandler _originalHandler, IStateMachine<TEntity> _stateMachine, IPayload _payload, IState<TEntity> _currentState)
		{
			if (_originalHandler != null)
			{
				_originalHandler(_stateMachine, _payload, _currentState);
			}
			PredicatePayload predicatePayload = (PredicatePayload)_payload;
			if (predicatePayload.Predicate != null && predicatePayload.Predicate())
			{
				predicatePayload.Trigger(false);
			}
		}
	}
}
