using System.Reflection;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;

public class StringParameters : ParameterContainer
{
	private const string DefaultValue = "";

	public string Value { get; set; }

	public StringParameters()
	{
		Value = string.Empty;
		base.ParameterDefinitions = new PropertyInfo[1] { GetType().GetProperty("Value") };
	}
}
