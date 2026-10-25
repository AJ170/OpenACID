using UnityEngine;

namespace UbiCore.Utils.Extensions
{
	public static class VectorExtensions
	{
		public static float DistanceSquared(this Vector3 _lhs, Vector3 _rhs)
		{
			return (_rhs - _lhs).sqrMagnitude;
		}

		public static float DistanceSquared(this Vector2 _lhs, Vector2 _rhs)
		{
			return (_rhs - _lhs).sqrMagnitude;
		}

		public static float LengthXZ(this Vector3 _v)
		{
			return new Vector2(_v.x, _v.z).magnitude;
		}

		public static float LengthXZSquared(this Vector3 _v)
		{
			return new Vector2(_v.x, _v.z).sqrMagnitude;
		}

		public static float DistanceXZ(this Vector3 _lhs, Vector3 _rhs)
		{
			return new Vector2(_lhs.x - _rhs.x, _lhs.z - _rhs.z).magnitude;
		}

		public static float DistanceXZSquared(this Vector3 _lhs, Vector3 _rhs)
		{
			return new Vector2(_lhs.x - _rhs.x, _lhs.z - _rhs.z).sqrMagnitude;
		}

		public static Vector3 Mask(this Vector3 _vec, Vector3 _mask)
		{
			return new Vector3((_mask.x != 0f) ? _vec.x : 0f, (_mask.y != 0f) ? _vec.y : 0f, (_mask.z != 0f) ? _vec.z : 0f);
		}

		public static Vector3 Mask(this Vector2 _vec, Vector2 _mask)
		{
			return new Vector3((_mask.x != 0f) ? _vec.x : 0f, (_mask.y != 0f) ? _vec.y : 0f);
		}

		public static Vector3 PairwiseMultiply(this Vector3 _vec, Vector3 _mult)
		{
			return new Vector3(_vec.x * _mult.x, _vec.y * _mult.y, _vec.z * _mult.z);
		}

		public static Vector2 PairwiseMultiply(this Vector2 _vec, Vector2 _mult)
		{
			return new Vector2(_vec.x * _mult.x, _vec.y * _mult.y);
		}

		public static Vector2 Clamped01(this Vector2 _v)
		{
			return new Vector2(Mathf.Clamp01(_v.x), Mathf.Clamp01(_v.y));
		}

		public static Vector3 Clamped01(this Vector3 _v)
		{
			return new Vector3(Mathf.Clamp01(_v.x), Mathf.Clamp01(_v.y), Mathf.Clamp01(_v.z));
		}
	}
}
