using System;

namespace Assets.Scripts.Mission.Utilities.AutoUpdating
{
	public static class Interpolation
	{
		public static float InterpolateSinusoidal(float _parameter)
		{
			return (float)(-0.5 * (Math.Cos(Math.PI * (double)_parameter) - 1.0));
		}
	}
}
