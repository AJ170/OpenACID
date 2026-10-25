using System.Reflection;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;

public class RandomFloatParameters : ParameterContainer
{
	private const float DefaultMin = 0f;

	private const float DefaultMax = 1f;

	public float Value
	{
		get
		{
			return LogicMain.Random.Rand(Min, Max);
		}
	}

	public float Min { get; set; }

	public float Max { get; set; }

	public RandomFloatParameters()
	{
		Min = 0f;
		Max = 1f;
		base.ParameterDefinitions = new PropertyInfo[2]
		{
			GetType().GetProperty("Min"),
			GetType().GetProperty("Max")
		};
	}
}
