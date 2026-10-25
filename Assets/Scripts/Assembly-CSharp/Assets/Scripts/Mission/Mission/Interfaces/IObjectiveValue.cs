namespace Assets.Scripts.Mission.Mission.Interfaces
{
	public interface IObjectiveValue
	{
		int Value { get; }

		event ValueChangedDelegate ValueChanged;
	}
}
