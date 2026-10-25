using UbiCore.Utils.Extensions;
using UnityEngine;

namespace UbiCore.Utils
{
	public static class VectorHelper
	{
		public static float DistanceSquared(Vector3 _lhs, Vector3 _rhs)
		{
			return _lhs.DistanceSquared(_rhs);
		}

		public static float DistanceSquared(Vector2 _lhs, Vector2 _rhs)
		{
			return _lhs.DistanceSquared(_rhs);
		}

		public static float SignedAngle(Vector3 _v1, Vector3 _v2, Vector3 _normal)
		{
			return Mathf.Atan2(Vector3.Dot(_normal, Vector3.Cross(_v1, _v2)), Vector3.Dot(_v1, _v2)) * 57.29578f;
		}
	}
}
