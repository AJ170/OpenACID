using System;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class PredicateCallbackDelayState<TEntity> : PredicateDelayState<TEntity> where TEntity : IGameEntity
	{
		public class PredicateCallbackPayload : PredicatePayload, IDelayPayload, ICallbackPayload, IPayload
		{
			public Action<bool> Callback { get; private set; }

			public PredicateCallbackPayload(IStateMachine<TEntity> _stateMachine)
				: base(_stateMachine)
			{
				Callback = base.Trigger;
			}
		}

		public PredicateCallbackDelayState(EStateType _stateType)
			: base(_stateType)
		{
		}

		public PredicateCallbackDelayState(EStateType _stateType, OnEnterHandler _onEnterHandler, OnExitHandler _onExitHandler, OnUpdateHandler _onUpdateHandler)
			: base(_stateType, _onEnterHandler, _onExitHandler, _onUpdateHandler)
		{
		}

		public override IPayload CreatePayload(IStateMachine<TEntity> _stateMachine)
		{
			return new PredicateCallbackPayload(_stateMachine);
		}
	}
}
