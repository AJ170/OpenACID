using System;
using System.Collections.Generic;

namespace MathNet.Numerics
{
	public static class Precision
	{
		private const int BinaryBaseNumber = 2;

		private const int DoublePrecision = 53;

		private const int SinglePrecision = 24;

		private static readonly double _doubleMachinePrecision;

		private static readonly double _singleMachinePrecision;

		private static readonly int _numberOfDecimalPlacesForDoubles;

		private static readonly int _numberOfDecimalPlacesForFloats;

		private static readonly double _defaultDoubleRelativeAccuracy;

		private static readonly float _defaultSingleRelativeAccuracy;

		public static double DoubleMachinePrecision
		{
			get
			{
				return _doubleMachinePrecision;
			}
		}

		public static double SingleMachinePrecision
		{
			get
			{
				return _singleMachinePrecision;
			}
		}

		public static int NumberOfDecimalPlacesForFloats
		{
			get
			{
				return _numberOfDecimalPlacesForFloats;
			}
		}

		public static int NumberOfDecimalPlacesForDoubles
		{
			get
			{
				return _numberOfDecimalPlacesForDoubles;
			}
		}

		static Precision()
		{
			_doubleMachinePrecision = Math.Pow(2.0, -53.0);
			_singleMachinePrecision = Math.Pow(2.0, -24.0);
			_defaultDoubleRelativeAccuracy = _doubleMachinePrecision * 10.0;
			_defaultSingleRelativeAccuracy = (float)(_singleMachinePrecision * 10.0);
			_numberOfDecimalPlacesForFloats = (int)Math.Ceiling(Math.Abs(Math.Log10(_singleMachinePrecision)));
			_numberOfDecimalPlacesForDoubles = (int)Math.Ceiling(Math.Abs(Math.Log10(_doubleMachinePrecision)));
		}

		public static int Magnitude(this double value)
		{
			if (value.Equals(0.0))
			{
				return 0;
			}
			double num = Math.Log10(Math.Abs(value));
			if (num < 0.0)
			{
				return (int)Math.Truncate(num - 1.0);
			}
			return (int)Math.Truncate(num);
		}

		public static int Magnitude(this float value)
		{
			if (value.Equals(0f))
			{
				return 0;
			}
			float num = Convert.ToSingle(Math.Log10(Math.Abs(value)));
			if (num < 0f)
			{
				return (int)Math.Truncate(num - 1f);
			}
			return (int)Math.Truncate(num);
		}

		public static double GetMagnitudeScaledValue(this double value)
		{
			if (value.Equals(0.0))
			{
				return value;
			}
			int num = value.Magnitude();
			return value * Math.Pow(10.0, -num);
		}

		private static long GetLongFromDouble(double value)
		{
			return BitConverter.DoubleToInt64Bits(value);
		}

		private static long GetDirectionalLongFromDouble(double value)
		{
			long longFromDouble = GetLongFromDouble(value);
			if (longFromDouble < 0)
			{
				return long.MinValue - longFromDouble;
			}
			return longFromDouble;
		}

		private static int GetDirectionalIntFromFloat(float value)
		{
			int num = FloatToInt32Bits(value);
			if (num < 0)
			{
				return int.MinValue - num;
			}
			return num;
		}

		public static double Increment(this double value)
		{
			return value.Increment(1);
		}

		public static double Increment(this double value, int count)
		{
			if (double.IsInfinity(value) || double.IsNaN(value) || count == 0)
			{
				return value;
			}
			if (count < 0)
			{
				return value.Decrement(-count);
			}
			long longFromDouble = GetLongFromDouble(value);
			longFromDouble = ((longFromDouble >= 0) ? (longFromDouble + count) : (longFromDouble - count));
			if (longFromDouble == long.MinValue)
			{
				return 0.0;
			}
			return BitConverter.Int64BitsToDouble(longFromDouble);
		}

		public static double Decrement(this double value)
		{
			return value.Decrement(1);
		}

		public static double Decrement(this double value, int count)
		{
			if (double.IsInfinity(value) || double.IsNaN(value) || count == 0)
			{
				return value;
			}
			if (count < 0)
			{
				return value.Decrement(-count);
			}
			long num = GetLongFromDouble(value);
			if (num == 0L)
			{
				num = long.MinValue;
			}
			num = ((num >= 0) ? (num - count) : (num + count));
			return BitConverter.Int64BitsToDouble(num);
		}

		public static double CoerceZero(this double a, int maxNumbersBetween)
		{
			return a.CoerceZero((long)maxNumbersBetween);
		}

		public static double CoerceZero(this double a, long maxNumbersBetween)
		{
			if (maxNumbersBetween < 0)
			{
				throw new ArgumentOutOfRangeException("maxNumbersBetween");
			}
			if (double.IsInfinity(a) || double.IsNaN(a))
			{
				return a;
			}
			if (0.0.NumbersBetween(a) <= (ulong)maxNumbersBetween)
			{
				return 0.0;
			}
			return a;
		}

		public static double CoerceZero(this double a, double maximumAbsoluteError)
		{
			if (maximumAbsoluteError < 0.0)
			{
				throw new ArgumentOutOfRangeException("maximumAbsoluteError");
			}
			if (double.IsInfinity(a) || double.IsNaN(a))
			{
				return a;
			}
			if (Math.Abs(a) < maximumAbsoluteError)
			{
				return 0.0;
			}
			return a;
		}

		public static double CoerceZero(this double a)
		{
			return a.CoerceZero(_doubleMachinePrecision);
		}

		public static void RangeOfMatchingFloatingPointNumbers(this double value, long maxNumbersBetween, out double bottomRangeEnd, out double topRangeEnd)
		{
			if (maxNumbersBetween < 1)
			{
				throw new ArgumentOutOfRangeException("maxNumbersBetween");
			}
			if (double.IsInfinity(value))
			{
				topRangeEnd = value;
				bottomRangeEnd = value;
				return;
			}
			if (double.IsNaN(value))
			{
				topRangeEnd = double.NaN;
				bottomRangeEnd = double.NaN;
				return;
			}
			long longFromDouble = GetLongFromDouble(value);
			if (longFromDouble < 0)
			{
				topRangeEnd = ((Math.Abs(long.MinValue - longFromDouble) < maxNumbersBetween) ? BitConverter.Int64BitsToDouble(maxNumbersBetween + (long.MinValue - longFromDouble)) : BitConverter.Int64BitsToDouble(longFromDouble - maxNumbersBetween));
				bottomRangeEnd = ((Math.Abs(longFromDouble) < maxNumbersBetween) ? double.MinValue : BitConverter.Int64BitsToDouble(longFromDouble + maxNumbersBetween));
			}
			else
			{
				topRangeEnd = ((long.MaxValue - longFromDouble < maxNumbersBetween) ? double.MaxValue : BitConverter.Int64BitsToDouble(longFromDouble + maxNumbersBetween));
				bottomRangeEnd = ((longFromDouble > maxNumbersBetween) ? BitConverter.Int64BitsToDouble(longFromDouble - maxNumbersBetween) : BitConverter.Int64BitsToDouble(long.MinValue + (maxNumbersBetween - longFromDouble)));
			}
		}

		public static double MaximumMatchingFloatingPointNumber(this double value, long maxNumbersBetween)
		{
			double bottomRangeEnd;
			double topRangeEnd;
			value.RangeOfMatchingFloatingPointNumbers(maxNumbersBetween, out bottomRangeEnd, out topRangeEnd);
			return topRangeEnd;
		}

		public static double MinimumMatchingFloatingPointNumber(this double value, long maxNumbersBetween)
		{
			double bottomRangeEnd;
			double topRangeEnd;
			value.RangeOfMatchingFloatingPointNumbers(maxNumbersBetween, out bottomRangeEnd, out topRangeEnd);
			return bottomRangeEnd;
		}

		public static void RangeOfMatchingNumbers(this double value, double relativeDifference, out long bottomRangeEnd, out long topRangeEnd)
		{
			if (relativeDifference < 0.0)
			{
				throw new ArgumentOutOfRangeException("relativeDifference");
			}
			if (double.IsInfinity(value))
			{
				throw new ArgumentOutOfRangeException("value");
			}
			if (double.IsNaN(value))
			{
				throw new ArgumentOutOfRangeException("value");
			}
			if (value.Equals(0.0))
			{
				topRangeEnd = GetLongFromDouble(relativeDifference);
				bottomRangeEnd = topRangeEnd;
				return;
			}
			long directionalLongFromDouble = GetDirectionalLongFromDouble(value + relativeDifference * Math.Abs(value));
			long directionalLongFromDouble2 = GetDirectionalLongFromDouble(value - relativeDifference * Math.Abs(value));
			long directionalLongFromDouble3 = GetDirectionalLongFromDouble(value);
			topRangeEnd = Math.Abs(directionalLongFromDouble - directionalLongFromDouble3);
			bottomRangeEnd = Math.Abs(directionalLongFromDouble3 - directionalLongFromDouble2);
		}

		[CLSCompliant(false)]
		public static ulong NumbersBetween(this double a, double b)
		{
			if (double.IsNaN(a) || double.IsInfinity(a))
			{
				throw new ArgumentOutOfRangeException("a");
			}
			if (double.IsNaN(b) || double.IsInfinity(b))
			{
				throw new ArgumentOutOfRangeException("b");
			}
			long directionalLongFromDouble = GetDirectionalLongFromDouble(a);
			long directionalLongFromDouble2 = GetDirectionalLongFromDouble(b);
			if (!(a >= b))
			{
				return (ulong)(directionalLongFromDouble2 - directionalLongFromDouble);
			}
			return (ulong)(directionalLongFromDouble - directionalLongFromDouble2);
		}

		public static bool AlmostEqual(this double a, double b)
		{
			double diff = a - b;
			return a.AlmostEqualWithError(b, diff, _defaultDoubleRelativeAccuracy);
		}

		public static bool AlmostEqual(this float a, float b)
		{
			double diff = a - b;
			return AlmostEqualWithError(a, b, diff, _defaultSingleRelativeAccuracy);
		}

		public static bool AlmostEqual<T>(this T a, T b) where T : IPrecisionSupport<T>
		{
			double diff = a.NormOfDifference(b);
			return a.Norm().AlmostEqualWithError(b.Norm(), diff, _defaultDoubleRelativeAccuracy);
		}

		public static bool AlmostEqualWithError(this float a, float b, double maximumError)
		{
			return AlmostEqualWithError(a, b, a - b, maximumError);
		}

		public static bool AlmostEqualWithError(this double a, double b, double maximumError)
		{
			return a.AlmostEqualWithError(b, a - b, maximumError);
		}

		public static bool AlmostEqualListWithError(this IList<double> a, IList<double> b, double maximumError)
		{
			if (a == null && b == null)
			{
				return true;
			}
			if (a == null || b == null || a.Count != b.Count)
			{
				return false;
			}
			for (int i = 0; i < a.Count; i++)
			{
				if (!a[i].AlmostEqualWithError(b[i], a[i] - b[i], maximumError))
				{
					return false;
				}
			}
			return true;
		}

		public static bool AlmostEqualWithError<T>(this T a, T b, double maximumError) where T : IPrecisionSupport<T>
		{
			return a.Norm().AlmostEqualWithError(b.Norm(), a.NormOfDifference(b), maximumError);
		}

		public static bool AlmostEqualListWithError<T>(this IList<T> a, IList<T> b, double maximumError) where T : IPrecisionSupport<T>
		{
			if (a == null && b == null)
			{
				return true;
			}
			if (a == null || b == null || a.Count != b.Count)
			{
				return false;
			}
			for (int i = 0; i < a.Count; i++)
			{
				if (!a[i].Norm().AlmostEqualWithError(b[i].Norm(), a[i].NormOfDifference(b[i]), maximumError))
				{
					return false;
				}
			}
			return true;
		}

		public static bool AlmostEqualWithError(this double a, double b, double diff, double maximumError)
		{
			if (double.IsInfinity(a) || double.IsInfinity(b))
			{
				return a == b;
			}
			if (double.IsNaN(a) || double.IsNaN(b))
			{
				return false;
			}
			if (Math.Abs(a) < _doubleMachinePrecision || Math.Abs(b) < _doubleMachinePrecision)
			{
				return a.AlmostEqualWithAbsoluteError(b, diff, maximumError);
			}
			return a.AlmostEqualWithRelativeError(b, diff, maximumError);
		}

		public static bool AlmostEqualWithAbsoluteError(this double a, double b, double diff, double maximumAbsoluteError)
		{
			if (double.IsInfinity(a) || double.IsInfinity(b))
			{
				return a == b;
			}
			if (double.IsNaN(a) || double.IsNaN(b))
			{
				return false;
			}
			return Math.Abs(diff) < maximumAbsoluteError;
		}

		public static bool AlmostEqualWithRelativeError(this double a, double b, double diff, double maximumRelativeError)
		{
			if (double.IsInfinity(a) || double.IsInfinity(b))
			{
				return a == b;
			}
			if (double.IsNaN(a) || double.IsNaN(b))
			{
				return false;
			}
			if ((a == 0.0 && Math.Abs(b) < maximumRelativeError) || (b == 0.0 && Math.Abs(a) < maximumRelativeError))
			{
				return true;
			}
			return Math.Abs(diff) < maximumRelativeError * Math.Max(Math.Abs(a), Math.Abs(b));
		}

		public static bool AlmostEqualInDecimalPlaces(this double a, double b, int decimalPlaces)
		{
			if (decimalPlaces <= 0)
			{
				throw new ArgumentOutOfRangeException("decimalPlaces");
			}
			if (double.IsNaN(a) || double.IsNaN(b))
			{
				return false;
			}
			if (double.IsInfinity(a) || double.IsInfinity(b))
			{
				return a == b;
			}
			if (Math.Abs(a) < _doubleMachinePrecision || Math.Abs(b) < _doubleMachinePrecision)
			{
				return a.AlmostEqualWithAbsoluteDecimalPlaces(b, decimalPlaces);
			}
			if (a.Equals(b))
			{
				return true;
			}
			return a.AlmostEqualWithRelativeDecimalPlaces(b, decimalPlaces);
		}

		public static bool AlmostEqualInDecimalPlaces(this float a, float b, int decimalPlaces)
		{
			if (decimalPlaces <= 0)
			{
				throw new ArgumentOutOfRangeException("decimalPlaces");
			}
			if (float.IsNaN(a) || float.IsNaN(b))
			{
				return false;
			}
			if (float.IsInfinity(a) || float.IsInfinity(b))
			{
				return a == b;
			}
			if ((double)Math.Abs(a) < _singleMachinePrecision || (double)Math.Abs(b) < _singleMachinePrecision)
			{
				return a.AlmostEqualWithAbsoluteDecimalPlaces(b, decimalPlaces);
			}
			if (a.Equals(b))
			{
				return true;
			}
			return a.AlmostEqualWithRelativeDecimalPlaces(b, decimalPlaces);
		}

		private static bool AlmostEqualWithRelativeDecimalPlaces(this double a, double b, int decimalPlaces)
		{
			int num = a.Magnitude();
			int num2 = b.Magnitude();
			if (Math.Max(num, num2) > Math.Min(num, num2) + 1)
			{
				return false;
			}
			double num3 = Math.Pow(10.0, -(decimalPlaces - 1)) / 2.0;
			if (!(a > b))
			{
				return b * Math.Pow(10.0, -num2) - num3 < a * Math.Pow(10.0, -num2);
			}
			return a * Math.Pow(10.0, -num) - num3 < b * Math.Pow(10.0, -num);
		}

		private static bool AlmostEqualWithRelativeDecimalPlaces(this float a, float b, int decimalPlaces)
		{
			int num = a.Magnitude();
			int num2 = b.Magnitude();
			if (Math.Max(num, num2) > Math.Min(num, num2) + 1)
			{
				return false;
			}
			float num3 = (float)Math.Pow(10.0, -(decimalPlaces - 1)) / 2f;
			if (!(a > b))
			{
				return b * (float)Math.Pow(10.0, -num2) - num3 < a * (float)Math.Pow(10.0, -num2);
			}
			return a * (float)Math.Pow(10.0, -num) - num3 < b * (float)Math.Pow(10.0, -num);
		}

		private static bool AlmostEqualWithAbsoluteDecimalPlaces(this double a, double b, int decimalPlaces)
		{
			double num = Math.Pow(10.0, -(decimalPlaces - 1));
			return Math.Abs(a - b) < num / 2.0;
		}

		private static bool AlmostEqualWithAbsoluteDecimalPlaces(this float a, float b, int decimalPlaces)
		{
			float num = (float)Math.Pow(10.0, -(decimalPlaces - 1));
			return Math.Abs(a - b) < num / 2f;
		}

		public static bool AlmostEqual(this double a, double b, long maxNumbersBetween)
		{
			if (maxNumbersBetween < 1)
			{
				throw new ArgumentOutOfRangeException("maxNumbersBetween");
			}
			if (double.IsInfinity(a) || double.IsInfinity(b))
			{
				return a == b;
			}
			if (double.IsNaN(a) || double.IsNaN(b))
			{
				return false;
			}
			long directionalLongFromDouble = GetDirectionalLongFromDouble(a);
			long directionalLongFromDouble2 = GetDirectionalLongFromDouble(b);
			if (!(a > b))
			{
				return directionalLongFromDouble + maxNumbersBetween >= directionalLongFromDouble2;
			}
			return directionalLongFromDouble2 + maxNumbersBetween >= directionalLongFromDouble;
		}

		public static bool AlmostEqual(this float a, float b, int maxNumbersBetween)
		{
			if (maxNumbersBetween < 1)
			{
				throw new ArgumentOutOfRangeException("maxNumbersBetween");
			}
			if (float.IsInfinity(a) || float.IsInfinity(b))
			{
				return a == b;
			}
			if (float.IsNaN(a) || float.IsNaN(b))
			{
				return false;
			}
			int directionalIntFromFloat = GetDirectionalIntFromFloat(a);
			int directionalIntFromFloat2 = GetDirectionalIntFromFloat(b);
			if (!(a > b))
			{
				return directionalIntFromFloat + maxNumbersBetween >= directionalIntFromFloat2;
			}
			return directionalIntFromFloat2 + maxNumbersBetween >= directionalIntFromFloat;
		}

		public static bool IsLarger(this double a, double b, long maxNumbersBetween)
		{
			if (double.IsNaN(a) || double.IsNaN(b))
			{
				return false;
			}
			return a.CompareTo(b, maxNumbersBetween) > 0;
		}

		public static bool IsLargerWithDecimalPlaces(this double a, double b, int decimalPlaces)
		{
			if (double.IsNaN(a) || double.IsNaN(b))
			{
				return false;
			}
			return a.CompareToInDecimalPlaces(b, decimalPlaces) > 0;
		}

		public static bool IsSmaller(this double a, double b, long maxNumbersBetween)
		{
			if (double.IsNaN(a) || double.IsNaN(b))
			{
				return false;
			}
			return a.CompareTo(b, maxNumbersBetween) < 0;
		}

		public static bool IsSmaller(this float a, float b, long maxNumbersBetween)
		{
			if (float.IsNaN(a) || float.IsNaN(b))
			{
				return false;
			}
			return CompareTo(a, b, maxNumbersBetween) < 0;
		}

		public static bool IsSmallerWithDecimalPlaces(this double a, double b, int decimalPlaces)
		{
			if (double.IsNaN(a) || double.IsNaN(b))
			{
				return false;
			}
			return a.CompareToInDecimalPlaces(b, decimalPlaces) < 0;
		}

		public static bool IsSmallerWithDecimalPlaces(this float a, float b, int decimalPlaces)
		{
			if (double.IsNaN(a) || double.IsNaN(b))
			{
				return false;
			}
			return CompareToInDecimalPlaces(a, b, decimalPlaces) < 0;
		}

		public static int CompareTo(this double a, double b, long maxNumbersBetween)
		{
			if (double.IsNaN(a) || double.IsNaN(b))
			{
				return a.CompareTo(b);
			}
			if (double.IsInfinity(a) || double.IsInfinity(b))
			{
				return a.CompareTo(b);
			}
			if (a.AlmostEqual(b, maxNumbersBetween))
			{
				return 0;
			}
			return a.CompareTo(b);
		}

		public static int CompareToInDecimalPlaces(this double a, double b, int decimalPlaces)
		{
			if (double.IsNaN(a) || double.IsNaN(b))
			{
				return a.CompareTo(b);
			}
			if (double.IsInfinity(a) || double.IsInfinity(b))
			{
				return a.CompareTo(b);
			}
			if (a.AlmostEqualInDecimalPlaces(b, decimalPlaces))
			{
				return 0;
			}
			return a.CompareTo(b);
		}

		public static float EpsilonOf(this float value)
		{
			if (float.IsInfinity(value) || float.IsNaN(value))
			{
				return float.NaN;
			}
			if (value.Equals(0f))
			{
				return float.Epsilon;
			}
			if (value < 0f)
			{
				return -1E-45f;
			}
			return float.Epsilon;
		}

		public static double PositiveEpsilonOf(this float value)
		{
			return 2f * value.EpsilonOf();
		}

		private static int FloatToInt32Bits(float value)
		{
			return BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
		}
	}
}
