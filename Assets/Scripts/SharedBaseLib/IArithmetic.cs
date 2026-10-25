public interface IArithmetic<TField>
{
	TField Zero { get; }

	TField One { get; }

	TField MinValue { get; }

	TField MaxValue { get; }

	bool Equal(TField a, TField b);

	bool AlmostEqual(TField a, TField b);

	TField EpsilonOf(TField a);

	TField Add(TField a, TField b);

	TField Subtract(TField a, TField b);

	TField Multiply(TField a, TField b);

	TField Divide(TField a, TField b);

	TField Sqrt(TField a);

	TField Abs(TField a);
}
