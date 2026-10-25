using System;

public class CubicSpline
{
	private float[] a;

	private float[] b;

	private float[] r;

	private TriDiagonalMatrixF m;

	private float[] xOrig;

	private float[] yOrig;

	private int _lastIndex;

	public float[] FitAndEval(float[] x, float[] y, float[] xs, float startSlope = float.NaN, float endSlope = float.NaN)
	{
		Fit(x, y, startSlope, endSlope);
		return Eval(xs);
	}

	public void Fit(float[] x, float[] y, float startSlope = float.NaN, float endSlope = float.NaN)
	{
		if (float.IsInfinity(startSlope) || float.IsInfinity(endSlope))
		{
			throw new Exception("startSlope and endSlope cannot be infinity.");
		}
		xOrig = x;
		yOrig = y;
		int num = x.Length;
		if (r == null || r.Length != num)
		{
			r = new float[num];
		}
		if (m == null || m.A.Length != num)
		{
			m = new TriDiagonalMatrixF(num);
		}
		if (float.IsNaN(startSlope))
		{
			float num2 = x[1] - x[0];
			m.C[0] = 1f / num2;
			m.B[0] = 2f * m.C[0];
			r[0] = 3f * (y[1] - y[0]) / (num2 * num2);
		}
		else
		{
			m.B[0] = 1f;
			r[0] = startSlope;
		}
		for (int i = 1; i < num - 1; i++)
		{
			float num2 = x[i] - x[i - 1];
			float num3 = x[i + 1] - x[i];
			m.A[i] = 1f / num2;
			m.C[i] = 1f / num3;
			m.B[i] = 2f * (m.A[i] + m.C[i]);
			float num4 = y[i] - y[i - 1];
			float num5 = y[i + 1] - y[i];
			r[i] = 3f * (num4 / (num2 * num2) + num5 / (num3 * num3));
		}
		if (float.IsNaN(endSlope))
		{
			float num2 = x[num - 1] - x[num - 2];
			float num4 = y[num - 1] - y[num - 2];
			m.A[num - 1] = 1f / num2;
			m.B[num - 1] = 2f * m.A[num - 1];
			r[num - 1] = 3f * (num4 / (num2 * num2));
		}
		else
		{
			m.B[num - 1] = 1f;
			r[num - 1] = endSlope;
		}
		float[] array = m.Solve(r);
		if (a == null || a.Length != num - 1)
		{
			a = new float[num - 1];
		}
		if (b == null || b.Length != num - 1)
		{
			b = new float[num - 1];
		}
		for (int j = 1; j < num; j++)
		{
			float num2 = x[j] - x[j - 1];
			float num4 = y[j] - y[j - 1];
			a[j - 1] = array[j - 1] * num2 - num4;
			b[j - 1] = (0f - array[j]) * num2 + num4;
		}
	}

	public float[] Eval(float[] x)
	{
		int num = x.Length;
		float[] array = new float[num];
		_lastIndex = 0;
		for (int i = 0; i < num; i++)
		{
			int nextXIndex = GetNextXIndex(x[i]);
			float num2 = (x[i] - xOrig[nextXIndex]) / (xOrig[nextXIndex + 1] - xOrig[nextXIndex]);
			array[i] = (1f - num2) * yOrig[nextXIndex] + num2 * yOrig[nextXIndex + 1] + num2 * (1f - num2) * (a[nextXIndex] * (1f - num2) + b[nextXIndex] * num2);
		}
		return array;
	}

	public float Eval(float x)
	{
		_lastIndex = 0;
		int nextXIndex = GetNextXIndex(x);
		float num = (x - xOrig[nextXIndex]) / (xOrig[nextXIndex + 1] - xOrig[nextXIndex]);
		return (1f - num) * yOrig[nextXIndex] + num * yOrig[nextXIndex + 1] + num * (1f - num) * (a[nextXIndex] * (1f - num) + b[nextXIndex] * num);
	}

	private int GetNextXIndex(float x)
	{
		while (_lastIndex < xOrig.Length - 2 && x > xOrig[_lastIndex + 1])
		{
			_lastIndex++;
		}
		return _lastIndex;
	}
}
