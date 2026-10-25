namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public delegate void StateChangedHandler(IStateMachine _stateMachine, EStateType _from, EStateType _to);
}
