using UnityEngine;

namespace Assets.Scripts.UI
{
	public static class ColorExtensions
	{
		public static int[] ToRGBA255(this Color _color)
		{
			int[] array = new int[4];
			float num = Mathf.Max(0f, Mathf.Min(1f, _color.r));
			float num2 = Mathf.Max(0f, Mathf.Min(1f, _color.g));
			float num3 = Mathf.Max(0f, Mathf.Min(1f, _color.b));
			float num4 = Mathf.Max(0f, Mathf.Min(1f, _color.a));
			array[0] = Mathf.FloorToInt((num != 1f) ? (num * 256f) : 255f);
			array[1] = Mathf.FloorToInt((num2 != 1f) ? (num2 * 256f) : 255f);
			array[2] = Mathf.FloorToInt((num3 != 1f) ? (num3 * 256f) : 255f);
			array[3] = Mathf.FloorToInt((num4 != 1f) ? (num4 * 256f) : 255f);
			return array;
		}

		public static string ToNGUIColorString(this Color _color)
		{
			int[] array = _color.ToRGBA255();
			return string.Format("[{0}{1}{2}]", array[0].ToString("X2"), array[1].ToString("X2"), array[2].ToString("X2"));
		}
	}
}
