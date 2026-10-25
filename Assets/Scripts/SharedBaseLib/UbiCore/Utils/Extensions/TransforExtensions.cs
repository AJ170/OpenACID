using System.Diagnostics;
using UnityEngine;

namespace UbiCore.Utils.Extensions
{
	public static class TransforExtensions
	{
		public static void ConstrainedLookAt(this Transform _transform, Vector3 _target, RotationAxis _axis)
		{
			Vector3 eulerAngles = Quaternion.LookRotation(_target - _transform.position).eulerAngles;
			Vector3 eulerAngles2 = _transform.rotation.eulerAngles;
			_transform.rotation = Quaternion.Euler(((_axis & RotationAxis.XAxis) != 0) ? eulerAngles.x : eulerAngles2.x, ((_axis & RotationAxis.YAxis) != 0) ? eulerAngles.y : eulerAngles2.y, ((_axis & RotationAxis.ZAxis) != 0) ? eulerAngles.z : eulerAngles2.z);
		}

		[DebuggerStepThrough]
		public static void ConstrainedLookAt(this Transform _transform, Transform _target, RotationAxis _axis)
		{
			_transform.ConstrainedLookAt(_target.position, _axis);
		}

		public static T FindComponentInParents<T>(this Transform t) where T : Component
		{
			Component component = t.GetComponent<T>();
			if (component != null)
			{
				return (T)component;
			}
			if (t.parent == null)
			{
				return null;
			}
			return t.parent.FindComponentInParents<T>();
		}
	}
}
