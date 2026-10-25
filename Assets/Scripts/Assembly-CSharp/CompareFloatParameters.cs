using System.Reflection;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.Helper;

public class CompareFloatParameters : ParameterContainer
{
	private const float DefaultValue = 0f;

	private const EComparisonOperator DefaultOperator = EComparisonOperator.Equal;

	public float Value { get; set; }

	public EComparisonOperator Operator { get; set; }

	public float LastEvaluation { get; private set; }

	public CompareFloatParameters()
	{
		Value = 0f;
		Operator = EComparisonOperator.Equal;
		base.ParameterDefinitions = new PropertyInfo[2]
		{
			GetType().GetProperty("Operator"),
			GetType().GetProperty("Value")
		};
	}

	public bool Evaluate(float v)
	{
		LastEvaluation = v;
		switch (Operator)
		{
		case EComparisonOperator.Equal:
			return v == Value;
		case EComparisonOperator.Less:
			return v < Value;
		case EComparisonOperator.LessOrEqual:
			return v <= Value;
		case EComparisonOperator.Greater:
			return v > Value;
		case EComparisonOperator.GreaterOrEqual:
			return v >= Value;
		default:
			return false;
		}
	}
}
