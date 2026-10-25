using UnityEngine;

namespace UbiCore.Utils.Extensions
{
	public static class PlaneExtensions
	{
		public static Vector3 Project(this Plane _plane, Vector3 _point)
		{
			Vector3 normalized = _plane.normal.normalized;
			return _point - _plane.GetDistanceToPoint(_point) * normalized;
		}

		public static bool RayCast(this Plane _plane, Ray _ray, out Vector3 _hitPoint)
		{
			float enter;
			if (_plane.Raycast(_ray, out enter))
			{
				_hitPoint = _ray.origin + _ray.direction * enter;
				return true;
			}
			_hitPoint = Vector3.zero;
			return false;
		}
	}
}
