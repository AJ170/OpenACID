using System;

namespace Assets.Scripts.Network.Missions.Math
{
	public struct Point3D : IPoint
	{
		public float x;

		public float y;

		public float z;

		public int Length
		{
			get
			{
				return 3;
			}
		}

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
				case 2:
					return z;
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
				case 2:
					z = value;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		public Point3D(float _x, float _y, float _z)
		{
			x = _x;
			y = _y;
			z = _z;
		}

		public Point3D(Point3D _point)
		{
			x = _point.x;
			y = _point.y;
			z = _point.z;
		}

		public bool Equals(Point3D other)
		{
			return this == other;
		}

		public override bool Equals(object obj)
		{
			if (object.ReferenceEquals(null, obj))
			{
				return false;
			}
			return obj is Point3D && Equals((Point3D)obj);
		}

		public override int GetHashCode()
		{
			int hashCode = x.GetHashCode();
			hashCode = (hashCode * 397) ^ y.GetHashCode();
			return (hashCode * 397) ^ z.GetHashCode();
		}

		public static bool operator ==(Point3D left, Point3D right)
		{
			return (double)System.Math.Abs(left.x - right.x) < 1.401298464324817E-45 && (double)System.Math.Abs(left.y - right.y) < 1.401298464324817E-45 && (double)System.Math.Abs(left.z - right.z) < 1.401298464324817E-45;
		}

		public static bool operator !=(Point3D a, Point3D b)
		{
			return !(a == b);
		}

		public static explicit operator Point2D(Point3D _input)
		{
			return new Point2D(_input.x, _input.z);
		}
	}
}
