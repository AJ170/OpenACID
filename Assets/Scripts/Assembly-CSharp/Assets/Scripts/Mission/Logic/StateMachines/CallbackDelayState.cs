using System;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class CallbackDelayState<TEntity> : DelayState<TEntity> where TEntity : IGameEntity
	{
		public class CallbackPayload : Payload, IDelayPayload, ICallbackPayload, IPayload
		{
			public Action<bool> Callback { get; private set; }

			public CallbackPayload(IStateMachine<TEntity> _stateMachine)
				: base(_stateMachine)
			{
				Callback = base.Trigger;
			}
		}

		public CallbackDelayState(EStateType _stateType)
			: this(_stateType, (OnEnterHandler)null, (OnExitHandler)null, (OnUpdateHandler)null)
		{
		}

		public CallbackDelayState(EStateType _stateType, OnEnterHandler _onEnterHandler, OnExitHandler _onExitHandler, OnUpdateHandler _onUpdateHandler)
			: base(_stateType, _onEnterHandler, _onExitHandler, _onUpdateHandler)
		{
		}

		public override IPayload CreatePayload(IStateMachine<TEntity> _stateMachine)
		{
			return new CallbackPayload(_stateMachine);
		}
	}
}
