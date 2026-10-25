using MathNet.Numerics;
using UnityEngine;

public class FloatArithmetic : Arithmetic<float>
{
	public override float Zero
	{
		get
		{
			return 0f;
		}
	}

	public override float One
	{
		get
		{
			return 1f;
		}
	}

	public override float MinValue
	{
		get
		{
			return float.MinValue;
		}
	}

	public override float MaxValue
	{
		get
		{
			return float.MaxValue;
		}
	}

	public override bool Equal(float a, float b)
	{
		return object.Equals(a, b);
	}

	public override bool AlmostEqual(float a, float b)
	{
		return a.AlmostEqual(b);
	}

	public override float EpsilonOf(float a)
	{
		return a.EpsilonOf();
	}

	public override float Add(float a, float b)
	{
		return a + b;
	}

	public override float Subtract(float a, float b)
	{
		return a - b;
	}

	public override float Multiply(float a, float b)
	{
		return a * b;
	}

	public override float Divide(float a, float b)
	{
		return a / b;
	}

	public override float Sqrt(float a)
	{
		return Mathf.Sqrt(a);
	}

	public override float Abs(float a)
	{
		return Mathf.Abs(a);
	}
}
