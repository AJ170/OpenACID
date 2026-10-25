using System;
using UnityEngine;

namespace UbiCore.Utils.Extensions
{
	public static class GenericExtensions
	{
		public static bool InRange<T>(this T _value, T _min, T _max) where T : IComparable
		{
			if (_min.CompareTo(_value) <= 0)
			{
				return _max.CompareTo(_value) >= 0;
			}
			return false;
		}

		public static Vector2 Abs(this Vector2 vector)
		{
			for (int i = 0; i < 2; i++)
			{
				vector[i] = Mathf.Abs(vector[i]);
			}
			return vector;
		}

		public static Vector2 DividedBy(this Vector2 vector, Vector2 divisor)
		{
			for (int i = 0; i < 2; i++)
			{
				vector[i] /= divisor[i];
			}
			return vector;
		}

		public static Vector2 Max(this Rect rect)
		{
			return new Vector2(rect.xMax, rect.yMax);
		}

		public static Vector2 IntersectionWithRayFromCenter(this Rect rect, Vector2 pointOnRay)
		{
			Vector2 vector = pointOnRay - rect.center;
			Vector2 vector2 = (rect.Max() - rect.center).DividedBy(vector.Abs());
			if (!(vector2.x < vector2.y))
			{
				return new Vector2(vector.x * vector2.y + rect.center.x, (vector.y > 0f) ? rect.yMax : rect.yMin);
			}
			return new Vector2((vector.x > 0f) ? rect.xMax : rect.xMin, vector.y * vector2.x + rect.center.y);
		}
	}
}
