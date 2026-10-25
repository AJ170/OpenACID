namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components
{
	public interface IHasDelegateParameterized : IHasDelegate
	{
		IParameterContainer ParameterContainer { get; set; }
	}
}
