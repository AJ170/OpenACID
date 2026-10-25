using System;
using System.Text;

public class TriDiagonalMatrixF
{
	public float[] A;

	public float[] B;

	public float[] C;

	private float[] cPrime;

	public int N
	{
		get
		{
			return (A != null) ? A.Length : 0;
		}
	}

	public float this[int row, int col]
	{
		get
		{
			switch (row - col)
			{
			case 0:
				return B[row];
			case -1:
				return C[row];
			case 1:
				return A[row];
			default:
				return 0f;
			}
		}
		set
		{
			switch (row - col)
			{
			case 0:
				B[row] = value;
				break;
			case -1:
				C[row] = value;
				break;
			case 1:
				A[row] = value;
				break;
			default:
				throw new ArgumentException("Only the main, super, and sub diagonals can be set.");
			}
		}
	}

	public TriDiagonalMatrixF(int n)
	{
		A = new float[n];
		B = new float[n];
		C = new float[n];
	}

	public string ToDisplayString(string fmt = "", string prefix = "")
	{
		if (N > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string format = "{0" + fmt + "}";
			for (int i = 0; i < N; i++)
			{
				stringBuilder.Append(prefix);
				for (int j = 0; j < N; j++)
				{
					stringBuilder.AppendFormat(format, this[i, j]);
					if (j < N - 1)
					{
						stringBuilder.Append(", ");
					}
				}
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString();
		}
		return prefix + "0x0 Matrix";
	}

	public float[] Solve(float[] d)
	{
		int n = N;
		if (d.Length != n)
		{
			throw new ArgumentException("The input d is not the same size as this matrix.");
		}
		if (cPrime == null || cPrime.Length != n)
		{
			cPrime = new float[n];
		}
		cPrime[0] = C[0] / B[0];
		for (int i = 1; i < n; i++)
		{
			cPrime[i] = C[i] / (B[i] - cPrime[i - 1] * A[i]);
		}
		float[] array = new float[n];
		array[0] = d[0] / B[0];
		for (int j = 1; j < n; j++)
		{
			array[j] = (d[j] - array[j - 1] * A[j]) / (B[j] - cPrime[j - 1] * A[j]);
		}
		float[] array2 = new float[n];
		array2[n - 1] = array[n - 1];
		for (int num = n - 2; num >= 0; num--)
		{
			array2[num] = array[num] - cPrime[num] * array2[num + 1];
		}
		return array2;
	}
}
