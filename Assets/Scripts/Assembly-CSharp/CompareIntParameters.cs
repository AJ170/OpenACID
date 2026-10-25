using System.Reflection;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.Helper;

public class CompareIntParameters : ParameterContainer
{
	private const int DefaultValue = 0;

	private const EComparisonOperator DefaultOperator = EComparisonOperator.Equal;

	public int Value { get; set; }

	public EComparisonOperator Operator { get; set; }

	public int LastEvaluation { get; private set; }

	public CompareIntParameters()
	{
		Value = 0;
		Operator = EComparisonOperator.Equal;
		base.ParameterDefinitions = new PropertyInfo[2]
		{
			GetType().GetProperty("Operator"),
			GetType().GetProperty("Value")
		};
	}

	public bool Evaluate(int v)
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
