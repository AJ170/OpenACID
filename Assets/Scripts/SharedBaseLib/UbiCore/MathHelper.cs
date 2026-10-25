using System;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace UbiCore
{
	public static class MathHelper
	{
		public static readonly float MathfEpsilon = 1.192093E-07f;

		public static readonly float Epsilon = MathfEpsilon;

		public static readonly Vector3[] Identity = new Vector3[3]
		{
			Vector3.right,
			Vector3.up,
			Vector3.forward
		};

		public static bool Approximately(float _a, float _b)
		{
			return Mathf.Abs(_b - _a) < Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(_a), Mathf.Abs(_b)), 8f * MathfEpsilon);
		}

		public static int NearestPowerOfTwo(int x)
		{
			int num = Mathf.NextPowerOfTwo(x);
			if (num - x > x - num / 2)
			{
				num /= 2;
			}
			return num;
		}

		public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
		{
			if (val.CompareTo(min) < 0)
			{
				return min;
			}
			if (val.CompareTo(max) > 0)
			{
				return max;
			}
			return val;
		}

		public static Vector3 ClosestPoint(Vector3 _lineStart, Vector3 _lineEnd, Vector3 _point)
		{
			float num = Vector3.Distance(_lineStart, _lineEnd);
			if (Approximately(num, 0f))
			{
				return _lineStart;
			}
			float num2 = Vector3.Dot(_point - _lineStart, _lineEnd - _lineStart) / (num * num);
			if (num2 < 0f)
			{
				return _lineStart;
			}
			if (num2 > 1f)
			{
				return _lineEnd;
			}
			return _lineStart + num2 * (_lineEnd - _lineStart);
		}

		public static Vector2 ClosestPoint(Vector2 _lineStart, Vector2 _lineEnd, Vector2 _point)
		{
			float num = Vector2.Distance(_lineStart, _lineEnd);
			if (Approximately(num, 0f))
			{
				return _lineStart;
			}
			float num2 = Vector2.Dot(_point - _lineStart, _lineEnd - _lineStart) / (num * num);
			if (num2 < 0f)
			{
				return _lineStart;
			}
			if (num2 > 1f)
			{
				return _lineEnd;
			}
			return _lineStart + num2 * (_lineEnd - _lineStart);
		}

		public static float ClosestDistanceSquared(Vector3 _lineStart, Vector3 _lineEnd, Vector3 _point)
		{
			return ClosestPoint(_lineStart, _lineEnd, _point).DistanceSquared(_point);
		}

		public static float ClosestDistanceSquared(Vector2 _lineStart, Vector2 _lineEnd, Vector2 _point)
		{
			return ClosestPoint(_lineStart, _lineEnd, _point).DistanceSquared(_point);
		}

		public static Vector2 LineIntersect(Vector3 ao, Vector3 ar, Vector3 bo, Vector3 br)
		{
			Vector3 vector = ar;
			Vector3 vector2 = -br;
			Vector3 vector3 = bo - ao;
			Vector2 vector4 = new Vector2(Vector3.Dot(vector, vector), Vector3.Dot(vector2, vector));
			Vector2 vector5 = new Vector2(Vector3.Dot(vector, vector2), Vector3.Dot(vector2, vector2));
			Vector2 vector6 = new Vector2(Vector3.Dot(vector, vector3), Vector3.Dot(vector2, vector3));
			float num = vector4.x * vector5.y - vector5.x * vector4.y;
			if (0f - Epsilon < num && num < Epsilon)
			{
				return new Vector2(Vector3.Dot(vector3, ar) / ar.sqrMagnitude, 0f);
			}
			num = 1f / num;
			Vector2 vector7 = new Vector2(vector5.y * num, (0f - vector5.x) * num);
			return new Vector2(y: Vector3.Dot(new Vector2((0f - vector4.y) * num, vector4.x * num), vector6), x: Vector3.Dot(vector7, vector6));
		}

		public static float LineSegmentDistance(Vector3 a0, Vector3 a1, Vector3 b0, Vector3 b1)
		{
			Vector3 vector = a1 - a0;
			Vector3 vector2 = b1 - b0;
			Vector3 vector3 = a0 - b0;
			float num = Vector3.Dot(vector, vector);
			float num2 = Vector3.Dot(vector, vector2);
			float num3 = Vector3.Dot(vector2, vector2);
			float num4 = Vector3.Dot(vector, vector3);
			float num5 = Vector3.Dot(vector2, vector3);
			float num7;
			float num6;
			float num8;
			float num9;
			if ((num7 = (num6 = num * num3 - num2 * num2)) < Epsilon)
			{
				num8 = 0f;
				num6 = 1f;
				num9 = num5;
				num7 = num3;
			}
			else
			{
				num8 = num2 * num5 - num3 * num4;
				num9 = num * num5 - num2 * num4;
				if (num8 < 0f)
				{
					num8 = 0f;
					num9 = num5;
					num7 = num3;
				}
				else if (num8 > num6)
				{
					num8 = num6;
					num9 = num5 + num2;
					num7 = num3;
				}
			}
			if (num9 < 0f)
			{
				num9 = 0f;
				if (0f - num4 < 0f)
				{
					num8 = 0f;
				}
				else if (0f - num4 > num)
				{
					num8 = num6;
				}
				else
				{
					num8 = 0f - num4;
					num6 = num;
				}
			}
			else if (num9 > num7)
			{
				num9 = num7;
				if ((double)(0f - num4 + num2) < 0.0)
				{
					num8 = 0f;
				}
				else if (0f - num4 + num2 > num)
				{
					num8 = num6;
				}
				else
				{
					num8 = 0f - num4 + num2;
					num6 = num;
				}
			}
			float num10 = ((Mathf.Abs(num8) < Epsilon) ? 0f : (num8 / num6));
			float num11 = ((Mathf.Abs(num9) < Epsilon) ? 0f : (num9 / num7));
			return (vector3 + num10 * vector - num11 * vector2).magnitude;
		}

		public static float LineSegmentDistance(Vector2 a0, Vector2 a1, Vector2 b0, Vector2 b1)
		{
			Vector2 vector = a1 - a0;
			Vector2 vector2 = b1 - b0;
			Vector2 vector3 = a0 - b0;
			float num = Vector2.Dot(vector, vector);
			float num2 = Vector2.Dot(vector, vector2);
			float num3 = Vector2.Dot(vector2, vector2);
			float num4 = Vector2.Dot(vector, vector3);
			float num5 = Vector2.Dot(vector2, vector3);
			float num7;
			float num6;
			float num8;
			float num9;
			if ((num7 = (num6 = num * num3 - num2 * num2)) < Epsilon)
			{
				num8 = 0f;
				num6 = 1f;
				num9 = num5;
				num7 = num3;
			}
			else
			{
				num8 = num2 * num5 - num3 * num4;
				num9 = num * num5 - num2 * num4;
				if (num8 < 0f)
				{
					num8 = 0f;
					num9 = num5;
					num7 = num3;
				}
				else if (num8 > num6)
				{
					num8 = num6;
					num9 = num5 + num2;
					num7 = num3;
				}
			}
			if (num9 < 0f)
			{
				num9 = 0f;
				if (0f - num4 < 0f)
				{
					num8 = 0f;
				}
				else if (0f - num4 > num)
				{
					num8 = num6;
				}
				else
				{
					num8 = 0f - num4;
					num6 = num;
				}
			}
			else if (num9 > num7)
			{
				num9 = num7;
				if ((double)(0f - num4 + num2) < 0.0)
				{
					num8 = 0f;
				}
				else if (0f - num4 + num2 > num)
				{
					num8 = num6;
				}
				else
				{
					num8 = 0f - num4 + num2;
					num6 = num;
				}
			}
			float num10 = ((Mathf.Abs(num8) < Epsilon) ? 0f : (num8 / num6));
			float num11 = ((Mathf.Abs(num9) < Epsilon) ? 0f : (num9 / num7));
			return (vector3 + num10 * vector - num11 * vector2).magnitude;
		}

		public static bool Intersects(this Bounds _bounds, Vector3 _center, float _radius)
		{
			Vector3 lhs = _bounds.center - _center;
			float num = 0f;
			Vector3 zero = Vector3.zero;
			for (int i = 0; i < 3; i++)
			{
				zero[i] = Vector3.Dot(lhs, Identity[i]);
				if (zero[i] < 0f - _bounds.extents[i])
				{
					float num2 = zero[i] + _bounds.extents[i];
					float num3 = num;
					float num4 = num2;
					num = num3 + num4 * num4;
					zero[i] = 0f - _bounds.extents[i];
				}
				else if (zero[i] > _bounds.extents[i])
				{
					float num2 = zero[i] - _bounds.extents[i];
					float num5 = num;
					float num6 = num2;
					num = num5 + num6 * num6;
					zero[i] = _bounds.extents[i];
				}
			}
			return num <= _radius * _radius;
		}
	}
}
