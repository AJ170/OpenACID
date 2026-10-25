using System;
using UnityEngine;

public class Interpolate
{
	public enum EaseType
	{
		Linear = 0,
		EaseInQuad = 1,
		EaseOutQuad = 2,
		EaseInOutQuad = 3,
		EaseInCubic = 4,
		EaseOutCubic = 5,
		EaseInOutCubic = 6,
		EaseInQuart = 7,
		EaseOutQuart = 8,
		EaseInOutQuart = 9,
		EaseInQuint = 10,
		EaseOutQuint = 11,
		EaseInOutQuint = 12,
		EaseInSine = 13,
		EaseOutSine = 14,
		EaseInOutSine = 15,
		EaseInExpo = 16,
		EaseOutExpo = 17,
		EaseInOutExpo = 18,
		EaseInCirc = 19,
		EaseOutCirc = 20,
		EaseInOutCirc = 21
	}

	public delegate float Function(float t);

	public const float HALF_PI = (float)Math.PI / 2f;

	public const float TWO_PI = (float)Math.PI * 2f;

	private static Function[] m_easeFunctions = new Function[22]
	{
		Linear, EaseInQuad, EaseOutQuad, EaseInOutQuad, EaseInCubic, EaseOutCubic, EaseInOutCubic, EaseInQuart, EaseOutQuart, EaseInOutQuart,
		EaseInQuint, EaseOutQuint, EaseInOutQuint, EaseInSine, EaseOutSine, EaseInOutSine, EaseInExpo, EaseOutExpo, EaseInOutExpo, EaseInCirc,
		EaseOutCirc, EaseInOutCirc
	};

	public static float Ease01(EaseType type, float t)
	{
		return m_easeFunctions[(int)type](t);
	}

	private static float Linear(float t)
	{
		return t;
	}

	private static float EaseInQuad(float t)
	{
		return t * t;
	}

	private static float EaseOutQuad(float t)
	{
		return -1f * t * (t - 2f);
	}

	private static float EaseInOutQuad(float t)
	{
		t *= 2f;
		if (t < 1f)
		{
			return 0.5f * t * t;
		}
		t -= 1f;
		float num = t;
		return -0.5f * (num * (num - 2f) - 1f);
	}

	private static float EaseInCubic(float t)
	{
		return t * t * t;
	}

	private static float EaseOutCubic(float t)
	{
		t -= 1f;
		float num = t;
		return num * num * t + 1f;
	}

	private static float EaseInOutCubic(float t)
	{
		t *= 2f;
		if (t < 1f)
		{
			return 0.5f * t * t * t;
		}
		t -= 2f;
		float num = t;
		return 0.5f * (num * num * t + 2f);
	}

	private static float EaseInQuart(float t)
	{
		return t * t * t * t;
	}

	private static float EaseOutQuart(float t)
	{
		t -= 1f;
		float num = t;
		return 0f - (num * num * t * t - 1f);
	}

	private static float EaseInOutQuart(float t)
	{
		t *= 2f;
		if (t < 1f)
		{
			return 0.5f * t * t * t * t;
		}
		t -= 2f;
		float num = t;
		return -0.5f * (num * num * t * t - 2f);
	}

	private static float EaseInQuint(float t)
	{
		return t * t * t * t * t;
	}

	private static float EaseOutQuint(float t)
	{
		t -= 1f;
		float num = t;
		return num * num * t * t * t + 1f;
	}

	private static float EaseInOutQuint(float t)
	{
		t *= 2f;
		if (t < 1f)
		{
			return 0.5f * t * t * t * t * t;
		}
		t -= 2f;
		float num = t;
		return 0.5f * (num * num * t * t * t + 2f);
	}

	private static float EaseInSine(float t)
	{
		return -1f * Mathf.Cos(t * ((float)Math.PI / 2f)) + 1f;
	}

	private static float EaseOutSine(float t)
	{
		return Mathf.Sin(t * ((float)Math.PI / 2f));
	}

	private static float EaseInOutSine(float t)
	{
		return -0.5f * (Mathf.Cos((float)Math.PI * t) - 1f);
	}

	private static float EaseInExpo(float t)
	{
		return Mathf.Pow(2f, 10f * (t - 1f));
	}

	private static float EaseOutExpo(float t)
	{
		return 0f - Mathf.Pow(2f, -10f * t) + 1f;
	}

	private static float EaseInOutExpo(float t)
	{
		t *= 2f;
		if (t < 1f)
		{
			return 0.5f * Mathf.Pow(2f, 10f * (t - 1f));
		}
		t -= 1f;
		return 0.5f * (0f - Mathf.Pow(2f, -10f * t) + 2f);
	}

	private static float EaseInCirc(float t)
	{
		return -1f * (Mathf.Sqrt(1f - t * t) - 1f);
	}

	private static float EaseOutCirc(float t)
	{
		t -= 1f;
		float num = t;
		return Mathf.Sqrt(1f - num * num);
	}

	private static float EaseInOutCirc(float t)
	{
		t *= 2f;
		if (t < 1f)
		{
			float num = t;
			return -0.5f * (Mathf.Sqrt(1f - num * num) - 1f);
		}
		t -= 2f;
		float num2 = t;
		return 0.5f * (Mathf.Sqrt(1f - num2 * num2) + 1f);
	}
}
