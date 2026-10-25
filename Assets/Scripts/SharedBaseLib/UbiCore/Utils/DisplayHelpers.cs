using UnityEngine;

namespace UbiCore.Utils
{
	public static class DisplayHelpers
	{
		private const string sChannel = "Helpers";

		private const float DEFAULT_DPI = 160f;

		private const float POINTS_PER_INCH = 72f;

		public static float DPI
		{
			get
			{
				if (Screen.dpi != 0f)
				{
					return Screen.dpi;
				}
				return 160f;
			}
		}

		public static Vector2 PixelToPoints(Vector2 _pixels, float _dpi)
		{
			return new Vector2(PixelToPoints(_pixels.x, _dpi), PixelToPoints(_pixels.y, _dpi));
		}

		public static float PixelToPoints(float _pixels, float _dpi)
		{
			return _pixels / _dpi * 72f;
		}

		public static Vector2 PointToPixels(Vector2 _points, float _dpi)
		{
			return new Vector2(PointToPixels(_points.x, _dpi), PointToPixels(_points.y, _dpi));
		}

		public static float PointToPixels(float _points, float _dpi)
		{
			return _points * _dpi / 72f;
		}
	}
}
