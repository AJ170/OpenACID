using Assets.Scripts.Network.Missions.Math;

namespace Assets.Scripts.Network.Missions.Helpers
{
	public class VectorExtensions
	{
		public static IPoint StringVectorToFloatArray(string _value)
		{
			if (_value == null)
			{
				return default(Point3D);
			}
			string[] array = _value.Split(':');
			int num = array.Length;
			IPoint point = (IPoint)((num != 2) ? ((object)default(Point3D)) : ((object)default(Point2D)));
			for (int i = 0; i < array.Length; i++)
			{
				float result;
				point[i] = ((!float.TryParse(array[i], out result)) ? 0f : result);
			}
			return point;
		}

		public static string FloatArrayToVectorString(IPoint _targetPosition)
		{
			string text = string.Empty;
			for (int i = 0; i < _targetPosition.Length; i++)
			{
				float num = _targetPosition[i];
				if (text != string.Empty)
				{
					text += ":";
				}
				text += num;
			}
			return text;
		}
	}
}
