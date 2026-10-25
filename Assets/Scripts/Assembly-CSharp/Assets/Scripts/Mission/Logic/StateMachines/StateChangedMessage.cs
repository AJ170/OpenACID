using UbiCore.Messenger;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class StateChangedMessage<TStateMachine> : Message where TStateMachine : IStateMachine
	{
		public TStateMachine StateMachine { get; private set; }

		public EStateType From { get; set; }

		public EStateType To { get; set; }

		public StateChangedMessage(TStateMachine _stateMachine)
		{
			StateMachine = _stateMachine;
		}
	}
}
