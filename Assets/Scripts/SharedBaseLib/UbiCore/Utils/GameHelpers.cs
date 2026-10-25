using System;

namespace UbiCore.Utils
{
	public static class GameHelpers
	{
		public static float NormalizedFactor(float _distance, float _min, float _max)
		{
			if (_min != _max)
			{
				return 1f - Math.Min(1f, Math.Max(0f, _distance - _min) / (_max - _min));
			}
			return 1f;
		}
	}
}
