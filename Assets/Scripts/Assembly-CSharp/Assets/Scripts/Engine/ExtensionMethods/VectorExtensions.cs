using UnityEngine;

namespace Assets.Scripts.Engine.ExtensionMethods
{
	public static class VectorExtensions
	{
		public static string ToVectorString(this Vector3 _vector)
		{
			return string.Format("{0}:{1}:{2}", _vector.x, _vector.y, _vector.z);
		}

		public static bool TryParseVector3(this string _vector3, out Vector3 _result)
		{
			_result = Vector3.zero;
			if (string.IsNullOrEmpty(_vector3))
			{
				return false;
			}
			string[] array = _vector3.Split(':');
			if (array.Length != 3)
			{
				return false;
			}
			float[] array2 = new float[3];
			for (int i = 0; i < 3; i++)
			{
				if (!float.TryParse(array[i], out array2[i]))
				{
					return false;
				}
			}
			_result = new Vector3(array2[0], array2[1], array2[2]);
			return true;
		}
	}
}
