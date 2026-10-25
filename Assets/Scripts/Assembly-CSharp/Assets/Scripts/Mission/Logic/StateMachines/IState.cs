namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public interface IState
	{
		EStateType StateType { get; }
	}
	public interface IState<out TEntity> : IState where TEntity : IEntity
	{
	}
}
