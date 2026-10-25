using System.Reflection;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;

public class FloatParameters : ParameterContainer
{
	private const float DefaultValue = 0f;

	public float Value { get; set; }

	public FloatParameters()
	{
		Value = 0f;
		base.ParameterDefinitions = new PropertyInfo[1] { GetType().GetProperty("Value") };
	}
}
