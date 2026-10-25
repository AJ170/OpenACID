using System;

public abstract class Arithmetic<TField> : IArithmetic<TField>
{
	public static Arithmetic<TField> Default
	{
		get
		{
			if (typeof(TField) == typeof(float))
			{
				return (Arithmetic<TField>)(object)new FloatArithmetic();
			}
			throw new InvalidOperationException();
		}
	}

	public abstract TField Zero { get; }

	public abstract TField One { get; }

	public abstract TField MinValue { get; }

	public abstract TField MaxValue { get; }

	public abstract bool Equal(TField a, TField b);

	public abstract bool AlmostEqual(TField a, TField b);

	public abstract TField EpsilonOf(TField a);

	public abstract TField Add(TField a, TField b);

	public abstract TField Subtract(TField a, TField b);

	public abstract TField Multiply(TField a, TField b);

	public abstract TField Divide(TField a, TField b);

	public abstract TField Sqrt(TField a);

	public abstract TField Abs(TField a);
}
