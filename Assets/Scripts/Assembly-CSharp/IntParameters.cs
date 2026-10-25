using System.Reflection;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;

public class IntParameters : ParameterContainer
{
	private const int DefaultValue = 0;

	public int Value { get; set; }

	public IntParameters()
	{
		Value = 0;
		base.ParameterDefinitions = new PropertyInfo[1] { GetType().GetProperty("Value") };
	}
}
