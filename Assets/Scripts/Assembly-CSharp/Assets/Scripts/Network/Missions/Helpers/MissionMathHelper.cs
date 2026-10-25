using System;
using System.Collections.Generic;
using Assets.Scripts.Engine.ExtensionMethods;
using Assets.Scripts.Network.Missions.Math;
using UnityEngine;

namespace Assets.Scripts.Network.Missions.Helpers
{
	public class MissionMathHelper
	{
		public static bool Contains2D(Point2D _testPosition, List<Point2D> _vertices, bool _startsInside = false)
		{
			int num = 0;
			int index = _vertices.Count - 1;
			while (num < _vertices.Count)
			{
				if (_vertices[num].y > _testPosition.y != _vertices[index].y > _testPosition.y && _testPosition.x < (_vertices[index].x - _vertices[num].x) * (_testPosition.y - _vertices[num].y) / (_vertices[index].y - _vertices[num].y) + _vertices[num].x)
				{
					_startsInside = !_startsInside;
				}
				index = num++;
			}
			return _startsInside;
		}

		public static double Distance(Point3D _point, Point3D _pointB, bool _useHeightFilter)
		{
			if (_useHeightFilter && (double)System.Math.Abs(_point.y - _pointB.y) > 0.5)
			{
				return double.PositiveInfinity;
			}
			return Distance2D(_point, _pointB);
		}

		public static double Distance2D(IPoint _pointA, IPoint _pointB)
		{
			double num = _pointA[0] - _pointB[0];
			double num2 = _pointA[_pointA.Length - 1] - _pointB[_pointB.Length - 1];
			return System.Math.Sqrt(num * num + num2 * num2);
		}

		public static double Distance(Point3D _point, Point3D _segmentStart, Point3D _segmentEnd, bool _useHeightFilter)
		{
			double num = (double)System.Math.Min(_segmentStart.y, _segmentEnd.y) - 0.5;
			double num2 = (double)System.Math.Max(_segmentStart.y, _segmentEnd.y) + 0.5;
			if (_useHeightFilter && ((double)_point.y < num || (double)_point.y > num2))
			{
				return double.PositiveInfinity;
			}
			return Distance2D(_point, _segmentStart, _segmentEnd);
		}

		public static double Distance(Point3D _lineStart, Point3D _lineEnd, Point3D fromPoint2D, Point3D toPoint2D)
		{
			double val = Distance(fromPoint2D, toPoint2D, _lineStart, false);
			double val2 = Distance(fromPoint2D, toPoint2D, _lineEnd, false);
			double val3 = System.Math.Min(val, val2);
			double val4 = Distance(_lineStart, _lineEnd, fromPoint2D, false);
			double val5 = Distance(_lineStart, _lineEnd, toPoint2D, false);
			double val6 = System.Math.Min(val4, val5);
			return System.Math.Min(val3, val6);
		}

		public static double Distance2D(IPoint _point, IPoint _segmentStart, IPoint _segmentEnd)
		{
			double num = _segmentEnd[0] - _segmentStart[0];
			float num2 = _segmentStart[_segmentStart.Length - 1];
			double num3 = _segmentEnd[_segmentEnd.Length - 1] - num2;
			double num4 = System.Math.Abs(num * (double)(num2 - _point[_point.Length - 1]) - (double)(_segmentStart[0] - _point[0]) * num3);
			double num5 = System.Math.Sqrt(num * num + num3 * num3);
			return num4 / num5;
		}

		public static float IntersectionLength2D(Point2D _p0, Point2D _p1, List<Point2D> _vertices)
		{
			List<Point2D> list = new List<Point2D>();
			for (int i = 0; i < _vertices.Count; i++)
			{
				Point2D point2D = _vertices[i];
				Point2D point2D2 = _vertices[(i + 1) % _vertices.Count];
				float num = _p1.x - _p0.x;
				float num2 = _p1.y - _p0.y;
				float num3 = point2D2.x - point2D.x;
				float num4 = point2D2.y - point2D.y;
				float num5 = (0f - num3) * num2 + num * num4;
				float num6 = _p0.x - point2D.x;
				float num7 = _p0.y - point2D.y;
				float num8 = ((0f - num2) * num6 + num * num7) / num5;
				float num9 = (num3 * num7 - num4 * num6) / num5;
				if (num8 >= 0f && num8 <= 1f && num9 >= 0f && num9 <= 1f)
				{
					float x = _p0.x + num9 * num;
					float y = _p0.y + num9 * num2;
					list.Add(new Point2D(x, y));
				}
			}
			if (Contains2D(_p0, _vertices))
			{
				list.Insert(0, _p0);
			}
			if (Contains2D(_p1, _vertices))
			{
				list.Add(_p1);
			}
			float num10 = 0f;
			for (int j = 0; j < list.Count - 1; j += 2)
			{
				Point2D point2D3 = list[j];
				Point2D point2D4 = list[j + 1];
				num10 += Magnitude(point2D3, point2D4);
			}
			return num10;
		}

		public static float Magnitude(IPoint _pointA, IPoint _pointB)
		{
			float num = 0f;
			for (int i = 0; i < _pointA.Length; i++)
			{
				float num2 = _pointB[i] - _pointA[i];
				num += num2 * num2;
			}
			return (float)System.Math.Sqrt(num);
		}

		public static int Clamp(int _min, int _max, int _value)
		{
			return System.Math.Min(_max, System.Math.Max(_min, _value));
		}

		public static int RoundToInt(float _value)
		{
			return (int)(_value + 0.5f);
		}

		public static float Repeat(float _value, float _length)
		{
			float num = _value / _length;
			return Mathf.Clamp((num - Mathf.Floor(num)) * _length, 0f, _length);
		}

		public static int FindBestValue(int _needle, ICollection<int> _stack)
		{
			int result = -1;
			if (_stack.Contains(_needle))
			{
				result = _needle;
			}
			else
			{
				int num = int.MinValue;
				foreach (int item in _stack)
				{
					num = System.Math.Max(num, item);
				}
				if (num != int.MinValue)
				{
					result = num;
				}
				else
				{
					int num2 = int.MaxValue;
					foreach (int item2 in _stack)
					{
						num2 = System.Math.Max(num2, item2);
					}
					if (num2 != int.MaxValue)
					{
						result = num2;
					}
				}
			}
			return result;
		}

		public static float ComputePathLength(List<Point3D> _path)
		{
			float num = 0f;
			for (int i = 0; i < _path.Count - 1; i++)
			{
				float num2 = Magnitude((Point2D)_path[i], (Point2D)_path[i + 1]);
				num += num2;
			}
			return num;
		}

		public static int FloatToFixed(float _value)
		{
			return RoundToInt(_value * 1000f);
		}

		public static int FixedToIntRounded(int _value)
		{
			int num = _value / 1000;
			return (_value % 1000 <= 500) ? num : (num + 1);
		}

		public static int FloatToCeilFixed(float _value)
		{
			return Mathf.CeilToInt(_value * 1000f);
		}

		public static int FixedToIntCeil(int _value)
		{
			int num = _value / 1000;
			return (_value % 1000 <= 1) ? num : (num + 1);
		}

		public static float FixedToFloat(int _value)
		{
			return (float)_value / 1000f;
		}

		public static bool IsInside(Vector2[] _corners, Vector3 _checkPosition)
		{
			int num = _corners.Length;
			bool flag = false;
			int num2 = 0;
			int num3 = num - 1;
			while (num2 < num)
			{
				if (_corners[num2].y >= _checkPosition.z != _corners[num3].y >= _checkPosition.z && _checkPosition.x <= (_corners[num3].x - _corners[num2].x) * (_checkPosition.z - _corners[num2].y) / (_corners[num3].y - _corners[num2].y) + _corners[num2].x)
				{
					flag = !flag;
				}
				num3 = num2++;
			}
			return flag;
		}

		private static Vector2 ClosestPointInSegmentTo(Vector2 _p, Vector2 _p1, Vector2 _p2)
		{
			Vector2 vector = _p2 - _p1;
			Vector2 lhs = _p - _p1;
			float num = Vector2.Dot(lhs, vector);
			if (num <= 0f)
			{
				return _p1;
			}
			float num2 = Vector2.Dot(vector, vector);
			if (num2 <= num)
			{
				return _p2;
			}
			float num3 = num / num2;
			return _p1 + num3 * vector;
		}

		public static Vector2 ClosestPointOnPolygon(Vector2[] _segments, Vector3 _point)
		{
			Vector2 vector = _point.To2D();
			double num = double.MaxValue;
			Vector2 result = Vector2.zero;
			for (int i = 0; i < _segments.Length; i++)
			{
				Vector2 p = _segments[i];
				Vector2 p2 = _segments[(i + 1) % _segments.Length];
				Vector2 vector2 = ClosestPointInSegmentTo(vector, p, p2);
				double num2 = (vector - vector2).sqrMagnitude;
				if (num2 < num)
				{
					num = num2;
					result = vector2;
				}
			}
			return result;
		}
	}
}
