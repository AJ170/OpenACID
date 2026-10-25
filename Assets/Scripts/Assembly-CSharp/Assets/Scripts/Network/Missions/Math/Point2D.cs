using System;

namespace Assets.Scripts.Network.Missions.Math
{
	public struct Point2D : IPoint
	{
		public float x;

		public float y;

		public float this[int _i]
		{
			get
			{
				switch (_i)
				{
				case 0:
					return x;
				case 1:
					return y;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			set
			{
				switch (_i)
				{
				case 0:
					x = value;
					break;
				case 1:
					y = value;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		public int Length
		{
			get
			{
				return 2;
			}
		}

		public Point2D(float _x, float _y)
		{
			x = _x;
			y = _y;
		}

		public Point2D(Point2D _point)
		{
			x = _point.x;
			y = _point.y;
		}

		public bool Equals(Point2D other)
		{
			return this == other;
		}

		public override bool Equals(object obj)
		{
			if (object.ReferenceEquals(null, obj))
			{
				return false;
			}
			return obj is Point2D && Equals((Point2D)obj);
		}

		public override int GetHashCode()
		{
			return (x.GetHashCode() * 397) ^ y.GetHashCode();
		}

		public static Point2D operator -(Point2D left, Point2D right)
		{
			return new Point2D(left.x - right.x, left.y - right.y);
		}

		public static bool operator ==(Point2D left, Point2D right)
		{
			return (double)System.Math.Abs(left.x - right.x) < 1.401298464324817E-45 && (double)System.Math.Abs(left.y - right.y) < 1.401298464324817E-45;
		}

		public static bool operator !=(Point2D a, Point2D b)
		{
			return !(a == b);
		}
	}
}
