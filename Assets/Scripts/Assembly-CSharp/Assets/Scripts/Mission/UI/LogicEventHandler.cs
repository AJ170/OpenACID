namespace Assets.Scripts.Mission.UI
{
	public delegate void LogicEventHandler<T>(T _event) where T : ILogicUiEvent;
}
