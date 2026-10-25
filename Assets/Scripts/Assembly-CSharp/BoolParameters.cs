using System.Reflection;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;

public class BoolParameters : ParameterContainer
{
	private const bool DefaultValue = false;

	public bool Value { get; set; }

	public BoolParameters()
	{
		Value = false;
		base.ParameterDefinitions = new PropertyInfo[1] { GetType().GetProperty("Value") };
	}
}
