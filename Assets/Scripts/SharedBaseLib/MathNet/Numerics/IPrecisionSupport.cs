namespace MathNet.Numerics
{
	public interface IPrecisionSupport<in T>
	{
		double Norm();

		double NormOfDifference(T otherValue);
	}
}
